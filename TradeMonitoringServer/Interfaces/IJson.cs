
namespace TradeMonitoringServer
{
    /// <summary>
    /// Classes which can be converted to Json
    /// </summary>
    public interface IJson
    {
        /// <summary>
        /// Convert object to Json
        /// </summary>
        string ToJson();
    }

    public interface ICloneable<T>
    {
        T Clone();

    }

}