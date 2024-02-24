using Camera.UI.Services;
using Joint.Data.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera.UI.ViewModels
{
    public class RegistrationViewModel :RoutableViewModelBase
    {
        private readonly IRegistrationService _service;
        private readonly INotificationService _notificationService;
        
        
        public RegistrationViewModel(IScreen screen, 
            RoutingState routingState, 
            IRegistrationService registrationService,
            INotificationService notificationService)
            : base(screen, routingState)
        {
            _service = registrationService;
            _notificationService = notificationService;
        }

        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _user, value);
            }
        }
        private User _user = new User();

        //Event
        public async void Registration()
        {
            var res = await _service.Registration(User);

            if (res.IsSuccess)
            {
                //TODO LOGIG OLEG!!!!
            }
            else
            {
                _notificationService.ShowError(res.Error);
            }
            
        }

    }
}
