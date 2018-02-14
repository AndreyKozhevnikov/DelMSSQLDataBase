using Microsoft.SqlServer.Management.Smo;
using System;
using System.Linq;
//the Microsoft.SqlServer.SqlManagementObjects NuGet package
namespace DelMSSQLDataBase {
    class Program {
        static void Main(string[] args) {

            var argumentValue = args[0].Substring(1);
            var sqlServer = new Server(@"(localdb)\mssqllocaldb");
            if(argumentValue == "all") {
                var count = sqlServer.Databases.Count;
                for(int i = count - 1; i >= 0; i--) {
                    var db = sqlServer.Databases[i];
                    var dtDiff = DateTime.Today - db.CreateDate;
                    if(db.IsSystemObject || dtDiff.TotalDays < 15) {
                        continue;
                    }
                    DeleteDataBase(sqlServer, db.Name);
                }
            } else {
                if(sqlServer.Databases.Contains(argumentValue)) {
                    DeleteDataBase(sqlServer, argumentValue);
                }
            }
            //Console.ReadKey(); 
        }
        static void DeleteDataBase(Server server, string dbName) {
            server.KillDatabase(dbName);
            Console.WriteLine("{0} deleted", dbName);
        }
        //        void DirectDelete() {

        //            string pathToSettingsFile = @"C:\MSSQLSettings.ini";
        //            StreamReader sr = new StreamReader(pathToSettingsFile);
        //            string st = sr.ReadToEnd();
        //            sr.Close();

        //            MsSqlConnector.GetSettingsFromTxt(st);
        //            MsSqlConnector.MsServerName = @"(localdb)\mssqllocaldb";
        //            MsSqlConnector.MsPass = "testDx55555";
        //            MsSqlConnector.Open();
        //            MsSqlConnector.MsDataBase = "master";
        //            string st1 = @"ALTER DATABASE [dxMyXPo123] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE;
        //DROP DATABASE [dxMyXPo123]";


        //            MsSqlConnector.MakeNonQuery(st1);
        //            MsSqlConnector.Close();
        //        }
    }
}
