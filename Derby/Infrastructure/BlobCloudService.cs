using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Derby.Infrastructure
{
    public class BlobCloudService
    {
        readonly CloudBlobContainer _container;

        public BlobCloudService()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Azure.Storage"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(CloudConfigurationManager.GetSetting("Azure.Storage.Container"));
        }

        public bool DoesBlobExist(string blobName)
        {
            var blob = _container.GetBlockBlobReference(blobName);
            return blob.Exists();
        }

        public CloudBlockBlob UploadBlob(string blobName, Stream inputStream)
        {
            CloudBlockBlob blobref = _container.GetBlockBlobReference(blobName);
            blobref.UploadFromStream(inputStream);
            return blobref;
        }

        public void DeleteBlob(string blobName)
        {
            var blob = _container.GetBlockBlobReference(blobName);
            blob.Delete();
        }
    }
}