﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeMakerUtilities
{

    public class Tuple<T1, T2>
    {
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }
        public Tuple(T1 first, T2 second)
        {
            Item1 = first;
            Item2 = second;
        }
    }

    public static class Tuple
    {
        public static Tuple<T1, T2> New<T1, T2>(T1 first, T2 second)
        {
            var tuple = new Tuple<T1, T2>(first, second);
            return tuple;
        }
    }
    public class Room
    {
        public Wall top { get; set; }
        public Wall bottom { get; set; }
        public Wall left { get; set; }
        public Wall right { get; set; }
        public bool visited { get; set; }
        public bool path { get; set; }

        public List<Tuple<Room, string>> neighbors = new List<Tuple<Room, string>>();
        public int row { get; set; }
        public int column { get; set; }

        public List<Target> targets = null;

        public Room()
        {
            top = new Wall();
            bottom = new Wall();
            left = new Wall();
            right = new Wall();
            row = 0;
            column = 0;
        }

        private static List<List<Room>> maze;
        private static Parameters parameters;
        public static List<List<Room>> generateRooms(Parameters parametersInput)
        {
            maze = new List<List<Room>>();
            parameters = parametersInput;

            decimal startingXHorizontals = parameters.XHorizontalStart;
            decimal startingYHorizontals = parameters.YHorizontalStart;
            decimal startingZHorizontals;

            decimal startingXVerticals = parameters.XVerticalStart;
            decimal startingYVerticals = parameters.YVerticalStart;
            decimal startingZVerticals;

            for (int r = 0; r < parameters.gridRows; r++)
            {
                List<Room> row = new List<Room>();
                startingZHorizontals = parameters.ZHorizontalStart;
                startingZVerticals = parameters.ZVerticalStart;

                for (int c = 0; c < parameters.gridColumns; c++)
                {
                    Room room = new Room()
                    {
                        top = new Wall()
                        {
                            render = true
                        },
                        bottom = new Wall()
                        {
                            render = false
                        },
                        left = new Wall()
                        {
                            render = true
                        },
                        right = new Wall()
                        {
                            render = false
                        },
                        row = r,
                        column = c
                    };

                    Element top = new Element(parameters.horizontalWide);
                    top.PosOffset = new Posoffset
                    {
                        x = startingXHorizontals,
                        y = startingYHorizontals,
                        z = startingZHorizontals
                    };
                    top.OrientationForward = new Orientationforward
                    {
                        x = 1,
                        y = 0,
                        z = 0
                    };


                    Element bottom = new Element(parameters.horizontalWide);
                    bottom.PosOffset = new Posoffset
                    {
                        x = startingXHorizontals,
                        y = startingYHorizontals,
                        z = startingZHorizontals
                    };
                    bottom.OrientationForward = new Orientationforward
                    {
                        x = 1,
                        y = 0,
                        z = 0
                    };
                    bottom.PosOffset.x = top.PosOffset.x - parameters.XHorizontalOffset;

                    // Render bottom wall
                    if (r == parameters.gridRows - 1)
                    {
                        room.bottom.render = true;
                    }

                    Element left = new Element(parameters.verticalWide);
                    left.PosOffset = new Posoffset
                    {
                        x = startingXVerticals,
                        y = startingYVerticals,
                        z = startingZVerticals
                    };
                    left.OrientationForward = new Orientationforward
                    {
                        x = 0,
                        y = 0,
                        z = 1
                    };

                    if (c == parameters.gridColumns - 1)
                    {
                        room.right.render = true;
                    }

                    Element right = new Element(parameters.verticalWide);
                    right.PosOffset = new Posoffset
                    {
                        x = startingXVerticals,
                        y = startingYVerticals,
                        z = startingZVerticals
                    };
                    right.OrientationForward = new Orientationforward
                    {
                        x = 0,
                        y = 0,
                        z = 1
                    };
                    right.PosOffset.z = left.PosOffset.z - parameters.ZHorizontalOffset;

                    room.top.element = top;
                    room.bottom.element = bottom;
                    room.left.element = left;
                    room.right.element = right;

                    row.Add(room);
                    startingZHorizontals -= parameters.ZHorizontalOffset;
                    startingZVerticals -= parameters.ZVerticalOffset;
                }

                startingXHorizontals -= parameters.XHorizontalOffset;
                startingXVerticals -= parameters.XVerticalOffset;

                maze.Add(row);
            }

            return maze;
        }

        public static List<List<Room>> makeMaze(string mapNameInput)
        {
            parameters = Parameters.generateParameters(true,true,mapNameInput);
            maze = generateRooms(parameters);

            Stack<Room> rooms = new Stack<Room>();

            Random random = new Random();

            Room startingRoom = maze[0][0];
            rooms.Push(startingRoom);

            while (rooms.Count != 0)
            {
                startingRoom.visited = true;
                List<Tuple<Room, string>> validNeighbours = getValidNeighbours(startingRoom);
                if (validNeighbours.Count > 0)
                {
                    int newRoomIndex = random.Next(0, validNeighbours.Count);
                    Room newRoom = validNeighbours[newRoomIndex].Item1;
                    // Remove wall
                    if (validNeighbours[newRoomIndex].Item2 == "Up")
                    {
                        if (parameters.horizontalWide)
                        {
                            startingRoom.top.element.ObjectID = "ShoothouseBarrierDoorDouble";
                        }
                        else
                        {
                            startingRoom.top.render = false;
                        }

                    }
                    else if (validNeighbours[newRoomIndex].Item2 == "Down")
                    {
                        if (parameters.horizontalWide)
                        {
                            maze[newRoom.row][newRoom.column].top.element.ObjectID = "ShoothouseBarrierDoorDouble";
                        }
                        else
                        {
                            maze[newRoom.row][newRoom.column].top.render = false;
                        }
                    }
                    else if (validNeighbours[newRoomIndex].Item2 == "Left")
                    {
                        if (parameters.verticalWide)
                        {
                            startingRoom.left.element.ObjectID = "ShoothouseBarrierDoorDouble";
                        }
                        else
                        {
                            startingRoom.left.render = false;
                        }

                    }
                    else if (validNeighbours[newRoomIndex].Item2 == "Right")
                    {
                        if (parameters.verticalWide)
                        {
                            maze[newRoom.row][newRoom.column].left.element.ObjectID = "ShoothouseBarrierDoorDouble";
                        }
                        else
                        {
                            maze[newRoom.row][newRoom.column].left.render = false;
                        }
                    }
                    startingRoom = maze[newRoom.row][newRoom.column];
                    rooms.Push(startingRoom);

                }
                else
                {
                    startingRoom = rooms.Pop();
                }

            }

            solveMaze();
            carvePath();
            hollowMaze();
            removeEntranceAndExit();
            addTargets();
            return maze;

        }

        public static void solveMaze()
        {
            // set all cells to not be visited
            for (int r = 0; r < maze.Count; r++)
            {
                for (int c = 0; c < maze[r].Count; c++)
                {
                    maze[r][c].visited = false;
                    getNeighbors(maze[r][c]);
                }
            }


            List<Room> visited = new List<Room>();
            List<Room> frontier = new List<Room>();

            Dictionary<Room, Room> solution = new Dictionary<Room, Room>();

            // Set current to Start Cell
            Room current = maze[parameters.entranceRow][parameters.entranceColumn];
            current.visited = true;

            // Add current to frontier
            frontier.Add(current);

            // Add current to visited
            visited.Add(current);

            while (frontier.Count > 0)
            {

                foreach (Tuple<Room, string> room in current.neighbors)
                {
                    // Is cell to the left valid and not visited?
                    if (room.Item2 == "Left" && !room.Item1.visited)
                    {
                        frontier.Add(room.Item1);
                        solution.Add(room.Item1, current);
                    }

                    // Is cell to the right valid and not visited?
                    if (room.Item2 == "Right" && !room.Item1.visited)
                    {
                        frontier.Add(room.Item1);
                        solution.Add(room.Item1, current);
                    }

                    // Is cell above valid and not visited?
                    if (room.Item2 == "Above" && !room.Item1.visited)
                    {
                        frontier.Add(room.Item1);
                        solution.Add(room.Item1, current);
                    }

                    // Is cell below valid and not visited?
                    if (room.Item2 == "Below" && !room.Item1.visited)
                    {
                        frontier.Add(room.Item1);
                        solution.Add(room.Item1, current);
                    }
                }

                current = frontier.Last();
                current.visited = true;
                frontier.Remove(frontier.Last());
            }

            current = maze[parameters.exitRow][parameters.exitColumn];
            current.path = true;
            while (current.row != parameters.entranceRow || current.column != parameters.entranceColumn)
            {
                current = solution[current];
                current.path = true;
            }



        }

        public static void getNeighbors(Room startingRoom)
        {

            // Check for valid move above current room if not in top row
            if (startingRoom.row != 0)
            {
                Room above = maze[startingRoom.row - 1][startingRoom.column];
                if (startingRoom.top.element.ObjectID != "ShoothouseBarrierWall")
                {
                    startingRoom.neighbors.Add(new Tuple<Room, string>(above, "Above"));
                }
            }

            // Check for valid move below current room if not in bottom row
            if (startingRoom.row != parameters.gridRows - 1)
            {
                Room below = maze[startingRoom.row + 1][startingRoom.column];
                if (below.top.element.ObjectID != "ShoothouseBarrierWall")
                {
                    startingRoom.neighbors.Add(new Tuple<Room, string>(below, "Below"));
                }
            }

            if (startingRoom.column != 0)
            {
                Room left = maze[startingRoom.row][startingRoom.column - 1];
                if (startingRoom.left.element.ObjectID != "ShoothouseBarrierWall")
                {
                    startingRoom.neighbors.Add(new Tuple<Room, string>(left, "Left"));
                }
            }

            if (startingRoom.column != parameters.gridColumns - 1)
            {
                Room right = maze[startingRoom.row][startingRoom.column + 1];
                if (right.left.element.ObjectID != "ShoothouseBarrierWall")
                {
                    startingRoom.neighbors.Add(new Tuple<Room, string>(right, "Right"));
                }
            }
        }

        public static void carvePath()
        {
            // Reset all walls
            for (int r = 0; r < maze.Count; r++)
            {
                for (int c = 0; c < maze[r].Count; c++)
                {
                    Room current = maze[r][c];
                    current.top.element.ObjectID = "ShoothouseBarrierWall";
                    current.bottom.element.ObjectID = "ShoothouseBarrierWall";
                    current.left.element.ObjectID = "ShoothouseBarrierWall";
                    current.right.element.ObjectID = "ShoothouseBarrierWall";
                }
            }

            // Loop through all the rooms
            for (int r = 0; r < maze.Count; r++)
            {
                for (int c = 0; c < maze[r].Count; c++)
                {
                    Room current = maze[r][c];
                    // Is room on the path?
                    if (current.path)
                    {
                        // loop through rooms neighbors
                        foreach (Tuple<Room, string> neighbor in current.neighbors)
                        {

                            if (neighbor.Item2 == "Above" && neighbor.Item1.path)
                            {
                                current.top.element.ObjectID = "ShoothouseBarrierDoorDouble";

                            }
                            else if (neighbor.Item2 == "Below" && neighbor.Item1.path)
                            {
                                neighbor.Item1.top.element.ObjectID = "ShoothouseBarrierDoorDouble";
                            }
                            else if (neighbor.Item2 == "Left" && neighbor.Item1.path)
                            {
                                current.left.element.ObjectID = "ShoothouseBarrierDoorDouble";
                            }

                            else if (neighbor.Item2 == "Right" && neighbor.Item1.path)
                            {
                                neighbor.Item1.left.element.ObjectID = "ShoothouseBarrierDoorDouble";
                            }
                        }
                    }
                }
            }
        }

        public static void hollowMaze()
        {
            // Loop through all the rooms
            for (int r = 0; r < maze.Count; r++)
            {
                for (int c = 0; c < maze[r].Count; c++)
                {
                    Room current = maze[r][c];
                    // Is room on the path?
                    if (!current.path)
                    {
                        // check room above
                        if (current.row != 0)
                        {
                            Room above = maze[current.row - 1][current.column];
                            if (!above.path)
                            {
                                current.top.render = false;
                                current.top.element.ObjectID = "None";

                                above.bottom.render = false;
                                above.bottom.element.ObjectID = "None";
                                
                            }
                        }

                        // check room right
                        if (current.column != parameters.gridColumns - 1)
                        {
                            Room right = maze[current.row][current.column + 1];
                            if (!right.path)
                            {
                                right.left.render = false;
                                right.left.element.ObjectID = "None";

                                current.right.render = false;
                                current.right.element.ObjectID = "None";
                            }
                        }
                    }
                }
            }
        }

        public static void addTargets()
        {
            Random targetChoice = new Random();
            Random targetYesNo = new Random();
            Random targetRoomChoice = new Random();
            int targetPlacementPercentage = 25;
            // Loop through all rooms and check what rooms can be moved into from current room
            for (int r = 0; r < maze.Count; r++)
            {
                for (int c = 0; c < maze[r].Count; c++)
                {
                    Room startingRoom = maze[r][c];
                    //Is the room on the path?
                    if (startingRoom.path)
                    {
                        // Determine if we are placing a target
                        int placeTarget = targetYesNo.Next(0, 100);
                        if (placeTarget < targetPlacementPercentage)
                        {
                            #region Get Neighbors
                            List<Tuple<Room, string>> nonPathNeighbors = new List<Tuple<Room, string>>();
                            // Get non path neighbors
                            if (startingRoom.row != 0)
                            {
                                Room above = maze[startingRoom.row - 1][startingRoom.column];
                                if (!above.path)
                                {
                                    nonPathNeighbors.Add(new Tuple<Room, string>(above, "Above"));
                                }
                            }
                            if (startingRoom.row != parameters.gridRows - 1)
                            {
                                Room below = maze[startingRoom.row + 1][startingRoom.column];
                                if (!below.path)
                                {
                                    nonPathNeighbors.Add(new Tuple<Room, string>(below, "Below"));
                                }
                            }
                            // check room left
                            if (startingRoom.column != 0)
                            {
                                Room left = maze[startingRoom.row][startingRoom.column - 1];
                                if (!left.path)
                                {
                                    nonPathNeighbors.Add(new Tuple<Room, string>(left, "Left"));
                                }
                            }
                            // check room right
                            if (startingRoom.column != parameters.gridColumns - 1)
                            {
                                Room right = maze[startingRoom.row][startingRoom.column + 1];
                                if (!right.path)
                                {
                                    nonPathNeighbors.Add(new Tuple<Room, string>(right, "Right"));
                                }
                            } 
                            #endregion


                            if (nonPathNeighbors.Count != 0)
                            {                                
                                int neighborIndex = targetRoomChoice.Next(0, nonPathNeighbors.Count);
                                Tuple<Room, string> chosenNeighor = nonPathNeighbors[neighborIndex];

                                #region Choose direction and change walls
                                string targetDirection = "";

                                if (chosenNeighor.Item2 == "Above")
                                {
                                    // Convert to a barrier
                                    startingRoom.top.render = true;
                                    startingRoom.top.element.ObjectID = "CompBarrierLow";
                                    targetDirection = "Below";

                                    // Add walls to chosen neighbor
                                    chosenNeighor.Item1.left.render = true;
                                    chosenNeighor.Item1.left.element.ObjectID = "ShoothouseBarrierWall";
                                    chosenNeighor.Item1.top.render = true;
                                    chosenNeighor.Item1.top.element.ObjectID = "ShoothouseBarrierWall";
                                    if (chosenNeighor.Item1.column != parameters.gridColumns - 1)
                                    {
                                        Room chosenNeighborRight = maze[chosenNeighor.Item1.row][chosenNeighor.Item1.column + 1];
                                        chosenNeighborRight.left.render = true;
                                        chosenNeighborRight.left.element.ObjectID = "ShoothouseBarrierWall";
                                    }



                                }

                                else if (chosenNeighor.Item2 == "Below")
                                {
                                    chosenNeighor.Item1.top.render = true;
                                    chosenNeighor.Item1.top.element.ObjectID = "CompBarrierLow";

                                    // Add walls to chosen neighbor
                                    chosenNeighor.Item1.left.render = true;
                                    chosenNeighor.Item1.left.element.ObjectID = "ShoothouseBarrierWall";
                                    if (chosenNeighor.Item1.column != parameters.gridColumns - 1)
                                    {
                                        Room chosenNeighborRight = maze[chosenNeighor.Item1.row][chosenNeighor.Item1.column + 1];
                                        chosenNeighborRight.left.render = true;
                                        chosenNeighborRight.left.element.ObjectID = "ShoothouseBarrierWall";
                                    }
                                    if (chosenNeighor.Item1.row != parameters.gridRows - 1)
                                    {
                                        Room chosenNeighborBelow = maze[chosenNeighor.Item1.row + 1][chosenNeighor.Item1.column];
                                        chosenNeighborBelow.top.render = true;
                                        chosenNeighborBelow.top.element.ObjectID = "ShoothouseBarrierWall";
                                    }
                                    targetDirection = "Above";
                                }

                                else if (chosenNeighor.Item2 == "Left")
                                {
                                    startingRoom.left.render = true;
                                    startingRoom.left.element.ObjectID = "CompBarrierLow"; targetDirection = "Left";

                                    chosenNeighor.Item1.left.render = true;
                                    chosenNeighor.Item1.left.element.ObjectID = "ShoothouseBarrierWall";
                                    chosenNeighor.Item1.top.render = true;
                                    chosenNeighor.Item1.top.element.ObjectID = "ShoothouseBarrierWall";
                                    if (chosenNeighor.Item1.row != parameters.gridRows - 1)
                                    {
                                        Room chosenNeighborBelow = maze[chosenNeighor.Item1.row + 1][chosenNeighor.Item1.column];
                                        chosenNeighborBelow.top.render = true;
                                        chosenNeighborBelow.top.element.ObjectID = "ShoothouseBarrierWall";
                                    }

                                }

                                else if (chosenNeighor.Item2 == "Right")
                                {
                                    chosenNeighor.Item1.left.render = true;
                                    chosenNeighor.Item1.left.element.ObjectID = "CompBarrierLow";
                                    targetDirection = "Right";
                                    chosenNeighor.Item1.top.render = true;
                                    chosenNeighor.Item1.top.element.ObjectID = "ShoothouseBarrierWall";
                                    if (chosenNeighor.Item1.column != parameters.gridColumns - 1)
                                    {
                                        Room chosenNeighborRight = maze[chosenNeighor.Item1.row][chosenNeighor.Item1.column + 1];
                                        chosenNeighborRight.left.render = true;
                                        chosenNeighborRight.left.element.ObjectID = "ShoothouseBarrierWall";
                                    }
                                    if (chosenNeighor.Item1.row != parameters.gridRows - 1)
                                    {
                                        Room chosenNeighborBelow = maze[chosenNeighor.Item1.row + 1][chosenNeighor.Item1.column];
                                        chosenNeighborBelow.top.render = true;
                                        chosenNeighborBelow.top.element.ObjectID = "ShoothouseBarrierWall";
                                    }

                                }
                                #endregion

                                chosenNeighor.Item1.targets = Target.getTargets(targetDirection, chosenNeighor.Item1, parameters);
                         
                            }
                        }
                    }
                }
            }
        }

        public static void removeEntranceAndExit()
        {
            // Remove entrance and exit walls

            // 4 possible exit scenarios
            // 1. Wide X Wide
            if (parameters.horizontalWide && parameters.verticalWide)
            {
                // Entrance
                maze[parameters.entranceRow][parameters.entranceColumn].bottom.element.ObjectID = "ShoothouseBarrierDoorSingle";
                // Exit
                maze[parameters.exitRow][parameters.exitColumn].left.element.ObjectID = "ShoothouseBarrierDoorSingle";
                
            }
            // 2. Narrow X Narrow
            else if (!parameters.horizontalWide && !parameters.verticalWide)
            {
                // Entrance
                maze[parameters.entranceRow][parameters.entranceColumn].bottom.render = false;
                // Exit
                maze[parameters.exitRow][parameters.exitColumn].left.render = false;               
            }
            // 3. Wide x Narrow
            else if (parameters.horizontalWide && !parameters.verticalWide)
            {
                // Entrance
                maze[parameters.entranceRow][parameters.entranceColumn].bottom.element.ObjectID = "ShoothouseBarrierDoorSingle";
                // Exit
                maze[parameters.exitRow][parameters.exitColumn].left.render = false;
            }
            // 4. Narrow x Wide
            else if (!parameters.horizontalWide && parameters.verticalWide)
            {

                maze[parameters.entranceRow][parameters.entranceColumn].bottom.render = false;
                // Exit
                maze[parameters.exitRow][parameters.exitColumn].left.element.ObjectID = "ShoothouseBarrierDoorSingle";
            }
            else
            {
                // Do nothing
            }

            // room entrance and exit targetGroups
            maze[parameters.entranceRow][parameters.entranceColumn].targets = null;
            maze[parameters.exitRow][parameters.exitColumn].targets = null;
        }

        public static List<string> getValidMoves(Room startingRoom)
        {
            List<string> validRooms = new List<string>();
            
            // Skip the exit room
            if (startingRoom.row == 0 && startingRoom.column == 0)
            {
                return validRooms;
            }

            // Check for valid move above current room if not in top row
            if (startingRoom.row != 0)
            {
                if (parameters.horizontalWide)
                {
                    if (startingRoom.top.element.ObjectID != "ShoothouseBarrierWall")
                    {
                        validRooms.Add("North");
                    }
                }
                else
                {
                    if (startingRoom.top.render == false)
                    {
                        validRooms.Add("North");
                    }
                }
            }

            // Check for valid move left of current room if not in left column
            if (startingRoom.column != 0)
            {
                if (parameters.verticalWide)
                {
                    if (startingRoom.left.element.ObjectID != "ShoothouseBarrierWall")
                    {
                        validRooms.Add("West");
                    }
                }
                else
                {
                    if (startingRoom.left.render == false)
                    {
                        validRooms.Add("West");
                    }
                }
            }

            // check for valid move below current room if not in last row
            if (startingRoom.row != parameters.gridRows - 1)
            {
                Room below = maze[startingRoom.row + 1][startingRoom.column];
                if (parameters.horizontalWide)
                {
                    
                    if (below.top.element.ObjectID != "ShoothouseBarrierWall")
                    {
                        validRooms.Add("South");
                    }
                }
                else
                {
                    if (below.top.render == false)
                    {
                        validRooms.Add("South");
                    }
                }
            }

            // check for valid move to the right of current room if not in last column
            if (startingRoom.column != parameters.gridColumns -1)
            {
                Room left = maze[startingRoom.row][startingRoom.column + 1];
                if (parameters.verticalWide)
                {
                    if (left.left.element.ObjectID != "ShoothouseBarrierWall")
                    {
                        validRooms.Add("East");
                    }
                }
                else
                {
                    if (left.left.render == false)
                    {
                        validRooms.Add("East");
                    }
                }
            }

            return validRooms;
        }

        public static List<Tuple<Room, string>> getValidNeighbours(Room startingRoom)
        {
            List<Tuple<Room, string>> validRooms = new List<Tuple<Room, string>>();

            // check room above
            if (startingRoom.row != 0)
            {
                Room above = maze[startingRoom.row - 1][startingRoom.column];
                if (!above.visited)
                {

                    validRooms.Add(new Tuple<Room, string>(above, "Up"));
                }
            }
            // check room below
            if (startingRoom.row != parameters.gridRows - 1)
            {
                Room below = maze[startingRoom.row + 1][startingRoom.column];
                if (!below.visited)
                {
                    validRooms.Add(new Tuple<Room, string>(below, "Down"));
                }
            }
            // check room left
            if (startingRoom.column != 0)
            {
                Room left = maze[startingRoom.row][startingRoom.column - 1];
                if (!left.visited)
                {
                    validRooms.Add(new Tuple<Room, string>(left, "Left"));
                }
            }
            // check room right
            if (startingRoom.column != parameters.gridColumns - 1)
            {
                Room right = maze[startingRoom.row][startingRoom.column + 1];
                if (!right.visited)
                {
                    validRooms.Add(new Tuple<Room, string>(right, "Right"));
                }
            }

            return validRooms;
        }

        
    }
}
