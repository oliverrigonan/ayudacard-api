using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace ayudacard_api.Azure.AzureStorage
{
    public class AzureStorageMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        private readonly CloudBlobContainer cloudBlobContainer;
        private readonly string[] cloudBlobMimeTypes = { "image/png", "image/jpeg", "image/jpg" };

        public AzureStorageMultipartFormDataStreamProvider(CloudBlobContainer blobContainer) : base("azure")
        {
            cloudBlobContainer = blobContainer;
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));
            if (headers == null) throw new ArgumentNullException(nameof(headers));

            if (!cloudBlobMimeTypes.Contains(headers.ContentType.ToString().ToLower()))
            {
                throw new NotSupportedException("Only jpeg and png are supported");
            }

            String fileName = Guid.NewGuid().ToString();

            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(fileName);

            if (headers.ContentType != null)
            {
                blob.Properties.ContentType = headers.ContentType.MediaType;
            }

            FileData.Add(new MultipartFileData(headers, blob.Name));

            return blob.OpenWrite();
        }
    }
}