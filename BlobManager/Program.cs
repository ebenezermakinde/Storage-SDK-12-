using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Threading.Tasks;

public class Program 
{
  private const string blobServiceEndpoint = "";
  private const string storageAccountName = "";
  private const string storageAccountKey = "";

  public static async Task Main(string[] args)
  {
    /*
    Connect to the storage account
    */
    StorageSharedKeyCredential accountCredentials = new StorageSharedKeyCredential(storageAccountName,storageAccountKey);
    BlobServiceClient serviceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), accountCredentials);
    AccountInfo info = await serviceClient.GetAccountInfoAsync();

    /*
    Print out metadata about the storage account
    */
    await Console.Out.WriteLineAsync($"Connected to Azure Storage Account: {storageAccountName}");
    await Console.Out.WriteLineAsync($"Account name:\t{storageAccountName}");
    await Console.Out.WriteLineAsync($"Account kind:\t{info?.AccountKind}");
    await Console.Out.WriteLineAsync($"Account sku:\t{info?.SkuName}");

    /*
    Invoke the EnumerateContainerAsync Method
    */
    await EnumerateContainersAsync(serviceClient);

    string existingContainerName = "mycontainer1";
    await EnumerateBlobsAsync(serviceClient, existingContainerName);


    /*
    Create the new container and assign the newContainerName
    */
    string newContainerName = "mycontainer1-obey";
    BlobContainerClient containerClient = await GetContainerAsync(serviceClient, newContainerName);

    string uploadedBlobName = "blobname.jpg";
    BlobClient blobClient = await GetBlobAsync(containerClient, uploadedBlobName);

    await Console.Out.WriteLineAsync($"Blob Url:\t{blobClient.Uri}");
  }

  /*
  List the containers in a blob
  */
  private static async Task EnumerateContainersAsync(BlobServiceClient client)
  {
    await foreach (BlobContainerItem container in client.GetBlobContainersAsync()) {
      await Console.Out.WriteLineAsync($"Container:\t{container.Name}");
    }
  }

  /*
  List the blobs within a container
  */
  private static async Task EnumerateBlobsAsync(BlobServiceClient client, string containerName)
  {
    BlobContainerClient container = client.GetBlobContainerClient(containerName);
    await Console.Out.WriteLineAsync($"Searching:\t{container.Name}");

    await foreach (BlobItem blob in container.GetBlobsAsync())
    {
        await Console.Out.WriteLineAsync($"Existing Blob:\t{blob.Name}");
    }
  }

  /*
  Creating a new container.
  */
  private static async Task<BlobContainerClient> GetContainerAsync(BlobServiceClient client, string containerName) 
  {
    BlobContainerClient container = client.GetBlobContainerClient(containerName);
    await container.CreateIfNotExistsAsync(PublicAccessType.Blob);
    await Console.Out.WriteLineAsync($"New container:\t{container.Name}");

    return container;
  }

  /*
  Access a blob Uri
  */
  private static async Task<BlobClient> GetBlobAsync(BlobContainerClient client, string blobName)
  {
    BlobClient blob = client.GetBlobClient(blobName);
    await Console.Out.WriteLineAsync($"Blob found:\t{blob.Name}");

    return blob;
  }
}