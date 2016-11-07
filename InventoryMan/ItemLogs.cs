using System;
using System.Linq;
using SQLite;
using System.IO;
using System.Collections.Generic;

namespace InventoryMan
{
    class ItemLogs
    {
        public static List<Log> Logs { get; set; } = new List<Log>();

        // Item DB path
        public static string path = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.Personal), "log_DB.txt");

        // create the log db
        public static void CreateLogsDatabase()
        {
            try
            {
                var connection = new SQLiteConnection(path);
                connection.CreateTable<Log>();
            }
            catch (SQLiteException)
            {

            }
        }

        // insert a new log into the log DB
        public static void InsertLog(Log log)
        {
            try
            {
                var db = new SQLiteConnection(path);

                db.Insert(log);
            }
            catch (SQLiteException)
            {

            }
        }

        // load the log DB into the log list
        public static void LoadLogDB()
        {
            var db = new SQLiteConnection(path);

            // query item log db for all stored logs
            Logs = db.Table<Log>().ToList();
        }

        // delete the item log from the log list
        public static void DeleteLog(List<Log> logs)
        {
            // remove all logs for the specified item
            Logs = (from log in Logs
                    where log.UID.ToString() != Items.SelectedItemIndex.ToString()
                    select log).ToList<Log>();
            
            var db = new SQLiteConnection(path);

            // remove each log of this item from the DB
            foreach (Log log in logs)
            {
                db.Delete(log);
            }
        }

        // select the log entries for a specific item
        public static List<Log> SearchLogs()
        {
            // make a new list to display the logs for the specified item
            List<Log>searchItems = (from query in Logs
                                    where query.UID.ToString() == Items.SelectedItemIndex.ToString()
                                    select query).ToList<Log>();

            return searchItems;
        }
    }
}