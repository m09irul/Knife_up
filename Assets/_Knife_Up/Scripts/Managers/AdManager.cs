using System.Collections.Generic;
using UnityEngine;

namespace OnefallGames
{
    public class AdManager : MonoBehaviour
    {

        [Header("Banner Ad config")]
        [SerializeField] private BannerAdType bannerAdType = BannerAdType.NONE;
        [SerializeField] private float showingBannerAdDelay = 0.5f;


        [Header("Interstitial Ad Config")]
        [SerializeField] private List<InterstitialAdConfig> listShowInterstitialAdConfig = new List<InterstitialAdConfig>();

        [Header("Rewarded Video Ad Config")]
        [SerializeField] private float showingRewardedVideoAdDelay = 0.2f;
        [SerializeField] private List<RewardedAdType> listRewardedAdType = new List<RewardedAdType>();

        [Header("Rewarded Coins Config")]
        [SerializeField] private int minRewardedCoins = 100;
        [SerializeField] private int maxRewardedCoins = 150;
        [SerializeField] private float rewardDelay = 0.5f;


        [Header("AdManager References")]
        [SerializeField] private AdmobController admobController = null;
        [SerializeField] private UnityAdController unityAdController = null;

        private List<int> listShowAdCount = new List<int>();
        private RewardedAdType readyAdType = RewardedAdType.UNITY;

        private bool isCalledback = false;
        private bool isRewarded = false;
        private void OnEnable()
        {
            IngameManager.IngameStateChanged += IngameManager_IngameStateChanged;
        }

        private void OnDisable()
        {
            IngameManager.IngameStateChanged -= IngameManager_IngameStateChanged;
        }
       

        private void Start()
        {
            foreach (InterstitialAdConfig o in listShowInterstitialAdConfig)
            {
                listShowAdCount.Add(o.GameStateCountForShowingAd);
            }

            //Show banner ad
            if (bannerAdType == BannerAdType.ADMOB)
            {
                admobController.ShowBannerAd(showingBannerAdDelay);
            }
            if (bannerAdType == BannerAdType.UNITY)
            {
                unityAdController.ShowBannerAd(showingBannerAdDelay);
            }
        }

        private void Update()
        {
            if (isCalledback)
            {
                isCalledback = false;
                if (isRewarded)
                {
                    if (ViewManager.Instance.ActiveViewType == ViewType.HOME_VIEW) //In Home scene
                    {
                        //Reward coins
                        int coins = Random.Range(minRewardedCoins, maxRewardedCoins) / 5 * 5;
                        FindObjectOfType<HomeManager>().CreateRewardedCoins(coins);
                    }
                    else
                    {
                        if (IngameManager.Instance.IngameState == IngameState.Revive)
                        {
                            IngameManager.Instance.SetContinueGame(); //Revive 
                        }
                        else
                        {
                            //Reward collected coins
                            int collectedCoins = ServicesManager.Instance.CoinManager.CollectedCoins;
                            ServicesManager.Instance.RewardCoinManager.RewardCollectedCoins(collectedCoins, rewardDelay);
                        }
                    }
                }
                else
                {
                    if (ViewManager.Instance.ActiveViewType == ViewType.INGAME_VIEW)
                    {
                        if (IngameManager.Instance.IngameState == IngameState.Revive)
                            IngameManager.Instance.GameOver();
                    }
                }
            }
        }


        private void IngameManager_IngameStateChanged(IngameState obj)
        {
            for (int i = 0; i < listShowAdCount.Count; i++)
            {
                if (listShowInterstitialAdConfig[i].GameStateForShowingAd == obj)
                {
                    listShowAdCount[i]--;
                    if (listShowAdCount[i] <= 0)
                    {
                        //Reset gameCount 
                        listShowAdCount[i] = listShowInterstitialAdConfig[i].GameStateCountForShowingAd;

                        for (int a = 0; a < listShowInterstitialAdConfig[i].ListInterstitialAdType.Count; a++)
                        {
                            InterstitialAdType type = listShowInterstitialAdConfig[i].ListInterstitialAdType[a];
                            if (type == InterstitialAdType.ADMOB && admobController.IsInterstitialAdReady())
                            {
                                admobController.ShowInterstitialAd(listShowInterstitialAdConfig[i].ShowAdDelay);
                                break;
                            }
                            else if (type == InterstitialAdType.UNITY && unityAdController.IsInterstitialAdReady())
                            {
                                unityAdController.ShowInterstitialAd(listShowInterstitialAdConfig[i].ShowAdDelay);
                                break;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Determines whether rewarded ad is ready.
        /// </summary>
        /// <returns></returns>
        public bool IsRewardedAdReady()
        {
            for(int i = 0; i < listRewardedAdType.Count; i++)
            {
                if (listRewardedAdType[i] == RewardedAdType.UNITY && unityAdController.IsRewardedAdReady())
                {
                    readyAdType = RewardedAdType.UNITY;
                    return true;
                }
                else if(listRewardedAdType[i] == RewardedAdType.ADMOB && admobController.IsRewardedAdReady())
                {
                    readyAdType = RewardedAdType.ADMOB;
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Show the rewarded video ad with delay time
        /// </summary>
        /// <param name="delay"></param>
        public void ShowRewardedAd()
        {
            if (readyAdType == RewardedAdType.UNITY)
            {
                unityAdController.ShowRewardedAd(showingRewardedVideoAdDelay);
            }
            else if (readyAdType == RewardedAdType.ADMOB)
            {
                admobController.ShowRewardedAd(showingRewardedVideoAdDelay);
            }
        }

        public void OnRewardedAdClosed(bool isFinishedVideo)
        {
            isCalledback = true;
            isRewarded = isFinishedVideo;
        }
    }
}
