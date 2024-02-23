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
    public class LoginViewModel : ViewModelBase, IRoutableViewModel
    {
        public LoginViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
        public IScreen HostScreen { get; }
        public RoutingState Router { get; } = new RoutingState();

        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

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

       


        //Events
        public void Login()
        {
            var service = new AuthorizationService();
            service.Authorization(User);
        }
        public void GoToRegistration()
        {
            ViewLocator.Router.Navigate.Execute(new RegistrationViewModel(ViewLocator.Screen));
        }
    }
}
