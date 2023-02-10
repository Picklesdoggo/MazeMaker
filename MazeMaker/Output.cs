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
            File.WriteAllText(mapName + "_gp_hangar_VFS.json", jsonUpdated);
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
        public string Type { get; set; }
        public Posoffset PosOffset { get; set; }
        public Orientationforward OrientationForward { get; set; }
        public Orientationup OrientationUp { get; set; }
        public int ObjectAttachedTo { get; set; }
        public int MountAttachedTo { get; set; }
        public List<string> LoadedRoundsInChambers { get; set; }
        public List<string> LoadedRoundsInMag { get; set; }
        public List<string> GenericInts { get; set; }
        public List<string> GenericStrings { get; set; }
        public List<string> GenericVector3s { get; set; }
        public List<string> GenericRotations { get; set; }
        public Flags Flags { get; set; }
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
