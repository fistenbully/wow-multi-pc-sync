<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.IO.Compression.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.IO.Compression.FileSystem.dll</Reference>
  <NuGetReference>WindowsAzure.Storage</NuGetReference>
  <Namespace>System.IO.Compression</Namespace>
  <Namespace>Microsoft.WindowsAzure.Storage</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Microsoft.WindowsAzure.Storage.Blob</Namespace>
</Query>

string containerName = "wowshare";
string connectionString = "";
string accountName = "";
string downloadPath = @"d:\temp\wowsharetest";

async Task Main()
{
	var blobs = GetMachines();
	blobs.Select(b => new Hyperlinq(() => DownloadZips(b), b)).Dump();
}

private void DownloadZips(string path)
{
	var cloudAccount = CloudStorageAccount.Parse(connectionString);

	var client = cloudAccount.CreateCloudBlobClient();
	var container = client.GetContainerReference(containerName);
	var p = string.Join("/", path.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray());
	var blob = container.GetDirectoryReference(p);
	var blobs = blob.ListBlobs();
	
	foreach (var element in blobs.OfType<CloudBlockBlob>())
	{
		var b = container.GetBlockBlobReference(element.Name);
		var target = Path.Combine(downloadPath, b.Name);
		if(!Directory.Exists(Path.GetDirectoryName(target)))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(target));
		}
		$"Download file: {b.Name}".Dump();
		b.DownloadToFile(target, FileMode.CreateNew);
	}
	
	"Download completed!".Dump();
}

private List<string> GetMachines()
{
	var cloudAccount = CloudStorageAccount.Parse(connectionString);

	var client = cloudAccount.CreateCloudBlobClient();
	var container = client.GetContainerReference(containerName);

	var blob = container.GetDirectoryReference(accountName);

	return blob.ListBlobs().Select(b => b.Uri.AbsolutePath).ToList();
}