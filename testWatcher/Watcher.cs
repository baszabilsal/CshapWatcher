using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Threading;

namespace testWatcher
{
    public class Watcher
    {

        public static void Main()
        {
            Run();

        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Run()
        {


            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = ConfigurationManager.AppSettings["Path"];
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.


            // Add event handlers.
           
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            var watcher = source as FileSystemWatcher;
            watcher.EnableRaisingEvents = false;
            bool dmcheck = true;
            string programname = "importsystemstatus";
     
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File in Folder: " + e.Name + " has input " + e.ChangeType);
            string filename = ConfigurationManager.AppSettings["PathPrograms"];



            Process thisProc = Process.GetCurrentProcess();

            while (dmcheck == true)
            {

                Console.WriteLine("start programs " + programname);
                bool check = false;

                foreach (Process clsProcess in Process.GetProcesses())
                {
                  
                    if (clsProcess.ProcessName.Contains(programname))
                    {
                        check = true;
                    }
                }

                if (check == false)
                {
                    try
                    {
                        Console.WriteLine("Program " + programname + "is opening...");
                        var proc = System.Diagnostics.Process.Start(filename);
                        dmcheck = false;
                    }
                    catch (Exception ex){ Console.WriteLine(ex);
                        dmcheck = false;
                    }
                    //System.Windows.MessageBox.Show("Application not open!");
                    //System.Windows.Application.Current.Shutdown();
                }
                if (check == true)
                {
                    Console.WriteLine("Programs " + programname + "opened");
                    Console.WriteLine("Wait...");
                    Thread.Sleep(50000);
                }
            }
            Console.WriteLine("End Process");
            watcher.EnableRaisingEvents = true;
        }
        }
    }

