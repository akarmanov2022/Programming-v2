using System;
using System.IO;
using Newtonsoft.Json;

namespace Contacts.Services;

public static class Serializer<T> where T : class
{

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