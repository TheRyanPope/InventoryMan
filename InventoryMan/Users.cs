using System.Collections.Generic;
using System.Linq;
using SQLite;
using System.IO;
using System;

namespace InventoryMan
{
    public class Users
    {
        public static List<User> UserList { get; set; } = new List<User>();

        public static string CurrentUser { get; set; }

        // User DB path
        public static string path = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.Personal), "login_DB.txt");

        // verify users credentials
        public static bool VerifyLogin(User person)
        {
            if (UserList.Any(user => user.Username == person.Username &&
                     user.Password == person.Password))
            {
                return true;
            }

            return false;
        }

        // add a new users
        public static List<User> AddUsers(User person)
        {
            if (!VerifyLogin(person))
            {
                UserList.Add(person);
                return UserList;
            }

            // person already exists
            return UserList;
        }

        // *****************
        // User DB methods
        // below this point
        // *****************

        // create the user db
        public static bool CreateUserDatabase()
        {
            try
            {
                var connection = new SQLiteConnection(path);
                connection.CreateTable<User>();

                return true;
            }
            catch (SQLiteException)
            {
                return false;
            }
        }

        // insert a new user into the user DB
        public static bool InsertUpdateUser(User user)
        {
            try
            {
                var db = new SQLiteConnection(path);

                if (db.Insert(user) != 0)
                    db.Update(user);

                return true;
            }
            catch (SQLiteException)
            {
                return false;
            }
        }

        // load the user DB into the user list
        public static void LoadUserDB()
        {
            var db = new SQLiteConnection(path);

            // query user db for all stored users
            UserList = db.Table<User>().ToList();
        }

    }
}