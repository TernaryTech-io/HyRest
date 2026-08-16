using System.Text.Json.Serialization;
using HyRest.Utilities;

namespace HyRest.OnBase.Core;

public class StandAloneKeywords : EditableKeywordGroup, IModifiableKeywordRecord
{
    internal StandAloneKeywords(OnBaseCore core, KeywordGroupModel group) : base(core, group)
    {
        
    }
    [JsonIgnore]
    private string? Name => null;
    [JsonIgnore]
    private string? SystemName => null;    
    [JsonIgnore]
    private long? Id => null;
    [HyRestConverter<JsonStringEnumConverter>]
    public override KeywordTypeGroupType GroupType => KeywordTypeGroupType.StandAlone;
    /// <summary>
    /// Will always return null for StandAloneKeywords
    /// </summary>
    [JsonIgnore]
    public override KeywordTypeGroup? KeywordTypeGroup => null;
    public override string? ToJson()
        => JsonUtility.Serialize(this);
}