using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ayudacard_api.Azure.BlobStorage
{
    public class BlobContainer
    {
        public static CloudBlockBlob GetBlobFromContainer(String fileName)
        {
            String cloudStorageConnectionString = ConfigurationManager.AppSettings["CloudStorageConnectionString"];
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(cloudStorageConnectionString);

            String cloudStorageContainerName = ConfigurationManager.AppSettings["CloudStorageContainerName"];
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(cloudStorageContainerName);

            cloudBlobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            cloudBlobContainer.CreateIfNotExists();

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);

            return cloudBlockBlob;
        }

        public static String GetCloudBlockBlobImageURI(String fileName)
        {
            CloudBlockBlob cloudBlockBlob = GetBlobFromContainer(fileName);
            return cloudBlockBlob.Uri.AbsoluteUri;
        }
    }
}