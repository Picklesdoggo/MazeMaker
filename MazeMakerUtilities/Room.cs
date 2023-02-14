using System;
using System.Collections.Generic;

namespace MazeMakerUtilities
{
    public class Room
    {
        public Wall top { get; set; }
        public Wall bottom { get; set; }
        public Wall left { get; set; }
        public Wall right { get; set; }
        public bool visited { get; set; }

        public int row { get; set; }
        public int column { get; set; }

        public Element target = null;

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

            float startingXHorizontals = parameters.XHorizontalStart;
            float startingYHorizontals = parameters.YHorizontalStart;
            float startingZHorizontals;

            float startingXVerticals = parameters.XVerticalStart;
            float startingYVerticals = parameters.YVerticalStart;
            float startingZVerticals;

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

        public static List<List<Room>> makeMaze(Parameters parametersInput)
        {
            parameters = parametersInput;
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

            removeEntranceAndExit();
            addTargets();
            return maze;

        }

        public static void addTargets()
        {
            int targets = 0;
            for (int r = 0; r < maze.Count; r++)
            {
                for (int c = 0; c < maze[r].Count; c++)
                {
                    // W X W
                    if (parameters.horizontalWide && parameters.verticalWide)
                    {

                        Room currentRoom = maze[r][c];
                        // Room has a door at the top but not left, check room below
                        if (currentRoom.top.element.ObjectID == "ShoothouseBarrierDoorDouble" && currentRoom.top.render == true &&
                            currentRoom.left.element.ObjectID == "ShoothouseBarrierWall" && currentRoom.left.render == true
                            )
                        {
                            
                            if (r != maze.Count - 1)
                            {
                                Room roomBelow = maze[r + 1][c];
                                // Room below has a wall at the top, check room to the right
                                if (roomBelow.top.element.ObjectID == "ShoothouseBarrierWall" && roomBelow.top.render == true)
                                {
                                    if (c != maze[r].Count - 1)
                                    {
                                        Room roomRight = maze[r][c + 1];
                                        if (roomRight.left.render == true && roomRight.left.element.ObjectID == "ShoothouseBarrierWall")
                                        {
                                            Element target = Output.getTarget("North", "StandingSteelTargetClassicPop");
                                            target.PosOffset.x = maze[r + 1][c].top.element.PosOffset.x + (parameters.XHorizontalOffset / 2 );
                                            target.PosOffset.z = maze[r + 1][c].right.element.PosOffset.z + (parameters.ZHorizontalOffset /2 );
                                            target.PosOffset.y = 3.5F;
                                            maze[r + 1][c].target = target;
                                            targets++;
                                        }
                                    }
                                    
                                }
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
                maze[8][7].bottom.element.ObjectID = "ShoothouseBarrierDoorSingle";
                // Exit
                maze[0][0].left.element.ObjectID = "ShoothouseBarrierDoorSingle";

            }
            // 2. Narrow X Narrow
            else if (!parameters.horizontalWide && !parameters.verticalWide)
            {
                // Entrance
                maze[24][23].bottom.render = false;
                // Exit
                maze[1][0].left.render = false;
            }
            // 3. Wide x Narrow
            else if (parameters.horizontalWide && !parameters.verticalWide)
            {
                // Entrance
                maze[24][7].bottom.element.ObjectID = "ShoothouseBarrierDoorSingle";
                // Exit
                maze[1][0].left.render = false;
            }
            // 4. Narrow x Wide
            else if (!parameters.horizontalWide && parameters.verticalWide)
            {

                maze[9][18].bottom.render = false;
                // Exit
                maze[0][0].left.element.ObjectID = "ShoothouseBarrierDoorSingle";
            }
            else
            {
                // Do nothing
            }
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