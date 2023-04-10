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

/// <summary>
/// Определяет модель представления главного окна.
/// </summary>
public class MainVm : INotifyPropertyChanged
{
    /// <summary>
    /// Хранит выбранный контакт.
    /// </summary>
    private Contact _selectedContact = new();

    /// <summary>
    /// Хранит значение, указывающее, что поля доступны только для чтения.
    /// </summary>
    private bool _readOnly = true;

    /// <summary>
    /// Хранит значение, указывающее, что кнопка добавления доступна.
    /// </summary>
    private bool _addEnabled = true;

    /// <summary>
    /// Хранит значение, указывающее, что кнопка редактирования доступна.
    /// </summary>
    private bool _editEnabled;

    /// <summary>
    /// Хранит значение, указывающее, что кнопка удаления доступна.
    /// </summary>
    private bool _removeEnabled;

    /// <summary>
    /// Хранит значение, указывающее, что кнопка применения изменений доступна.
    /// </summary>
    private Visibility _applyVisibility = Hidden;

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

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Возвращает коллекцию контактов.
    /// </summary>
    public ObservableCollection<Contact> Contacts { get; }

    /// <summary>
    /// Устанавливает и возвращает значение, указывающее, что кнопка применения изменений доступна.
    /// </summary>
    public Visibility ApplyVisibility
    {
        get => _applyVisibility;
        private set => SetField(ref _applyVisibility, value);
    }

    /// <summary>
    /// Устанавливает и возвращает значение, указывающее, что происходит редактирование контакта.
    /// </summary>
    private bool Editing { get; set; }

    /// <summary>
    /// Устанавливает и возвращает значение, указывающее, что поля доступны только для чтения.
    /// </summary>
    public bool AddEnabled
    {
        get => _addEnabled;
        private set => SetField(ref _addEnabled, value);
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
    /// Устанавливает и возвращает значение, указывающее, что кнопка редактирования доступна.
    /// </summary>
    public bool EditEnabled
    {
        get => _editEnabled;
        private set => SetField(ref _editEnabled, value);
    }

    /// <summary>
    /// Устанавливает и возвращает значение, указывающее, что кнопка удаления доступна.
    /// </summary>
    public bool RemoveEnabled
    {
        get => _removeEnabled;
        private set => SetField(ref _removeEnabled, value);
    }

    /// <summary>
    /// Устанавливает и возвращает значение выбранного контакта.
    /// </summary>
    public Contact SelectedContact
    {
        get => _selectedContact;
        set
        {
            SetField(ref _selectedContact, value);
            ApplyVisibility = Hidden;
            ReadOnly = true;
            AddEnabled = true;
            RemoveEnabled = true;
            EditEnabled = true;
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
            ApplyVisibility = Visible;
            AddEnabled = false;
            EditEnabled = false;
            RemoveEnabled = false;
            ReadOnly = false;
        }
    );

    /// <summary>
    /// Возвращает команду применения изменений.
    /// </summary>
    public RelayCommand ApplyCommand => _applyCommand ??= new RelayCommand(() =>
        {
            ReadOnly = true;
            AddEnabled = true;
            ApplyVisibility = Hidden;

            if (Editing) return;
            Contacts.Add(SelectedContact);
        }
    );

    /// <summary>
    /// Возвращает команду редактирования контакта.
    /// </summary>
    public RelayCommand EditCommand => _editCommand ??= new RelayCommand(() =>
        {
            ApplyVisibility = Visible;
            AddEnabled = false;
            EditEnabled = false;
            RemoveEnabled = false;
            ReadOnly = false;
            Editing = true;
        }
    );

    /// <summary>
    /// Возвращает команду удаления контакта.
    /// </summary>
    public RelayCommand RemoveCommand => _removeCommand ??= new RelayCommand(() =>
        {
            Contacts.Remove(SelectedContact);
            SelectedContact = new Contact();
            if (Contacts.Count != 0) return;
            EditEnabled = false;
            RemoveEnabled = false;
        }
    );

    /// <summary>
    /// Возвращает команду закрытия окна.
    /// </summary>
    public RelayCommand CloseWindowCommand => _closeWindowCommand ??= new RelayCommand(() =>
        Serializer<ObservableCollection<Contact>>.ToJson(Contacts, App.DefaultSavePath));

    /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
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