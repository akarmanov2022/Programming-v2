using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Contacts.Model;

namespace Contacts.ViewModel;

/// <summary>
/// Модель представления контакта.
/// </summary>
public class ContactViewModel : ObservableValidator
{
    /// <summary>
    /// Хранит контакт.
    /// </summary>
    private readonly Contact? _contact;

    /// <summary>
    /// Хранит значение, указывающее, что поля доступны только для чтения.
    /// </summary>
    private bool _readOnly = true;


    /// <inheritdoc />
    public ContactViewModel(Contact? contact)
    {
        _contact = contact;
        ValidateAllProperties();
    }
    
    /// <inheritdoc />
    public ContactViewModel()
    {
        _contact = null;
    }

    /// <summary>
    /// Устанавливает и возвращает значение Имени контакта.
    /// </summary>
    [MaxLength(100)]
    [Required(AllowEmptyStrings = false)]
    public string? Name
    {
        get => _contact?.LastName;
        set
        {
            if (_contact == null) return;
            ValidateProperty(value);
            _contact.LastName = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Устанавливает и возвращает значение номера телефона контакта.
    /// </summary>
    [MaxLength(100)]
    [Required(AllowEmptyStrings = false)]
    [Phone(ErrorMessage = "Phone Number can contains only digits and symbols '+()- '. Example: +7 (999) 111-22-33")]
    public string? PhoneNumber
    {
        get => _contact?.Phone;
        set
        {
            if (_contact == null) return;
            ValidateProperty(value);
            _contact.Phone = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Устанавливает и возвращает значение адреса электронной почты контакта.
    /// </summary>
    [EmailAddress]
    [MaxLength(100)]
    [Required(AllowEmptyStrings = false)]
    public string? Email
    {
        get => _contact?.Email;
        set
        {
            if (_contact == null) return;
            ValidateProperty(value);
            _contact.Email = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Устанавливает и возвращает значение, указывающее, что поля доступны только для чтения.
    /// </summary>
    public bool ReadOnly
    {
        get => _readOnly;
        set => SetProperty(ref _readOnly, value);
    }
}