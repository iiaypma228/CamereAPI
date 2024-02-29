using System;
using System.ComponentModel;
using Joint.Data.Models;

namespace Camera.UI.Validators;

public class UserValidator : IDataErrorInfo
{
    private readonly User _user;

    public UserValidator(User user)
    {
        _user = user ?? throw new ArgumentNullException(nameof(user));
    }

    public string Email
    {
        get => _user.Email;
        set => _user.Email = value;
    }

    public string Password
    {
        get => _user.Password;
        set => _user.Password = value;
    }

    public string this[string columnName]
    {
        get
        {
            switch (columnName)
            {
                case nameof(Email):
                    // Добавьте правила валидации для Email
                    if (string.IsNullOrEmpty(Email))
                    {
                        return "Email is required.";
                    }
                    // Дополнительные проверки для Email, например, формат адреса электронной почты
                    // ...

                    break;

                case nameof(Password):
                    // Добавьте правила валидации для Password
                    if (string.IsNullOrEmpty(Password))
                    {
                        return "Password is required.";
                    }
                    // Дополнительные проверки для Password, например, минимальная длина
                    // ...

                    break;

                // Добавьте дополнительные варианты для других полей, если необходимо

                default:
                    return null;
            }

            return null;
        }
    }

    public string Error => null;
}