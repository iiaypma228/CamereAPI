using Camera.UI.Services;
using Joint.Data.Models;
using ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;

namespace Camera.UI.ViewModels
{
    public class LoginViewModel : RoutableViewModelBase, IValidatableViewModel
    {
        
        //ЗАВИСИМОТИ КОТОРЫЕ Я ПОЛУЧАЮ В КОНСТРУКТОРЕ(Microsoft.DependencyIjection САМ ЛОЖИТ СЕРВИСЫ В КОНСТРУКТОР!!!)
        #region -- Dependency --

        private RegistrationViewModel _registrationViewModel;

        private IAuthorizationService _service;

        private INotificationService _notificationService;

        private readonly IServiceCollection _serviceProvider;

        private HomeViewModel _homeViewModel;
        
        #endregion
        
        
        public LoginViewModel(IScreen screen, 
            RoutingState routingState, 
            RegistrationViewModel registrationViewModel, 
            IAuthorizationService service,
            INotificationService notificationService,
            IServiceCollection serviceProvider,
            HomeViewModel homeViewModel
            ) :
            base(screen, routingState)
        {
            _homeViewModel = homeViewModel;
            _serviceProvider = serviceProvider;
            _service = service;
            _notificationService = notificationService;
            _registrationViewModel = registrationViewModel;
            this.ValidationRule(
                viewModel => viewModel.User.Email,
                name => !string.IsNullOrWhiteSpace(name),
                "You must specify a valid name");
        }
        
        [Reactive] public ValidationContext ValidationContext { get; set; } = new ValidationContext();
        [Reactive] public User User { get; set; } = new User();
        
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
                _serviceProvider.TryAddSingleton<IScreen>(_homeViewModel);
                _serviceProvider.TryAddSingleton<RoutingState>(_homeViewModel.Router);
                this.RoutingState.Navigate.Execute(_homeViewModel);
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
