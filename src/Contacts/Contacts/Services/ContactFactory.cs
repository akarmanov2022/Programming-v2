using System;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using Contacts.Model;

namespace Contacts.Services;

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
    public static Contact? RandomGenerate()
    {
        try
        {
            const string uri = "https://api.randomdatatools.ru/?unescaped=true&params=LastName,FirstName,Phone,Email";
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            
            var response = Http.SendAsync(request).Result;

            var json = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<Contact>(json);
        }
        catch (Exception e)
        {
            MessageBox.Show(
                $"Error: {e.Message}", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }
    }
}