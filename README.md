# RestHttpClient
Simple REST HttpClient with authentication strategy



## IRestHttpSerializer for System.Text.Json.Serializer

```
public class SystemTextJsonSerializer : IRestHttpSerializer
{
    public static JsonSerializerOptions DefaultSerializerOptions = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public string SerializeContent<TContent>(TContent content)
    {
        return JsonSerializer.Serialize(content, _jsonSerializerOptions);
    }

    public TContent DeserializeContent<TContent>(string content)
    {
        return JsonSerializer.Deserialize<TResponse>(result, _jsonSerializerOptions);
    }
}
```



## IRestHttpSerializer for NewtonJsonSerializer

```
public class NewtonJsonSerializer : IRestHttpSerializer
{
    public string SerializeContent<TContent>(TContent content)
    {
        return JsonConvert.SerializeObject(content);
    }

    public TContent DeserializeContent<TContent>(string content)
    {
        return JsonConvert.DeserializeObject<TContent>(content);
    }
}
```