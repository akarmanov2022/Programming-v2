using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Contacts.Model;

public sealed class Contact : INotifyPropertyChanged
{
    private string _firstName = "";

    private string _lastName = "";

    private string _email = "";

    private string _phone = "";

    private Contact(Contact contact)
    {
        FirstName = contact.FirstName;
        LastName = contact.LastName;
        Email = contact.Email;
        Phone = contact.Phone;
    }

    public Contact()
    {
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string FirstName
    {
        get => _firstName;
        set => SetField(ref _firstName, value);
    }

    public string LastName
    {
        get => _lastName;
        set => SetField(ref _lastName, value);
    }

    public string Email
    {
        get => _email;
        set => SetField(ref _email, value);
    }

    public string Phone
    {
        get => _phone;
        set => SetField(ref _phone, value);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public static Contact CopyOf(Contact contact)
    {
        return new Contact(contact);
    }
}