using Microsoft.SqlServer.Management.Smo;
using System;
using System.IO;
using System.Linq;
//the Microsoft.SqlServer.SqlManagementObjects NuGet package
namespace DelMSSQLDataBase {
    class Program {
        static void Main(string[] args) {
            var namesToExcludeFile = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory+"namesToExclude.txt");
            var namesToExclude = namesToExcludeFile.Split(';'); 
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
                    bool flag = false;
                    var dbname = db.Name.ToLower();
                    foreach(var nm in namesToExclude) {
                        var exname = nm.ToLower();
                        if(dbname.Contains(exname)) {
                            flag = true;
                            break;
                        }
                    }
                    if(flag) {
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

    }
}
