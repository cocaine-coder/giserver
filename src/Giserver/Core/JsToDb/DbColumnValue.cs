namespace Giserver.Core.JsToDb;

public class DbColumnValue<T>
{
    public string ColumnName { get; set; }

    public T? Value { get; set; }
}