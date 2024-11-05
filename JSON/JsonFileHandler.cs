using Laboration_3.ViewModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace Laboration_3.JSON
{
    internal class JsonFileHandler
    {
        public static string GetFilePath()
        {
            string appDataFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); //filePath till AppData/Locals
            string directoryFilePath = Path.Combine(appDataFilePath, "Laboration_3"); // filePath till AppData/Locals/Laboration_3

            if (!Directory.Exists(directoryFilePath))  //Skapa mappen om det inte existerar
            {
                Directory.CreateDirectory(directoryFilePath);
            }

            string filePath = Path.Combine(directoryFilePath, "Laboration_3.json"); // filePath till AppData/Locals/Laboration_3/Laboration_3.json
            return filePath;
        }

        public static async Task WriteToJson(string filePath, ObservableCollection<QuestionPackViewModel> packs )
        {
            var options = new JsonSerializerOptions()
            {
                IncludeFields = true,
                IgnoreReadOnlyProperties = false,
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(packs, options);
            
            await File.WriteAllTextAsync(filePath, jsonString); //skriva en json-string till jsonfil
        }


        //public void ReadFromJson(string filePath)
        //{
        //    // Läs från fil 
        //    string jsonString = await File.ReadAllText(filePath);
        //    return ObservableCollection < QuestionPackViewModel > Packs = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(jsonString);
        //}


    }
}
