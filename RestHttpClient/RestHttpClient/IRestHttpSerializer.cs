namespace RestHttpClient
{
    public interface IRestHttpSerializer
    {
        string SerializeContent<TContent>(TContent content);
        TContent DeserializeContent<TContent>(string content);
    }
}