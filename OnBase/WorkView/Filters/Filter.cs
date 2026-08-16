namespace HyRest.OnBase.WorkView;

public class Filter : OnBaseItemTypeService<OnBaseWorkView, FilterModel>
{
    private bool _hydrated { get; set; } = false;
    private Class? _class { get; set; }
    private List<ColumnAttribute> _columns { get; set; }
    private List<EntryConstraintAttribute> _entryConstraints { get; set; }
    private List<FixedConstraintAttribute> _fixedConstraints { get; set; }
    private List<SortAttribute> _sortAttributes { get; set; }
    internal Filter(OnBaseWorkView module, FilterModel item) : base(module, item)
    {

    }
    public Class Class
    {
        get
        {
            if (_class == null)
            {
                if (!_hydrated)
                    GetFilterDetails().Wait(Module.App.RequestTimeOut);
                _class = Module.Classes.Find(Item.ClassId);
            }
            return _class;
        }
    }
    public IReadOnlyCollection<ColumnAttribute> Columns
    {
        get
        {
            if (!_hydrated)
                GetFilterDetails().Wait(Module.App.RequestTimeOut);
            return _columns;
        }
    }
    public IReadOnlyCollection<EntryConstraintAttribute> EntryConstraints
    {
        get
        {
            if (!_hydrated)
                GetFilterDetails().Wait(Module.App.RequestTimeOut);
            return _entryConstraints;
        }
    }
    public IReadOnlyCollection<FixedConstraintAttribute> FixedConstraints
    {
        get
        {
            if (!_hydrated)
                GetFilterDetails().Wait(Module.App.RequestTimeOut);
            return _fixedConstraints;
        }
    }
    public IReadOnlyCollection<SortAttribute> SortAttributes
    {
        get
        {
            if (!_hydrated)
                GetFilterDetails().Wait(Module.App.RequestTimeOut);
            return _sortAttributes;
        }
    }
    private async Task GetFilterDetails()
    {
        var details = await Module.Service.GetFilter(Item.Id);
        if (details != null)
        {
            Item.ColumnAttributes = details.ColumnAttributes;
            Item.EntryConstraints = details.EntryConstraints;
            Item.FixedConstraints = details.FixedConstraints;
            Item.SortAttributes = details.SortAttributes;
            Item.ClassId = details.ClassId;
        }
        _hydrated = true;
    }
}


