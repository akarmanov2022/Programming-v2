using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contacts.Model;
using Contacts.Services;

namespace Contacts.ViewModel;

/// <summary>
/// Определяет модель представления главного окна.
/// </summary>
public sealed class MainViewModel : ObservableObject
{
    /// <summary>
    /// Хранит значение, указывающее, что поля доступны только для чтения.
    /// </summary>
    private bool _readOnly = true;

    /// <summary>
    /// Хранит значение, указывающее, что выбран контакт из списка.
    /// </summary>
    private bool _selecting;

    /// <summary>
    /// Хранит выбранный контакт.
    /// </summary>
    private Contact? _selectedContact;

    private Contact? _copyContact;

    private ContactViewModel _contactViewModel = new();

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MainViewModel"/>.
    /// </summary>
    /// <param name="contacts">Коллекция контактов.</param>
    public MainViewModel(ObservableCollection<Contact> contacts)
    {
        Contacts = contacts;

        GenerateCommand = new RelayCommand(GenerateCommandExecute);
        AddCommand = new RelayCommand(AddCommandExecute);
        CloseWindowCommand = new RelayCommand(CloseWindowCommandExecute);
        EditCommand = new RelayCommand(EditCommandExecute);
        RemoveCommand = new RelayCommand(RemoveCommandExecute);
        ApplyCommand = new RelayCommand(ApplyCommandExecute);
    }

    /// <summary>
    /// Возвращает коллекцию контактов.
    /// </summary>
    public ObservableCollection<Contact> Contacts { get; }


    /// <summary>
    /// Устанавливает и возвращает значение, указывающее, что выбран контакт.
    /// </summary>
    public bool Selecting
    {
        get => _selecting;
        set => SetProperty(ref _selecting, value);
    }

    /// <summary>
    /// Устанавливает и возвращает значение, указывающее, что поля доступны только для чтения.
    /// </summary>
    public bool ReadOnly
    {
        get => _readOnly;
        private set => SetProperty(ref _readOnly, value);
    }

    /// <summary>
    /// Устанавливает и возвращает значение выбранного контакта.
    /// </summary>
    public Contact? SelectedContact
    {
        get => _selectedContact;
        set
        {
            ReadOnly = true;
            Selecting = true;
            RestoreContact();

            if (!SetProperty(ref _selectedContact, value)) return;
            if (_selectedContact != null) ContactViewModel = new ContactViewModel(_selectedContact);
        }
    }

    public ContactViewModel ContactViewModel
    {
        get => _contactViewModel;
        private set => SetProperty(ref _contactViewModel, value);
    }

    /// <summary>
    /// Возвращает команду генерации случайного контакта.
    /// </summary>
    public RelayCommand? GenerateCommand { get; }

    /// <summary>
    /// Возвращает команду добавления контакта.
    /// </summary>
    public RelayCommand? AddCommand { get; }

    /// <summary>
    /// Возвращает команду применения изменений.
    /// </summary>
    public RelayCommand? ApplyCommand { get; }

    /// <summary>
    /// Возвращает команду редактирования контакта.
    /// </summary>
    public RelayCommand? EditCommand { get; }

    /// <summary>
    /// Возвращает команду удаления контакта.
    /// </summary>
    public RelayCommand? RemoveCommand { get; }

    /// <summary>
    /// Возвращает команду закрытия окна.
    /// </summary>
    public RelayCommand? CloseWindowCommand { get; }

    private void RemoveCommandExecute()
    {
        if (SelectedContact == null) return;
        var indexOf = Contacts.IndexOf(SelectedContact);
        Contacts.Remove(SelectedContact);
        Selecting = indexOf < Contacts.Count;
        if (indexOf < Contacts.Count)
        {
            SelectedContact = Contacts[indexOf];
        }
        else
        {
            Selecting = false;
            ContactViewModel = new ContactViewModel();
        }
    }

    private void EditCommandExecute()
    {
        _copyContact = SelectedContact?.Clone() as Contact;
        ContactViewModel.ReadOnly = false;
        ReadOnly = false;
        Selecting = false;
    }

    private void CloseWindowCommandExecute()
    {
        RestoreContact();
        Serializer<ObservableCollection<Contact>>.ToJson(Contacts, App.DefaultSavePath);
    }

    private void ApplyCommandExecute()
    {
        ContactViewModel.ReadOnly = true;
        ReadOnly = true;
        Selecting = true;

        if (_copyContact == null)
        {
            if (SelectedContact == null) return;
            Contacts.Add(SelectedContact);
        }
        else
        {
            _copyContact = null;
        }
    }

    private void AddCommandExecute()
    {
        SelectedContact = new Contact();
        ContactViewModel.ReadOnly = false;
        ReadOnly = false;
        Selecting = false;
    }

    private void GenerateCommandExecute()
    {
        var contact = ContactFactory.RandomContact();
        if (contact == null) return;
        Contacts.Add(contact);
    }

    private void RestoreContact()
    {
        if (_copyContact == null) return;
        if (SelectedContact != null)
        {
            var indexOf = Contacts.IndexOf(SelectedContact);
            Contacts[indexOf] = _copyContact;
        }

        _copyContact = null;
    }
}