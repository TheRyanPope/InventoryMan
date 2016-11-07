using SQLite;

namespace InventoryMan
{
    class Item
    {
        public string Name { get; set; }

        [PrimaryKey]
        public string PartNumber { get; set; }

        // Max 250 chars
        public string Description { get; set; }

        public string Location { get; set; }

        public string LeadTime { get; set; }

        public int Qoh { get; set; }

        // Max 2000 chars
        public string Comments { get; set; }

        public Item()
        {
            // SQLite requires a parameterless constructor
        }

        public Item(string name, string partNumber, string description, string location, string leadTime, 
            int qoh, string comments)
        {
            Name = name;
            PartNumber = partNumber;
            Description = description;
            Location = location;
            LeadTime = leadTime;
            Qoh = qoh;
            Comments = comments;
        }
    }
}