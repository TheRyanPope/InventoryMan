using SQLite;

namespace InventoryMan
{
    public class User
    {
        [PrimaryKey]
        public string Username { get; set; }

        public string Password { get; set; }

        public User()
        {
            // SQLite requires a parameterless constructor
        }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}