
namespace TradeMonitoringServer
{
    /// <summary>
    /// Can be cloned
    /// Not using ICloneable because it is not recommended
    /// https://docs.microsoft.com/en-us/dotnet/api/system.icloneable?view=netcore-3.1
    /// </summary>
    public interface IClone<T>
    {
        T Clone();

    }

}