﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeMakerUtilities
{
    public class Target
    {
        public Element element = null;

        private static List<string> targetGroups = new List<string>()
        {
            "StandingSteelIPSCSimpleRed",
            "StandingSteelIPSCMiniRed",
            "StandingSteelIPSCClassicRed",
            "DestructableOnPost"
        };

        private static List<string> destructableTypes = new List<string>()
        {
            "SodaCanSarge",
            "Watermelon",
            "ClayPotSmall",
            "ClayPotLarge",
            "Apple",
            "GlassBeerBottleGutshot"
        };

        private static Random targetChoice = new Random();
        private static Random destructableChoice = new Random();
        public static List<Target> getTargets(string direction, Room targetRoom, Parameters inputParameters)
        {
            List<Target> chosenTargets = new List<Target>();
            string targetType = targetGroups[targetChoice.Next(0, targetGroups.Count)];

            if (targetType.Contains("StandingSteel"))
            {
                chosenTargets.Add(getStandingTarget(direction,targetType,targetRoom,inputParameters));
            }
            else if (targetType == "DestructableOnPost")
            {
                chosenTargets = destructableOnPost(targetRoom, inputParameters);
            }

            return chosenTargets;
        }

        private static List<Target> destructableOnPost(Room targetRoom, Parameters inputParameters)
        {
            List<Target> destructableOnPost = new List<Target>();
            
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
            destructableOnPost.Add(leftPost);

            // Destructable
            string destructableType = destructableTypes[destructableChoice.Next(0, destructableTypes.Count)];
            Target dessturcable = new Target();
            dessturcable.element = new Element();
            dessturcable.element.PosOffset = new Posoffset()
            {
                x = targetRoom.top.element.PosOffset.x - (inputParameters.XHorizontalOffset / 2),               
                z = targetRoom.right.element.PosOffset.z + (inputParameters.ZHorizontalOffset / 2)
            };
            dessturcable.element.OrientationForward = new Orientationforward()
            {
                x = 0,
                y = 0,
                z = 1
            };
            dessturcable.element.OrientationUp = new Orientationup()
            {
                x = 0,
                y = 1,
                z = 0
            };
            
            dessturcable.element.Flags = new Flags()
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
            if (destructableType == "Watermelon")
            {
                dessturcable.element.PosOffset.y = 1.33M;
            }
            else if (destructableType == "ClayPotSmall")
            {
                dessturcable.element.PosOffset.y = 1.30M;
            }
            else if (destructableType == "ClayPotLarge")
            {
                dessturcable.element.PosOffset.y = 1.36M;
            }
            else if (destructableType == "Apple")
            {
                dessturcable.element.PosOffset.y = 1.25M;
            }
            else if (destructableType == "GlassBeerBottleGutshot")
            {
                dessturcable.element.PosOffset.y = 1.31M;
            }
            else if (destructableType == "SodaCanSarge")
            {
                dessturcable.element.PosOffset.y = 1.265M;
            }
            dessturcable.element.ObjectID = destructableType;
            destructableOnPost.Add(dessturcable);

            return destructableOnPost;
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
    }
}
