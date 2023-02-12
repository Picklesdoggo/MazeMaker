using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeMakerUtilities
{
    public class Parameters
    {
        public float XHorizontalAStart { get; set; }
        public float XHorizontalAOffset { get; set; }
        public float ZHorizontalAStart { get; set; }
        public float ZHorizontalAOffset { get; set; }
        public float YHorizontalAStart { get; set; }


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

            // 4 possible scenarios
            // 1. Wide x Wide
            if (parameters.horizontalWide && parameters.verticalWide)
            {
                parameters.gridColumns = 9;
                parameters.XVerticalStart = 12.5F;
                parameters.XVerticalOffset = 3.2F;
                parameters.ZVerticalStart = 14.2F;
                parameters.ZVerticalOffset = 3.0F;
                parameters.YVerticalStart = 0;

                parameters.gridRows = 9;
                parameters.XHorizontalAStart = 14.1F;
                parameters.XHorizontalAOffset = 3.2F;
                parameters.ZHorizontalAStart = 12.8F;
                parameters.ZHorizontalAOffset = 3F;
                parameters.YHorizontalAStart = 0;
            }
            // 2. Narrow x Narrow
            else if (!parameters.horizontalWide && !parameters.verticalWide)
            {
                parameters.gridColumns = 28;
                parameters.XVerticalStart = 13.5F;
                parameters.XVerticalOffset = 1.2F;
                parameters.ZVerticalStart = 14.7F;
                parameters.ZVerticalOffset = 1.0F;
                parameters.YVerticalStart = 0;

                parameters.gridRows = 25;
                parameters.XHorizontalAStart = 14.1F;
                parameters.XHorizontalAOffset = 1.2F;
                parameters.ZHorizontalAStart = 14.3F;
                parameters.ZHorizontalAOffset = 1F;
                parameters.YHorizontalAStart = 0;
            }
            // 3. Wide x Narrow
            else if (parameters.horizontalWide &&  !parameters.verticalWide)
            {
                parameters.gridColumns = 9;                
                parameters.XVerticalStart = 13.5F;
                parameters.XVerticalOffset = 1.2F;
                parameters.ZVerticalStart = 14.0F;
                parameters.ZVerticalOffset = 3.0F;
                parameters.YVerticalStart = 0;

                parameters.gridRows = 25;
                parameters.XHorizontalAStart = 14.1F;
                parameters.XHorizontalAOffset = 1.2F;
                parameters.ZHorizontalAStart = 12.6F;
                parameters.ZHorizontalAOffset = 3F;
                parameters.YHorizontalAStart = 0;

            }
            // 4. Narrow x Wide
            else if (!parameters.horizontalWide && parameters.verticalWide)
            {
                parameters.gridColumns = 23;
                parameters.XVerticalStart = 12.7F;
                parameters.XVerticalOffset = 3.0F;
                parameters.ZVerticalStart = 14.7F;
                parameters.ZVerticalOffset = 1.2F;
                parameters.YVerticalStart = 0;

                parameters.gridRows = 9;
                parameters.XHorizontalAStart = 14.3F;
                parameters.XHorizontalAOffset = 3.0F;
                parameters.ZHorizontalAStart = 14.1F;
                parameters.ZHorizontalAOffset = 1.2F;
                parameters.YHorizontalAStart = 0;

            }
            else
            {
                // Do nothing
            }

            return parameters;
        }
    }
}
