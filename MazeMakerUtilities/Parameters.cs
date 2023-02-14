namespace MazeMakerUtilities
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
                parameters.XHorizontalStart = 14.1F;
                parameters.XHorizontalOffset = 3.2F;
                parameters.ZHorizontalStart = 12.8F;
                parameters.ZHorizontalOffset = 3F;
                parameters.YHorizontalStart = 0;
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
                parameters.XHorizontalStart = 14.1F;
                parameters.XHorizontalOffset = 1.2F;
                parameters.ZHorizontalStart = 14.3F;
                parameters.ZHorizontalOffset = 1F;
                parameters.YHorizontalStart = 0;
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
                parameters.XHorizontalStart = 14.1F;
                parameters.XHorizontalOffset = 1.2F;
                parameters.ZHorizontalStart = 12.6F;
                parameters.ZHorizontalOffset = 3F;
                parameters.YHorizontalStart = 0;

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

                parameters.gridRows = 10;
                parameters.XHorizontalStart = 14.3F;
                parameters.XHorizontalOffset = 3.0F;
                parameters.ZHorizontalStart = 14.1F;
                parameters.ZHorizontalOffset = 1.2F;
                parameters.YHorizontalStart = 0;

            }
            else
            {
                // Do nothing
            }

            return parameters;
        }
    }
}
