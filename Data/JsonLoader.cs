using Olimpijske_igre.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Olimpijske_igre.Services
{
    public class JsonLoader
    {
        public static Dictionary<string, List<Team>> LoadGroups(string fileName)
        {
            try
            {
                string filePath = GetFilePath(fileName);
                string jsonString = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<Dictionary<string, List<Team>>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Failed to deserialize JSON: {ex.Message}");
                throw;
            }
        }

        public static Dictionary<string, List<ExhibitionMatch>> LoadExhibitionMatches(string fileName)
        {
            try
            {
                string filePath = GetFilePath(fileName);
                string jsonString = File.ReadAllText(filePath);
                // Učitavanje JSON-a u Dictionary gde je ključ naziv tima, a vrednost lista utakmica
                return JsonSerializer.Deserialize<Dictionary<string, List<ExhibitionMatch>>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Failed to deserialize JSON: {ex.Message}");
                throw;
            }
        }

        private static string GetFilePath(string fileName)
        {
            // Uzmi trenutni direktorijum aplikacije
            string baseDirectory = AppContext.BaseDirectory;

            // Kombinuj direktorijum sa nazivom fajla
            return Path.Combine(baseDirectory, fileName);
        }

    }

}
