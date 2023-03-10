using adventureWorks.Entities;
using adventureWorks.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
  private static void Main(string[] args)
  {
    Console.Clear();

    using IHost host = Host.CreateApplicationBuilder().Build();
    IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
    string ConnectString = config.GetValue<string>("ConnectionStrings:Default");

    // string ConnectString = "Server=localhost;Database=AdventureWorksLT;User Id=sa;Password=1980pP012#";
    string Sql = "SELECT top 3 * FROM SalesLT.Product";

    ProductRepository repo = new();
    //List<Product> list = repo.Search(ConnectString, Sql);
    List<Product> list = repo.Search<Product>(ConnectString, Sql);


    Console.WriteLine("*** Display the Data ***");
    // Display Data
    foreach (var item in list)
    {
      Console.WriteLine(item.ToString());
    }

    Console.WriteLine();
    Console.WriteLine($"Total Items: {list.Count}");
    Console.WriteLine();
  }
}