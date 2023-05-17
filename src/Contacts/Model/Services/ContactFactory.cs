using System.Text.Json;
using Contacts.Model;

namespace Model.Services;

/// <summary>
/// Фабрика контактов.
/// </summary>
public static class ContactFactory
{
    /// <summary>
    /// Хранит экземпляр <see cref="HttpClient"/>.
    /// </summary>
    private static readonly HttpClient Http = new();

    /// <summary>
    /// Генерирует случайный контакт. Через API сервиса randomdatatools.ru.
    /// </summary>
    /// <returns>Случайный контакт.</returns>
    public static Contact? RandomContact()
    {
        const string uri = "https://api.randomdatatools.ru/?unescaped=true&params=LastName,FirstName,Phone,Email";
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        var response = Http.SendAsync(request).Result;

        var json = response.Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<Contact>(json);
    }
}