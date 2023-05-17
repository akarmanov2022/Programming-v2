using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Contacts.Model;

/// <summary>
/// Определяет контакт.
/// </summary>
public sealed class Contact : INotifyPropertyChanged, ICloneable
{
    /// <summary>
    /// Хранит имя.
    /// </summary>
    private string _firstName = "";

    /// <summary>
    /// Хранит фамилию.
    /// </summary>
    private string? _lastName = "";

    /// <summary>
    /// Хранит адрес электронной почты.
    /// </summary>
    private string? _email = "";

    /// <summary>
    /// Хранит номер телефона.
    /// </summary>
    private string? _phone = "";

    /// <summary>
    /// Задает значение поля и вызывает событие <see cref="PropertyChanged"/>.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;


    /// <summary>
    /// Устанавливает и возвращает имя.
    /// </summary>
    public string FirstName
    {
        get => _firstName;
        set => SetField(ref _firstName, value);
    }

    /// <summary>
    /// Устанавливает и возвращает фамилию.
    /// </summary>
    public string? LastName
    {
        get => _lastName;
        set => SetField(ref _lastName, value);
    }

    /// <summary>
    /// Устонавливает и возвращает адрес электронной почты.
    /// </summary>
    public string? Email
    {
        get => _email;
        set => SetField(ref _email, value);
    }

    /// <summary>
    /// Устанавливает и возвращает номер телефона.
    /// </summary>
    public string? Phone
    {
        get => _phone;
        set => SetField(ref _phone, value);
    }

    /// <inheritdoc cref="ICloneable.Clone"/>
    public object Clone()
    {
        return new Contact
        {
            LastName = LastName,
            FirstName = FirstName,
            Phone = Phone,
            Email = Email
        };
    }

    /// <summary>
    /// Вызывает событие <see cref="PropertyChanged"/>.
    /// </summary>
    /// <param name="propertyName">Имя свойства.</param>
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Устанавливает значение поля и вызывает событие <see cref="PropertyChanged"/>.
    /// </summary>
    /// <param name="field">Имя поля.</param>
    /// <param name="value">Значение.</param>
    /// <param name="propertyName">Имя свойства.</param>
    /// <typeparam name="T">Тип поля.</typeparam>
    /// <returns>Возвращает <see langword="true"/> если значение поля было изменено, иначе <see langword="false"/>.</returns>
    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}