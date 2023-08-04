using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace OnefallGames
{
    public class PlayingViewController : MonoBehaviour
    {

        [SerializeField] private RectTransform topBarTrans = null;
        [SerializeField] private RectTransform knivesPanelTrans = null;
        [SerializeField] private Text currentLevelTxt = null;
        [SerializeField] private Text bossNameTxt = null;
        [SerializeField] private Text totalCoinsTxt = null;

        private List<Image> listKnifeIconTemp = new List<Image>();
        private Image lastdisableKnifeIcon = null;
        public void OnShow()
        {
            ViewManager.Instance.MoveRect(knivesPanelTrans, knivesPanelTrans.anchoredPosition, new Vector2(0, knivesPanelTrans.anchoredPosition.y), 0.5f);
            ViewManager.Instance.MoveRect(topBarTrans, topBarTrans.anchoredPosition, new Vector2(topBarTrans.anchoredPosition.x, 0), 0.5f);

            if (IngameManager.Instance.LevelType == LevelType.NORMAL_LEVEL)
            {
                bossNameTxt.gameObject.SetActive(false);
                currentLevelTxt.gameObject.SetActive(true);
                currentLevelTxt.text = "Level: " + IngameManager.Instance.CurrentLevel.ToString();
            }
            else
            {
                bossNameTxt.gameObject.SetActive(true);
                currentLevelTxt.gameObject.SetActive(false);
            }


            if (!IngameManager.Instance.IsRevived)
            {
                foreach (Image o in listKnifeIconTemp)
                {
                    o.gameObject.SetActive(false);
                }
                listKnifeIconTemp.Clear();
                lastdisableKnifeIcon = null;

                for (int i = 0; i < IngameManager.Instance.KnifeNumber; i++)
                {
                    Image knifeIcon = PoolManager.Instance.GetKnifeIcon();
                    knifeIcon.gameObject.SetActive(true);
                    knifeIcon.transform.SetParent(knivesPanelTrans);
                    knifeIcon.transform.localScale = Vector3.one;
                    knifeIcon.transform.localEulerAngles = new Vector3(0, 0, 25f);
                    listKnifeIconTemp.Add(knifeIcon);
                }
            }
        }

        private void OnDisable()
        {
            knivesPanelTrans.anchoredPosition = new Vector2(-150, knivesPanelTrans.anchoredPosition.y);
            topBarTrans.anchoredPosition = new Vector2(topBarTrans.anchoredPosition.x, 150);

        }


        private void Update()
        {
            totalCoinsTxt.text = ServicesManager.Instance.CoinManager.TotalCoins.ToString();
        }


        public void DisableKnife()
        {
            lastdisableKnifeIcon = listKnifeIconTemp[0];
            listKnifeIconTemp[0].gameObject.SetActive(false);
            listKnifeIconTemp.RemoveAt(0);
        }

        public void EnableKnife()
        {
            lastdisableKnifeIcon.gameObject.SetActive(true);
            listKnifeIconTemp.Add(lastdisableKnifeIcon);
        }


        public void SetBossName(string name)
        {
            bossNameTxt.text = name;
        }

    }
}
