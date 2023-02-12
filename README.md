# MazeMaker

A GUI application for creating "Shoot Houses" within the GP Hanger scene. There are 2 options of creating Shoot Houses:

1. Manual Creation: this option allows for full control over each individual wall within the Shoot House. If a "Wide Wall" is selected it allows the user to choose the type of walls from the following list:
    1. Single Door
    2. Double Door
    3. Window
    4. Wall
    5. Low Competition Barrier
If a "Narrow Wall" is selected it allows the user to simply toggle the wall on and off.

2. Random Maze Generation: this options will automatically generate a random maze from the starting door to the exit door. If a "Wide Wall" is selected double doors will be used for the maze path. If a "Narrow Wall" is seletected the maze will be genearted by removing walls.

In order to streamline the saving process you may place a file called "config.txt" in the same folder as the exe, this file should contain a single line only consisting of the desired save location. The typical location where Game Planner Scene files are stored is "C:\Users\XXXX\Documents\My Games\H3VR\Vault\SceneConfigs\gp_hangar" with XXXX being the current user name.
