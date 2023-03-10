using System.Data;

namespace adventureWorks.Common
{
  public static class DataReaderExtensions
    {
        public static T GetData<T>(this IDataReader dr, string name, T retunValue = default){
            var value = dr[name];
            if(!value.Equals(DBNull.Value)){
                retunValue = (T)value;
            }
            return retunValue;
        }
    }
}