using System;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace StartServer
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
 			Copy the 'runtime' folder from your Minecraft location (usually C:\Program Files (x86)\Minecraft)
 			The Minecraft java distribution from this folder will be used to run the server
 			Ensure server jar file has the word "server" in it somewhere eg C:\Minecraft Server 1.15.2\server.1.15.2.jar
        	*/
        	// find the working directory, eg C:\Minecraft Server 1.15.2
        	string useMemory = "1024"; // default amount
        	int amount = 0;
        	string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        	File.WriteAllText(Path.Combine(appPath,"eula.txt"), "eula=true");
        	// find file "SetMemory.txt"
        	if (File.Exists(Path.Combine(appPath, "SetMemory.txt")))
        	{
        		useMemory = File.ReadAllText(Path.Combine(appPath, "SetMemory.txt"));
        		if(int.TryParse(useMemory, out amount))
				{
					if(amount < 0 || amount > 64)
					{
						amount = 1;
						File.WriteAllText(Path.Combine(appPath,"SetMemory.txt"), amount.ToString());
					}
				}
        		else
        		{
        			amount = 1;
        		}
				useMemory = GetMemoryAllocation(amount);
				Console.WriteLine("debug: using " + useMemory + " MB of RAM");
        	}
        	else
        	{
        		Console.WriteLine("How much memory do you want to use for your server?");
        		Console.WriteLine("e.g 1 = 1GB, 2 = 2GB, 8 = 8GB etc");
        		Console.WriteLine("Type the amount and press Enter");
				Console.WriteLine("If you mess it up the default 1G (1024 MB) will be used!");
				Console.WriteLine("You can edit 'SetMemory.txt' directly");
				
				if(int.TryParse(Console.ReadLine(), out amount))
				{
					if(amount < 0 || amount > 64)
					{
						amount = 1;
					}
					File.WriteAllText(Path.Combine(appPath,"SetMemory.txt"), amount.ToString());
				}
				useMemory = GetMemoryAllocation(amount);
        	}
        	//find any .jar file with the word 'server' in it
        	string[] files = Directory.GetFiles(appPath, "*.jar");
        	string serverFile = string.Empty;
        	foreach (string file in files)
        	{
        		FileInfo fi = new FileInfo(file);
        		Console.WriteLine("debug: jar file found: " + fi.Name);
        		if(fi.Name.ToLower().Contains("server"))
        		{
        			//if file name has spaces in the path, the path must be enclosed in quotes
        			serverFile = EscapeCommandLineArguments(new string[]{fi.FullName});
        			break;
        		}
        	}
        	Console.WriteLine("debug: exe running from " + appPath);
            string startFile = Path.Combine(appPath, "runtime", "jre-x64","bin","java.exe");
            Console.WriteLine("debug: about to run java from " + startFile);
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = startFile; // eg C:\Minecraft Server 1.15.2\runtime\jre-x64\bin\java.exe
            //args = " -Xmx####M -Xms####M -jar server.x.x.x.jar nogui"
            string javaArgs = " -Xmx" + useMemory + "M -Xms" + useMemory + "M -jar ";
            start.Arguments = javaArgs + serverFile + " nogui";
            Console.WriteLine("debug: arguments " + start.Arguments);
            Console.WriteLine("Starting server in 5 seconds");
            Thread.Sleep(5000);
            Process.Start(start);
            //Console.ReadKey(); //used during debugging
        }
        private static string EscapeCommandLineArguments(string[] args)
		{
			string arguments = "";
			foreach (string arg in args)
			{
				arguments += " \"" + arg.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
			}
			return arguments;
		}
        private static string GetMemoryAllocation(int GB)
        {
        	int MB = GB * 1024; // eg 1024, 2048, 4096
        	return MB.ToString();
        }
    }
}