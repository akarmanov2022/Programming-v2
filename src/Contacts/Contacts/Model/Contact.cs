using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Contacts.Model;

/// <summary>
/// Определяет контакт.
/// </summary>
public sealed class Contact : ObservableValidator, ICloneable
{
    /// <summary>
    /// Хранит имя.
    /// </summary>
    private string? _firstName;

    /// <summary>
    /// Хранит фамилию.
    /// </summary>
    private string? _lastName;

    /// <summary>
    /// Хранит адрес электронной почты.
    /// </summary>
    private string? _email;

    /// <summary>
    /// Хранит номер телефона.
    /// </summary>
    private string? _phone;

    public Contact()
    {
        FirstName = "";
        LastName = "";
        Phone = "";
        Email = "";
    }

    /// <summary>
    /// Устанавливает и возвращает имя.
    /// </summary>
    public string? FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value, validate: true);
    }

    /// <summary>
    /// Устанавливает и возвращает фамилию.
    /// </summary>
    [MaxLength(100)]
    [Required(AllowEmptyStrings = false)]
    public string? LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value, validate: true);
    }

    /// <summary>
    /// Устонавливает и возвращает адрес электронной почты.
    /// </summary>
    public string? Email
    {
        get => _email;
        set => SetProperty(ref _email, value, validate: true);
    }

    /// <summary>
    /// Устанавливает и возвращает номер телефона.
    /// </summary>
    public string? Phone
    {
        get => _phone;
        set => SetProperty(ref _phone, value, validate: true);
    }

    public object Clone()
    {
        return new Contact
        {
            _firstName = LastName,
            FirstName = FirstName,
            Phone = Phone,
            Email = Email
        };
    }
}