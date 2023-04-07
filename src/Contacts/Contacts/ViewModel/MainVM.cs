using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Contacts.Model;
using Contacts.Services;
using static System.Windows.Visibility;

namespace Contacts.ViewModel;

public class MainVm : INotifyPropertyChanged
{
    private Contact _selectedContact = new();

    private bool _isReadOnly = true;

    private bool _isEnabled = true;

    private Visibility _visibility = Hidden;

    private RelayCommand? _generateCommand;

    private RelayCommand? _addCommand;

    private RelayCommand<Contact>? _applyCommand;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<Contact> Contacts { get; } = new();

    public bool IsEnabled
    {
        get => _isEnabled;
        set => SetField(ref _isEnabled, value);
    }

    public Visibility Visibility
    {
        get => _visibility;
        set => SetField(ref _visibility, value);
    }

    public bool IsReadOnly
    {
        get => _isReadOnly;
        private set => SetField(ref _isReadOnly, value);
    }

    public Contact SelectedContact
    {
        get => _selectedContact;
        set => SetField(ref _selectedContact, value);
    }

    public RelayCommand GenerateCommand => _generateCommand ??= new RelayCommand(
        () =>
        {
            var contact = ContactFactory.RandomGenerate();
            if (contact == null) return;
            Contacts.Add(contact);
        }
    );

    public RelayCommand AddCommand => _addCommand ??= new RelayCommand(
        () =>
        {
            IsEnabled = false;
            IsReadOnly = false;
            Visibility = Visible;
            SelectedContact = new Contact();
        }
    );

    public RelayCommand<Contact> ApplyCommand => _applyCommand ??= new RelayCommand<Contact>(
        contact =>
        {
            if (contact == null) return;
            IsReadOnly = true;
            IsEnabled = true;
            Visibility = Hidden;
            Contacts.Add(contact);
        }
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