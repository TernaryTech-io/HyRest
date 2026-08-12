using Ternary.DataConversions.Extensions;

namespace HyRest.CaseManagement;

public class ColumnAttribute : OnBaseRestService
{
    private OnBaseWorkView _module => (OnBaseWorkView)base.Module;
    private ColumnModel _item { get; set; }
    //We can't do anything yet until we know the parent class of the attribute.
    List<long> DataAddressParts => _item.DataAddress.Split('.').Select(p => p.ConvertTo<long>()).ToList();
    internal ColumnAttribute(OnBaseWorkView module, ColumnModel item, Class attributeClass) : base(module)
    {
        _item = item;
        AttributeClass = attributeClass;
    }
    public Class AttributeClass { get; }
    public Attribute? Attribute => AttributeClass.Attributes.FirstOrDefault(a => a.Id == DataAddressParts.Last());
    public AttributeDataType DataType => Attribute?.DataType ?? AttributeDataType.Get(_item.DataType);
    public int Width => _item.Width.ConvertTo<int>();
    public string DataAddress => _item.DataAddress;
}

public class EntryConstraintAttribute : OnBaseRestService
{
    private OnBaseWorkView _module => (OnBaseWorkView)base.Module;
    List<long> DataAddressParts
        => _item.DataAddress != null ? _item.DataAddress.Split('.').Select(p => p.ConvertTo<long>()).ToList() : [];
    private EntryConstraintModel _item { get; set; }
    internal EntryConstraintAttribute(OnBaseWorkView module, EntryConstraintModel item, Class attributeClass) : base(module)
    {
        _item = item;
        AttributeClass = attributeClass;
    }
    public Class AttributeClass { get; }
    public Attribute? Attribute => AttributeClass.Attributes.FirstOrDefault(a => a.Id == DataAddressParts.Last());
    public AttributeDataType DataType => Attribute?.DataType ?? AttributeDataType.Get(_item.DataType);
    public string? Prompt => _item.Prompt;
    public string? DataAddress => _item.DataAddress;
    public DataSetOptions DataSetOptions => _item.DataSetOptions;
    public Operator Operator => _item.Operator;
}

public class FixedConstraintAttribute : OnBaseRestService
{
    private OnBaseWorkView _module => (OnBaseWorkView)base.Module;
    List<long> DataAddressParts
        => _item.DataAddress != null ? _item.DataAddress.Split('.').Select(p => p.ConvertTo<long>()).ToList() : [];
    private ConstraintModel _item { get; set; }
    internal FixedConstraintAttribute(OnBaseWorkView module, ConstraintModel item, Class attributeClass) : base(module)
    {
        _item = item;
        AttributeClass = attributeClass;
    }
    public Class AttributeClass { get; }
    public Attribute? Attribute => AttributeClass.Attributes.FirstOrDefault(a => a.Id == DataAddressParts.Last());
    public AttributeDataType DataType => Attribute.DataType;
    public string? Value => _item.Value;
    public string? DataAddress => _item.DataAddress;
    public Operator Operator => _item.Operator;
    public bool LeftParenthesis => (int)_item.LeftParenthesisCount > 0;
    public bool RightParenthesis => (int)_item.RightParenthesisCount > 0;
}

public class SortAttribute : OnBaseRestService
{
    private OnBaseWorkView _module => (OnBaseWorkView)base.Module;
    List<long> DataAddressParts
        => _item.DataAddress != null ? _item.DataAddress.Split('.').Select(p => p.ConvertTo<long>()).ToList() : [];
    private SortModel _item { get; set; }
    internal SortAttribute(OnBaseWorkView module, SortModel item, Class attributeClass) : base(module)
    {
        _item = item;
        AttributeClass = attributeClass;
    }
    public Class AttributeClass { get; }
    public Attribute? Attribute => AttributeClass.Attributes.FirstOrDefault(a => a.Id == DataAddressParts.Last());
    public AttributeDataType DataType => Attribute.DataType;
    public string? DataAddress => _item.DataAddress;
    public SortOrder SortOrder => _item.SortOrder;
}