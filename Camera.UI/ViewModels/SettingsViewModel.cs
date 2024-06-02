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
    public  class SettingsViewModel : RoutableViewModelBase
    {
        public SettingsViewModel(IScreen screen, 
            RoutingState routingState) : base(screen, routingState)
        {
        }
        private User _user { get; set; } = new User();

        public string Email
        {
            get => _user.Email;
            set
            {
                _user.Email = value;
                this.RaisePropertyChanged();
            }
        }

        public string Password
        {
            get => _user.Password;
            set
            {
                _user.Password = value;
                this.RaisePropertyChanged();
            }
        }
        [Reactive] public string RetryPassword { get; set; }

    }
}
