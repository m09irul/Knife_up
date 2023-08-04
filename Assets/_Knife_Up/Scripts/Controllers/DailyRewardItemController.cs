using UnityEngine;
using UnityEngine.UI;
using System;

namespace OnefallGames
{
    public class DailyRewardItemController : MonoBehaviour
    {
        [Header("Item References")]
        [SerializeField] private Text rewardDayTxt = null;
        [SerializeField] private Text rewardedCoinTxt = null;
        [SerializeField] private GameObject claimedPanel = null;



        /// <summary>
        /// Determine whether this reward item is claimed or not
        /// </summary>
        public bool IsClaimed { get { return PlayerPrefs.GetInt(IS_CLAIMED_PPK, 0) == 1; } }

        private string IS_CLAIMED_PPK { get { return "CLAIMED_" + dayItem.ToString(); } }
        private string SAVED_DAY_TIME_PPK { get { return "TIME_" + dayItem.ToString(); } }

        private DayItem dayItem = DayItem.DAY_1;

        public void SetValues(DayItem dayItem, int rewardedCoins)
        {
            //Set values
            this.dayItem = dayItem;

            //Update UI
            rewardDayTxt.text = ("DAY " + this.dayItem.ToString().Split('_')[1]).ToUpper();
            rewardedCoinTxt.text = rewardedCoins.ToString();

            if (!IsClaimed) //Still not unlock yet
            {
                claimedPanel.SetActive(false);
            }
            else //Already unlocked
            {
                claimedPanel.SetActive(true);
            }

            if (dayItem == DayItem.DAY_1 && string.IsNullOrEmpty(PlayerPrefs.GetString(SAVED_DAY_TIME_PPK, string.Empty)))
            {
                SetDayTimeValue();
            }
        }


        /// <summary>
        /// Set the current daytime for this item.
        /// </summary>
        public void SetDayTimeValue()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            int second = DateTime.Now.Second;

            string savedDayTime = year + ":" + month + ":" + day + ":" + hour + ":" + minute + ":" + second;
            PlayerPrefs.SetString(SAVED_DAY_TIME_PPK, savedDayTime);
        }


        /// <summary>
        /// Set this item to be claimed
        /// </summary>
        public void SetClaimReward()
        {
            PlayerPrefs.SetInt(IS_CLAIMED_PPK, 1);
            claimedPanel.SetActive(true);
        }


        /// <summary>
        /// reset all the values of this item to default. 
        /// </summary>
        public void ResetValues()
        {
            PlayerPrefs.SetInt(IS_CLAIMED_PPK, 0);
            claimedPanel.SetActive(false);
        }


        /// <summary>
        /// Return the time remain to claim the reward from the day the app was open.
        /// </summary>
        /// <returns></returns>
        public double TimeRemains()
        {
            //Get the saved date time
            string savedDay = PlayerPrefs.GetString(SAVED_DAY_TIME_PPK);
            int year = int.Parse(savedDay.Split(':')[0]);
            int month = int.Parse(savedDay.Split(':')[1]);
            int day = int.Parse(savedDay.Split(':')[2]);
            int hour = int.Parse(savedDay.Split(':')[3]);
            int minute = int.Parse(savedDay.Split(':')[4]);
            int second = int.Parse(savedDay.Split(':')[5]);
            DateTime savedDateTime = new DateTime(year, month, day, hour, minute, second);

            //Calculate the remain seconds
            TimeSpan timePassed = DateTime.Now.Subtract(savedDateTime);
            double remainSeconds = (dayItem == DayItem.DAY_1) ? 0 : (24 - timePassed.TotalHours) * 60 * 60;
            if (remainSeconds <= 0)
                remainSeconds = 0;

            //Parse the seconds to time format
            return remainSeconds;
        }
    }
}
