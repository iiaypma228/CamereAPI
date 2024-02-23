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
    public class RegistrationViewModel : ViewModelBase, IRoutableViewModel
    {
        public RegistrationViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
        public IScreen HostScreen { get; }
        public RoutingState Router { get; } = new RoutingState();

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
        public void Registration()
        {
            var service = new RegistrationService();
            service.Registration(User);
        }

    }
}
