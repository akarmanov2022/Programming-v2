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