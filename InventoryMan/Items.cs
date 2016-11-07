using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InventoryMan
{
    class Items
    {
        public static List<Item> ItemList { get; set; } = new List<Item>();

        public static List<string> ItemDetails { get; set; } = new List<string>();

        public static int SelectedItemIndex { get; set; } = -1;

        // Item DB path
        public static string path = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.Personal), "items_DB.txt");

        // add a new items
        public static List<Item> AddItem(Item item)
        {
            if (!VerifyItem(item))
            {
                ItemList.Add(item);
            }

            return ItemList;
        }

        // add details of specified item
        public static List<string> AddItemDetails(Item item)
        {
            ItemDetails.Clear();

            ItemDetails.Add(item.Name);
            ItemDetails.Add(item.PartNumber);
            ItemDetails.Add(item.Description);
            ItemDetails.Add(item.Location);
            ItemDetails.Add(item.LeadTime);
            ItemDetails.Add(item.Qoh.ToString());
            ItemDetails.Add(item.Comments);

            return ItemDetails;
        }

        // verify if item already exists
        public static bool VerifyItem(Item item)
        {
            if (ItemList.Any(itm => itm.PartNumber == item.PartNumber))
            {
                // item is on the list
                return true;
            }

            // item is not on the list
            return false;
        }

        public static string[] ItemListToArray()
        {
            int index = 0;
            string[] listLabels;

            if (ItemList.Count != 0)
            {
                listLabels = new string[ItemList.Count];

                foreach (Item item in ItemList)
                {
                    listLabels[index] = item.Name + "      (" + item.PartNumber + ")";
                    index++;
                }

                return listLabels;
            }
            else
            {
                listLabels = new string[1];
                listLabels[0] = "No Items In Inventory";
                return listLabels;
            }
        }

        public static string[] SearchItemsToArray(List<Item> searchResults)
        {
            int index = 0;
            string[] listLabels;

            if (searchResults.Count != 0)
            {
                listLabels = new string[searchResults.Count];

                foreach (Item item in searchResults)
                {
                    listLabels[index] = item.Name + "   (" + item.PartNumber + ")";
                    index++;
                }

                return listLabels;
            }
            else
            {
                listLabels = new string[1];
                listLabels[0] = "No Results";
                return listLabels;
            }
        }

        public static void DiscoItem()
        {
            // remove the item from inventory
            DeleteItem();
            ItemList.Remove(ItemList[SelectedItemIndex]);
            
        }

        public static void ReceiveItem()
        {
            // increase QOH
            ItemList[SelectedItemIndex].Qoh++;

            DateTime localTime = DateTime.Now;

            Log log = new Log("Received 1 " + ItemList[SelectedItemIndex].Name + "\n" +
                localTime + "\n", SelectedItemIndex);

            // update this items log
            ItemLogs.Logs.Add(log);

            // update the item log DB
            ItemLogs.InsertLog(log);
        }

        public static void ConsumeItem()
        {
            // decrease QOH
            ItemList[SelectedItemIndex].Qoh--;

            DateTime localTime = DateTime.Now;

            Log log = new Log("Sold 1 " + ItemList[SelectedItemIndex].Name + "\n" +
                localTime + "\n", SelectedItemIndex);

            // update this items log
            ItemLogs.Logs.Add(log);

            // update the item log DB
            ItemLogs.InsertLog(log);
        }

        // *****************
        // User DB methods
        // below this point
        // *****************

        // create the item db
        public static void CreateItemDatabase()
        {
            try
            {
                var connection = new SQLiteConnection(path);
                connection.CreateTable<Item>();
            }
            catch (SQLiteException)
            {

            }
        }

        // insert a new item into the item DB
        public static void InsertItem(Item item)
        {
            try
            {
                var db = new SQLiteConnection(path);

                db.Insert(item);
            }
            catch (SQLiteException)
            {

            }
        }

        // update an item into the item DB
        public static void UpdateItem(Item item)
        {
            try
            {
                var db = new SQLiteConnection(path);

                db.Update(item);
            }
            catch (SQLiteException)
            {

            }
        }

        // load the user DB into the item list
        public static void LoadItemDB()
        {
            var db = new SQLiteConnection(path);

            // query item db for all stored items
            ItemList = db.Table<Item>().ToList();

        }

        // delete the item from the item list
        public static void DeleteItem()
        {
            var db = new SQLiteConnection(path);
            db.Delete(ItemList[SelectedItemIndex]);
        }

        // delete the item from the item DB
        public static void DeleteLog()
        {
            var db = new SQLiteConnection(path);

            // remove each log of this item from the DB
            db.Delete(ItemList[SelectedItemIndex]);
        }

    }
}