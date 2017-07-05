using System;
using System.Drawing;

namespace MiNETDevTools.Graphics.Biomes
{
    /// <summary>
    /// Heat Map background colour calculations
    /// Derived from: http://blogs.technet.com/andrew/archive/2007/12/06/heat-maps-in-sql-server-reporting-services-2005.aspx
    /// </summary>
    public class HeatMap
    {
        #region Public Method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redStartVal"></param>
        /// <param name="yellowStartVal"></param>
        /// <param name="greenStartVal"></param>
        /// <param name="val"></param>
        /// <returns>The RGB hex color string</returns>
        public static Color GetColor(decimal redStartVal, decimal yellowStartVal, decimal greenStartVal, decimal val)
        {
            // color points
            int[] Red = new int[] { 255, 255, 255 }; // #FCBF7B
            int[] Yellow = new int[] { 254, 255, 132 }; // #FEEB84
            int[] Green = new int[] { 99, 190, 123 };  // #63BE7B
            int[] White = new int[] { 255, 255, 255 }; // #FFFFFF

            // value that corresponds to the color that represents the tier above the value - determined later
            Decimal highValue = 0.0M;
            // value that corresponds to the color that represents the tier below the value
            Decimal lowValue = 0.0M;
            // next higher and lower color tiers (set to corresponding member variable values)
            int[] highColor = null;
            int[] lowColor = null;

            // 3-integer array of color values (r,g,b) that will ultimately be converted to hex
            int[] rgb = null;


            // If value lower than green start value, it must be green.
            if (val <= greenStartVal)
            {
                rgb = Green;
            }
            // determine if value lower than the baseline of the red tier
            else if (val >= redStartVal)
            {
                rgb = Red;
            }

            // if not, then determine if value is between the red and yellow tiers
            else if (val > yellowStartVal)
            {
                highValue = redStartVal;
                lowValue = yellowStartVal;
                highColor = Red;
                lowColor = Yellow;
            }

            // if not, then determine if value is between the yellow and green tiers
            else if (val > greenStartVal)
            {
                highValue = yellowStartVal;
                lowValue = greenStartVal;
                highColor = Yellow;
                lowColor = Green;
            }
            // must be green
            else
            {
                rgb = Green;
            }

            // get correct color values for values between dark red and green
            if (rgb == null)
            {
                rgb = GetColorValues(highValue, lowValue, highColor, lowColor, val);
            }

            // return the hex string
            return Color.FromArgb(rgb[0], rgb[1], rgb[2]);
        }
        #endregion

        /// <summary>
        /// Looks at the passed in value and high and low boundary values and determines the
        /// relative color values (rgb) in proportion to the relative distance the value is found 
        /// between the boundaries. The rgb color string is calculated proportionately between the 
        /// highColor/lowColor commensurate with the proportions the passed-in value exists within 
        /// the boundary values
        /// </summary>
        /// <param name="highBound">closest decimal boundary above the value (val)</param>
        /// <param name="lowBound">closest decimal boundary above the value (val)</param>
        /// <param name="highColor">color (int[] {r,g,b}) that corresponds to the highBound</param>
        /// <param name="lowColor">color (int[] {r,g,b}) that corresponds to the lowBound</param>
        /// <param name="val">pass rate</param>
        /// <returns>a 3-member int[] array (rgb)</returns>
        private static int[] GetColorValues(decimal highBound, decimal lowBound, int[] highColor, int[] lowColor, decimal val)
        {
            // proportion the val is between the high and low bounds
            decimal ratio = (val - lowBound) / (highBound - lowBound);
            int[] rgb = new int[3];
            // step through each color and find the value that represents the approriate proportional value 
            // between the high and low colors
            for (int i = 0; i < 3; i++)
            {
                int hc = (int)highColor[i];
                int lc = (int)lowColor[i];
                // high color is lower than low color - reverse the subtracted vals
                bool reverse = hc < lc;
                // difference between the high and low values
                int diff = reverse ? lc - hc : hc - lc;
                // lowest value of the two
                int baseVal = reverse ? hc : lc;
                rgb[i] = (int)Math.Round((decimal)diff * ratio) + baseVal;
            }
            return rgb;
        }
    }
}
