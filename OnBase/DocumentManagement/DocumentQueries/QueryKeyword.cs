using System.Text.Json.Serialization;
using HyRest.Utilities;

namespace HyRest.DocumentManagement;

public class QueryKeyword : OnBaseRestService<IOnBaseDocumentAPI>
{
    private OnBaseCore _core => (OnBaseCore)base.Module;
    private KeywordType? _keywordType { get; set; }
    internal QueryKeyword(OnBaseCore core) : base(core)
    {
       
    }
    /// <summary>
    /// Get's the keyword type Id for the keyword, If you set this value to the Keyword Type Name, the id will be retrieved.
    /// </summary>
    public string Id { get => _keywordType?.Id.ToString() ?? string.Empty; set => SetKeywordTypeId(value); }
    /// <summary>
    /// The keyword value.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Represents the operator for the keyword value of
    /// <br/>this query keyword. Defaults to Equal if not present.
    /// </summary>
    [HyRestConverter<JsonStringEnumConverter>]
    public QueryKeywordOperator Operator { get; set; }

    /// <summary>
    /// Represents the relation of this query keyword to
    /// <br/>other query keywords. Defaults to And if not present.
    /// </summary>
    [HyRestConverter<JsonStringEnumConverter>]
    public QueryKeywordRelation Relation { get; set; }

    private void SetKeywordTypeId(string? value)
    {
        if (value == null)
            return;
        if (long.TryParse(value, out long result))
        {
            _keywordType = _core.KeywordTypes.Find(result);
        }
        else
            _keywordType = _core.KeywordTypes.Find(value);
    }
    internal QueryKeywordModel GetModel()
    {
        if (_keywordType != null)
        {
            var provider = _keywordType.CreateKeywordDataTypeHandler();
            return new QueryKeywordModel
            {
                Id = Id,
                Value = provider.ToString(Value),
                Operator = Operator,
                Relation = Relation
            };
        }
        else
            throw new Exception("The keyword type could not be located.");
        
    }
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}