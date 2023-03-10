using System.Data;
namespace adventureWorks.Dal
{
  public abstract partial class DatabaseContext : IDisposable
  {
    public DatabaseContext(string connectString)
    {
      ConnectionString = connectString;
      Init();
    }

    public string ConnectionString { get; set; }
    public string ParameterPrefix { get; set; }
    public IDbCommand CommandObject { get; set; }
    public IDataReader DataReaderObject { get; set; }

    public virtual IDbConnection CreateConnection()
    {
      return CreateConnection(ConnectionString);
    }
    public abstract IDbConnection CreateConnection(string connectString);

    public virtual IDbCommand CreateCommand(string sql)
    {
      return CreateCommand(CreateConnection(), sql);
    }
    public abstract IDbCommand CreateCommand(IDbConnection cnn, string sql);
    public abstract IDataParameter CreateParameter(string paramName, object value);
    public abstract IDataParameter CreateParameter();
    public abstract IDataParameter GetParameter(string paramName);

    public virtual IDataReader CreateDataReader()
    {
      return CreateDataReader(CommandObject, CommandBehavior.CloseConnection);
    }

    public virtual IDataReader CreateDataReader(CommandBehavior cmdBehavior)
    {
      return CreateDataReader(CommandObject, cmdBehavior);
    }
    public virtual IDataReader CreateDataReader(IDbCommand cmd, CommandBehavior cmdBehavior = CommandBehavior.CloseConnection)
    {
      // Open Connection
      cmd.Connection.Open();

      // Create DataReader
      DataReaderObject = cmd.ExecuteReader(cmdBehavior);
      return DataReaderObject;
    }

    public void Dispose()
    {
      // Close/Dispose of data reader object
      if (DataReaderObject != null)
      {
        DataReaderObject.Close();
        DataReaderObject.Dispose();
      }

      //Close/Dispose of command object
      if (CommandObject != null)
      {
        if (CommandObject.Connection != null)
        {
          if (CommandObject.Transaction != null)
          {
            CommandObject.Transaction.Dispose();
          }
          CommandObject.Connection.Close();
          CommandObject.Connection.Dispose();
        }
        CommandObject.Dispose();
      }

    }

    protected virtual void Init()
    {
      ParameterPrefix = string.Empty;
    }


  }
}