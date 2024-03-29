﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
namespace MazeMakerUtilities
{

    [Serializable]
    public class Output
    {
        

        public Output()
        {
        }

        public string FileName { get; set; }
        public string ReferencePath { get; set; }
        public string Creator { get; set; }
        public List<string> ModsUsed { get; set; }
        public List<Object> Objects { get; set; }
        public string QuickbeltLayoutName { get; set; }

        public static void saveMap(List<List<Room>> mazeRooms, Parameters parameters, string folderName, Output baseFile)
        {
            Output generatedOutput = new Output();

            generatedOutput.FileName = parameters.mapName;
            generatedOutput.ReferencePath = @"Vault\SceneConfigs\gp_hangar\" + parameters.mapName + "gp_hangar_VFS.json";
            generatedOutput.Creator = "picklesDoggo";
            generatedOutput.ModsUsed = new List<string>();
            generatedOutput.Objects = new List<Object>();
            generatedOutput.QuickbeltLayoutName = "";

            int index = 0;

            if (baseFile != null)
            {
                // Loop through baseFile objects
                foreach (Object o in baseFile.Objects)
                {
                    o.Index = index;
                    index++;
                    generatedOutput.Objects.Add(o);
                }
            }

            for (int r = 0; r < mazeRooms.Count; r++)
            {
                for (int c = 0; c < mazeRooms[r].Count; c++)
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

                    if (mazeRooms[r][c].bottom.render)
                    {
                        Object bottom = new Object
                        {
                            IsContainedIn = -1,
                            QuickbeltSlotIndex = -1,
                            InSlotOfRootObjectIndex = -1,
                            InSlotOfElementIndex = -1,
                            Elements = new List<Element>(),
                            Index = index
                        };
                        bottom.Elements.Add(mazeRooms[r][c].bottom.element);
                        generatedOutput.Objects.Add(bottom);
                        index++;
                    }

                    if (mazeRooms[r][c].left.render)
                    {
                        Object left = new Object
                        {
                            IsContainedIn = -1,
                            QuickbeltSlotIndex = -1,
                            InSlotOfRootObjectIndex = -1,
                            InSlotOfElementIndex = -1,
                            Elements = new List<Element>(),
                            Index = index
                        };
                        left.Elements.Add(mazeRooms[r][c].left.element);
                        generatedOutput.Objects.Add(left);
                        index++;
                    }

                    if (mazeRooms[r][c].right.render)
                    {
                        Object right = new Object
                        {
                            IsContainedIn = -1,
                            QuickbeltSlotIndex = -1,
                            InSlotOfRootObjectIndex = -1,
                            InSlotOfElementIndex = -1,
                            Elements = new List<Element>(),
                            Index = index
                        };
                        right.Elements.Add(mazeRooms[r][c].right.element);
                        generatedOutput.Objects.Add(right);
                        index++;
                    }
                    
                    if (mazeRooms[r][c].targets != null)
                    {
                        foreach(Target t in mazeRooms[r][c].targets)
                        {
                            Object target = new Object
                            {
                                IsContainedIn = -1,
                                QuickbeltSlotIndex = -1,
                                InSlotOfRootObjectIndex = -1,
                                InSlotOfElementIndex = -1,
                                Elements = new List<Element>(),
                                Index = index
                            };
                            target.Elements.Add(t.element);
                            generatedOutput.Objects.Add(target);
                            index++;
                        }
                       
                    }
                }
            }

            //System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(generatedOutput.GetType());
            //MemoryStream ms = new MemoryStream();
            //serializer.WriteObject(ms, generatedOutput);
            //string jsonUpdated = Encoding.Default.GetString(ms.ToArray());


            
            string jsonUpdated = JsonConvert.SerializeObject(generatedOutput);
            // Did user select file path
            if (folderName != "Default")
            {
                // Yes, save there
                File.WriteAllText(folderName + "\\" + parameters.mapName + "_gp_hangar_VFS.json", jsonUpdated);
            }
            else
            {
                // No, save where exe lives
                File.WriteAllText(parameters.mapName + "_gp_hangar_VFS.json", jsonUpdated);
            }
            //MazeSave mazeSave = new MazeSave();
            //mazeSave.maze = mazeRooms;
            //mazeSave.parameters = parameters;
            //string jsonMaze = JsonConvert.SerializeObject(mazeSave, Formatting.Indented);
            //File.WriteAllText(parameters.mapName + ".json", jsonMaze);
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

        public Object()
        {


        }
    }

    public class Element
    {
        public Element()
        {


        }

        [JsonProperty(Order = 1)]
        public int Index { get; set; }

        [JsonProperty(Order = 2)]
        public string ObjectID { get; set; }

        [JsonProperty(Order = 3)]
        public string Type = "object";

        [JsonProperty(Order = 4)]
        public Posoffset PosOffset { get; set; }

        [JsonProperty(Order = 5)]
        public Orientationforward OrientationForward { get; set; }

        [JsonProperty(Order = 6)]
        public Orientationup OrientationUp = new Orientationup
        {
            x = 0,
            y = 1,
            z = 0
        };

        [JsonProperty(Order = 7)]
        public int ObjectAttachedTo = -1;

        [JsonProperty(Order = 8)]
        public int MountAttachedTo = -1;

        [JsonProperty(Order = 9)]
        public List<string> LoadedRoundsInChambers = new List<string>();

        [JsonProperty(Order = 10)]
        public List<string> LoadedRoundsInMag = new List<string>();

        [JsonProperty(Order = 11)]
        public List<string> GenericInts = new List<string>();

        [JsonProperty(Order = 12)]
        public List<string> GenericStrings = new List<string>();

        [JsonProperty(Order = 13)]
        public List<GenericVector3s> GenericVector3s = new List<GenericVector3s>();

        [JsonProperty(Order = 14)]
        public List<GenericRotations> GenericRotations = new List<GenericRotations>();


        [JsonProperty(Order = 15)]
        public Flags Flags { get;set; }
  
        public Element(bool wide)
        {
            if (wide)
            {
                ObjectID = "ShoothouseBarrierWall";
                Flags = new Flags()
                {
                    _keys = new List<string>()
                            {
                                "IsKinematicLocked",
                                "IsPickupLocked",
                                "QuickBeltSpecialStateEngaged",
                                "SpringState"
                            },

                    _values = new List<string>()
                            {
                                "True",
                                "True",
                                "False",
                                "False"
                            }
                };

            }
            else
            {
                ObjectID = "ShoothouseBarrierWallNarrow";
                Flags = new Flags()
                {
                    _keys = new List<string>()
                            {
                                "IsKinematicLocked",
                                "IsPickupLocked",
                                "QuickBeltSpecialStateEngaged",
                                "SpringState"
                            },

                    _values = new List<string>()
                            {
                                "True",
                                "True",
                                "False",
                                "False"
                            }
                };
            }
        }


    }

    public class Posoffset
    {       
        public decimal x { get; set; }
        public decimal y { get; set; }
        public decimal z { get; set; }
    }

    public class Orientationforward
    {
        public decimal x { get; set; }
        public decimal y { get; set; }
        public decimal z { get; set; }
    }

    public class GenericVector3s
    {
        public decimal x { get; set; }
        public decimal y { get; set; }
        public decimal z { get; set; }
    }

    public class GenericRotations
    {
        public decimal x { get; set; }
        public decimal y { get; set; }
        public decimal z { get; set; }
        public decimal w { get; set; }
    }

    public class Orientationup
    {
        public decimal x { get; set; }
        public decimal y { get; set; }
        public decimal z { get; set; }
    }

    public class Flags
    {
        public List<string> _keys { get; set; }
        public List<string> _values { get; set; }
    }
}
