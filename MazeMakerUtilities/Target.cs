using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeMakerUtilities
{
    public class ManualTarget
    {
        public string targetName { get; set; }
        public double width { get; set; }
        public double height { get; set; }
    }

    public class Target
    {
        public Element element = null;

        private static List<string> targetGroups = new List<string>()
        {
            "StandingSteelIPSCSimpleRed",
            "StandingSteelIPSCMiniRed",
            "StandingSteelIPSCClassicRed",
            "DestructibleOnPost"
             
        };

        private static List<ManualTarget> manualTargets = new List<ManualTarget>()
        {
            new ManualTarget{targetName = "StandingSteelDuelingTree", height = 0.75, width = 0.75},
            new ManualTarget{targetName = "StandingSteelIPSCClassic", height = 0.55, width = 0.55},
            new ManualTarget{targetName = "StandingSteelIPSCClassicBlue", height = 0.55, width = 0.55},
            new ManualTarget{targetName = "StandingSteelIPSCClassicRed", height = 0.55, width = 0.55},
            new ManualTarget{targetName = "StandingSteelIPSCMini", height = 0.55, width = 0.55},
            new ManualTarget{targetName = "StandingSteelIPSCMiniBlue", height = 0.55, width = 0.55},
            new ManualTarget{targetName = "StandingSteelIPSCMiniRed", height = 0.55, width = 0.55},
            new ManualTarget{targetName = "StandingSteelIPSCSimple", height = 0.55, width = 0.55},
            new ManualTarget{targetName = "StandingSteelIPSCSimpleBlue", height = 0.55, width = 0.55},
            new ManualTarget{targetName = "StandingSteelIPSCSimpleRed", height = 0.55, width = 0.55},
            new ManualTarget{targetName = "StandingSteelSpinner", height = 0.30, width = 0.30},
            new ManualTarget{targetName = "StandingSteelTargetClassicPop", height = 0.25, width = 0.95},
            new ManualTarget{targetName = "StandingSteelTargetMiniClassicPop", height = 0.25, width = 0.65},
            new ManualTarget{targetName = "StandingSteelTargetMiniPepperPop", height = 0.25, width = 0.80},
            new ManualTarget{targetName = "StandingSteelTargetPepperPop", height = 0.25, width = 1.15},
            new ManualTarget{targetName = "StandingSteelTargetSpeedPop", height = 0.25, width = 0.80},
            new ManualTarget{targetName = "WoodStandeeSoldierLeft", height = 0.55, width = 0.55},
            new ManualTarget{targetName = "WoodStandeeSoldierRight", height = 0.55, width = 0.55},
            new ManualTarget{targetName = "WoodStandeeUncleSpam", height = 0.55, width = 0.55}
        };

        public static List<ManualTarget> getManualTargets()
        {
            return manualTargets;
        }

        private static List<string> destructibleTypes = new List<string>()
        {
            "SodaCanSarge",
            "Watermelon",
            "ClayPotSmall",
            "ClayPotLarge",
            "Apple",
            "GlassBeerBottleGutshot"
        };

        private static Random targetChoice = new Random();
        private static Random destructibleChoice = new Random();
        public static List<Target> getTargets(string direction, Room targetRoom, Parameters inputParameters)
        {
            List<Target> chosenTargets = new List<Target>();
            string targetType = targetGroups[targetChoice.Next(0, targetGroups.Count)];

            if (targetType.Contains("StandingSteel"))
            {
                chosenTargets.Add(getStandingTarget(direction,targetType,targetRoom,inputParameters));
            }
            else if (targetType == "DestructibleOnPost")
            {
                chosenTargets = destructibleOnPost(targetRoom, inputParameters);
            }

            return chosenTargets;
        }

        private static List<Target> destructibleOnPost(Room targetRoom, Parameters inputParameters)
        {
            List<Target> destructibleOnPost = new List<Target>();
            
            // Left Post
            Target leftPost = new Target();
            leftPost.element = new Element();

            leftPost.element.PosOffset = new Posoffset()
            {
                x = targetRoom.top.element.PosOffset.x - (inputParameters.XHorizontalOffset / 2),
                y = 0.6M,
                z = targetRoom.right.element.PosOffset.z + (inputParameters.ZHorizontalOffset / 2)
            };

            leftPost.element.OrientationForward = new Orientationforward()
            {
                x = 0,
                y = 1,
                z = 0
            };

            leftPost.element.OrientationUp = new Orientationup()
            {
                x = 0,
                y = 0,
                z = -1
            };
            leftPost.element.ObjectID = "CompBoard4ft";
            leftPost.element.Flags = new Flags()
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
            destructibleOnPost.Add(leftPost);

            // Destructible
            string destructibleType = destructibleTypes[destructibleChoice.Next(0, destructibleTypes.Count)];
            Target dessturcible = new Target();
            dessturcible.element = new Element();
            dessturcible.element.PosOffset = new Posoffset()
            {
                x = targetRoom.top.element.PosOffset.x - (inputParameters.XHorizontalOffset / 2),               
                z = targetRoom.right.element.PosOffset.z + (inputParameters.ZHorizontalOffset / 2)
            };
            dessturcible.element.OrientationForward = new Orientationforward()
            {
                x = 0,
                y = 0,
                z = 1
            };
            dessturcible.element.OrientationUp = new Orientationup()
            {
                x = 0,
                y = 1,
                z = 0
            };
            
            dessturcible.element.Flags = new Flags()
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

            // Set Y offset
            if (destructibleType == "Watermelon")
            {
                dessturcible.element.PosOffset.y = 1.33M;
            }
            else if (destructibleType == "ClayPotSmall")
            {
                dessturcible.element.PosOffset.y = 1.30M;
            }
            else if (destructibleType == "ClayPotLarge")
            {
                dessturcible.element.PosOffset.y = 1.36M;
            }
            else if (destructibleType == "Apple")
            {
                dessturcible.element.PosOffset.y = 1.25M;
            }
            else if (destructibleType == "GlassBeerBottleGutshot")
            {
                dessturcible.element.PosOffset.y = 1.31M;
            }
            else if (destructibleType == "SodaCanSarge")
            {
                dessturcible.element.PosOffset.y = 1.265M;
            }
            dessturcible.element.ObjectID = destructibleType;
            destructibleOnPost.Add(dessturcible);

            return destructibleOnPost;
        }

        public static Target getStandingTarget(string direction, string targetType, Room targetRoom, Parameters inputParameters)
        {
            Target target = new Target();
            Element element = new Element();
            element.PosOffset = new Posoffset()
            {
                x = targetRoom.top.element.PosOffset.x - (inputParameters.XHorizontalOffset / 2),
                y = 0,
                z = targetRoom.right.element.PosOffset.z + (inputParameters.ZHorizontalOffset / 2)
            };


            #region Direction

            if (direction == "Right")
            {
                element.OrientationForward = new Orientationforward()
                {
                    x = 0,
                    y = 0,
                    z = 1
                };
            }
            else if (direction == "Left")
            {
                element.OrientationForward = new Orientationforward()
                {
                    x = 0,
                    y = 0,
                    z = -1
                };
            }
            else if (direction == "Above")
            {
                element.OrientationForward = new Orientationforward()
                {
                    x = 1,
                    y = 0,
                    z = 0
                };
            }
            else if (direction == "Below")
            {
                element.OrientationForward = new Orientationforward()
                {
                    x = -1,
                    y = 0,
                    z = 0
                };
            }

            #endregion

            element.ObjectID = targetType;

            element.Flags = new Flags()
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

            target.element = element;
            return target;
        }

        public static Target getManualTarget(decimal xOffset, decimal ZOffset, string objectID)
        {
            Target target = new Target();
            Element element = new Element();
            element.PosOffset = new Posoffset()
            {
                x = xOffset,
                y = 0,
                z = ZOffset
            };

            element.OrientationForward = new Orientationforward()
            {
                x = -1,
                y = 0,
                z = 0
            };

            element.ObjectID = objectID;

            element.Flags = new Flags()
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

            target.element = element;
            return target;
        }
    }
}
