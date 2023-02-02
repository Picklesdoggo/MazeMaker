using System;
using System.Collections.Generic;
using System.Linq;
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
