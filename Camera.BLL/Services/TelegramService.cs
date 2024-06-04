using Camera.BLL.Interfaces;
using Joint.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using Camera.DAL.Interfaces.Repositories;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.IO;

namespace Camera.BLL.Services
{
    public class TelegramService : ITelegramService
    {
        private TelegramBotClient _botClient;
        private ILogger<TelegramService> _logger;
        private CancellationTokenSource _cts = new();
        private IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public TelegramService(ILogger<TelegramService> logger,
            IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public void Init()
        {
            _botClient = new TelegramBotClient(_configuration["Telegram:ApiKey"]);



            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };

            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: _cts.Token
            );

        }

        public void Notify(NotifyToSend notifyToSend)
        {

            var chatId = Read(i => i.UserId == notifyToSend.UserId).FirstOrDefault();
            if (chatId != null)
            {
                _botClient.SendPhotoAsync(
                    chatId: chatId.TelegramId,
                    photo: InputFile.FromStream(System.IO.File.Open(notifyToSend.PathToFile, FileMode.Open)),
                    caption: notifyToSend.Message);
            }

        }


        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {

                // Only process Message updates: https://core.telegram.org/bots/api#message
                if (update.Message is not { } message)
                    return;
                // Only process text messages
                if (message.Text is not { } messageText)
                    return;

                var chatId = message.Chat.Id;

                var user = scope.ServiceProvider.GetRequiredService<IBotClientRepository>().Read(i => i.TelegramId == chatId).FirstOrDefault();

                if (user == null)
                {
                    await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Ви не зареєстровані! Введіть почту:");
                    Save(new BotClient()
                    {
                        TelegramId = chatId,
                        Step = Joint.Data.Constants.BotSteps.InputEmail
                    });
                }
                else if (user.Step == Joint.Data.Constants.BotSteps.InputEmail)
                {
                    if (Regex.Match(messageText, "^[\\w\\.-]+@[a-zA-Z\\d\\.-]+\\.[a-zA-Z]{2,}$").Success)
                    {
                        await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Введіть пароль");
                        var res = scope.ServiceProvider.GetRequiredService<IUserService>().ReadByEmail(messageText);
                        user.UserId = res?.Id;
                        user.Step = Joint.Data.Constants.BotSteps.InputPassword;
                        Save(user);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Почта неправильна! Спробуйте ще раз");
                    }

                }
                else if (user.Step == Joint.Data.Constants.BotSteps.InputPassword)
                {
                    if (user.UserId == null)
                    {
                        await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Почта або пароль не коректні! Введіть почту");
                        user.Step = Joint.Data.Constants.BotSteps.InputEmail;
                        Save(user);
                        return;
                    }
                    var programUser = scope.ServiceProvider.GetRequiredService<IUserService>().Read(user.UserId);
                    programUser.Password = messageText;
                    var res = scope.ServiceProvider.GetRequiredService<IUserService>().IsUserAuth(programUser);
                    if (res != null)
                    {
                        await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Сука угадал!");
                        user.Step = Joint.Data.Constants.BotSteps.Authorizated;
                        Save(user);
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Почта або пароль не коректні! Введіть почту");
                        user.Step = Joint.Data.Constants.BotSteps.InputEmail;
                        Save(user);
                        return;
                    }
                }
                else if (user.Step == Joint.Data.Constants.BotSteps.Authorizated)
                {
                    var res = scope.ServiceProvider.GetRequiredService<ICameraService>().Read(i => i.UserId == user.UserId);
                    var resultColumn = new StringBuilder();
                    foreach (var item in res)
                    {
                        resultColumn.AppendLine($"Назва: {item.Name}, Тип підключення: {item.Connection}, Данні для підключення:  {item.ConnectionData}");
                    }
                    await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Ви авторизовані!" +
                        "\nТут будуть приходити сповіщення." +
                        "\nВаші камери: " + resultColumn);
                }

                Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

                // Echo received message text
                /*Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "You said:\n" + messageText,
                    cancellationToken: cancellationToken);*/
            }
        }

        Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public void Save(BotClient item)
        {
            this.Save(new List<Joint.Data.Models.BotClient>() { item });
        }

        public void Save(IList<BotClient> items)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _repository = scope.ServiceProvider.GetRequiredService<IBotClientRepository>();
                foreach (var item in items)
                {
                    var old = _repository.Read(i => i.Id == item.Id);

                    if (old.Any())
                    {
                        _repository.Update(item);
                    }
                    else
                    {
                        _repository.Create(item);
                    }

                }
                _repository.Save();
            }

        }

        public IList<BotClient> Read()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _repository = scope.ServiceProvider.GetRequiredService<IBotClientRepository>();
                return _repository.Read(i => true).ToList();
            }

        }

        public IList<BotClient> Read(Expression<Func<BotClient, bool>> where)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _repository = scope.ServiceProvider.GetRequiredService<IBotClientRepository>();
                return _repository.Read(where).ToList();
            }

        }

        public BotClient Read(object id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _repository = scope.ServiceProvider.GetRequiredService<IBotClientRepository>();
                return _repository.Read(i => i.Id == (int)id).FirstOrDefault();
            }

        }

        public void Delete(BotClient item)
        {
            this.Delete(new List<Joint.Data.Models.BotClient>() { item });
        }

        public void Delete(IList<BotClient> items)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _repository = scope.ServiceProvider.GetRequiredService<IBotClientRepository>();
                foreach (var item in items)
                {
                    var old = _repository.Read(i => i.Id == item.Id);

                    if (old.Any())
                    {
                        _repository.Delete(old.FirstOrDefault());
                    }
                }
                _repository.Save();
            }

        }

        public void Dispose()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _repository = scope.ServiceProvider.GetRequiredService<IBotClientRepository>();
                _repository.Dispose();
            }

        }
    }
}
