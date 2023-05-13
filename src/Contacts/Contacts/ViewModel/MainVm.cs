using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using Contacts.Model;
using Contacts.Services;

namespace Contacts.ViewModel;

/// <summary>
/// Определяет модель представления главного окна.
/// </summary>
public sealed class MainVm : INotifyPropertyChanged
{
    /// <summary>
    /// Хранит выбранный контакт.
    /// </summary>
    private Contact? _selectedContact;

    /// <summary>
    /// Хранит значение, указывающее, что поля доступны только для чтения.
    /// </summary>
    private bool _readOnly = true;
    
    /// <summary>
    /// Хранит значение, указывающее, что выбран контакт из списка.
    /// </summary>
    private bool _selecting;

    /// <summary>
    /// Хранит команду генерации случайного контакта.
    /// </summary>
    private RelayCommand? _generateCommand;

    /// <summary>
    /// Хранит команду добавления контакта.
    /// </summary>
    private RelayCommand? _addCommand;

    /// <summary>
    /// Хранит команду редактирования контакта.
    /// </summary>
    private RelayCommand? _editCommand;

    /// <summary>
    /// Хранит команду применения изменений.
    /// </summary>
    private RelayCommand? _applyCommand;

    /// <summary>
    /// Хранит команду удаления контакта.
    /// </summary>
    private RelayCommand? _removeCommand;

    /// <summary>
    /// Хранит команду закрытия окна.
    /// </summary>
    private RelayCommand? _closeWindowCommand;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="MainVm"/>.
    /// </summary>
    /// <param name="contacts">Коллекция контактов.</param>
    public MainVm(ObservableCollection<Contact> contacts)
    {
        Contacts = contacts;
    }

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public MainVm()
    {
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Возвращает коллекцию контактов.
    /// </summary>
    public ObservableCollection<Contact> Contacts { get; } = null!;

    /// <summary>
    /// Устанавливает и возвращает значение, указывающее, что происходит редактирование контакта.
    /// </summary>
    private bool Editing { get; set; }

    /// <summary>
    /// Устанавливает и возвращает значение, указывающее, что выбран контакт.
    /// </summary>
    public bool Selecting
    {
        get => _selecting;
        set => SetField(ref _selecting, value);
    }

    /// <summary>
    /// Устанавливает и возвращает значение, указывающее, что поля доступны только для чтения.
    /// </summary>
    public bool ReadOnly
    {
        get => _readOnly;
        private set => SetField(ref _readOnly, value);
    }

    /// <summary>
    /// Устанавливает и возвращает значение выбранного контакта.
    /// </summary>
    public Contact? SelectedContact
    {
        get => _selectedContact;
        set
        {
            SetField(ref _selectedContact, value);
            ReadOnly = true;
            Selecting = true;
        }
    }

    /// <summary>
    /// Возвращает команду генерации случайного контакта.
    /// </summary>
    public RelayCommand GenerateCommand => _generateCommand ??= new RelayCommand(() =>
        {
            var contact = ContactFactory.RandomContact();
            if (contact == null) return;
            Contacts.Add(contact);
        }
    );

    /// <summary>
    /// Возвращает команду добавления контакта.
    /// </summary>
    public RelayCommand AddCommand => _addCommand ??= new RelayCommand(() =>
        {
            SelectedContact = new Contact();
            ReadOnly = false;
            Editing = false;
            Selecting = false;
        }
    );

    /// <summary>
    /// Возвращает команду применения изменений.
    /// </summary>
    public RelayCommand ApplyCommand => _applyCommand ??= new RelayCommand(() =>
        {
            ReadOnly = true;
            Selecting = true;

            if (SelectedContact != null && !Editing)
            {
                Contacts.Add(SelectedContact);
            }
        }
    );

    /// <summary>
    /// Возвращает команду редактирования контакта.
    /// </summary>
    public RelayCommand EditCommand => _editCommand ??= new RelayCommand(() =>
        {
            ReadOnly = false;
            Editing = true;
            Selecting = false;
        }
    );

    /// <summary>
    /// Возвращает команду удаления контакта.
    /// </summary>
    public RelayCommand RemoveCommand => _removeCommand ??= new RelayCommand(() =>
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
            }
        }
    );

    /// <summary>
    /// Возвращает команду закрытия окна.
    /// </summary>
    public RelayCommand CloseWindowCommand => _closeWindowCommand ??= new RelayCommand(() =>
        Serializer<ObservableCollection<Contact>>.ToJson(Contacts, App.DefaultSavePath));

    /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Устанавливает значение поля и вызывает событие <see cref="PropertyChanged"/>.
    /// </summary>
    /// <param name="field">Ссылка на поле.</param>
    /// <param name="value">Значение поля.</param>
    /// <param name="propertyName">Имя свойства.</param>
    /// <typeparam name="T">Тип поля.</typeparam>
    /// <returns>Возвращает <see langword="true"/>, если значение поля было изменено.</returns>
    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}