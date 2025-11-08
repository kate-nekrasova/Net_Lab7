namespace Net_Lab7
{
    public interface IFileLoader
    {
        Task<List<T>>? LoadAsync<T>(string filePath);
    }
}
