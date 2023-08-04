using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace OnefallGames
{
    public class ReviveViewController : MonoBehaviour
    {

        [SerializeField] private RectTransform tryAgainImageTrans = null;
        [SerializeField] private RectTransform reviveBtnTrans = null;
        [SerializeField] private RectTransform sunbrustImageTrans = null;
        [SerializeField] private RectTransform closeReviveViewBtnTrans = null;
        [SerializeField] private Text countDownTxt = null;


        public void OnShow()
        {
            ViewManager.Instance.ScaleRect(tryAgainImageTrans, Vector2.zero, Vector2.one, 0.5f);
            ViewManager.Instance.ScaleRect(sunbrustImageTrans, Vector2.zero, Vector2.one, 0.5f);
            countDownTxt.gameObject.SetActive(false);
            StartCoroutine(CRWaitAndStartCountDown());
        }

        private void OnDisable()
        {
            tryAgainImageTrans.localScale = Vector2.zero;
            sunbrustImageTrans.localScale = Vector2.zero;
            reviveBtnTrans.localScale = Vector2.zero;
            closeReviveViewBtnTrans.localScale = Vector2.zero;
            countDownTxt.gameObject.SetActive(false);
        }


        /// <summary>
        /// Wait for the an amount of time and start count down. 
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRWaitAndStartCountDown()
        {
            yield return new WaitForSeconds(0.5f);
            ViewManager.Instance.ScaleRect(reviveBtnTrans, Vector2.zero, Vector2.one, 0.5f);
            countDownTxt.gameObject.SetActive(true);
            countDownTxt.text = IngameManager.Instance.ReviveCountDownTime.ToString();
            StartCoroutine(CRCoutingDown());
            StartCoroutine(CRScalingCountDownText());
            yield return new WaitForSeconds(1f);
            ViewManager.Instance.ScaleRect(closeReviveViewBtnTrans, Vector2.zero, Vector2.one, 0.5f);
        }



        /// <summary>
        /// Start counting down revive wait time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRCoutingDown()
        {
            float countingTime = IngameManager.Instance.ReviveCountDownTime;
            float t = 0;
            while (t < countingTime)
            {
                t += Time.deltaTime;
                float factor = t / countingTime;
                countDownTxt.text = Mathf.RoundToInt(Mathf.Lerp(countingTime, 0f, factor)).ToString();
                yield return null;
                sunbrustImageTrans.eulerAngles += Vector3.forward * 100f * Time.deltaTime;
            }
            IngameManager.Instance.GameOver();
        }



        /// <summary>
        /// Sacling the count down text.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRScalingCountDownText()
        {
            float scalingTime = 0.25f;
            float t = 0;
            while (gameObject.activeInHierarchy)
            {
                t = 0;
                while (t < scalingTime)
                {
                    t += Time.deltaTime;
                    float factor = t / scalingTime;
                    countDownTxt.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 0.8f, factor);
                    yield return null;
                }

                t = 0;
                while (t < scalingTime)
                {
                    t += Time.deltaTime;
                    float factor = t / scalingTime;
                    countDownTxt.rectTransform.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, factor);
                    yield return null;
                }
            }
        }





        public void ReviveBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            StopAllCoroutines();
            ServicesManager.Instance.AdManager.ShowRewardedAd();
        }

        public void CloseReviveViewBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            StopAllCoroutines();
            IngameManager.Instance.GameOver();
        }
    }
}
