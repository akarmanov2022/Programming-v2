using System;
using System.IO;
using Newtonsoft.Json;

namespace Contacts.Services;

/// <summary>
/// Стаический класс с набором методов для сериализации и десериализации объектов.
/// </summary>
/// <typeparam name="T">Тип объекта.</typeparam>
public static class Serializer<T> where T : class
{

    /// <summary>
    /// Сериализует объект в JSON.
    /// </summary>
    /// <param name="o">Объект.</param>
    /// <param name="filePath">Путь к файлу.</param>
    public static void ToJson(T o, string filePath)
    {
        try
        {
            using var sw = new StreamWriter(Path.Combine(filePath, "save.json"));
            var serializer = new JsonSerializer();
            serializer.Serialize(sw, o);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    /// <summary>
    /// Десериализует объект из JSON.
    /// </summary>
    /// <param name="filePath">Путь к файлу.</param>
    /// <returns>Объект.</returns>
    public static T? FromJson(string filePath)
    {
        try
        {
            using var sr = new StreamReader(Path.Combine(filePath, "save.json"));
            var serializer = new JsonSerializer();
            return serializer.Deserialize(sr, typeof(T)) as T;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}