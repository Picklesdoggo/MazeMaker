# MazeMaker

A GUI application for creating "Shoot Houses" within the GP Hanger scene. There are 2 options of creating Shoot Houses:

# Manual Creation: 

This option allows for full control over each individual wall within the Shoot House. If a "Wide Wall" is selected it allows the user to choose the type of walls from the following list:
    1. Single Door
    2. Double Door
    3. Window
    4. Wall
    5. Low Competition Barrier
If a "Narrow Wall" is selected it allows the user to simply toggle the wall on and off.

To modify multiple walls at a time hold down ctrl and mouse over the walls. 

# Random Maze Generation: 

This options will automatically generate a random maze from the starting door to the exit door. Only wide walls are used in random maze generation. Targets will randomly be placed in dead-ends. Currently only "popper" targets are implemented, spring targets will not reset after being shot.

# Save Location
In order to streamline the saving process you may place a file called "config.txt" in the same folder as the exe, this file should contain a single line only consisting of the desired save location. The typical location where Game Planner Scene files are stored is "C:\Users\XXXX\Documents\My Games\H3VR\Vault\SceneConfigs\gp_hangar" with XXXX being the current user name.

# Code Library
The code which actually generates the mazes is free for anyone who wishes to intergrate it into an in game mod. The methods requried to do this are:
1. `Parameters generateParameters(bool verticalWideInput, bool horizontalWideInput, string mapNameInput)` this will setup the starting locations and offsets
2. `List<List<Room>> makeMaze(Parameters parametersInput)` this takes in the parameters created in method one and generates a random maze
3. `void saveMap(List<List<Room>> mazeRooms, string mapName, string folderName)` this takes in the maze from method two, a map name and a folder name. If the folder name of "default" is provided it will save the json file in the same location as the exe

# Future Plans
1. Allow user placed targets 
2. Loading of saved maps 
