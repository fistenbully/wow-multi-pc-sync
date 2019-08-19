<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.IO.Compression.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.IO.Compression.FileSystem.dll</Reference>
  <NuGetReference>WindowsAzure.Storage</NuGetReference>
  <Namespace>System.IO.Compression</Namespace>
  <Namespace>Microsoft.WindowsAzure.Storage</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

string  containerName = "wowshare";
string connectionString = "";
	string accountName = "";
	
async Task Main()
{
	var wowFolderPath = @"F:\World of Warcraft\_retail_";
	var interfaceFolder = "Interface";
	var wtf = "WTF";

	var output = @"D:\temp";

	var machineName = Environment.MachineName;

	var files = new List<string>{
		Zip(wowFolderPath, output, interfaceFolder),
		Zip(wowFolderPath, output, wtf)
	};
	
	await UploadFilesToClod(files,accountName, machineName);
}

private async Task UploadFilesToClod(List<string> files, string accountName, string machineName)
{

	
	var cloudAccount = CloudStorageAccount.Parse(connectionString);

	var client = cloudAccount.CreateCloudBlobClient();
	var container = client.GetContainerReference(containerName);

	foreach (var element in files)
	{
		var blob = container.GetBlockBlobReference(Path.Combine(accountName, machineName, Path.GetFileName(element)));

		await blob.UploadFromFileAsync(element);
	}
}

private string Zip(string wowFolder, string output, string kind)
{
	var inputFolder = Path.Combine(wowFolder, kind);
	var outputFile = Path.Combine(output, $"{kind}.zip");

	if (File.Exists(outputFile))
	{
		File.Delete(outputFile);
	}

	ZipFile.CreateFromDirectory(inputFolder, outputFile);

	return outputFile;
}

// Define other methods and classes here