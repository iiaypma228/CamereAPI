using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Camera.UI.Test.Extensions;
using Camera.UI.Views;

namespace Camera.UI.Test;

public class TestLogin : BaseTest
{
    [Fact]
    public async Task TestDoLogin() 
    {
        // wait for initial setup
        await Task.Delay(100);

        var loginView = GetWindowContentAs<LoginView>();
        
        var email = loginView.Find<TextBox>("EmailTextBox");
        email.SendText("123@gmail.com");
        var password = loginView.Find<TextBox>("PasswordTextBox");
        
        password.SendText("123");

        var loginButton = loginView.Find<Button>("LoginButton");
        
        loginButton.Command.Execute(null);
        await Task.Delay(2000);
    }
    
    [Fact]
    public async Task TestDoRegistration()
    {
        
        var loginView = GetWindowContentAs<LoginView>();
        
        loginView.Find<Button>("GoToRegistration").Command.Execute(null);
        await Task.Delay(100);

        var registrView = GetWindowContentAs<RegistrationView>();
        
        registrView.Find<TextBox>("EmailTextBox").SendText("oleg@gmail.com");
        registrView.Find<TextBox>("PasswordTextBox").SendText("123");
        registrView.Find<TextBox>("RetryPasswordTextBox").SendText("123");

        registrView.Find<Button>("RegistrationButton").Command.Execute(null);
        await Task.Delay(2000);
    }
}