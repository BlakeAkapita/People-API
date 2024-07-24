using System.Collections.Generic;
using System.IO;
using ApiTest.Models;
using Newtonsoft.Json;

namespace ApiTest.Helpers
{
    public class JsonHelper
    {
        private static readonly string JsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data.json");

        public static List<Person> ReadFromJsonFile()
        {
            using StreamReader file = File.OpenText(JsonFilePath);
            string jsonContent = File.ReadAllText(JsonFilePath);
            return JsonConvert.DeserializeObject<PersonList>(jsonContent).People;
        }

        public static void WriteToJsonFile(List<Person> data)
        {
            using StreamWriter file = File.CreateText(JsonFilePath);
            JsonSerializer serializer = new JsonSerializer();

            var personList = new PersonList { People = data };

            serializer.Serialize(file, personList);
        }
    }
}
