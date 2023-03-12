using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeMakerUtilities
{
    public class Target
    {
        public Element element = null;

        private static List<string> targets = new List<string>()
        {
            "StandingSteelIPSCSimpleRed",
            "StandingSteelIPSCMiniRed",
            "StandingSteelIPSCClassicRed",
            "CanOnPost"
        };
        private static Random targetChoice = new Random();
        public static List<Target> getTargets(string direction, Room targetRoom, Parameters inputParameters)
        {
            List<Target> chosenTargets = new List<Target>();
            string targetType = targets[targetChoice.Next(0, targets.Count)];

            if (targetType.Contains("StandingSteel"))
            {
                chosenTargets.Add(getStandingTarget(direction,targetType,targetRoom,inputParameters));
            }
            else if (targetType == "CanOnPost")
            {
                chosenTargets = getCanOnPost(targetRoom, inputParameters);
            }

            return chosenTargets;
        }

        private static List<Target> getCanOnPost(Room targetRoom, Parameters inputParameters)
        {
            List<Target> canOnPost = new List<Target>();
            
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
            canOnPost.Add(leftPost);

            // Soda Can
            Target sodaCan = new Target();
            sodaCan.element = new Element();
            sodaCan.element.PosOffset = new Posoffset()
            {
                x = targetRoom.top.element.PosOffset.x - (inputParameters.XHorizontalOffset / 2),
                y = 1.26M,
                z = targetRoom.right.element.PosOffset.z + (inputParameters.ZHorizontalOffset / 2)
            };
            sodaCan.element.OrientationForward = new Orientationforward()
            {
                x = 0,
                y = 0,
                z = 1
            };
            sodaCan.element.OrientationUp = new Orientationup()
            {
                x = 0,
                y = 1,
                z = 0
            };
            sodaCan.element.ObjectID = "SodaCanSarge";
            sodaCan.element.Flags = new Flags()
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
            canOnPost.Add(sodaCan);

            return canOnPost;
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
