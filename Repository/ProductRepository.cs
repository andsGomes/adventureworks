#nullable disable
using System.Data;
using System.Data.SqlClient;
using adventureWorks.Common;
using adventureWorks.Entities;

namespace adventureWorks.Repository
{
  public class ProductRepository
  {

    public virtual List<Product> Search(string connectString, string sql)
    {
      List<Product> ret;
      using SqlServerDatabaseContext dbContext = new SqlServerDatabaseContext(connectString);
      // Create Command Object with Sql
      dbContext.CreateCommand(sql);
      ret = BuildEntityList(dbContext.CreateDataReader());
      return ret;
    }

    protected virtual List<Product> BuildEntityList(IDataReader rdr)
    {
      List<Product> ret = new();

      // Loop through all rows in the data reader
      while (rdr.Read())
      {
        // Create new object and add to collection
        ret.Add(new Product
        {
          ProductID = rdr.GetData<int>("ProductID"),
          Name = rdr.GetData<string>("Name"),
          ProductNumber = rdr.GetData<string>("ProductNumber"),
          Color = rdr.GetData<string>("Color"),
          StandardCost = rdr.GetData<decimal>("StandardCost"),
          ListPrice = rdr.GetData<decimal>("ListPrice"),
          SellStartDate = rdr.GetData<DateTime>("SellStartDate"),
          SellEndDate = rdr.GetData<DateTime>("SellEndDate"),
          DiscontinuedDate = rdr.GetData<DateTime>("DiscontinuedDate")
        });
      }

      return ret;
    }
  }
}