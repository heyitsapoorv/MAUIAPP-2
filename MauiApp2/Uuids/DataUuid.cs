namespace MauiApp2.Uuids;

public class DataUuid
{
    public static Guid[] DataServiceUuids { get; private set; } = new Guid[] { new Guid("0003ABCD-0000-1000-8000-00805F9B0131") }; //"Heart Rate Service"
    public static Guid DataServiceUuid { get; private set; } = new Guid("0003ABCD-0000-1000-8000-00805F9B0131"); //"Heart Rate Service"
    public static Guid DataCharacteristicUuid { get; private set; } = new Guid("00031201-0000-1000-8000-00805f9b0130"); //"Heart Rate Measurement"
    
}