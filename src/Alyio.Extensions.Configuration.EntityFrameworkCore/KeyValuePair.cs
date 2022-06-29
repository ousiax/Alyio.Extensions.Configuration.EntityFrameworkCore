namespace Alyio.Extensions.Configuration.EntityFrameworkCore;

internal record KeyValuePair : IEquatable<KeyValuePair<string, string>>
{
    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;

    public bool Equals(KeyValuePair<string, string> other)
    {
        return string.Equals(Key, other.Key, StringComparison.InvariantCulture);
    }
}
