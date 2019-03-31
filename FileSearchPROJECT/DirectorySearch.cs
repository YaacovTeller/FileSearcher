using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace FileSearchPROJECT
{
    class DirectorySearch
    {
        static int folderCount = 0;
        static int fileCount = 0;
        static int folderException = 0;
        static int fileException = 0;
        static bool atRoot = true;
        static void dirSearch(DirectoryInfo dirstr)
        {
            try
            {
                foreach (DirectoryInfo a in dirstr.GetDirectories())
                {
                    try
                    {
                        if (atRoot == true)
                        {
                            foreach (FileInfo b in dirstr.GetFiles())
                            {
                                fileCount++;
                                //     Console.WriteLine(b.FullName);
                                atRoot = false;
                            }
                        }
                        //    Console.WriteLine(a.FullName);
                        folderCount++;
                        foreach (FileInfo b in a.GetFiles())
                        {
                            fileCount++;
                            //      Console.WriteLine(b.FullName);
                        }
                        dirSearch(a);
                    }
                    catch (Exception ex)
                    {
                        fileException++;
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                folderException++;
                Console.WriteLine(ex.Message);
            }
        }
        public static void reset()
        {
            folderCount = 0;
            fileCount = 0;
            folderException = 0;
            fileException = 0;
            atRoot = true;
        }
        public static void results()
        {
            Console.WriteLine(folderCount + " folder(s), containing " + fileCount + " files");
            if (fileException != 0 || folderException != 0)
            {
                Console.WriteLine(folderException + " folder error(s), " + fileException + " file error(s)");
            }
            Console.WriteLine();
        }

        public static void userSearch()
        {
            DirectoryInfo userString = null;
            Console.WriteLine("Enter a directory to search:");
            try
            {
                userString = new DirectoryInfo(Console.ReadLine());
            }
            catch
            {

            }
            finally
            {
                if (userString != null)
                {
                    dirSearch(userString);
                }
                else
                {
                    userString = new DirectoryInfo(@"C:\");
                    Console.WriteLine("Searching \"{0}\"", userString);
                    dirSearch(userString);
                }
            }
            //     C:\Users\jbt\source\repos\API\API\ang-api\e2e
            //     O:\\NET Courses\\91448-7\\Flow-Charts
            //     dirSearch(@"O:\NET Courses\91448-7\Flow-Charts");
            results();
            reset();
            userSearch();
        }
    }
}
