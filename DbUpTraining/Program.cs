using Dapper;
using Dapper.Contrib.Extensions;
using DbUp;
using System;
using System.Data.SqlClient;
using System.Reflection;

namespace DbUpTraining
{
    class Program
    {
        private const string CONNECTION_STRING = "Server=A-305-03;Database=BookStore;Trusted_Connection=true;";
        static void Main(string[] args)
        {
            EnsureDatabase.For.SqlDatabase(CONNECTION_STRING);
            using(var connection = new SqlConnection(CONNECTION_STRING))
            {
                var upgrader =
                    DeployChanges.To
                        .SqlDatabase(CONNECTION_STRING)
                        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                        .LogToConsole()
                        .Build();

                var result = upgrader.PerformUpgrade();
                if (!result.Successful)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(result.Error);
                    Console.ResetColor();
#if DEBUG
                    Console.ReadLine();
#endif
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
                Console.ResetColor();
                var book = new Book
                {
                    Name = "Abay",
                    Price = 5000,
                };
                //connection.Insert(book);
                connection.Execute("Insert into Books values(@Id, @Name, @Price);", book);
            }
        }
    }
}
