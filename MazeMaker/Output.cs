using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MazeMaker
{
    public class Output
    {
        public string FileName { get; set; }
        public string ReferencePath { get; set; }
        public string Creator { get; set; }
        public List<string> ModsUsed { get; set; }
        public List<Object> Objects { get; set; }
        public string QuickbeltLayoutName { get; set; }



        public static void saveMap(List<List<Room>> mazeRooms, string mapName)
        {
            Output generatedOutput = new Output();

            generatedOutput.FileName = mapName;
            generatedOutput.ReferencePath = @"Vault\SceneConfigs\gp_hangar\" + mapName + "gp_hangar_VFS.json";
            generatedOutput.Creator = "picklesDoggo";
            generatedOutput.ModsUsed = new List<string>();
            generatedOutput.Objects = new List<Object>();

            int index = 0;

            for (int r = 0; r < mazeRooms.Count; r++)
            {
                for (int c = 0; c < mazeRooms[r].Count;c++)
                {

                    if (mazeRooms[r][c].top.render)
                    {
                        Object top = new Object
                        {
                            IsContainedIn = -1,
                            QuickbeltSlotIndex = -1,
                            InSlotOfRootObjectIndex = -1,
                            InSlotOfElementIndex = -1,
                            Elements = new List<Element>(),
                            Index = index
                        };
                        top.Elements.Add(mazeRooms[r][c].top.element);
                        generatedOutput.Objects.Add(top);
                        index++;
                    }


                    //if (mazeRooms[r][c].bottom.render)
                    //{
                    //    Object bottom = new Object
                    //    {
                    //        IsContainedIn = -1,
                    //        QuickbeltSlotIndex = -1,
                    //        InSlotOfRootObjectIndex = -1,
                    //        InSlotOfElementIndex = -1,
                    //        Elements = new List<Element>(),
                    //        Index = index
                    //    };
                    //    bottom.Elements.Add(mazeRooms[r][c].bottom.element);
                    //    generatedOutput.Objects.Add(bottom);
                    //    index++;
                    //}

                }
            }




            string jsonUpdated = JsonConvert.SerializeObject(generatedOutput, Formatting.Indented);
            File.WriteAllText("C:\\Users\\John\\Documents\\My Games\\H3VR\\Vault\\SceneConfigs\\gp_hangar\\" + mapName + "_gp_hangar_VFS.json", jsonUpdated);
        }
    }

    public class Object
    {
        public int Index { get; set; }
        public int IsContainedIn { get; set; }
        public int QuickbeltSlotIndex { get; set; }
        public int InSlotOfRootObjectIndex { get; set; }
        public int InSlotOfElementIndex { get; set; }
        public List<Element> Elements { get; set; }
    }

    public class Element
    {
        public int Index { get; set; }
        public string ObjectID { get; set; }
        public string Type = "object";
        public Posoffset PosOffset { get; set; }
        public Orientationforward OrientationForward { get; set; }

        public Orientationup OrientationUp = new Orientationup
        {
            x = 0,
            y = 1,
            z = 0
        };
        public int ObjectAttachedTo = -1;
        public int MountAttachedTo = -1;
        public List<string> LoadedRoundsInChambers = new List<string>();
        public List<string> LoadedRoundsInMag = new List<string>();
        public List<string> GenericInts = new List<string>();
        public List<string> GenericStrings = new List<string>();
        public List<string> GenericVector3s = new List<string>();
        public List<string> GenericRotations = new List<string>();
        public Flags Flags = new Flags
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

        public Element(Parameters parameters)
        {
            if (parameters.horizontalWide)
            {
                ObjectID = "ShoothouseBarrierWall";
            }
            else
            {
                ObjectID = "ShoothouseBarrierWallNarrow";
            }
        }

    }

    public class Posoffset
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
    }

    public class Orientationforward
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
    }

    public class Orientationup
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
    }

    public class Flags
    {
        public List<string> _keys { get; set; }
        public List<string> _values { get; set; }
    }
}
