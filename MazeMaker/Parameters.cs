using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeMaker
{
    public class Parameters
    {
        public float XHorizontalStart { get; set; }
        public float XHorizontalOffset { get; set; }
        public float ZHorizontalStart { get; set; }
        public float ZHorizontalOffset { get; set; }
        public float YHorizontalStart { get; set; }
        public float XVerticalStart { get; set; }
        public float XVerticalOffset { get; set; }
        public float ZVerticalStart { get; set; }
        public float ZVerticalOffset { get; set; }
        public float YVerticalStart { get; set; }
        public int gridColumns { get; set; }
        public int gridRows { get; set; }
        
        public string mapName { get; set; }
        public bool horizontalWide { get; set; }
        public bool verticalWide { get; set; }

        public static Parameters generateParameters(bool verticalWideInput, bool horizontalWideInput, string mapNameInput)
        {
            Parameters parameters = new Parameters();
            parameters.verticalWide = verticalWideInput;
            parameters.horizontalWide = horizontalWideInput;
            parameters.mapName = mapNameInput;

            if (parameters.verticalWide)
            {
                parameters.gridRows = 9;

                parameters.XVerticalStart = 12.5F;
                parameters.XVerticalOffset = 3.2F;

                parameters.ZVerticalStart = 14.2F;
                parameters.ZVerticalOffset = 3.0F;

                parameters.YVerticalStart = 0;
            }
            else
            {
                parameters.gridRows = 25;

                parameters.XVerticalStart = 13.5F;
                parameters.XVerticalOffset = 1.2F;

                parameters.ZVerticalStart = 14.7F;
                parameters.ZVerticalOffset = 1.0F;

                parameters.YVerticalStart = 0;
            }

            if (parameters.horizontalWide)
            {
                parameters.gridColumns = 9;

                parameters.XHorizontalStart = 14.1F;
                parameters.XHorizontalOffset = 3.2F;

                parameters.ZHorizontalStart = 12.8F;
                parameters.ZHorizontalOffset = 3F;

                parameters.YHorizontalStart = 0;

            }
            else
            {
                parameters.gridColumns = 28;

                parameters.XHorizontalStart = 14.1F;
                parameters.XHorizontalOffset = 1.2F;

                parameters.ZHorizontalStart = 14.3F;
                parameters.ZHorizontalOffset = 1F;

                parameters.YHorizontalStart = 0;

            }

            return parameters;
        }
    }
}
