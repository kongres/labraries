﻿namespace Kongrevsky.Utilities.Math
{
    #region << Using >>

    using System;

    #endregion

    public static class DecimalUtils
    {
        /// <summary>
        /// Detects if Decimal is less than specified value
        /// </summary>
        /// <param name="decimal1"></param>
        /// <param name="decimal2"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool LessThan(this decimal decimal1, decimal decimal2, int precision = 2)
        {
            return Math.Round(decimal1 - decimal2, precision) < 0;
        }

        /// <summary>
        /// Detects if Decimal is less than or equal to specified value
        /// </summary>
        /// <param name="decimal1"></param>
        /// <param name="decimal2"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool LessThanOrEqualTo(this decimal decimal1, decimal decimal2, int precision = 2)
        {
            return Math.Round(decimal1 - decimal2, precision) <= 0;
        }

        /// <summary>
        /// Detects if Decimal is greater than specified value
        /// </summary>
        /// <param name="decimal1"></param>
        /// <param name="decimal2"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool GreaterThan(this decimal decimal1, decimal decimal2, int precision = 2)
        {
            return Math.Round(decimal1 - decimal2, precision) > 0;
        }

        /// <summary>
        /// Detects if Decimal is greater than or equal to specified value
        /// </summary>
        /// <param name="decimal1"></param>
        /// <param name="decimal2"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool GreaterThanOrEqualTo(this decimal decimal1, decimal decimal2, int precision = 2)
        {
            return Math.Round(decimal1 - decimal2, precision) >= 0;
        }

        /// <summary>
        /// Detects if Decimal is almost equal to specified value
        /// </summary>
        /// <param name="decimal1"></param>
        /// <param name="decimal2"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        public static bool AlmostEquals(this decimal decimal1, decimal decimal2, int precision = 2)
        {
            return Math.Round(decimal1 - decimal2, precision) == 0;
        }

        /// <summary>
        /// Truncate value to specified precision
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="precision">Precision</param>
        /// <returns></returns>
        public static decimal TruncateEx(this decimal value, byte precision)
        {
            var round = Math.Round(value, precision);

            if (value > 0 && round > value)
                return round - new decimal(1, 0, 0, false, precision);

            if (value < 0 && round < value)
                return round + new decimal(1, 0, 0, false, precision);

            return round;
        }
    }
}