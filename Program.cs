using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Management.Automation;

namespace Курсова
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] URLs = new string[8];
            URLs[0] = "https://www.youtube.com/";
            URLs[1] = "https://www.hltv.org/";
            URLs[2] = "https://www.ratatype.ua/u3723941/";
            URLs[3] = "https://rozetka.com.ua/";
            URLs[4] = "https://hot-game.info/";
            URLs[5] = "https://rutracker.net/forum/index.php";
            URLs[6] = "https://www.faceit.com/ru/home";
            URLs[7] = "https://classroom.google.com/u/1/h";
            for (int i = 0; i < URLs.Length; i++)
            {
                string result = GetHost(URLs[i]);
                Console.WriteLine(result);
            }
            Console.WriteLine("Введіть путь к папкам с файлами:");
            string directoryPath = "C:/Users/Klaimen/source/repos/Курсова/DownloadSites";
            string[] directory = Directory.GetFiles(directoryPath);
            Sorting(directory, directoryPath);
            Console.ReadLine();
        }
        static string GetHost(string url)
        {
            int index;
            string host = "";
            url = url.Replace("https://", "");
            url = url.Replace("http://", "");
            url = url.Replace("www.", "");
            index = url.IndexOf("/");
            if (index != -1)
                host = url.Remove(index);
            else host = url;
            return host;
        }
        static string GetURL(string filePath)
        {
            string output = "";
            using (var ps = PowerShell.Create())
            {
                string script = String.Format(@"Get-Content ""{0}"" -Stream Zone.Identifier", filePath);

                ps.AddScript(script).Invoke();
                Collection<PSObject> results = ps.Invoke();
                foreach (var result in results)
                {
                    Console.WriteLine(results.ToString());
                }
            }
            return output;
        }
        static void Sorting(string[] directory, string directoryPath)
        {
            foreach (string filePath in directory)
            { 
                string fileName = Path.GetFileName(filePath);
                string url = GetURL(filePath);
                string siteHost = GetHost(url);
                string toSiteDirectory = Path.Combine(directoryPath, siteHost);
                string newPath = Path.Combine(toSiteDirectory, fileName);
                if (Directory.Exists(toSiteDirectory) == false)
                {
                    Directory.CreateDirectory(toSiteDirectory);
                    File.Move(filePath, newPath);
                    Console.WriteLine("Файл {0} перемiщений в {1}", fileName, toSiteDirectory);
                }
                else 
                {
                    File.Move(filePath, newPath);
                    Console.WriteLine("Файл {0} перемiщений в {1}", fileName, toSiteDirectory);
                }
            }
        }
    }
}
