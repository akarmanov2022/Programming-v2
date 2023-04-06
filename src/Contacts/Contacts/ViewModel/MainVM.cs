using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using Contacts.Model;
using Contacts.Services;

namespace Contacts.ViewModel;

public class MainVm : INotifyPropertyChanged
{
    private Contact? _selectedContact;

    private bool _isReadOnly;

    private RelayCommand<Contact>? _generateCommand;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<Contact> Contacts { get; } = new();

    public bool IsReadOnly
    {
        get => _isReadOnly;
        set => SetField(ref _isReadOnly, value);
    }

    public Contact? SelectedContact
    {
        get => _selectedContact;
        set => SetField(ref _selectedContact, value);
    }

    public RelayCommand<Contact> GenerateCommand => _generateCommand ??= new RelayCommand<Contact>(
        contact  =>
        {
            var generate = ContactFactory.RandomGenerate();
            if (generate == null) return;
            Contacts.Add(generate);
        },
        contact => true
        );

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
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
}