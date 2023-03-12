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
            "StandingSteelIPSCClassicRed"
        };
        private static Random targetChoice = new Random();
        public static List<Target> getTargets(string direction)
        {
            List<Target> chosenTargets = new List<Target>();
            Target target = new Target();
            Element element = new Element();
            
            string targetType = targets[targetChoice.Next(0,targets.Count)];
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

            element.PosOffset = new Posoffset();

            target.element = element;
            chosenTargets.Add(target);
            return chosenTargets;
        }
    }
}
