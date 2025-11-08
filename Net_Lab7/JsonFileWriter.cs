using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Net_Lab7
{
    public class JsonFileWriter : IFileSaver
    {
        public async Task SaveAsync<T>(string filePath, List<T> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
        }
    }
}
