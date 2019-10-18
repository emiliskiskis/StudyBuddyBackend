using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StudyBuddyBackend.Database
{
    public static class AsyncReader
    {
        public static async Task<string> ReadAll(Stream s)
        {
            await using var stream = new MemoryStream();
            byte[] buffer = new byte[2048];
            int bytesRead;
            while ((bytesRead = await s.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                stream.Write(buffer, 0, bytesRead);
            }

            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}
