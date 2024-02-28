using Camera.UI.Services;
using Joint.Data.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera.UI.ViewModels
{
    public class RegistrationViewModel : RoutableViewModelBase
    {
        [Reactive] public User User { get; set; } = new User();

        private readonly IRegistrationService _service;
        private readonly INotificationService _notificationService;
        //private LoginViewModel _loginViewModel;


        public RegistrationViewModel(IScreen screen, 
            RoutingState routingState,
            //LoginViewModel loginViewModel,
            IRegistrationService registrationService,
            INotificationService notificationService)
            : base(screen, routingState)
        {
            _service = registrationService;
            _notificationService = notificationService;
            //_loginViewModel = loginViewModel;
        }


        //Event
        public async void Registration()
        {
            var res = await _service.Registration(User);

            if (res.IsSuccess)
            {
                _notificationService.ShowInfo("Ви зареєструвалися!");
                GoToLogin();
            }
            else
            {
                _notificationService.ShowError(res.Error);
            }
            
        }
        public void GoToLogin() => this.RoutingState.NavigateBack.Execute();

    }
}
