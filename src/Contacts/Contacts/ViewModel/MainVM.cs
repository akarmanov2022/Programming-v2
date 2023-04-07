using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using CommunityToolkit.Mvvm.Input;
using Contacts.Model;
using Contacts.Services;
using static System.Windows.Visibility;

namespace Contacts.ViewModel;

public class MainVm : INotifyPropertyChanged
{
    private Contact _selectedContact = new();

    private bool _isReadOnly = true;

    private bool _isAddEnabled = true;

    private bool _isEditEnabled;

    private bool _isRemoveEnabled;

    private Visibility _applyVisibility = Hidden;

    private RelayCommand? _generateCommand;

    private RelayCommand? _addCommand;

    private RelayCommand? _editCommand;

    private RelayCommand? _applyCommand;

    private RelayCommand? _removeCommand;

    private RelayCommand? _closeWindowCommand;

    public MainVm(ObservableCollection<Contact> contacts)
    {
        Contacts = contacts;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<Contact> Contacts { get; }

    public Visibility ApplyVisibility
    {
        get => _applyVisibility;
        set => SetField(ref _applyVisibility, value);
    }

    public bool IsEditing { get; set; }

    public bool IsAddEnabled
    {
        get => _isAddEnabled;
        set => SetField(ref _isAddEnabled, value);
    }

    public bool IsReadOnly
    {
        get => _isReadOnly;
        private set => SetField(ref _isReadOnly, value);
    }

    public bool IsEditEnabled
    {
        get => _isEditEnabled;
        set => SetField(ref _isEditEnabled, value);
    }

    public bool IsRemoveEnabled
    {
        get => _isRemoveEnabled;
        set => SetField(ref _isRemoveEnabled, value);
    }

    public Contact SelectedContact
    {
        get => _selectedContact;
        set
        {
            SetField(ref _selectedContact, value);
            ApplyVisibility = Hidden;
            IsReadOnly = true;
            IsAddEnabled = true;
            IsRemoveEnabled = true;
            IsEditEnabled = true;
        }
    }

    public RelayCommand GenerateCommand => _generateCommand ??= new RelayCommand(() =>
        {
            var contact = ContactFactory.RandomGenerate();
            if (contact == null) return;
            Contacts.Add(contact);
        }
    );

    public RelayCommand AddCommand => _addCommand ??= new RelayCommand(() =>
        {
            SelectedContact = new Contact();
            ApplyVisibility = Visible;
            IsAddEnabled = false;
            IsEditEnabled = false;
            IsRemoveEnabled = false;
            IsReadOnly = false;
        }
    );

    public RelayCommand ApplyCommand => _applyCommand ??= new RelayCommand(() =>
        {
            IsReadOnly = true;
            IsAddEnabled = true;
            ApplyVisibility = Hidden;

            if (IsEditing) return;
            Contacts.Add(SelectedContact);
        }
    );

    public RelayCommand EditCommand => _editCommand ??= new RelayCommand(() =>
        {
            ApplyVisibility = Visible;
            IsAddEnabled = false;
            IsEditEnabled = false;
            IsRemoveEnabled = false;
            IsReadOnly = false;
            IsEditing = true;
        }
    );

    public RelayCommand RemoveCommand => _removeCommand ??= new RelayCommand(() =>
        {
            Contacts.Remove(SelectedContact);
            SelectedContact = new Contact();
            if (Contacts.Count != 0) return;
            IsEditEnabled = false;
            IsRemoveEnabled = false;
        }
    );

    public RelayCommand CloseWindowCommand => _closeWindowCommand ??= new RelayCommand(() =>
        Serializer<ObservableCollection<Contact>>.ToJson(Contacts, App.DefaultSavePath));

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