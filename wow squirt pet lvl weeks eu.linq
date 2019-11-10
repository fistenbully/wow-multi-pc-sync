<Query Kind="Program" />

void Main()
{
	var endDate = DateTime.Parse("2020.12.31");
	var squietDate = DateTime.Parse("2019.08.16");
	
	var squirtUp = GetDays(squietDate, 15,endDate).Dump("Squirt");
	
 var pws = DateTime.Parse("2019.08.28");
var pW = PW(pws, endDate).Dump("Weekly");
 
 
 var sqDay = pW.Where(p => squirtUp.Any(u => u == p  ));
 sqDay.Dump();
 
}

public IEnumerable<DateTime> PW(DateTime s, DateTime till)
{
	var cD = s;
	do
	{
		yield return cD;
		
		for (int i = 0; i < 6; i++)
		{
			yield return cD.AddDays(i);
		}
		
		cD = cD.AddDays(7*7);
	}
	while (cD < till);
}

private IEnumerable<DateTime> GetDays(DateTime start, int daysToAdd, DateTime till)
{
	var cD = start;
	do{
		cD = cD.AddDays(daysToAdd);
		yield return cD;
	}
	while (cD < till);
	
}

// Define other methods and classes here
