using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
//using FileSearchPROJECT;

namespace BL
{
    public static class Searches
    {
        static public int folderCount = 1;
        static public int fileCount = 0;
        static public int folderException = 0;
        static public int fileException = 0;
        static public int resultsCount = 0;
        static public DateTime searchEndTime;
        static public List<string> resultsList = new List<string>();
        static public int i;

        public static List<string> dirConditionedSearch(DirectoryInfo dirstr, string searchTxt)
        {
             fileSearch(dirstr, searchTxt);
             foreach (DirectoryInfo a in dirstr.GetDirectories())
             {
                 try
                 {
                     folderCount++;
                     dirConditionedSearch(a, searchTxt);
                 }
                 catch (Exception ex)
                 {
                     fileException++;
  //                   ConditionedSearch.writeError(ex.Message);
                 }
             }
            return resultsList;
        }

        public static void fileSearch(DirectoryInfo folder, string searchTxt)
        {
            foreach (FileInfo b in folder.GetFiles())
            {
                fileCount++;
                if (b.Name.ToLower().Contains(searchTxt))
                {
                    resultsList.Add(b.FullName);
                    resultsCount++;
  //                  ConditionedSearch.writeLine(b.FullName);
                }
            }
        }

        public static void reset()
        {
            resultsList = new List<string>();
            folderCount = 0;
            fileCount = 0;
            folderException = 0;
            fileException = 0;
            resultsCount = 0;
        }

        public static void addToDB(string searchTxt, string fullDirectory)
        {
  //          Console.ForegroundColor = ConsoleColor.DarkGreen;
  //          ConditionedSearch.writeLine("Saving to database, please wait...");   

            int resultsLooper = resultsList.Count > 0 ? resultsList.Count : 1;
            for (i = 1; i <= resultsLooper; i++)
            {
                SearchTable ks = new SearchTable
                {
                    Search_String = searchTxt,
                    Directory = fullDirectory,
                    Results = resultsList.Count > 0 ? resultsList[i - 1] : null,
                    Date = searchEndTime
                };
                DAL.DataClass.searchDB.SearchTables.Add(ks);
            }
            DAL.DataClass.searchDB.SaveChanges();
  //          ConditionedSearch.writeLine("Save successful!");
  //          ConditionedSearch.writeLine("");
        }
    }
}
    

