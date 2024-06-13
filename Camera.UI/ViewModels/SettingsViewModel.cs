using Camera.UI.Localize;
using Camera.UI.Services;
using Joint.Data.Models;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Camera.UI.ViewModels
{
    public class SettingsViewModel : RoutableViewModelBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRegistrationService _registrationService;
        private readonly INotificationService _notificationService;
        public SettingsViewModel(IScreen screen, RoutingState routingState, IConfiguration configuration,
            IRegistrationService registrationService,
            INotificationService notificationService) : base(screen, routingState)
        {
            _configuration = configuration;
            _registrationService = registrationService;
            _notificationService = notificationService;
            //LocalizeDeffault = new KeyValuePair<string, string>(_configuration["Localize"], Localize[_configuration["Localize"]]);
            //this.ConfigureValidation();
            LoadCurrentLocale();
            this.WhenAnyValue(x => x.Email).Skip(1).Take(1)
                .Subscribe(i => this.ValidationRule(x => x.Email, v => v != null && Regex.Match(v, "^[\\w\\.-]+@[a-zA-Z\\d\\.-]+\\.[a-zA-Z]{2,}$").Success, Resources.textEmailNotTemplate));
            this.WhenAnyValue(x => x.Password).Skip(1).Take(1).Subscribe(i => this.ValidationRule(x => x.Password, v => !string.IsNullOrEmpty(v), Resources.textPasswordIsRequired));
            this.WhenAnyValue(x => x.RetryPassword).Skip(1).Take(1).Subscribe(i =>
            {
                this.ValidationRule(x => x.RetryPassword, v => !string.IsNullOrEmpty(v), Resources.textPasswordIsRequired);
                this.ValidationRule(x => x.RetryPassword, v => v == Password, Resources.textPasswordNotEqualsRetryPassowrd);
            });        
        }
        [Reactive] public Dictionary<string, string> Localize { get; set; } = new Dictionary<string, string> { { "uk", "Українська" }, { "en", "English (Англійська)" } };

        [Reactive] public User User { get; set; }

        public string Email
        {
            get =>  User?.Email;
            set
            {
                User.Email = value;
                this.RaisePropertyChanged();
            }
        }

        public string Password
        {
            get => User?.Password;
            set
            {
                User.Password = value;
                this.RaisePropertyChanged();
            }
        }
        
        public bool TelegramVerified
        {
            get => User?.TelegramVerified ?? false;
            set
            {
                User.TelegramVerified = value;
                this.RaisePropertyChanged();
            }
        }
        

        [Reactive] public string RetryPassword { get; set; }

        public async void LoadUser()
        {
            var d = (await _registrationService.GetMe()).Data;
            d.Password = "";
            User = d;
            Email = d.Email;
            TelegramVerified = d.TelegramVerified;
        }

        public void OpenTG()
        {
            var url = "https://t.me/OlegCameraNotification_bot";
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        
        public KeyValuePair<string, string> LocalizeDeffault
        {
            get => _localizeDeffault ?? new KeyValuePair<string, string>();
            set
            {
                if (_localizeDeffault != null)
                {
                    CurrentLocale(value.Key);
                }
                _localizeDeffault = value;
                this.RaisePropertyChanged();

            }
        }
        private KeyValuePair<string, string>? _localizeDeffault = null;

        public async void ChangeEmail()
        {
            ConfigureValidation();
            if (HasErrors)
            {
                _notificationService.ShowError("Форма не валідна!");
                return;
            }

            User.Email = Email;
            User.Password = Password;

            var res = await _registrationService.ChangeUser(User);
            if (!res.IsSuccess)
            {
                _notificationService.ShowError(res.Error);
            }
            else
            {
                _notificationService.ShowInfo("");
            }
        }
        
        private void CurrentLocale(string newLocale)
        {
            UpdateAppSetting("Localize", newLocale);
        }

        private void LoadCurrentLocale()
        {
            var code = _configuration["Localize"];

            if (code == "en")
            {
                LocalizeDeffault = new KeyValuePair<string, string>( "en", "English (Англійська)" );
            }
            else
            {
                LocalizeDeffault = new KeyValuePair<string, string>("uk", "Українська");
            }
            
            
        }
        
        private void UpdateAppSetting(string key, string value)
        {
            var configJson = File.ReadAllText("appsettings.json");
            var config = JsonSerializer.Deserialize<Dictionary<string, object>>(configJson);
            config[key] = value;
            var updatedConfigJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("appsettings.json", updatedConfigJson);
        }
        
        private void ConfigureValidation()
        {
            if (ValidationContext.Validations.Count < 4)
            {
                this.ValidationRule(x => x.Email, v => v != null && Regex.Match(v, "^[\\w\\.-]+@[a-zA-Z\\d\\.-]+\\.[a-zA-Z]{2,}$").Success, Resources.textEmailNotTemplate);
                this.ValidationRule(x => x.Password, v => !string.IsNullOrEmpty(v), Resources.textPasswordIsRequired);
                this.ValidationRule(x => x.RetryPassword, v => !string.IsNullOrEmpty(v), Resources.textPasswordIsRequired);
                this.ValidationRule(x => x.RetryPassword, v => v == Password, Resources.textPasswordNotEqualsRetryPassowrd);

            }
        }
    }
}
