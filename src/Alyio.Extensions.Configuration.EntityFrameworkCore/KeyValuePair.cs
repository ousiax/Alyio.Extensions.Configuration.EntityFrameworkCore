namespace Alyio.Extensions.Configuration.EntityFrameworkCore;

internal record KeyValuePair
{
    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;
}
