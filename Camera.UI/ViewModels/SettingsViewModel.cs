using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Camera.UI.ViewModels
{
    public partial class SettingsViewModel : RoutableViewModelBase
    {
        private readonly IConfiguration _configuration;
        public SettingsViewModel(IScreen screen, RoutingState routingState, IConfiguration configuration) : base(screen, routingState)
        {
            _configuration = configuration;
        }
    }
}
