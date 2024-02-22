using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Jido.Config;

public class JidoConfig
{
    private readonly JsonSerializerOptions _serializerOptions;

    public JidoConfig() => _serializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true
    };

    [JsonIgnore] private string PersistentFileLocation { get; set; }

    #region Methods

    /// <summary>
    /// Persist `this` in a file
    /// </summary>
    public void Persist()
    {
        using var sw = new StreamWriter(PersistentFileLocation);
        var json = JsonSerializer.Serialize(this, _serializerOptions);
        sw.Write(json);
    }

    /// <summary>
    /// Persist `this` in a file asynchronously
    /// </summary>
    public async Task PersistAsync()
    {
        await using var sw = new StreamWriter(PersistentFileLocation);
        var json = JsonSerializer.Serialize(this, _serializerOptions);
        await sw.WriteAsync(json);
    }

    /// <summary>
    /// Initialize the config with data
    /// </summary>
    /// <param name="persistentFileLocation">Location of the file to persist the data to</param>
    /// <returns></returns>
    public static JidoConfig Init(string persistentFileLocation)
    {
        using var sr = new StreamReader(persistentFileLocation);
        var config = JsonSerializer.Deserialize<JidoConfig>(sr.ReadToEnd()) ?? new JidoConfig();
        config.PersistentFileLocation = persistentFileLocation;
        return config;
    }

    #endregion Methods
}
