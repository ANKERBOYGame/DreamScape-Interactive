using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DreamScape_Interactive.Models;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace DreamScape_Interactive.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "dreamscape.db");
            _connectionString = $"Data Source={dbPath}";
        }

        public async Task InitializeDatabaseAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            string createTable = @"
                CREATE TABLE IF NOT EXISTS Items (
                    Id              INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name            TEXT    NOT NULL,
                    Description     TEXT    NOT NULL DEFAULT '',
                    Type            TEXT    NOT NULL DEFAULT '',
                    Rarity          INTEGER NOT NULL DEFAULT 0,
                    Power           INTEGER NOT NULL DEFAULT 0,
                    Speed           INTEGER NOT NULL DEFAULT 0,
                    Durability      INTEGER NOT NULL DEFAULT 0,
                    MagicProperties TEXT    NOT NULL DEFAULT ''
                );";

            using (var cmd = new SqliteCommand(createTable, connection))
            {
                await cmd.ExecuteNonQueryAsync();
            }

            // Seed sample items if table is empty
            using (var countCmd = new SqliteCommand("SELECT COUNT(*) FROM Items;", connection))
            {
                long count = (long)(await countCmd.ExecuteScalarAsync() ?? 0L);
                if (count == 0)
                {
                    await SeedSampleDataAsync(connection);
                }
            }
        }

        private async Task SeedSampleDataAsync(SqliteConnection connection)
        {
            var items = new[]
            {
                new Item
                {
                    Name = "Zwaard van de Dageraad",
                    Description = "Een glanzend zwaard gesmeed uit het licht van de eerste zonsopgang.",
                    Type = "Wapen",
                    Rarity = 85,
                    Power = 90,
                    Speed = 60,
                    Durability = 75,
                    MagicProperties = "Vuur, Licht"
                },
                new Item
                {
                    Name = "Schild van de Eeuwigheid",
                    Description = "Een oud schild bedekt met mystieke runen die de drager beschermen.",
                    Type = "Pantser",
                    Rarity = 70,
                    Power = 20,
                    Speed = 30,
                    Durability = 95,
                    MagicProperties = "Barrière, Regeneratie"
                },
                new Item
                {
                    Name = "Toverstaf van Sterrenval",
                    Description = "Een staf gemaakt van een gevallen meteoriet, geladen met kosmische energie.",
                    Type = "Toverkracht",
                    Rarity = 95,
                    Power = 85,
                    Speed = 55,
                    Durability = 50,
                    MagicProperties = "Elektriciteit, Zwaartekracht"
                },
                new Item
                {
                    Name = "Leren Laarzen van de Wind",
                    Description = "Lichte laarzen waarmee de drager snel als de wind kan bewegen.",
                    Type = "Accessoire",
                    Rarity = 55,
                    Power = 10,
                    Speed = 95,
                    Durability = 65,
                    MagicProperties = "Snelheid, Lucht"
                },
                new Item
                {
                    Name = "Amulette van het Schaduwpad",
                    Description = "Een donker kristal dat de drager in de schaduw laat versmelten.",
                    Type = "Accessoire",
                    Rarity = 80,
                    Power = 40,
                    Speed = 70,
                    Durability = 60,
                    MagicProperties = "Onzichtbaarheid, Schaduw"
                }
            };

            string insert = @"
                INSERT INTO Items (Name, Description, Type, Rarity, Power, Speed, Durability, MagicProperties)
                VALUES (@Name, @Description, @Type, @Rarity, @Power, @Speed, @Durability, @MagicProperties);";

            foreach (var item in items)
            {
                using var cmd = new SqliteCommand(insert, connection);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Description", item.Description);
                cmd.Parameters.AddWithValue("@Type", item.Type);
                cmd.Parameters.AddWithValue("@Rarity", item.Rarity);
                cmd.Parameters.AddWithValue("@Power", item.Power);
                cmd.Parameters.AddWithValue("@Speed", item.Speed);
                cmd.Parameters.AddWithValue("@Durability", item.Durability);
                cmd.Parameters.AddWithValue("@MagicProperties", item.MagicProperties);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<Item>> GetAllItemsAsync()
        {
            var items = new List<Item>();

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqliteCommand("SELECT Id, Name, Description, Type, Rarity, Power, Speed, Durability, MagicProperties FROM Items ORDER BY Name;", connection);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                items.Add(new Item
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Type = reader.GetString(3),
                    Rarity = reader.GetInt32(4),
                    Power = reader.GetInt32(5),
                    Speed = reader.GetInt32(6),
                    Durability = reader.GetInt32(7),
                    MagicProperties = reader.GetString(8)
                });
            }

            return items;
        }
    }
}
