using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace OnefallGames
{
    #region Ingame enums
    public enum IngameState
    {
        Prepare,
        Playing,
        Revive,
        GameOver,
        CompletedLevel,
    }
    public enum LevelType
    {
        NORMAL_LEVEL,
        BOSS_LEVEL,
    }

    public enum ItemType
    {
        COIN,
    }


    public enum DayItem
    {
        DAY_1,
        DAY_2,
        DAY_3,
        DAY_4,
        DAY_5,
        DAY_6,
        DAY_7,
        DAY_8,
        DAY_9,
    }

    #endregion


    #region Ads enums
    public enum BannerAdType
    {
        NONE,
        UNITY,
        ADMOB,
    }

    public enum InterstitialAdType
    {
        UNITY,
        ADMOB,
    }
    public enum RewardedAdType
    {
        UNITY,
        ADMOB,
    }
    #endregion


    #region View enums
    public enum ViewType
    {
        HOME_VIEW = 1,
        INGAME_VIEW = 2,
        LOADING_VIEW = 3,
        CHARACTER_VIEW = 4,
    }
    #endregion


    #region Classes
    [System.Serializable]
    public class NormalLevelConfiguration
    {
        [Header("Level Number Configuration")]
        [SerializeField] private int minLevel = 1;
        public int MinLevel { get { return minLevel; } }
        [SerializeField] private int maxLevel = 5;
        public int MaxLevel { get { return maxLevel; } }

        [Header("Background Configuration")]
        [SerializeField] private Sprite backgroundSprite = null;
        public Sprite BackgroundSprite { get { return backgroundSprite; } }


        [Header("Knife Amount Configuration")]
        [SerializeField] [Range(1, 15)] private int minKnifeAmount = 1;
        public int MinKnifeAmount { get { return minKnifeAmount; } }
        [SerializeField] [Range(1, 15)] private int maxKnifeAmount = 1;
        public int MaxKnifeAmount { get { return maxKnifeAmount; } }


        [Header("Target Configuration")]
        [SerializeField] [Range(20, 500)] private int minTargetRotatingSpeed = 20;
        public int MinTargetRotatingSpeed { get { return minTargetRotatingSpeed; } }
        [SerializeField] [Range(20, 500)] private int maxTargetRotatingSpeed = 20;
        public int MaxTargetRotatingSpeed { get { return maxTargetRotatingSpeed; } }

        [SerializeField] [Range(0, 360)] private int minTargetRotatingAmount = 50;
        public int MinTargetRotatingAmount { get { return minTargetRotatingAmount; } }
        [SerializeField] [Range(0, 360)] private int maxTargetRotatingAmount = 300;
        public int MaxTargetRotatingAmount { get { return maxTargetRotatingAmount; } }


        [SerializeField] private LerpType[] lerpTypes = null;
        public LerpType[] LerpTypes { get { return lerpTypes; } }



        [Header("Coin Configuration")]
        [SerializeField] [Range(0f, 1f)] private float coinFrequency = 0.1f; 
        public float CoinFrequency { get { return coinFrequency; } }
        [SerializeField] [Range(0, 16)] private int minCoinAmount = 1;
        public int MinCoinAmount { get { return minCoinAmount; } }
        [SerializeField] [Range(2, 16)] private int maxCoinAmount = 1;
        public int MaxCoinAmount { get { return maxCoinAmount; } }


        [Header("Static Knife Configuration")]
        [SerializeField] [Range(0f, 1f)] private float staticKnifeFrequency = 0.1f;
        public float StaticKnifeFrequency { get { return staticKnifeFrequency; } }
        [SerializeField] [Range(0, 16)] private int minStaticKnifeAmount = 1;
         public int MinStaticKnifeAmount { get { return minStaticKnifeAmount; } }
        [SerializeField] [Range(2, 16)] private int maxStaticKnifeAmount = 1;
        public int MaxStaticKnifeAmount { get { return maxStaticKnifeAmount; } }

        [Header("Obstacle Configuration")]
        [SerializeField] [Range(0f, 1f)] private float obstacleFrequency = 0.1f;
        public float ObstacleFrequency { get { return obstacleFrequency; } }
        [SerializeField] [Range(0, 10)] private int minObstacleAmount = 1;
        public int MinObstacleAmount { get { return minObstacleAmount; } }
        [SerializeField] [Range(2, 10)] private int maxObstacleAmount = 1;
        public int MaxObstacleAmount { get { return maxObstacleAmount; } }

    }


    [System.Serializable]
    public class BossLevelConfiguration
    {
        [Header("Boss Sprite Configuration")]
        [SerializeField] private Sprite bossSprite = null;
        public Sprite BossSprite { get { return bossSprite; } }

        [Header("Boss Name Configuration")]
        [SerializeField] private string bossName = string.Empty;
        public string BossName { get { return bossName; } }

        [Header("Target Normal Level Configuration")]
        [SerializeField] private int targetNormalLevel = 10;
        public int TargetNormalLevel { get { return targetNormalLevel; } }

        [Header("Knife Amount Configuration")]
        [SerializeField] [Range(0, 16)] private int knifeAmount = 0;
        public int KnifeAmount { get { return knifeAmount; } }

        [Header("Boss Rotating Speed Configuration")]
        [SerializeField] [Range(50, 400)] private int minRotatingSpeed = 50;
        public int MinRotatingSpeed { get { return minRotatingSpeed; } }
        [SerializeField] [Range(50, 400)] private int maxRotatingSpeed = 100;
        public int MaxRotatingSpeed { get { return maxRotatingSpeed; } }

        [Header("Boss Rotating Amount Configuration")]
        [SerializeField] [Range(0, 360)] private int minRotatingAmount = 50;
        public int MinRotatingAmount { get { return minRotatingAmount; } }
        [FormerlySerializedAs("maxRotatingAngle")]
        [SerializeField] [Range(0, 360)] private int maxRotatingAmount = 300;
        public int MaxRotatingAmount { get { return maxRotatingAmount; } }

        [Header("Lerp Types Configuration")]
        [SerializeField] private LerpType[] lerpTypes = null;
        public LerpType[] LerpTypes { get { return lerpTypes; } }

        [Header("Other Parameters Configuration")]
        [SerializeField] [Range(0, 17)] private int coinAmount = 0;
        public int CoinAmount { get { return coinAmount; } }
        [SerializeField] [Range(0, 10)] private int staticKnifeAmount = 0;
        public int StaticKnifeAmount { get { return staticKnifeAmount; } }
        [SerializeField] [Range(0, 10)] private int obstacleAmount = 0;
        public int ObstacleAmount { get { return obstacleAmount; } }
    }



    [System.Serializable]
    public class SoundClip
    {
        [SerializeField] private AudioClip audioClip = null;
        public AudioClip AudioClip { get { return audioClip; } }
    }

    [System.Serializable]
    public class InterstitialAdConfig
    {
        public IngameState GameStateForShowingAd = IngameState.Prepare;
        public int GameStateCountForShowingAd = 3;
        public float ShowAdDelay = 0.2f;
        public List<InterstitialAdType> ListInterstitialAdType = new List<InterstitialAdType>();
    }



    [System.Serializable]
    public class TargetObjectData
    {
        public int CoinNumber { private set; get; }
        public void SetCoinNumber(int coinNumber)
        {
            CoinNumber = coinNumber;
        }


        public int StaticKnifeNumber { private set; get; }
        public void SetStaticKnifeNumber(int staticKnifeNumber)
        {
            StaticKnifeNumber = staticKnifeNumber;
        }



        public int ObstacleNumber { private set; get; }
        public void SetObstacleNumber(int obstacleNumber)
        {
            ObstacleNumber = obstacleNumber;
        }
    }



    [System.Serializable]
    public class TargetParameterData
    {
        public int MinRotatingSpeed { private set; get; }
        public void SetMinRotatingSpeed(int minRotatingSpeed)
        {
            MinRotatingSpeed = minRotatingSpeed;
        }


        public int MaxRotatingSpeed { private set; get; }
        public void SetMaxRotatingSpeed(int maxRotatingSpeed)
        {
            MaxRotatingSpeed = maxRotatingSpeed;
        }


        public int MinRotatingAngle { private set; get; }
        public void SetMinRotatingAngle(int minRotatingAngle)
        {
            MinRotatingAngle = minRotatingAngle;
        }


        public int MaxRotatingAngle { private set; get; }
        public void SetMaxRotatingAngle(int maxRotatingAngle)
        {
            MaxRotatingAngle = maxRotatingAngle;
        }


        public LerpType[] LerpTypes { private set; get; }
        public void SetLerpTypes(LerpType[] lerpTypes)
        {
            LerpTypes = lerpTypes;
        }
    }


    public class PlayerLeaderboardData
    {
        public string Name { private set; get; }
        public void SetName(string name)
        {
            Name = name;
        }

        public int Level { private set; get; }
        public void SetLevel(int level)
        {
            Level = level;
        }
    }
    #endregion
}
