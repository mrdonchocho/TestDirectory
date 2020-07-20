using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace TestDirectory
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting operation to move files from directory at " + DateTime.Now);
            string fileName = "test.txt";
            string TARGET_DIRECTORY = ConfigurationManager.AppSettings["fileLocation"].ToString();
            List<string> TARGET_DIRECTORIES = ConfigurationManager.AppSettings["fileLocations"].Split(',').ToList();
            TARGET_DIRECTORIES.Remove(string.Empty);
            int DirLength = TARGET_DIRECTORIES.Count();
            string DOMAIN = ConfigurationManager.AppSettings["DOMAIN"].ToString();
            string REMOTE_COMPUTER_NAME = ConfigurationManager.AppSettings["REMOTE_COMPUTER_NAME"].ToString();
            string USER_NAME = ConfigurationManager.AppSettings["USER_NAME"].ToString();
            string PASSWORD = ConfigurationManager.AppSettings["PASSWORD"].ToString();
            string REMOTE_DIRECTORY = ConfigurationManager.AppSettings["REMOTE_DIRECTORY"].ToString();

            string sourceFile = Path.Combine(REMOTE_DIRECTORY, fileName);
            string destFile = Path.Combine(TARGET_DIRECTORIES[0], fileName);

            foreach (string dir in TARGET_DIRECTORIES)
            {
                if (!Directory.Exists(dir) && !string.IsNullOrEmpty(dir))
                {
                    try
                    {
                        Directory.CreateDirectory(dir);
                        Console.WriteLine("Target directory not found and has now been created: " + dir);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Unable to create Target Directory : '{dir}' | Error : {ex.Message}");
                        throw;
                    }
                }
            }

            if (Directory.Exists(TARGET_DIRECTORIES[0]))
            {
                Console.WriteLine("Target directories found: " + TARGET_DIRECTORIES.Count());
                try
                {
                    var remoteDir = @"\\" + REMOTE_DIRECTORY;
                    if (Directory.Exists(REMOTE_DIRECTORY))
                    {
                        Console.WriteLine("Remote directory found: " + remoteDir);
                        string[] files = Directory.GetFiles(REMOTE_DIRECTORY);
                        Console.WriteLine("Number of files gotten from remote directory: " + files.Length);
                        //Move the files to target directory
                        foreach (string s in files)
                        {
                            foreach (string directory in TARGET_DIRECTORIES)
                            {
                                if (directory == TARGET_DIRECTORIES.Last())
                                {
                                    // Use static Path methods to extract only the file name from the path.
                                    fileName = Path.GetFileName(s);
                                    destFile = Path.Combine(directory, fileName);
                                    File.Move(s, destFile);
                                    Console.WriteLine($"File Name: {fileName} has been moved to: '{directory}' successfully at :  " + DateTime.Now);
                                }
                                else
                                {
                                    // Use static Path methods to extract only the file name from the path.
                                    fileName = Path.GetFileName(s);
                                    destFile = Path.Combine(directory, fileName);
                                    File.Copy(s, destFile);
                                    Console.WriteLine($"File Name: {fileName} has been copied to: '{directory}' successfully at :  " + DateTime.Now);

                                }
                            }
                        }
                        Console.WriteLine("Files moved successfully at :  " + DateTime.Now);
                    }
                    else
                    {
                        Console.WriteLine("Remote Directory does not exist :  " + REMOTE_DIRECTORY);
                    }
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
            else
            {
                Console.WriteLine("Target Directory does not exist :  " + TARGET_DIRECTORIES[0]);
            }
            Console.WriteLine("Ending operation to move files from directory at " + DateTime.Now);
            Console.ReadLine();
        }
    }
}
