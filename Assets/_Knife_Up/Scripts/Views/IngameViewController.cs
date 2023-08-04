using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace OnefallGames
{
    public class IngameViewController : MonoBehaviour
    {

        [SerializeField] private PlayingViewController playingViewControl = null;
        [SerializeField] private ReviveViewController reviveViewControl = null;
        [SerializeField] private EndGameViewController endGameViewControl = null;
        [SerializeField] private Image fadeOutPanel = null;

        public PlayingViewController PlayingViewControl { get { return playingViewControl; } }

        public void OnShow()
        {
            IngameManager.IngameStateChanged += IngameManager_IngameStateChanged;
            playingViewControl.gameObject.SetActive(false);
            reviveViewControl.gameObject.SetActive(false);
            endGameViewControl.gameObject.SetActive(false);
            StartCoroutine(CRFadingPanel());
        }

        private void OnDisable()
        {
            IngameManager.IngameStateChanged -= IngameManager_IngameStateChanged;
        }

        private void IngameManager_IngameStateChanged(IngameState obj)
        {
            if (obj == IngameState.Revive)
            {
                StartCoroutine(CRShowReviveView());
            }
            else if (obj == IngameState.GameOver || obj == IngameState.CompletedLevel)
            {
                StartCoroutine(CRShowEndGameView());
            }
            else if (obj == IngameState.Playing)
            {
                playingViewControl.gameObject.SetActive(true);
                playingViewControl.OnShow();

                reviveViewControl.gameObject.SetActive(false);
                endGameViewControl.gameObject.SetActive(false);
            }
        }


        private IEnumerator CRFadingPanel()
        {
            Color startColor = fadeOutPanel.color;
            startColor.a = 1;
            fadeOutPanel.color = startColor;
            fadeOutPanel.gameObject.SetActive(true);
            float fadingTime = 0.5f;
            float t = 0;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
            while (t < fadingTime)
            {
                t += Time.deltaTime;
                float factor = t / fadingTime;
                fadeOutPanel.color = Color.Lerp(startColor, endColor, factor);
                yield return null;
            }
            fadeOutPanel.gameObject.SetActive(false);
            IngameManager.Instance.PlayingGame();
        }


        private IEnumerator CRShowEndGameView()
        {
            yield return new WaitForSeconds(0.75f);
            endGameViewControl.gameObject.SetActive(true);
            endGameViewControl.OnShow();
            reviveViewControl.gameObject.SetActive(false);
            playingViewControl.gameObject.SetActive(false);
        }

        private IEnumerator CRShowReviveView()
        {
            yield return new WaitForSeconds(0.15f);
            reviveViewControl.gameObject.SetActive(true);
            reviveViewControl.OnShow();

            playingViewControl.gameObject.SetActive(false);
            endGameViewControl.gameObject.SetActive(false);
        }
    }
}
