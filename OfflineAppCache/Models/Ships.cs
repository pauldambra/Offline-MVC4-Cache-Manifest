namespace OfflineAppCache.Models
{
    public class Ship
    {
        public Ship(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; }
        public string Type { get; set; }
    }
}