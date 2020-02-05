<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.IO.Compression.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.IO.Compression.FileSystem.dll</Reference>
  <NuGetReference>WindowsAzure.Storage</NuGetReference>
  <Namespace>System.IO.Compression</Namespace>
  <Namespace>Microsoft.WindowsAzure.Storage</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

string containerName = "wowshare";
string connectionString = "";
string accountName = "";
string wowFolderPath = @"C:\Program Files (x86)\World of Warcraft\_retail_";

async Task Main()
{
	var addons = Path.Combine(wowFolderPath, @"Interface\AddOns");
	var wtf = Path.Combine(wowFolderPath, @"WTF");

	var lastChangedDate = DateTime.Now;

	var fswAddon = new FileSystemWatcher(addons);
	ManualResetEvent workToDo = new ManualResetEvent(false);
	fswAddon.NotifyFilter = NotifyFilters.LastWrite;
	fswAddon.Changed += (source, e) => { workToDo.Set(); };
	fswAddon.Created += (source, e) => { workToDo.Set(); };
	fswAddon.IncludeSubdirectories = true;
	fswAddon.EnableRaisingEvents = true;

	var fswWtf = new FileSystemWatcher(wtf);
	fswWtf.NotifyFilter = NotifyFilters.LastWrite;
	fswWtf.Changed += (source, e) => { workToDo.Set(); };
	fswWtf.Created += (source, e) => { workToDo.Set(); };
	fswWtf.IncludeSubdirectories = true;
	fswWtf.EnableRaisingEvents = true;

	var timer = new System.Timers.Timer(5000);
	timer.Elapsed += async (source, e) =>
	 {
	 	"triggered".Dump();
		 var output = @"c:\temp";

		 var machineName = Environment.MachineName;

		 var files = new List<string>{
		Zip(addons, output),
		Zip(wtf, output )
		 };
		 
		 timer.Stop();
		 //	await UploadFilesToClod(files, accountName, machineName);
	 };

	while (true)
	{
		if (workToDo.WaitOne())
		{
			workToDo.Reset();

			if (timer.Enabled)
			{
				timer.Stop();
			}

			timer.Start();

		}

	}
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

private string Zip(string path, string output)
{
	var inputFolder = path;
	var kind = Path.GetFileName(path);
	var outputFile = Path.Combine(output, $"{kind}.zip");

	if (File.Exists(outputFile))
	{
		File.Delete(outputFile);
	}

	ZipFile.CreateFromDirectory(inputFolder, outputFile);

	return outputFile;
}

// Define other methods and classes here