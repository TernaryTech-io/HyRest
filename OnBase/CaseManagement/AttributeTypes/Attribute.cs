using System.Globalization;
using Ternary.DataConversions.Providers;

namespace HyRest.CaseManagement;

public class Attribute : OnBaseItemTypeService<IOnBaseWorkViewAPI, OnBaseWorkView, AttributeModel>
{
    internal IDataTypeConversionProvider handler => DataType.GetProvider(this);
    internal CultureInfo Culture => new CultureInfo(Module.App.ClientOptions.DefaultLanguage);
    public Attribute(OnBaseWorkView module, AttributeModel item) : base(module, item)
    {
        Console.WriteLine($"Attribute: {item.Name} ({item.DataType})");
    }
    public AttributeDataType DataType => AttributeDataType.Alphanumeric;//AttributeDataType.Get(Item.DataType);
}