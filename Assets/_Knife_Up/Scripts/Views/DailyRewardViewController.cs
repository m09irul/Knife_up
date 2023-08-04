using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace OnefallGames
{
    public class DailyRewardViewController : MonoBehaviour
    {
        [SerializeField] private RectTransform dailyRewardPanelTrans = null;
        [SerializeField] private GameObject blackPanel = null;
        [SerializeField] private Text nextRewardCoinTxt = null;
        [SerializeField] private Button claimBtn = null;
        [SerializeField] private Text claimTxt = null;
        [SerializeField] private GameObject freeCoinsBtn = null;
        [SerializeField] private DailyRewardItemController[] dailyRewardItemControls = null;


        public bool IsRewardAvailable { get; private set; }

        private int currentIndex = -1;
        private bool isFillItemValues = false;


        public void OnShow()
        {
            if (!isFillItemValues)
            {
                isFillItemValues = true;

                //Fill the value for the items 
                for (int i = 0; i < ServicesManager.Instance.DailyRewardManager.DailyRewardItems.Length; i++)
                {
                    DailyRewardItem item = ServicesManager.Instance.DailyRewardManager.DailyRewardItems[i];
                    dailyRewardItemControls[i].SetValues(item.GetDayItem, item.GetRewardedCoins);
                }
            }

            //Update the view
            blackPanel.SetActive(true);
            ViewManager.Instance.MoveRect(dailyRewardPanelTrans, dailyRewardPanelTrans.anchoredPosition, new Vector2(0, dailyRewardPanelTrans.anchoredPosition.y), 0.5f);

            //Update the index
            for (int i = 0; i < dailyRewardItemControls.Length; i++)
            {
                if (!dailyRewardItemControls[i].IsClaimed)
                {
                    //Set the current daytime for this item
                    currentIndex = i;
                    break;
                }
            }

            freeCoinsBtn.SetActive(ServicesManager.Instance.AdManager.IsRewardedAdReady());
        }

        private void Update()
        {
            if (currentIndex != -1)
            {
                //Update the time remains
                double timeRemains = dailyRewardItemControls[currentIndex].TimeRemains();

                //Update rewarded coins
                nextRewardCoinTxt.text = ServicesManager.Instance.DailyRewardManager.DailyRewardItems[currentIndex].GetRewardedCoins.ToString();

                //Update the claim button
                if (timeRemains > 0)
                {
                    IsRewardAvailable = false;
                    claimBtn.interactable = false;
                    claimTxt.text = Utilities.SecondsToTimeFormat(timeRemains);
                }
                else
                {
                    IsRewardAvailable = true;
                    claimBtn.interactable = true;
                    claimTxt.text = "CLAIM";
                }
            }
        }



        /// <summary>
        /// Get the amount of time till next reward.
        /// </summary>
        /// <returns></returns>
        public int GetAmountOfTimeTillNextReward()
        {
            if (currentIndex != -1)
            {
                return (int)dailyRewardItemControls[currentIndex].TimeRemains();
            }
            else
            {
                return 300;
            }
        }



        public void ClaimBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();

            //Claim the reward of the current DailyRewardItemControl
            int rewardedCoins = ServicesManager.Instance.DailyRewardManager.DailyRewardItems[currentIndex].GetRewardedCoins;
            ServicesManager.Instance.RewardCoinManager.RewardTotalCoins(rewardedCoins, 0.2f);
            dailyRewardItemControls[currentIndex].SetClaimReward();

            //We just claimed the last item, we need to reset all the items
            if (currentIndex == dailyRewardItemControls.Length - 1)
            {
                //Reset all daily reward item
                foreach (DailyRewardItemController o in dailyRewardItemControls)
                {
                    o.ResetValues();
                }

                currentIndex = 0;
                dailyRewardItemControls[currentIndex].SetDayTimeValue();
            }
            else
            {
                //Set the day for next item
                currentIndex++;
                dailyRewardItemControls[currentIndex].SetDayTimeValue();
            }

            nextRewardCoinTxt.text = ServicesManager.Instance.DailyRewardManager.DailyRewardItems[currentIndex].GetRewardedCoins.ToString();
            FindObjectOfType<HomeManager>().CreateRewardedCoins(rewardedCoins);
            dailyRewardPanelTrans.anchoredPosition = new Vector2(700, dailyRewardPanelTrans.anchoredPosition.y);
            blackPanel.SetActive(false);
            gameObject.SetActive(false);
        }
        public void FreeCoinsBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            dailyRewardPanelTrans.anchoredPosition = new Vector2(700, dailyRewardPanelTrans.anchoredPosition.y);
            blackPanel.SetActive(false);
            gameObject.SetActive(false);
            ServicesManager.Instance.AdManager.ShowRewardedAd();
        }

        public void CloseBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            dailyRewardPanelTrans.anchoredPosition = new Vector2(700, dailyRewardPanelTrans.anchoredPosition.y);
            ViewManager.Instance.HomeViewController.OnSubViewClose();
            blackPanel.SetActive(false);
            gameObject.SetActive(false);
        }
    }

}
