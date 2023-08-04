using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace OnefallGames
{
    public class HomeViewController : MonoBehaviour
    {

        [SerializeField] private RectTransform topBarViewTrans = null;
        [SerializeField] private RectTransform gameNameTrans = null;
        [SerializeField] private RectTransform playBtnTrans = null;
        [SerializeField] private RectTransform rewardBtnTrans = null;
        [SerializeField] private RectTransform characterBtnTrans = null;
        [SerializeField] private RectTransform settingBtnTrans = null;
        [SerializeField] private RectTransform soundBtnsTrans = null;
        [SerializeField] private RectTransform musicBtnsTrans = null;
        [SerializeField] private RectTransform rateAppBtnTrans = null;
        [SerializeField] private RectTransform removeAdsBtnTrans = null;
        [SerializeField] private RectTransform nativeShareBtnTrans = null;
        [SerializeField] private RectTransform leaderboardBtnTrans = null;
        [SerializeField] private GameObject soundOnBtn = null;
        [SerializeField] private GameObject soundOffBtn = null;
        [SerializeField] private GameObject musicOnBtn = null;
        [SerializeField] private GameObject musicOffBtn = null;
        [SerializeField] private GameObject warning = null;
        [SerializeField] private Text totalCoinsTxt = null;
        [SerializeField] private Text currentLevelTxt = null;
        [SerializeField] private Image fadeOutPanel = null;
        [SerializeField] private DailyRewardViewController dailyRewardViewControl = null;
        [SerializeField] private LeaderboardViewController leaderboardViewController = null;


        public DailyRewardViewController DailyRewardViewController { get { return dailyRewardViewControl; } }


        private int settingButtonTurn = 1;
        public void OnShow()
        {
            StartCoroutine(CRFadingoutPanel());
            leaderboardViewController.gameObject.SetActive(false);

            //Update texts
            currentLevelTxt.text = "Level: " + PlayerPrefs.GetInt(PlayerPrefsKeys.PPK_SAVED_NORMAL_LEVEL, 1).ToString();

            //Update sound btns
            if (ServicesManager.Instance.SoundManager.IsSoundOff())
            {
                soundOnBtn.gameObject.SetActive(false);
                soundOffBtn.gameObject.SetActive(true);
            }
            else
            {
                soundOnBtn.gameObject.SetActive(true);
                soundOffBtn.gameObject.SetActive(false);
            }

            //Update music btns
            if (ServicesManager.Instance.SoundManager.IsMusicOff())
            {
                musicOffBtn.gameObject.SetActive(true);
                musicOnBtn.gameObject.SetActive(false);
            }
            else
            {
                musicOffBtn.gameObject.SetActive(false);
                musicOnBtn.gameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            topBarViewTrans.anchoredPosition = new Vector2(topBarViewTrans.anchoredPosition.x, 250);
            gameNameTrans.localScale = Vector3.zero;
            playBtnTrans.localScale = Vector3.zero;
            rewardBtnTrans.anchoredPosition = new Vector2(rewardBtnTrans.anchoredPosition.x, -200f);
            characterBtnTrans.anchoredPosition = new Vector2(characterBtnTrans.anchoredPosition.x, -200f);
            settingBtnTrans.anchoredPosition = new Vector2(settingBtnTrans.anchoredPosition.x, -200f);
        }

        private void Update()
        {
            totalCoinsTxt.text = ServicesManager.Instance.CoinManager.TotalCoins.ToString();
        }


        public void OnSubViewClose()
        {
            ViewManager.Instance.ScaleRect(gameNameTrans, Vector2.zero, Vector2.one, 0.5f);
            StartCoroutine(CRShowingBottomButtons());
            CheckAndEnableWarning();
        }

        private void CheckAndEnableWarning()
        {
            if (ServicesManager.Instance.AdManager.IsRewardedAdReady())
            {
                warning.SetActive(true);
            }
            else
            {
                if (dailyRewardViewControl.gameObject.activeInHierarchy)
                {
                    if (dailyRewardViewControl.IsRewardAvailable)
                    {
                        warning.SetActive(true);
                    }
                    else
                    {
                        warning.SetActive(false);
                    }
                }
                else
                {
                    warning.SetActive(false);
                }
            }
        }


        private IEnumerator CRShowingBottomButtons()
        {
            ViewManager.Instance.ScaleRect(playBtnTrans, Vector2.zero, Vector2.one, 0.5f);
            yield return new WaitForSeconds(0.15f);
            ViewManager.Instance.MoveRect(rewardBtnTrans, rewardBtnTrans.anchoredPosition, new Vector2(rewardBtnTrans.anchoredPosition.x, 150), 0.5f);
            yield return new WaitForSeconds(0.15f);
            ViewManager.Instance.MoveRect(characterBtnTrans, characterBtnTrans.anchoredPosition, new Vector2(characterBtnTrans.anchoredPosition.x, 150), 0.5f);
            yield return new WaitForSeconds(0.15f);
            ViewManager.Instance.MoveRect(settingBtnTrans, settingBtnTrans.anchoredPosition, new Vector2(settingBtnTrans.anchoredPosition.x, 150), 0.5f);
        }


        private IEnumerator CRFadingoutPanel()
        {
            fadeOutPanel.gameObject.SetActive(true);
            float fadingTime = 0.5f;
            float t = 0;
            Color startColor = fadeOutPanel.color;
            startColor.a = 1;
            fadeOutPanel.color = startColor;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
            while (t < fadingTime)
            {
                t += Time.deltaTime;
                float factor = t / fadingTime;
                fadeOutPanel.color = Color.Lerp(startColor, endColor, factor);
                yield return null;
            }
            fadeOutPanel.gameObject.SetActive(false);

            //Move topBar from current position to the top of the screen
            ViewManager.Instance.MoveRect(topBarViewTrans, topBarViewTrans.anchoredPosition, new Vector2(topBarViewTrans.anchoredPosition.x, 0), 0.5f);
            ViewManager.Instance.ScaleRect(gameNameTrans, Vector2.zero, Vector2.one, 0.5f);
            ViewManager.Instance.ScaleRect(playBtnTrans, Vector2.zero, Vector2.one, 0.5f);

            StartCoroutine(CRShowingBottomButtons());
            Invoke("CheckAndEnableWarning", 0.5f);
        }


        //////////////////////////////////////////////////////////////////////UI Functions


        public void PlayBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            StartCoroutine(CRHandlingPlayBtn());
        }
        private IEnumerator CRHandlingPlayBtn()
        {
            fadeOutPanel.gameObject.SetActive(true);
            float fadingTime = 0.5f;
            float t = 0;
            Color startColor = fadeOutPanel.color;
            startColor.a = 0;
            fadeOutPanel.color = startColor;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1);
            while (t < fadingTime)
            {
                t += Time.deltaTime;
                float factor = t / fadingTime;
                fadeOutPanel.color = Color.Lerp(startColor, endColor, factor);
                yield return null;
            }
            ViewManager.Instance.LoadScene("Ingame", 0f);
        }

        public void NativeShareBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ServicesManager.Instance.ShareManager.NativeShare();
        }
        public void CharacterBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ViewManager.Instance.LoadScene("Character", 0.25f);
        }

        public void RewardBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            StartCoroutine(CRHandleDailyrewardBtn());
        }

        private IEnumerator CRHandleDailyrewardBtn()
        {
            ViewManager.Instance.ScaleRect(gameNameTrans, Vector2.one, Vector2.zero, 0.5f);
            ViewManager.Instance.ScaleRect(playBtnTrans, playBtnTrans.localScale, Vector2.zero, 0.5f);
            ViewManager.Instance.MoveRect(rewardBtnTrans, rewardBtnTrans.anchoredPosition, new Vector2(rewardBtnTrans.anchoredPosition.x, -200), 0.5f);
            yield return new WaitForSeconds(0.15f);
            ViewManager.Instance.MoveRect(characterBtnTrans, characterBtnTrans.anchoredPosition, new Vector2(characterBtnTrans.anchoredPosition.x, -200), 0.5f);
            yield return new WaitForSeconds(0.15f);
            ViewManager.Instance.MoveRect(settingBtnTrans, settingBtnTrans.anchoredPosition, new Vector2(settingBtnTrans.anchoredPosition.x, -200), 0.5f);
            yield return new WaitForSeconds(0.5f);
            dailyRewardViewControl.gameObject.SetActive(true);
            dailyRewardViewControl.OnShow();
            warning.SetActive(false);
        }

        public void SettingBtn()
        {
            settingButtonTurn *= -1;
            StartCoroutine(CRHandleSettingBtn());
        }
        private IEnumerator CRHandleSettingBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            if (settingButtonTurn == -1)
            {
                ViewManager.Instance.MoveRect(soundBtnsTrans, soundBtnsTrans.anchoredPosition, new Vector2(0, soundBtnsTrans.anchoredPosition.y), 0.5f);
                ViewManager.Instance.MoveRect(removeAdsBtnTrans, removeAdsBtnTrans.anchoredPosition, new Vector2(0, removeAdsBtnTrans.anchoredPosition.y), 0.5f);

                yield return new WaitForSeconds(0.08f);

                ViewManager.Instance.MoveRect(musicBtnsTrans, musicBtnsTrans.anchoredPosition, new Vector2(0, musicBtnsTrans.anchoredPosition.y), 0.5f);
                ViewManager.Instance.MoveRect(nativeShareBtnTrans, nativeShareBtnTrans.anchoredPosition, new Vector2(0, nativeShareBtnTrans.anchoredPosition.y), 0.5f);

                yield return new WaitForSeconds(0.08f);

                ViewManager.Instance.MoveRect(rateAppBtnTrans, rateAppBtnTrans.anchoredPosition, new Vector2(0, rateAppBtnTrans.anchoredPosition.y), 0.5f);
                ViewManager.Instance.MoveRect(leaderboardBtnTrans, leaderboardBtnTrans.anchoredPosition, new Vector2(0, leaderboardBtnTrans.anchoredPosition.y), 0.5f);
            }
            else
            {
                ViewManager.Instance.MoveRect(soundBtnsTrans, soundBtnsTrans.anchoredPosition, new Vector2(-150, soundBtnsTrans.anchoredPosition.y), 0.5f);
                ViewManager.Instance.MoveRect(removeAdsBtnTrans, removeAdsBtnTrans.anchoredPosition, new Vector2(150, removeAdsBtnTrans.anchoredPosition.y), 0.5f);

                yield return new WaitForSeconds(0.08f);

                ViewManager.Instance.MoveRect(musicBtnsTrans, musicBtnsTrans.anchoredPosition, new Vector2(-150, musicBtnsTrans.anchoredPosition.y), 0.5f);
                ViewManager.Instance.MoveRect(nativeShareBtnTrans, nativeShareBtnTrans.anchoredPosition, new Vector2(150, nativeShareBtnTrans.anchoredPosition.y), 0.5f);

                yield return new WaitForSeconds(0.08f);

                ViewManager.Instance.MoveRect(rateAppBtnTrans, rateAppBtnTrans.anchoredPosition, new Vector2(-150, rateAppBtnTrans.anchoredPosition.y), 0.5f);
                ViewManager.Instance.MoveRect(leaderboardBtnTrans, leaderboardBtnTrans.anchoredPosition, new Vector2(150, leaderboardBtnTrans.anchoredPosition.y), 0.5f);
            }
        }


        public void ToggleSound()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ServicesManager.Instance.SoundManager.ToggleSound();
            if (ServicesManager.Instance.SoundManager.IsSoundOff())
            {
                soundOnBtn.gameObject.SetActive(false);
                soundOffBtn.gameObject.SetActive(true);
            }
            else
            {
                soundOnBtn.gameObject.SetActive(true);
                soundOffBtn.gameObject.SetActive(false);
            }
        }

        public void ToggleMusic()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ServicesManager.Instance.SoundManager.ToggleMusic();
            if (ServicesManager.Instance.SoundManager.IsMusicOff())
            {
                musicOffBtn.gameObject.SetActive(true);
                musicOnBtn.gameObject.SetActive(false);
            }
            else
            {
                musicOffBtn.gameObject.SetActive(false);
                musicOnBtn.gameObject.SetActive(true);
            }
        }



        public void LeaderboardBtn()
        {
            SettingBtn();
            StartCoroutine(CRHanleLeaderboardBtn());
        }
        private IEnumerator CRHanleLeaderboardBtn()
        {
            ViewManager.Instance.ScaleRect(gameNameTrans, Vector2.one, Vector2.zero, 1f);
            ViewManager.Instance.ScaleRect(playBtnTrans, Vector2.one, Vector2.zero, 0.5f);
            yield return new WaitForSeconds(0.15f);
            ViewManager.Instance.MoveRect(rewardBtnTrans, rewardBtnTrans.anchoredPosition, new Vector2(rewardBtnTrans.anchoredPosition.x, -200f), 0.5f);
            yield return new WaitForSeconds(0.08f);
            ViewManager.Instance.MoveRect(characterBtnTrans, characterBtnTrans.anchoredPosition, new Vector2(characterBtnTrans.anchoredPosition.x, -200f), 0.5f);
            yield return new WaitForSeconds(0.08f);
            ViewManager.Instance.MoveRect(settingBtnTrans, settingBtnTrans.anchoredPosition, new Vector2(settingBtnTrans.anchoredPosition.x, -200f), 0.5f);
            yield return new WaitForSeconds(0.5f);
            leaderboardViewController.gameObject.SetActive(true);
            leaderboardViewController.OnShow();
        }

        public void RateAppBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            Application.OpenURL(ServicesManager.Instance.ShareManager.AppUrl);
        }
        public void RemoveAdsBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
        }

    }
}
