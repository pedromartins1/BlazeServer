I am releasing a somewhat-functional Blaze emulator.
You can use it to join a Battlefield 3 dedicated server.
Multiple players are not supported.

You have to use this with a BF3 1.6.0.0 exe that has SSL check disabled and the gosredirector project in my GitHub account.

Make sure to retrieve NuGet packages when you open up the project.

Rename bf3_ssl_patched_without_drm_1.6.0.0.exe to bf3.exe and put it in the game folder.

Use these arguments to start the game:
start bf3.exe -webMode MP -Origin_NoAppFocus -onlineEnvironment prod -loginToken 4-8-15-16-23-42 -AuthToken 4-8-15-16-23-42 -requestState State_ClaimReservation -requestStateParams "<data putinsquad=\"false\" gameid=\"1\" personaref=\"2\" levelmode=\"mp\"></data>"

login and auth token is not checked on login

The provided exe has:
a) SSL check patched
b) DRM loading removed (thanks to walmart team)

Set up a mysql server, import blaze.sql, check the data in the database, make relevant changes to config.yml, move config.yml to same directory as BlazeServer.exe.

Edit hosts file and redirect gosredirector.ea.com and 373244-gosprapp357.ea.com to localhost or other PC where you have Blazeserver running

Log & Configuration class by NTAuthority
Certificates dumped with help from WarrantyVoider
Blaze research by Pedro Martins

TODO:
add BlazeHub
replace TCPServer with Task(s)-powered server
support multiple clients
make seperate JSON API C# app for logging in (for using with a 'Battlelog'-like app written in C#)
add dinput8 project
add BlazeAPI project -- need to make it first
lots of stuff (stats, 'remove player after leave', etc...)
add a tutorial how to disable SSL check in decrypted walmart 1.6.0.0 executable

If you have time, fork the project and make pull requests. I will review them.

win32re@gmail.com