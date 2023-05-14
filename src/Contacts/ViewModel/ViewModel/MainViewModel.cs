using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Contacts.Model;
using Model.Services;

namespace ViewModel.ViewModel;

/// <summary>
/// Определяет модель представления главного окна.
/// </summary>
public sealed partial class MainViewModel : ObservableObject
{
    /// <summary>
    /// Хранит значение, указывающее, что поля доступны только для чтения.
    /// </summary>
    [ObservableProperty]
    private bool _readOnly = true;

    /// <summary>
    /// Хранит значение, указывающее, что выбран контакт из списка.
    /// </summary>
    [ObservableProperty]
    private bool _selecting;

    /// <summary>
    /// Хранит копию контакта.
    /// </summary>
    [ObservableProperty]
    private Contact? _copyContact;

    /// <summary>
    /// Хранит выбранный контакт.
    /// </summary>
    private Contact? _selectedContact;

    /// <summary>
    /// Хранит экземпляр класса <see cref="ContactViewModel"/>.
    /// </summary>
    [ObservableProperty]
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

    /// <summary>
    /// Выполняет команду удаления контакта.
    /// </summary>
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

    /// <summary>
    /// Выполняет команду редактирования контакта.
    /// </summary>
    private void EditCommandExecute()
    {
        CopyContact = SelectedContact?.Clone() as Contact;
        ContactViewModel.ReadOnly = false;
        ReadOnly = false;
        Selecting = false;
    }

    /// <summary>
    /// Выполняет команду закрытия окна.
    /// </summary>
    private void CloseWindowCommandExecute()
    {
        RestoreContact();
        Serializer<ObservableCollection<Contact>>.ToJson(Contacts);
    }

    /// <summary>
    /// Выполняет команду применения изменений.
    /// </summary>
    private void ApplyCommandExecute()
    {
        ContactViewModel.ReadOnly = true;
        ReadOnly = true;
        Selecting = true;

        if (CopyContact == null)
        {
            if (SelectedContact == null) return;
            Contacts.Add(SelectedContact);
        }
        else
        {
            CopyContact = null;
        }
    }

    /// <summary>
    /// Выполняет команду добавления контакта.
    /// </summary>
    private void AddCommandExecute()
    {
        SelectedContact = new Contact();
        ContactViewModel.ReadOnly = false;
        ReadOnly = false;
        Selecting = false;
    }

    /// <summary>
    /// Выполняет команду генерации случайного контакта.
    /// </summary>
    private void GenerateCommandExecute()
    {
        var contact = ContactFactory.RandomContact();
        if (contact == null) return;
        Contacts.Add(contact);
    }

    /// <summary>
    /// Сбрасывает изменения.
    /// </summary>
    private void RestoreContact()
    {
        if (CopyContact == null) return;
        if (SelectedContact != null)
        {
            var indexOf = Contacts.IndexOf(SelectedContact);
            Contacts[indexOf] = CopyContact;
        }

        CopyContact = null;
    }
}