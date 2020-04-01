# StartMinecraftServer
C# project to start a Minecraft server
There are many tutorials on Youtbe showing how to setup and start a Minecraft server. They all require a bit of work with batch files, command prompts and editing text files.
Some computers may not have Java installed.
To use the compiled StartServer.exe file you need to copy the runtime folder from C:\Program Files (x86)\Minecraft to the local folder you want to run the server from.
The C# executable StartServer.exe is placed alongside the downloaded server.jar file and the copied runtime folder.
Double-click it to start.
It will create a text file called eula.txt and write eula=true into it.
On first run it will ask how much memory (in GB) you want to allocate to your server. This will be written to a text file called SetMemory.txt
