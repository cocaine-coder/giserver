namespace Giserver.Core.JsToDb;

public class JsTypeValue : DbColumnValue<string>
{
    public JsType Type { get; set; }
}