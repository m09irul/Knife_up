using UnityEngine;

namespace OnefallGames
{
    public class ServicesManager : MonoBehaviour
    {
        public static ServicesManager Instance { private set; get; }

        [SerializeField] private CoinManager coinManager = null;
        [SerializeField] private SoundManager soundManager = null;
        [SerializeField] private ShareManager shareManager = null;
        [SerializeField] private AdManager adManager = null;
        [SerializeField] private DailyRewardManager dailyRewardManager = null;
        [SerializeField] private RewardCoinManager rewardCoinManager = null;
        [SerializeField] private CharacterContainer characterContainer = null;
        [SerializeField] private LeaderboardManager leaderboardManager = null;

        public CoinManager CoinManager { get { return coinManager; } }
        public SoundManager SoundManager { get { return soundManager; } }
        public ShareManager ShareManager { get { return shareManager; } }
        public AdManager AdManager { get { return adManager; } }
        public DailyRewardManager DailyRewardManager { get { return dailyRewardManager; } }
        public RewardCoinManager RewardCoinManager { get { return rewardCoinManager; } }
        public CharacterContainer CharacterContainer { get { return characterContainer; } }
        public LeaderboardManager LeaderboardManager { get { return leaderboardManager; } }

        private void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}

