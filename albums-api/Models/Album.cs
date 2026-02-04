using System.Text.Json;

namespace albums_api.Models
{
    public record Album(int Id, string Title, string Artist, int Year, double Price, string Image_url)
    {
        private static readonly string JsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "albums.json");
        private static readonly object _lock = new object();

        private static List<Album> LoadFromFile()
        {
            try
            {
                if (!File.Exists(JsonFilePath))
                {
                    // Create directory if it doesn't exist
                    var directory = Path.GetDirectoryName(JsonFilePath);
                    if (directory != null && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Initialize with default data
                    var defaultAlbums = new List<Album>(){
                        new Album(1, "You, Me and an App Id", "Daprize", 2020, 10.99, "https://aka.ms/albums-daprlogo"),
                        new Album(2, "Seven Revision Army", "The Blue-Green Stripes", 2019, 13.99, "https://aka.ms/albums-containerappslogo"),
                        new Album(3, "Scale It Up", "KEDA Club", 2018, 13.99, "https://aka.ms/albums-kedalogo"),
                        new Album(4, "Lost in Translation", "MegaDNS", 2017, 12.99,"https://aka.ms/albums-envoylogo"),
                        new Album(5, "Lock Down Your Love", "V is for VNET", 2016, 12.99, "https://aka.ms/albums-vnetlogo"),
                        new Album(6, "Sweet Container O' Mine", "Guns N Probeses", 2015, 14.99, "https://aka.ms/albums-containerappslogo")
                    };
                    SaveToFile(defaultAlbums);
                    return defaultAlbums;
                }

                var json = File.ReadAllText(JsonFilePath);
                return JsonSerializer.Deserialize<List<Album>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Album>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading albums: {ex.Message}");
                return new List<Album>();
            }
        }

        private static void SaveToFile(List<Album> albums)
        {
            try
            {
                var directory = Path.GetDirectoryName(JsonFilePath);
                if (directory != null && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonSerializer.Serialize(albums, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(JsonFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving albums: {ex.Message}");
            }
        }

        public static List<Album> GetAll()
        {
            lock (_lock)
            {
                return LoadFromFile();
            }
        }

        public static Album? GetById(int id)
        {
            lock (_lock)
            {
                var albums = LoadFromFile();
                return albums.FirstOrDefault(a => a.Id == id);
            }
        }

        public static List<Album> GetAlbums()
        {
            return GetAll();
        }

        public static List<Album> GetByYear(int year)
        {
            lock (_lock)
            {
                var albums = LoadFromFile();
                return albums.Where(a => a.Year == year).ToList();
            }
        }

        public static Album Add(Album album)
        {
            lock (_lock)
            {
                var albums = LoadFromFile();
                var newId = albums.Any() ? albums.Max(a => a.Id) + 1 : 1;
                var newAlbum = album with { Id = newId };
                albums.Add(newAlbum);
                SaveToFile(albums);
                return newAlbum;
            }
        }

        public static Album? Update(int id, Album album)
        {
            lock (_lock)
            {
                var albums = LoadFromFile();
                var index = albums.FindIndex(a => a.Id == id);
                if (index == -1) return null;

                var updatedAlbum = album with { Id = id };
                albums[index] = updatedAlbum;
                SaveToFile(albums);
                return updatedAlbum;
            }
        }

        public static bool Delete(int id)
        {
            lock (_lock)
            {
                var albums = LoadFromFile();
                var album = albums.FirstOrDefault(a => a.Id == id);
                if (album == null) return false;

                albums.Remove(album);
                SaveToFile(albums);
                return true;
            }
        }
    }
}
