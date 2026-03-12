namespace DreamScape_Interactive.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        /// <summary>Zeldzaamheid (0–100)</summary>
        public int Rarity { get; set; }

        /// <summary>Kracht (0–100)</summary>
        public int Power { get; set; }

        /// <summary>Snelheid (0–100)</summary>
        public int Speed { get; set; }

        /// <summary>Duurzaamheid (0–100)</summary>
        public int Durability { get; set; }

        /// <summary>Magische eigenschappen</summary>
        public string MagicProperties { get; set; } = string.Empty;
    }
}
