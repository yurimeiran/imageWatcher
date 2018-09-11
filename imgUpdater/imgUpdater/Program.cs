using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Permissions;
using System.Diagnostics;
using MySql;
using GriffithElder.Database.MySql;
using MySql.Data.MySqlClient;

namespace imgUpdater
{


    public class db_conn
    {
        private MySqlConnect connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public db_conn()
        {
            Initialize();
        }

        //Initialize values
        public void Initialize()
        {
            server = "localhost";
            database = "***";
            uid = "****";
            password = "*****";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnect(server, database, uid, password);

        }

        private bool OpenConnection()
        {
            try
            {
                connection.IsConnected();
                Console.WriteLine("Connected");
                return true;
                
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }
       
    
    }

  

    public class Watcher
    {

        public static void Main()
        {

            Run();

        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]

        public static void Run()
        {
            
            //06-10-2017 using GetCurrentDirectory() get directory where executable is located
            string path = System.IO.Directory.GetCurrentDirectory();

            // 05-10-2017 specify the path manually otherwise use method to get the director path as string, like the above code 
            //string path = "C:\\Users\\Yuri\\Documents\\Visual Studio 2012\\Projects\\ConsoleApplication2\\ConsoleApplication2\\bin\\Debug\\";

            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();

            watcher.Path = path;
            /* Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch jpeg files.
            watcher.Filter = "*.jpg";// "*.jpeg";
           

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
          Console.WriteLine("Press \'q\' to quit.");
          while (Console.Read() != 'q') ;
        }



        private static object Directory(string p)
        {
            throw new NotImplementedException();
        }

        // Define the event handlers.
        public static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);

            //06-10-2017 when file changed , created, or deleted create text file and make record
            using (StreamWriter write = new StreamWriter("onChange.txt", true))
            {
                write.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
                write.Close();

            }

            
            string query = string.Format("INSERT into anpr.images VALUES (null, {0}, {1}, {2});", DateTime.Now, e.Name, e.FullPath);
            
           

        }


        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);

            //06-10-2017 when file is renamed create 
            using (StreamWriter write = new StreamWriter("onChange.txt", true))
            {
                write.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
                write.Close();

            }
        }

       
    }



}
