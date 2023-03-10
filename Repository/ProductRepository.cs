
using System.Reflection;
using System.Data;
using adventureWorks.Common;
using adventureWorks.Entities;


namespace adventureWorks.Repository
{
  public class ProductRepository
  {

    // public virtual List<Product> Search(string connectString, string sql)
    // {
    //   List<Product> ret;
    //   using SqlServerDatabaseContext dbContext = new SqlServerDatabaseContext(connectString);
    //   // Create Command Object with Sql
    //   dbContext.CreateCommand(sql);
    //   ret = BuildEntityList(dbContext.CreateDataReader());
    //   return ret;
    // }

    public virtual List<TEntity> Search<TEntity>(string connectString, string sql)
    {
      List<TEntity> ret;

      using SqlServerDatabaseContext dbContext = new(connectString);
      dbContext.CreateCommand(sql);

      ret = BuildEntityList<TEntity>(dbContext.CreateDataReader());

      return ret;
    }


    // First implementation of the BuilderEntityList method
    // protected virtual List<Product> BuildEntityList(IDataReader rdr)
    // {
    //   List<Product> ret = new();
    //   // Loop through all rows in the data reader
    //   while (rdr.Read())
    //   {
    //     // Create new object and add to collection
    //     ret.Add(new Product
    //     {
    //       ProductID = rdr.GetData<int>("ProductID"),
    //       Name = rdr.GetData<string>("Name"),
    //       ProductNumber = rdr.GetData<string>("ProductNumber"),
    //       Color = rdr.GetData<string>("Color"),
    //       StandardCost = rdr.GetData<decimal>("StandardCost"),
    //       ListPrice = rdr.GetData<decimal>("ListPrice"),
    //       SellStartDate = rdr.GetData<DateTime>("SellStartDate"),
    //       SellEndDate = rdr.GetData<DateTime>("SellEndDate"),
    //       DiscontinuedDate = rdr.GetData<DateTime>("DiscontinuedDate")
    //     });
    //   }

    //   return ret;
    // }

    //  New implememtation of the BuildEntityList method, transforms it into a generic method
    protected virtual List<TEntity> BuildEntityList<TEntity>(IDataReader rdr)
    {
      List<TEntity> ret = new();
      string colunName;
      // Get all the properties in <TEntity>
      PropertyInfo[] props = typeof(TEntity).GetProperties();
      // Loop throungh all rows in the data reader
      while (rdr.Read())
      {
        // Create new instance of Entity
        TEntity entity = Activator.CreateInstance<TEntity>();
        // Loop throungh columns in data reader
        for (int index = 0; index < rdr.FieldCount; index++)
        {
          // Get field name from data reader
          colunName = rdr.GetName(index);
          // Get property that matches the field name
          PropertyInfo col = props.FirstOrDefault(col => col.Name == colunName);
          if (col != null)
          {
            // Get the value from the table
            var value = rdr[colunName];
            // Assign value to property if not null
            if (!value.Equals(DBNull.Value))
            {
              col.SetValue(entity, value, null);
            }
          }
        }
        ret.Add(entity);
      }
      return ret;
    }









  }
}