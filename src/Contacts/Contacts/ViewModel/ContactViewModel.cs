using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using Contacts.Model;

namespace Contacts.ViewModel;

public class ContactViewModel : ObservableValidator
{
    private readonly Contact _contact;

    private bool _readOnly = true;


    public ContactViewModel(Contact contact)
    {
        _contact = contact;
        ValidateAllProperties();
    }

    [MaxLength(100)]
    [Required(AllowEmptyStrings = false)]
    public string Name
    {
        get => _contact.LastName;
        set
        {
            _contact.LastName = value;
            ValidateProperty(value);
            OnPropertyChanged();
        }
    }

    [MaxLength(100)]
    [Required(AllowEmptyStrings = false)]
    [Phone(ErrorMessage = "Phone Number can contains only digits and symbols '+()- '. Example: +7 (999) 111-22-33")]
    public string PhoneNumber
    {
        get => _contact.Phone;
        set
        {
            _contact.Phone = value;
            ValidateProperty(value);
            OnPropertyChanged();
        }
    }

    [EmailAddress]
    [MaxLength(100)]
    [Required(AllowEmptyStrings = false)]
    public string Email
    {
        get => _contact.Email;
        set
        {
            _contact.Email = value;
            ValidateProperty(value);
            OnPropertyChanged();
        }
    }

    public bool ReadOnly
    {
        get => _readOnly;
        set => SetProperty(ref _readOnly, value);
    }
}