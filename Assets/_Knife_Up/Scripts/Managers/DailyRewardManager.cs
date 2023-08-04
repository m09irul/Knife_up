using UnityEngine;
using System;

namespace OnefallGames
{
    [Serializable]
    public class DailyRewardItem
    {
        [SerializeField] private DayItem DayItem = DayItem.DAY_1;

        /// <summary>
        /// Get the day type of this DailyRewardItem.
        /// </summary>
        public DayItem GetDayItem { get { return DayItem; } }


        [SerializeField] private int RewardedCoins = 0;


        /// <summary>
        /// The amount of coins to reward for player.
        /// </summary>
        public int GetRewardedCoins { get { return RewardedCoins; } }
    }

    public class DailyRewardManager : MonoBehaviour
    {
        [SerializeField] private DailyRewardItem[] dailyRewardItems = null;
        public DailyRewardItem[] DailyRewardItems { get { return dailyRewardItems; } }
    }

}
