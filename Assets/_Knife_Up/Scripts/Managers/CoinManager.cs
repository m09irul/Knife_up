using UnityEngine;

namespace OnefallGames
{
    public class CoinManager : MonoBehaviour
    {
        [SerializeField] int initialCoins = 0;

        public int CollectedCoins { private set; get; }


        public int TotalCoins
        {
            private set
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_TOTAL_COINS, value);
            }
            get
            {
                return PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_TOTAL_COINS, initialCoins);
            }
        }


        /// <summary>
        /// Add to total coins by given amount
        /// </summary>
        /// <param name="amount"></param>
        public void AddTotalCoins(int amount)
        {
            TotalCoins += amount;
            PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_TOTAL_COINS, TotalCoins);
        }


        /// <summary>
        /// Remove from total coins by given amount
        /// </summary>
        /// <param name="amount"></param>
        public void RemoveTotalCoins(int amount)
        {
            TotalCoins -= amount;
            PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_TOTAL_COINS, TotalCoins);
        }


        /// <summary>
        /// Set the TotalCoins by the given amount.
        /// </summary>
        /// <param name="amount"></param>
        public void SetTotalCoins(int amount)
        {
            TotalCoins = amount;
            PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_TOTAL_COINS, TotalCoins);
        }

        /// <summary>
        /// Add collected coins by given amount
        /// </summary>
        /// <param name="amount"></param>
        public void AddCollectedCoins(int amount)
        {
            CollectedCoins += amount;
        }


        /// <summary>
        /// Remove from collected coins by given amount
        /// </summary>
        /// <param name="amount"></param>
        public void RemoveCollectedCoins(int amount)
        {
            CollectedCoins -= amount;
        }


        /// <summary>
        /// Set the CollectedCoins by the given amount.
        /// </summary>
        /// <param name="amount"></param>
        public void SetCollectedCoins(int amount)
        {
            CollectedCoins = amount;
        }



        /// <summary>
        /// Reset collected coins
        /// </summary>
        public void ResetCollectedCoins()
        {
            CollectedCoins = 0;
        }

    }
}
