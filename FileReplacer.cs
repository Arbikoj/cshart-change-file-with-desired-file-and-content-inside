using System;
using System.IO;

namespace FolderCrawler
{
    public class FileReplacer
    {
        // Method to replace custom files containing specific text with dynamically generated PHP content
        public void ReplaceFilesInFolder(string rootFolderPath, string targetFileName, string filterText)
        {
            if (!Directory.Exists(rootFolderPath))
            {
                Console.WriteLine("The specified root folder path does not exist.");
                return;
            }

            // Find all occurrences of the target file name in the specified folder and subfolders
            var files = Directory.GetFiles(rootFolderPath, targetFileName, SearchOption.AllDirectories);

            foreach (var filePath in files)
            {
                try
                {
                    string fileContent = File.ReadAllText(filePath);

                    // If the content of the file contains the filter text, replace it
                    if (fileContent.Contains(filterText))
                    {
                        // Calculate relative path to 'baseurl.php' from the root folder
                        string relativePathToBaseUrl = CalculateRelativePathToBaseUrl(rootFolderPath, filePath);

                        // Generate custom content
                        string newContent = GenerateCustomContent(relativePathToBaseUrl);

                        // Write the custom content to the target file
                        File.WriteAllText(filePath, newContent);

                        Console.WriteLine($"Replaced content of {filePath} with custom content.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
                }
            }

            Console.WriteLine("File replacement process complete.");
        }

    // Method to calculate the relative path to 'baseurl.php' from the root folder
    private string CalculateRelativePathToBaseUrl(string rootFolderPath, string filePath)
    {
        // Normalize paths to remove any trailing directory separator
        string normalizedRoot = Path.GetFullPath(rootFolderPath).TrimEnd(Path.DirectorySeparatorChar);
        string normalizedFileDir = Path.GetDirectoryName(Path.GetFullPath(filePath)) ?? "";

        // Calculate the relative depth between the file path and the root folder
        var rootUri = new Uri(normalizedRoot + Path.DirectorySeparatorChar);
        var fileUri = new Uri(normalizedFileDir + Path.DirectorySeparatorChar);
        Uri relativeUri = rootUri.MakeRelativeUri(fileUri);

        // Adjust depth to exclude the file’s immediate directory level
        int depth = Math.Max(0, relativeUri.ToString().Split('/').Length - 1);

        // Construct the relative path using "../" for each level
        string relativePath = string.Join("/", Enumerable.Repeat("..", depth)) + "/baseurl.php";

        return relativePath;
    }


        // Method to generate custom PHP content with dynamic relative path to 'baseurl.php'
        private string GenerateCustomContent(string relativePathToBaseUrl)
        {
            return $@"<?php
require_once dirname(__FILE__) . '/{relativePathToBaseUrl}';
?>
<script language=""javascript"">
    window.location.href = ""<?php echo $baseurl; ?>"";
</script>";
        }
    }
}
