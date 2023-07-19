using Azure.Storage.Blobs;

namespace back_end.Utilidades
{
    public class AlmacenadorAzureStorage : IAlmacenadorArchivos
    {
        private string connectionString;

        public AlmacenadorAzureStorage(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task<string> GuardarArchivo(string contenedor, IFormFile archivo)
        {
            var cliente = new BlobContainerClient(connectionString, contenedor);
            await cliente.CreateIfNotExistsAsync();
            cliente.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

            var extension = Path.GetExtension(archivo.FileName);
            var archivoNombre = $"{Guid.NewGuid()}{extension}";
            var blob = cliente.GetBlobClient(archivoNombre);
            await blob.UploadAsync(archivo.OpenReadStream());
            return blob.Uri.ToString();
        }

        public async Task BorrarArchivos(string ruta, string contenedor)
        {
            if (string.IsNullOrEmpty(ruta))
            {
                return;
            }

            var client = new BlobContainerClient(connectionString, contenedor);
            await client.CreateIfNotExistsAsync();
            var archivo = Path.GetFileName(ruta);
            var blob = client.GetBlobClient(archivo);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<string> EditarArchivo(string contenedor, IFormFile archivo, string ruta)
        {
            await BorrarArchivos(ruta, contenedor);
            return await GuardarArchivo(contenedor, archivo);
        }
    }
}
