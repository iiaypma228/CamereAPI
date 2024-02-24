using Camera.UI.Services;
using Joint.Data.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI.Fody.Helpers;

namespace Camera.UI.ViewModels
{
    public class LoginViewModel : RoutableViewModelBase
    {
        //ТАКАЯ ЗАПИСЬ С АТРИБУТОМ РЕАКТИВ САМА ГЕНЕРИРУЕТ КОД С НУЖНЫМИ ГЕТТЕРАМИ И СЕТТЕРАМИ КАК МЫ ДЕЛАЛИ
        [Reactive] public User User { get; set; } = new User();

        //ЗАВИСИМОТИ КОТОРЫЕ Я ПОЛУЧАЮ В КОНСТРУКТОРЕ(Microsoft.DependencyIjection САМ ЛОЖИТ СЕРВИСЫ В КОНСТРУКТОР!!!)
        #region -- Dependency --

        private RegistrationViewModel _registrationViewModel;

        private IAuthorizationService _service;

        private INotificationService _notificationService;

        #endregion
        
        
        public LoginViewModel(IScreen screen, 
            RoutingState routingState, 
            RegistrationViewModel registrationViewModel, 
            IAuthorizationService service,
            INotificationService notificationService
            ) :
            base(screen, routingState)
        {
            _service = service;
            _notificationService = notificationService;
            _registrationViewModel = registrationViewModel;
        }
        
        //Events
        public async void Login()
        {
            //_notificationService.ShowInfo("dasdas");

            //МЕТОД ОБЯЗАТЕЛЬНО ДОЛЖЕН БЫТЬ АСИНХРОННЫМ, ЧТО БЫ МЕТОД ЗАПУСКАЛСЯ В ОТЛДЕЛЬНОМ ПОТОКЕ
            //И НАША ГРАФИКА(ОКНО) НЕ БЫЛО ЗАМОРОЖЕНО ПОТОМУ ЧТО ДРУГОЙ МЕТОД РАБОТАЕТ
            var res = await _service.AuthorizationAsync(User);

            if (res.IsSuccess)
            {
                //TODO LOGIC OLEG!!!
            }
            else
            {
                _notificationService.ShowError(res.Error);
            }
            
        }

        //ЛЯМБДА СОКРАЩЕННАЯ ЗАПИСЬ МЕТОДА, ВМЕСТО 4 СТРОК ОДНА!!
        public void GoToRegistration() => this.RoutingState.Navigate.Execute(_registrationViewModel);
        
    }
}
