using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OnefallGames
{
    public class IngameManager : MonoBehaviour
    {

        public static IngameManager Instance { private set; get; }
        public static event System.Action<IngameState> IngameStateChanged = delegate { };

        public IngameState IngameState
        {
            get
            {
                return ingameState;
            }
            private set
            {
                if (value != ingameState)
                {
                    ingameState = value;
                    IngameStateChanged(ingameState);
                }
            }
        }


        [Header("Enter a number of level to test. Set back to 0 to disable this feature.")]
        [SerializeField] private int testingNormalLevel = 0;
        [SerializeField] private int testingBossLevel = 0;


        [Header("Ingame Config")]
        [SerializeField] private float reviveCountDownTime = 5f;
        [SerializeField] private float knifeShootingUpSpeed = 100f;
        [Header("Boss Levels Configuration")]
        [SerializeField] private List<BossLevelConfiguration> listBossLevelConfig = new List<BossLevelConfiguration>();
        [Header("Normal Levels Configuration")]
        [SerializeField] private List<NormalLevelConfiguration> listNormalLevelConfig = new List<NormalLevelConfiguration>();

        [Header("Ingame References")]
        [SerializeField] private TargetController targetController = null;
        [SerializeField] private BossController bossController = null;
        [SerializeField] private SpriteRenderer backgroundSpriteRenderer = null;

        public LevelType LevelType { private set; get; }
        public float ReviveCountDownTime { get { return reviveCountDownTime; } }
        public int CurrentLevel { private set; get; }
        public int KnifeNumber { private set; get; }
        public bool IsRevived { private set; get; }

        private IngameState ingameState = IngameState.GameOver;
        private NormalLevelConfiguration normalLevelConfig = null;
        private List<DynamicKnifeController> listDynamicKnifeController = new List<DynamicKnifeController>();
        private TargetParameterData targetParameterData = null;
        private DynamicKnifeController currentShootableKnife = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(Instance.gameObject);
                Instance = this;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }


        private void Start()
        {
            Application.targetFrameRate = 60;
            ViewManager.Instance.OnShowView(ViewType.INGAME_VIEW);
            IngameState = IngameState.Prepare;
            ingameState = IngameState.Prepare;

            //Setup variables
            IsRevived = false;

            //Init the CurrentLevel
            if (!PlayerPrefs.HasKey(PlayerPrefsKeys.PPK_SAVED_NORMAL_LEVEL))
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_SAVED_NORMAL_LEVEL, 1);
            }
            CurrentLevel = (testingNormalLevel != 0) ? testingNormalLevel : PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_NORMAL_LEVEL);


            //Init the SAVED_BOSS_LEVEL_PPK key
            if (!PlayerPrefs.HasKey(PlayerPrefsKeys.PPK_SAVED_BOSS_TARGET_NORMAL_LEVEL))
            {
                PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_SAVED_BOSS_TARGET_NORMAL_LEVEL, listBossLevelConfig[0].TargetNormalLevel);
            }


            if (testingBossLevel > 0 || (CurrentLevel - 1) == PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_BOSS_TARGET_NORMAL_LEVEL))
            {
                LevelType = LevelType.BOSS_LEVEL;
                targetController.gameObject.SetActive(false);
                bossController.gameObject.SetActive(true);

                //Create boss state
                foreach (BossLevelConfiguration o in listBossLevelConfig)
                {
                    if (testingBossLevel == o.TargetNormalLevel || (CurrentLevel - 1) == o.TargetNormalLevel)
                    {
                        targetParameterData = new TargetParameterData();
                        targetParameterData.SetMinRotatingSpeed(o.MinRotatingSpeed);
                        targetParameterData.SetMaxRotatingSpeed(o.MaxRotatingSpeed);
                        targetParameterData.SetMinRotatingAngle(o.MaxRotatingAmount);
                        targetParameterData.SetMinRotatingAngle(o.MaxRotatingAmount);
                        targetParameterData.SetLerpTypes(o.LerpTypes);
                        CreateBossLevel(o);
                        break;
                    }
                }
            }
            else
            {
                LevelType = LevelType.NORMAL_LEVEL;
                targetController.gameObject.SetActive(true);
                bossController.gameObject.SetActive(false);

                foreach (NormalLevelConfiguration o in listNormalLevelConfig)
                {
                    if (CurrentLevel >= o.MinLevel && CurrentLevel < o.MaxLevel)
                    {
                        normalLevelConfig = o;
                        targetParameterData = new TargetParameterData();
                        targetParameterData.SetMinRotatingSpeed(o.MinTargetRotatingSpeed);
                        targetParameterData.SetMaxRotatingSpeed(o.MaxTargetRotatingSpeed);
                        targetParameterData.SetMinRotatingAngle(o.MinTargetRotatingAmount);
                        targetParameterData.SetMinRotatingAngle(o.MaxTargetRotatingAmount);
                        targetParameterData.SetLerpTypes(o.LerpTypes);

                        CreateNormalLevel(o);
                        backgroundSpriteRenderer.sprite = o.BackgroundSprite;
                        break;
                    }
                }
            }
        }



        private void Update()
        {
            if (ingameState == IngameState.Playing)
            {
                if (currentShootableKnife != null)
                {
                    if (Input.GetMouseButtonDown(0) && currentShootableKnife.IsReadyToShoot)
                    {
                        currentShootableKnife.ShootUp(knifeShootingUpSpeed);
                        currentShootableKnife = null;
                        ViewManager.Instance.IngameViewController.PlayingViewControl.DisableKnife();
                    }
                }
            }
        }




        /// <summary>
        /// Actual start the game (call Ingame_Playing event).
        /// </summary>
        public void PlayingGame()
        {
            //Fire event
            IngameState = IngameState.Playing;
            ingameState = IngameState.Playing;

            //Add another actions here
            if (IsRevived)
            {
                ResumeBackgroundMusic(0.5f);

                if (LevelType == LevelType.NORMAL_LEVEL)
                {
                    targetController.StartRotating(targetParameterData);
                }
                else
                {
                    bossController.StartRotating(targetParameterData);
                }
                ViewManager.Instance.IngameViewController.PlayingViewControl.EnableKnife();
                MoveNextKnifeToReadyPosition();
            }
            else
            {
                PlayBackgroundMusic(0.5f);
                if (LevelType == LevelType.NORMAL_LEVEL)
                {
                    targetController.StartRotating(targetParameterData);
                }
                else
                {
                    bossController.StartRotating(targetParameterData);
                }
                MoveNextKnifeToReadyPosition();
            }
        }


        /// <summary>
        /// Call Ingame_Revive event.
        /// </summary>
        public void Revive()
        {
            //Fire event
            IngameState = IngameState.Revive;
            ingameState = IngameState.Revive;

            //Add another actions here
            PauseBackgroundMusic(0.5f);
        }


        /// <summary>
        /// Call Ingame_GameOver event.
        /// </summary>
        public void GameOver()
        {
            //Fire event
            IngameState = IngameState.GameOver;
            ingameState = IngameState.GameOver;

            //Add another actions here
            StopBackgroundMusic(0f);
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.gameOver);
        }


        /// <summary>
        /// Call Ingame_CompletedLevel event.
        /// </summary>
        public void CompletedLevel()
        {
            //Fire event
            IngameState = IngameState.CompletedLevel;
            ingameState = IngameState.CompletedLevel;

            //Add another actions here
            StopBackgroundMusic(0f);
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.levelCompleted);

            if (LevelType == LevelType.NORMAL_LEVEL)
            {
                //Save the next normal level and play effect
                if (testingNormalLevel == 0)
                {
                    PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_SAVED_NORMAL_LEVEL, CurrentLevel + 1);
                }
                targetController.PlayTargetBrokenEffect();
            }
            else
            {
                //Save the next boss level and play effect
                int lastSavedTargetLevel = PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_BOSS_TARGET_NORMAL_LEVEL);
                for(int i = 0; i < listBossLevelConfig.Count; i++)
                {
                    BossLevelConfiguration bossLevelConfig = listBossLevelConfig[i];
                    if (bossLevelConfig.TargetNormalLevel > lastSavedTargetLevel)
                    {
                        PlayerPrefs.SetInt(PlayerPrefsKeys.PPK_SAVED_BOSS_TARGET_NORMAL_LEVEL, bossLevelConfig.TargetNormalLevel);
                        break;
                    }
                }
                bossController.PlayBrokenEffect();
            }
        }

        private void PlayBackgroundMusic(float delay)
        {
            StartCoroutine(CRPlayBGMusic(delay));
        }

        private IEnumerator CRPlayBGMusic(float delay)
        {
            yield return new WaitForSeconds(delay);
            ServicesManager.Instance.SoundManager.PlayMusic(ServicesManager.Instance.SoundManager.background, 0.5f);
        }

        private void StopBackgroundMusic(float delay)
        {
            StartCoroutine(CRStopBGMusic(delay));
        }

        private IEnumerator CRStopBGMusic(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (ServicesManager.Instance.SoundManager.background != null)
                ServicesManager.Instance.SoundManager.StopMusic(0.5f);
        }

        private void PauseBackgroundMusic(float delay)
        {
            StartCoroutine(CRPauseBGMusic(delay));
        }

        private IEnumerator CRPauseBGMusic(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (ServicesManager.Instance.SoundManager.background != null)
                ServicesManager.Instance.SoundManager.PauseMusic();
        }

        private void ResumeBackgroundMusic(float delay)
        {
            StartCoroutine(CRResumeBGMusic(delay));
        }

        private IEnumerator CRResumeBGMusic(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (ServicesManager.Instance.SoundManager.background != null)
                ServicesManager.Instance.SoundManager.ResumeMusic();
        }


        /// <summary>
        /// Create normal level with given NormalLevelConfig.
        /// </summary>
        /// <param name="o"></param>
        private void CreateNormalLevel(NormalLevelConfiguration o)
        {
            //Create dynamic knives
            int knifeCount = Random.Range(o.MinKnifeAmount, o.MaxKnifeAmount);
            KnifeNumber = knifeCount;
            for (int i = 0; i < knifeCount; i++)
            {
                DynamicKnifeController dynamicKnifeController = PoolManager.Instance.GetDynamicKnifeController();
                dynamicKnifeController.transform.position = (Vector2)Camera.main.ViewportToWorldPoint(new Vector2(0.5f, -0.1f));
                listDynamicKnifeController.Add(dynamicKnifeController);
                dynamicKnifeController.gameObject.SetActive(true);
            }

            TargetObjectData targetObjectData = new TargetObjectData();
            targetObjectData.SetCoinNumber((Random.value <= o.CoinFrequency) ? Random.Range(normalLevelConfig.MinCoinAmount, normalLevelConfig.MaxCoinAmount) : 0);
            targetObjectData.SetStaticKnifeNumber((Random.value <= o.StaticKnifeFrequency) ? Random.Range(normalLevelConfig.MinStaticKnifeAmount, normalLevelConfig.MaxStaticKnifeAmount) : 0);
            targetObjectData.SetObstacleNumber((Random.value <= o.ObstacleFrequency) ? Random.Range(normalLevelConfig.MinObstacleAmount, normalLevelConfig.MaxObstacleAmount) : 0);
            targetController.CreateObjects(targetObjectData);
        }



        /// <summary>
        /// Create boss level with given BossLevelConfig.
        /// </summary>
        /// <param name="o"></param>
        private void CreateBossLevel(BossLevelConfiguration o)
        {
            bossController.SetSprite(o.BossSprite);
            ViewManager.Instance.IngameViewController.PlayingViewControl.SetBossName(o.BossName);

            //Create dynamic knives
            KnifeNumber = o.KnifeAmount;
            for (int i = 0; i < o.KnifeAmount; i++)
            {
                DynamicKnifeController dynamicKnifeController = PoolManager.Instance.GetDynamicKnifeController();
                dynamicKnifeController.transform.position = (Vector2)Camera.main.ViewportToWorldPoint(new Vector2(0.5f, -0.1f));
                listDynamicKnifeController.Add(dynamicKnifeController);
                dynamicKnifeController.gameObject.SetActive(true);
            }

            TargetObjectData targetObjectData = new TargetObjectData();
            targetObjectData.SetCoinNumber(o.CoinAmount);
            targetObjectData.SetStaticKnifeNumber(o.StaticKnifeAmount);
            targetObjectData.SetObstacleNumber(o.ObstacleAmount);
            bossController.CreateObjects(targetObjectData);
        }


        //////////////////////////////////////////////////Publish functions

        /// <summary>
        /// Continue the game
        /// </summary>
        public void SetContinueGame()
        {
            IsRevived = true;

            //Recreate the fall down knife
            DynamicKnifeController dynamicKnifeController = PoolManager.Instance.GetDynamicKnifeController();
            dynamicKnifeController.transform.position = (Vector2)Camera.main.ViewportToWorldPoint(new Vector2(0.5f, -0.1f));
            listDynamicKnifeController.Insert(0, dynamicKnifeController);
            dynamicKnifeController.gameObject.SetActive(true);

            PlayingGame();
        }


        /// <summary>
        /// Handle action when the dynamic knie hit other knife or an obstacle.
        /// </summary>
        public void HandleGameOver()
        {
            if (IsRevived || !ServicesManager.Instance.AdManager.IsRewardedAdReady())
            {
                GameOver();
            }
            else
            {
                Revive();
            }
        }


        /// <summary>
        /// Move the next dynamic knife to ready position.
        /// </summary>
        public void MoveNextKnifeToReadyPosition()
        {
            listDynamicKnifeController[0].gameObject.SetActive(true);
            listDynamicKnifeController[0].MoveToReadyPosition();
            currentShootableKnife = listDynamicKnifeController[0];
            listDynamicKnifeController.RemoveAt(0);
        }


        /// <summary>
        /// Is out of dynamic knives.
        /// </summary>
        /// <returns></returns>
        public bool IsOutOfKnives()
        {
            return listDynamicKnifeController.Count == 0;
        }
    }
}
