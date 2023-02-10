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

                    Element top = new Element(parameters);
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


                    Element bottom = new Element(parameters);
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


                    room.top.element = top;

                    row.Add(room);
                    startingZHorizontals -= parameters.ZHorizontalOffset;
                }

                startingXHorizontals -= parameters.XHorizontalOffset;

                mazeRooms.Add(row);
            }

            return mazeRooms;
        }
    }

   

}
