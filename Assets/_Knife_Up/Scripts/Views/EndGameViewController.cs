using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace OnefallGames
{
    public class EndGameViewController : MonoBehaviour
    {

        [SerializeField] private RectTransform topBarTrans = null;
        [SerializeField] private RectTransform levelFailedTextTrans = null;
        [SerializeField] private RectTransform replayBtnTrans = null;
        [SerializeField] private RectTransform shareBtnTrans = null;
        [SerializeField] private RectTransform characterBtnTrans = null;
        [SerializeField] private RectTransform homeBtnTrans = null;
        [SerializeField] private Text currentLevelTxt = null;
        [SerializeField] private Text totalCoinsTxt = null;
        [SerializeField] private Image fadeOutPanel = null;
        [SerializeField] private Image blackPanel = null;
        public void OnShow()
        {
            if (IngameManager.Instance.IngameState == IngameState.CompletedLevel)
            {
                blackPanel.gameObject.SetActive(false);
                fadeOutPanel.gameObject.SetActive(true);
                StartCoroutine(CRFadingPanel());
            }
            else
            {
                currentLevelTxt.text = "Level: " + IngameManager.Instance.CurrentLevel.ToString();
                fadeOutPanel.gameObject.SetActive(false);
                blackPanel.gameObject.SetActive(true);
                ViewManager.Instance.MoveRect(topBarTrans, topBarTrans.anchoredPosition, new Vector2(topBarTrans.anchoredPosition.x, 0), 0.5f);
                ViewManager.Instance.ScaleRect(levelFailedTextTrans, Vector2.zero, Vector2.one, 0.5f);
                StartCoroutine(CRShowingBottomButtons());
            }
        }


        private void OnDisable()
        {
            topBarTrans.anchoredPosition = new Vector2(topBarTrans.anchoredPosition.x, 200);
            replayBtnTrans.localScale = Vector3.zero;
            levelFailedTextTrans.localScale = Vector3.zero;
            shareBtnTrans.anchoredPosition = new Vector2(shareBtnTrans.anchoredPosition.x, -200f);
            characterBtnTrans.anchoredPosition = new Vector2(characterBtnTrans.anchoredPosition.x, -200f);
            homeBtnTrans.anchoredPosition = new Vector2(homeBtnTrans.anchoredPosition.x, -200f);
        }

        private void Update()
        {
            totalCoinsTxt.text = ServicesManager.Instance.CoinManager.TotalCoins.ToString();
        }


        private IEnumerator CRShowingBottomButtons()
        {
            ViewManager.Instance.ScaleRect(replayBtnTrans, Vector2.zero, Vector2.one, 0.5f);
            yield return new WaitForSeconds(0.15f);
            ViewManager.Instance.MoveRect(shareBtnTrans, shareBtnTrans.anchoredPosition, new Vector2(shareBtnTrans.anchoredPosition.x, 150), 0.5f);
            yield return new WaitForSeconds(0.15f);
            ViewManager.Instance.MoveRect(characterBtnTrans, characterBtnTrans.anchoredPosition, new Vector2(characterBtnTrans.anchoredPosition.x, 150), 0.5f);
            yield return new WaitForSeconds(0.15f);
            ViewManager.Instance.MoveRect(homeBtnTrans, homeBtnTrans.anchoredPosition, new Vector2(homeBtnTrans.anchoredPosition.x, 150), 0.5f);
        }


        private IEnumerator CRFadingPanel()
        {
            Color startColor = fadeOutPanel.color;
            startColor.a = 0;
            fadeOutPanel.color = startColor;
            fadeOutPanel.gameObject.SetActive(true);
            float fadingTime = 0.5f;
            float t = 0;
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

        public void HomeBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ViewManager.Instance.LoadScene("Home", 0.25f);
        }
    }
}
