using System.Collections.Generic;

namespace MazeMakerUtilities
{
    public class Parameters
    {
        public decimal XHorizontalStart { get; set; }
        public decimal XHorizontalOffset { get; set; }
        public decimal ZHorizontalStart { get; set; }
        public decimal ZHorizontalOffset { get; set; }
        public decimal YHorizontalStart { get; set; }


        public decimal XVerticalStart { get; set; }
        public decimal XVerticalOffset { get; set; }
        public decimal ZVerticalStart { get; set; }
        public decimal ZVerticalOffset { get; set; }
        public decimal YVerticalStart { get; set; }

        public int gridColumns { get; set; }
        public int gridRows { get; set; }

        public int exitRow { get; set; }
        public int exitColumn { get; set; }
        public int entranceRow { get; set; }
        public int entranceColumn { get; set; }
        public string mapName { get; set; }
        public bool horizontalWide { get; set; }
        public bool verticalWide { get; set; }

        public List<string> targets = new List<string>()
        {
            "StandingSteelIPSCSimpleRed",
            "StandingSteelIPSCMiniRed",
            "StandingSteelIPSCClassicRed"
        };

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
                parameters.XVerticalStart = 12.5M;
                parameters.XVerticalOffset = 3.2M;
                parameters.ZVerticalStart = 14.2M;
                parameters.ZVerticalOffset = 3.0M;
                parameters.YVerticalStart = 0;

                parameters.gridRows = 9;
                parameters.XHorizontalStart = 14.1M;
                parameters.XHorizontalOffset = 3.2M;
                parameters.ZHorizontalStart = 12.8M;
                parameters.ZHorizontalOffset = 3M;
                parameters.YHorizontalStart = 0;

                parameters.entranceRow = 8;
                parameters.entranceColumn = 7;

                parameters.exitRow = 0;
                parameters.exitColumn = 0;
            }
            // 2. Narrow x Narrow
            else if (!parameters.horizontalWide && !parameters.verticalWide)
            {
                parameters.gridColumns = 28;
                parameters.XVerticalStart = 13.5M;
                parameters.XVerticalOffset = 1.2M;
                parameters.ZVerticalStart = 14.7M;
                parameters.ZVerticalOffset = 1.0M;
                parameters.YVerticalStart = 0;

                parameters.gridRows = 25;
                parameters.XHorizontalStart = 14.1M;
                parameters.XHorizontalOffset = 1.2M;
                parameters.ZHorizontalStart = 14.3M;
                parameters.ZHorizontalOffset = 1M;
                parameters.YHorizontalStart = 0;

                parameters.entranceRow = 24;
                parameters.entranceColumn = 23;

                parameters.exitRow = 1;
                parameters.exitColumn = 0;
            }
            // 3. Wide x Narrow
            else if (parameters.horizontalWide &&  !parameters.verticalWide)
            {
                parameters.gridColumns = 9;                
                parameters.XVerticalStart = 13.5M;
                parameters.XVerticalOffset = 1.2M;
                parameters.ZVerticalStart = 14.0M;
                parameters.ZVerticalOffset = 3.0M;
                parameters.YVerticalStart = 0;

                parameters.gridRows = 25;
                parameters.XHorizontalStart = 14.1M;
                parameters.XHorizontalOffset = 1.2M;
                parameters.ZHorizontalStart = 12.6M;
                parameters.ZHorizontalOffset = 3M;
                parameters.YHorizontalStart = 0;

                parameters.entranceRow = 24;
                parameters.entranceColumn = 27;

                parameters.exitRow = 1;
                parameters.exitColumn = 0;

            }
            // 4. Narrow x Wide
            else if (!parameters.horizontalWide && parameters.verticalWide)
            {
                parameters.gridColumns = 23;
                parameters.XVerticalStart = 12.7M;
                parameters.XVerticalOffset = 3.0M;
                parameters.ZVerticalStart = 14.7M;
                parameters.ZVerticalOffset = 1.2M;
                parameters.YVerticalStart = 0;

                parameters.gridRows = 10;
                parameters.XHorizontalStart = 14.3M;
                parameters.XHorizontalOffset = 3.0M;
                parameters.ZHorizontalStart = 14.1M;
                parameters.ZHorizontalOffset = 1.2M;
                parameters.YHorizontalStart = 0;

                parameters.entranceRow = 9;
                parameters.entranceColumn = 18;

                parameters.exitRow = 0;
                parameters.exitColumn = 0;

            }
            else
            {
                // Do nothing
            }

            return parameters;
        }
    }
}
