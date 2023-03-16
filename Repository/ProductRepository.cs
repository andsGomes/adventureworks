using System.Text;

using System.Reflection;
using System.Data;
using adventureWorks.Common;
using adventureWorks.Entities;
using System.ComponentModel.DataAnnotations.Schema;
namespace adventureWorks.Repository
{
  public class ProductRepository
  {

    public string SchemaName { get; set; }
    public string TableName { get; set; }
    public List<ColumnMapper> Columns { get; set; }
    public string SQL { get; set; }

    public ProductRepository()
    {
      Init();
    }
    protected virtual void Init()
    {
      SchemaName = "dbo";
      TableName = string.Empty;
      SQL = string.Empty;
      Columns = new List<ColumnMapper>();
    }

    // public virtual List<Product> Search(string connectString, string sql)
    // {
    //   List<Product> ret;
    //   using SqlServerDatabaseContext dbContext = new SqlServerDatabaseContext(connectString);
    //   // Create Command Object with Sql
    //   dbContext.CreateCommand(sql);
    //   ret = BuildEntityList(dbContext.CreateDataReader());
    //   return ret;
    // }

    public virtual List<TEntity> Search<TEntity>(string connectString)
    {
      List<TEntity> ret;
      
      // Build SELECT statement
      SQL = BuildSelectSql<TEntity>();

      using SqlServerDatabaseContext dbContext = new(connectString);
      dbContext.CreateCommand(SQL);

      ret = BuildEntityList<TEntity>(dbContext.CreateDataReader());

      return ret;
    }
    protected virtual void SetTableAndSchemaName(Type typ)
    {
      // Is there is a [Table] attribute?
      TableAttribute table = typ.GetCustomAttribute<TableAttribute>();
      // Assume table name is the class name 
      TableName = typ.Name;
      if (table != null)
      {
        // Set properties form [table] attribute
        TableName = table.Name;
        SchemaName = table.Schema ?? SchemaName;
      }
    }
    // Add method to create a SELECT statement
    protected virtual string BuildSelectSql<TEntity>()
    {
      Type typ = typeof(TEntity);
      StringBuilder sb = new StringBuilder(2048);
      string comma = string.Empty;
      // Build column mapping collection 
      Columns = BuildColumnCollection<TEntity>();
      // Set Table and Schema properties; 
      SetTableAndSchemaName(typ);
      // Build the SELECT statemnet
      sb.Append("SELECT ");
      foreach (ColumnMapper item in Columns)
      {
        // Add column
        sb.Append($"{comma}[{item.ColumnName}]");
        comma = ", ";
      }
      // Add From shema.table
      sb.Append($"From {SchemaName}.{TableName}");
      return sb.ToString();
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
    //  New generic implementation of the method to create the query return colletion
    protected virtual List<ColumnMapper> BuildColumnCollection<TEntity>()
    {
      List<ColumnMapper> ret = new List<ColumnMapper>();
      ColumnMapper colMap;
      // Get all the properties in <TEntity>
      PropertyInfo[] props = typeof(TEntity).GetProperties();
      // Loop Through all properties
      foreach (PropertyInfo prop in props)
      {
        //Is there a [NotMapped] attribute?
        NotMappedAttribute nm = prop.GetCustomAttribute<NotMappedAttribute>();
        // Only add properties that map to a column
        if (nm == null)
        {
          // Create a column mapping object
          colMap = new ColumnMapper()
          {
            PropertyInfo = prop,
            ColumnName = prop.Name,
          };
          // Is column name in [Colunm] attr
          ColumnAttribute ca = prop.GetCustomAttribute<ColumnAttribute>();
          if (ca != null && !string.IsNullOrEmpty(ca.Name))
          {
            // Set column name from [Column] attr
            colMap.ColumnName = ca.Name;
          }
          // Create colletion of columns
          ret.Add(colMap);
        }

      }
      return ret;

    }
    //  New implememtation of the BuildEntityList method, transforms it into a generic method
    protected virtual List<TEntity> BuildEntityList<TEntity>(IDataReader rdr)
    {
      List<TEntity> ret = new();

      // Loop through all rows in the data reader
      while (rdr.Read())
      {
        // Create new instance of Entity
        TEntity entity = Activator.CreateInstance<TEntity>();

        // Loop through columns collection
        for (int index = 0; index < Columns.Count; index++)
        {
          // Get the value from the reader
          var value = rdr[Columns[index].ColumnName];

          // Assign value to the property if not null
          if (!value.Equals(DBNull.Value))
          {
            Columns[index].PropertyInfo.SetValue(entity, value, null);
          }
        }

        // Add new entity to the list
        ret.Add(entity);
      }

      return ret;
    }










  }
}