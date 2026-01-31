namespace MEDVBiServ.HelperClass
{
    using MEDVBiServ.Models;
    using System.IO.Compression;
    using System.Text;
    using System.Text.Json;

    public static class MedvPresSerializer
    {
        public static byte[] ToMedvPresBytes(MedvPresProject project)
        {
            var json = JsonSerializer.Serialize(project, new JsonSerializerOptions
            {
                WriteIndented = false
            });

            var raw = Encoding.UTF8.GetBytes(json);

            using var ms = new MemoryStream();
            using (var gzip = new GZipStream(ms, CompressionLevel.SmallestSize, leaveOpen: true))
            {
                gzip.Write(raw, 0, raw.Length);
            }
            return ms.ToArray();
        }

        public static MedvPresProject FromMedvPresBytes(byte[] bytes)
        {
            using var input = new MemoryStream(bytes);
            using var gzip = new GZipStream(input, CompressionMode.Decompress);
            using var output = new MemoryStream();
            gzip.CopyTo(output);

            var json = Encoding.UTF8.GetString(output.ToArray());
            return JsonSerializer.Deserialize<MedvPresProject>(json) ?? new MedvPresProject();
        }
    }

}
