using System;

namespace FolderCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            FileReplacer fileReplacer = new FileReplacer();

            Console.WriteLine("Enter the path of the folder to crawl (e.g., C:\\path\\to\\folder): ");
            string folderPath = Console.ReadLine();

            Console.WriteLine("Enter the name of the file to search for (e.g., index.php): ");
            string targetFileName = Console.ReadLine();

            Console.WriteLine("Enter the text to filter for in the files (e.g., 'hello'): ");
            string filterText = Console.ReadLine();

            Console.WriteLine("Replacing files...");
            fileReplacer.ReplaceFilesInFolder(folderPath, targetFileName, filterText);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
