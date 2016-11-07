using SQLite;

namespace InventoryMan
{
    public class Log
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int UID { get; set; }

        public string Entry { get; set; }

        public Log()
        {
            // SQLite requires a parameterless constructor
        }

        public Log(string entry, int uid)
        {
            Entry = entry;
            UID = uid;
        }
    }
}