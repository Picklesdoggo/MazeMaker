using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeMaker
{
    public class Room
    {
        public Wall top { get; set; }
        public Wall bottom { get; set; }
        public Wall left { get; set; }
        public Wall right { get; set; }

        public Room()
        {
            top = new Wall();
            bottom = new Wall();
            left = new Wall();
            right = new Wall();
        }

        public static List<List<Room>> generateRooms(Parameters parameters)
        {
            List<List<Room>> mazeRooms = new List<List<Room>>();

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

                    };

                    Element top = new Element();
                    
                    if (parameters.horizontalWide)
                    {
                        top.ObjectID = "ShoothouseBarrierWall";
                    }
                    else
                    {
                        top.ObjectID = "ShoothouseBarrierWallNarrow";
                    }
                    top.Type = "object";

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

                    top.OrientationUp = new Orientationup
                    {
                        x = 0,
                        y = 1,
                        z = 0
                    };

                    top.ObjectAttachedTo = -1;
                    top.MountAttachedTo = -1;
                    top.LoadedRoundsInChambers = new List<string>();
                    top.LoadedRoundsInMag = new List<string>();
                    top.GenericInts = new List<string>();
                    top.GenericStrings = new List<string>();
                    top.GenericVector3s = new List<string>();
                    top.GenericRotations = new List<string>();
                    top.Flags = new Flags
                    {
                        _keys = new List<string>()
                            {
                                "IsKinematicLocked",
                                "IsPickupLocked",
                                "QuickBeltSpecialStateEngaged"
                            },

                        _values = new List<string>()
                            {
                                "True",
                                "True",
                                "False"
                            }
                    };

                    Element bottom = top;
                    bottom.PosOffset.x -= parameters.XVerticalOffset;

                    // Render bottom wall
                    if (r == parameters.gridRows - 1)
                    {
                        room.bottom.render = true;
                    }


                    room.top.element = top;

                    row.Add(room);
                    startingZVerticals -= parameters.ZVerticalOffset;
                }

                startingXHorizontals -= parameters.XHorizontalOffset;

                mazeRooms.Add(row);
            }

            return mazeRooms;
        }
    }

   

}
