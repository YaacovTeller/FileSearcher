using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using DAL;
using BL;

namespace FileSearchPROJECT
{
    public static class ConditionedSearch
    {
        static bool defaultDirectory;
        static public string userString = "";
        static public DirectoryInfo userDirectory = null;
        static public string searchTxt = "";
        static public string searchTxtEdit = "";
        static public DateTime searchStartTime;
        static public List<string> UIresultsList = new List<string>();

        public static void userConditionedSearch()
        {
            enterSearchKey();
            enterDirectory();      
            implementSearch();
            UIresultsList = BL.Searches.dirConditionedSearch(userDirectory, searchTxtEdit);
            results();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Saving to database, please wait...");
            BL.Searches.addToDB(searchTxt, userDirectory.FullName);
            Console.WriteLine("Save successful!");
            Console.WriteLine();
            BL.Searches.reset();
            newSearchMenu();
        }

        public static void mainMenu()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("1. Enter file name to search, default search on 'c:\\'");
            Console.WriteLine("2. Enter file name to search and parent diectory to search in.");
            Console.WriteLine("3. Exit");
            var x = Console.ReadKey(true).Key;
            switch (x)
            {
                case ConsoleKey.D1:
                    defaultDirectory = true;
                    userConditionedSearch();
                    break;
                case ConsoleKey.D2:
                    defaultDirectory = false;
                    userConditionedSearch();
                    break;
                case ConsoleKey.D3:     
                    break;
               default:
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please select a valid option:");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine();
                   mainMenu();
                    break;
            }
        }
        public static void newSearchMenu()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Clear screen and search again? [Y/N]");
            var x = Console.ReadKey(true).Key;
            switch (x)
            {
                case ConsoleKey.Y:
                    Console.Clear();
                    mainMenu();
                    break;
                case ConsoleKey.N:
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please select a valid option:");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine();
                    newSearchMenu();
                    break;
            }
        }

        public static void enterDirectory()
        {
            if(defaultDirectory == true)
            {
                userDirectory = new DirectoryInfo(@"C:\");
                //userString = new DirectoryInfo(@"D:\Games_FromC\HalfLife");
            }
            else {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Enter a directory to search:");
                do
                {
                    userString = Console.ReadLine();
                    if (userString == "")
                    {
                        userString = "INVALID";
                    }
                    try
                    {
                        userDirectory = BL.Searches.validateDirectory(userString);
                    }
                    catch { }
                    if (userDirectory == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid directory");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                while (userDirectory==null);
            }
        }

        public static void enterSearchKey()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Enter search string:");
            searchTxt = Console.ReadLine();    
        }

        public static void implementSearch()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Searching \"{0}\" for \"{1}\"", userDirectory.FullName, searchTxt);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            searchTxtEdit = searchTxt.ToLower();
            searchStartTime = DateTime.Now;
        }

        public static void results()
        {
            Console.WriteLine();
            displayList();
            BL.Searches.searchEndTime = DateTime.Now;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Search ran from " + searchStartTime.ToString("h:mm:ss:ff tt")
                +" to " + BL.Searches.searchEndTime.ToString("h:mm:ss:ff tt"));
            Console.WriteLine(BL.Searches.folderCount + " folders searched, containing "+ BL.Searches.fileCount +" files");
            Console.WriteLine(BL.Searches.resultsCount + " instances of \"" + searchTxt+"\"");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Encountered " + BL.Searches.fileException + " forbidden folders");
            Console.WriteLine();
    //        Console.ForegroundColor = ConsoleColor.Red;
        }
        public static void displayList()
        {
            foreach (var x in UIresultsList)
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("");
        }
        //public static void writeLine(string str)
        //{
        //    Console.WriteLine(str);
        //}
        //public static void writeError(string str)
        //{
        //    Console.ForegroundColor = ConsoleColor.DarkYellow;
        //    Console.WriteLine(str);
        //    Console.ForegroundColor = ConsoleColor.DarkGray;
        //}
    }
}
