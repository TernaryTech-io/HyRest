namespace HyRest.Cache;

public struct CacheKey
{
    public string? Prefix { get; set; }
    public string TypeOf { get; set; }
    public string IdOf { get; set; }
    public override string ToString()
    {
        return $"{Prefix ?? Prefix + "-"}{TypeOf}-{IdOf}";
    }
    public static CacheKey Create<T>(T item, string? prefix = null)
        where T : class, IOnBaseIdentifiable
    {
        return new CacheKey
        {
            Prefix = prefix,
            TypeOf = item.GetType().Name,
            IdOf = item.Id.ToString()
        };
    }
    public static CacheKey Create(string id, Type type, string? prefix = null)
    {
        return new CacheKey
        {
            Prefix = prefix,
            TypeOf = type.Name,
            IdOf = id
        };
    }
}
