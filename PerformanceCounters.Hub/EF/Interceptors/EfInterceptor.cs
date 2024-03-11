using System.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace PerformanceCounters.Hub.EF.Interceptors
{
  public class EfInterceptor : DbCommandInterceptor
  {
    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
      Console.WriteLine($"Executing SQL query: {command.CommandText}");

      if (eventData.Result != null)
      {
        var dataTable = new DataTable();
        dataTable.Load(result);

        Console.WriteLine($"Index#\t {string.Join("\t", dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName))}");

        for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
        {
          var row = dataTable.Rows[rowIndex];
          Console.WriteLine($"Index: {rowIndex}\t {string.Join("\t", row.ItemArray)}");
        }

        return dataTable.CreateDataReader();
      }

      return base.ReaderExecuted(command, eventData, result);
    }

    public override async ValueTask<DbDataReader> ReaderExecutedAsync(
      DbCommand command,
      CommandExecutedEventData eventData,
      DbDataReader result,
      CancellationToken cancellationToken = default)
    {
      Console.WriteLine($"Executing SQL query: {command.CommandText}");

      if (eventData.Result != null)
      {
        var dataTable = new DataTable();
        dataTable.Load(result);

        Console.WriteLine($"Index#\t {string.Join("\t", dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName))}");

        for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
        {
          var row = dataTable.Rows[rowIndex];
          Console.WriteLine($"Index: {rowIndex}\t {string.Join("\t", row.ItemArray)}");
        }

        return dataTable.CreateDataReader();
      }

      return result;
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
      Console.WriteLine($"Executing SQL query: {command.CommandText}");
      return base.ReaderExecuting(command, eventData, result);
    }

    public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
    {
      Console.WriteLine($"Executing SQL query: {command.CommandText}");
      return base.ScalarExecuting(command, eventData, result);
    }

    public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
    {
      Console.WriteLine($"Executing SQL query: {command.CommandText}");
      return base.NonQueryExecuting(command, eventData, result);
    }
  }
}
