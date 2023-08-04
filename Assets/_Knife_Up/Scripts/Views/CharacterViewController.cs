using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OnefallGames
{
    public class CharacterViewController : MonoBehaviour
    {

        [SerializeField] private RectTransform topBarTrans = null;
        [SerializeField] private RectTransform bottomBarTrans = null;
        [SerializeField] private Text totalCoinsTxt = null;
        [SerializeField] private Text characterPriceTxt = null;
        [SerializeField] private Button selectBtn = null;
        [SerializeField] private Button unlockBtn = null;
        [SerializeField] private Image fadeOutPanel = null;

        private CharacterManager characterController = null;
        private CharacterInforController currentCharacterInforController = null;
        public void OnShow()
        {
            StartCoroutine(CRFadingOutPanel());
            if (characterController == null)
            {
                characterController = FindObjectOfType<CharacterManager>();
            }
        }

        private void OnDisable()
        {
            topBarTrans.anchoredPosition = new Vector2(topBarTrans.anchoredPosition.x, 150);
            bottomBarTrans.anchoredPosition = new Vector2(topBarTrans.anchoredPosition.x, -350);
        }

        private void Update()
        {
            totalCoinsTxt.text = ServicesManager.Instance.CoinManager.TotalCoins.ToString();
        }


        public void UpdateUI(CharacterInforController characterInfor)
        {
            currentCharacterInforController = characterInfor;
            if (!characterInfor.IsUnlocked) //The character is not unlocked yet
            {
                selectBtn.gameObject.SetActive(false);
                unlockBtn.gameObject.SetActive(true);
                characterPriceTxt.text = characterInfor.CharacterPrice.ToString();
                if (ServicesManager.Instance.CoinManager.TotalCoins >= characterInfor.CharacterPrice)
                {
                    //Enough coins -> allow user buy this character
                    unlockBtn.interactable = true;
                }
                else
                {
                    //Not enough coins -> dont allow user buy this character
                    unlockBtn.interactable = false;
                }
            }
            else//The character is already unlocked
            {
                unlockBtn.gameObject.SetActive(false);
                selectBtn.gameObject.SetActive(true);
            }
        }


        /// <summary>
        /// Fading out panel.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRFadingOutPanel()
        {
            fadeOutPanel.gameObject.SetActive(true);
            float fadingTime = 0.5f;
            float t = 0;
            Color startColor = fadeOutPanel.color;
            startColor.a = 1;
            fadeOutPanel.color = startColor;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
            yield return new WaitForSeconds(0.5f);
            while (t < fadingTime)
            {
                t += Time.deltaTime;
                float factor = t / fadingTime;
                fadeOutPanel.color = Color.Lerp(startColor, endColor, factor);
                yield return null;
            }
            fadeOutPanel.gameObject.SetActive(false);

            ViewManager.Instance.MoveRect(topBarTrans, topBarTrans.anchoredPosition, new Vector2(topBarTrans.anchoredPosition.x, 0), 0.5f);
            ViewManager.Instance.MoveRect(bottomBarTrans, bottomBarTrans.anchoredPosition, new Vector2(bottomBarTrans.anchoredPosition.x, 0), 0.5f);
        }



        public void UnlockBtn()
        {
            currentCharacterInforController.Unlock();
            UpdateUI(currentCharacterInforController);
        }

        public void SelectBtn()
        {
            ServicesManager.Instance.CharacterContainer.SetSelectedCharacterIndex(currentCharacterInforController.SequenceNumber);
            BackBtn();
        }

        public void BackBtn()
        {
            ViewManager.Instance.PlayClickButtonSound();
            ViewManager.Instance.LoadScene("Home", 0.3f);
        }
    }
}
