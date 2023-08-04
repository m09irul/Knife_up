using System;
using UnityEngine;

namespace OnefallGames
{
    public class Utilities
    {

        /// <summary>
        /// Covert the given seconds to time format.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string SecondsToTimeFormat(double seconds)
        {
            int hours = (int)seconds / 3600;
            int mins = ((int)seconds % 3600) / 60;
            seconds = Math.Round(seconds % 60, 0);
            return hours + ":" + mins + ":" + seconds;
        }

        /// <summary>
        /// Covert the given seconds to minutes format.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static string SecondsToMinutesFormat(double seconds)
        {
            int mins = ((int)seconds % 3600) / 60;
            seconds = Math.Round(seconds % 60, 0);
            return mins + ":" + seconds;
        }
    }
}
