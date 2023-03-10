using adventureWorks.Dal;
using System.Data;
using System.Data.SqlClient;

namespace adventureWorks.Common
{
  public partial class SqlServerDatabaseContext : DatabaseContext
  {
    public SqlServerDatabaseContext(string connectString) : base(connectString) { }
    protected override void Init()
    {
      base.Init();
      ParameterPrefix = "@";
    }
    public override SqlConnection CreateConnection(string connectionString)
    {
      return new SqlConnection(connectionString);
    }
    public override SqlCommand CreateCommand(IDbConnection cnn, string sql)
    {
      CommandObject = new SqlCommand(sql, (SqlConnection)cnn);
      CommandObject.CommandType = CommandType.Text;
      return (SqlCommand)CommandObject;
    }
    public override SqlParameter CreateParameter(string paramName, object value)
    {
      if (!paramName.StartsWith(ParameterPrefix))
      {
        paramName = ParameterPrefix + paramName;
      }

      return new SqlParameter(paramName, value);
    }
    public override SqlParameter CreateParameter()
    {
      return new SqlParameter();
    }
    public override SqlParameter GetParameter(string paramName)
    {
      if (!paramName.StartsWith(ParameterPrefix))
      {
        paramName = ParameterPrefix + paramName;
      }
      return ((SqlCommand)CommandObject).Parameters[paramName];
    }
    public override SqlDataReader CreateDataReader(IDbCommand cmd, CommandBehavior cmdBehavior = CommandBehavior.CloseConnection)
    {
      // Open Connection
      cmd.Connection.Open();
      // Create DataReader
      DataReaderObject = cmd.ExecuteReader(cmdBehavior);
      return (SqlDataReader)DataReaderObject;


    }
  }
}