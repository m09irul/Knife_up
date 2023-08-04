using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace OnefallGames
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { private set; get; }

        [SerializeField] private StaticKnifeController staticKnifeControllerPrefab = null;
        [SerializeField] private DynamicKnifeController dynamicKnifeControllerPrefab = null;
        [SerializeField] private CoinController coinControllerPrefab = null;
        [SerializeField] private ObstacleController obstacleControllerPrefab = null;
        [SerializeField] private Image knifeIconPrefab = null;

        private List<CoinController> listCoinController = new List<CoinController>();
        private List<StaticKnifeController> listStaticKnifeController = new List<StaticKnifeController>();
        private List<DynamicKnifeController> listDynamicKnifeController = new List<DynamicKnifeController>();
        private List<ObstacleController> listObstacleController = new List<ObstacleController>();
        private List<Image> listKnifeIcon = new List<Image>();

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

            CharacterInforController selectedcharacterInfor = ServicesManager.Instance.CharacterContainer.CharacterInforControllers[ServicesManager.Instance.CharacterContainer.SelectedCharacterIndex];
            dynamicKnifeControllerPrefab.GetComponent<SpriteRenderer>().sprite = selectedcharacterInfor.Sprite;
            staticKnifeControllerPrefab.GetComponent<SpriteRenderer>().sprite = selectedcharacterInfor.Sprite;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }


        /// <summary>
        /// Get the size of dynamic knife.
        /// </summary>
        /// <returns></returns>
        public Vector2 GetDynamicKnifeSize()
        {
            return dynamicKnifeControllerPrefab.GetComponent<BoxCollider2D>().size;
        }




        /// <summary>
        /// Get an inactive coin control.
        /// </summary>
        /// <returns></returns>
        public CoinController GetCoinController()
        {
            //Find an inactive coin control
            CoinController coinControl = listCoinController.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();
            if (coinControl == null)
            {
                coinControl = Instantiate(coinControllerPrefab, Vector3.zero, Quaternion.identity);
                coinControl.gameObject.SetActive(false);
                listCoinController.Add(coinControl);
            }

            return coinControl;
        }



        /// <summary>
        /// Get an inactive coin control.
        /// </summary>
        /// <returns></returns>
        public ObstacleController GetObstacleController()
        {
            //Find an inactive ObstacleController
            ObstacleController obstacleControl = listObstacleController.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();
            if (obstacleControl == null)
            {
                obstacleControl = Instantiate(obstacleControllerPrefab, Vector3.zero, Quaternion.identity);
                obstacleControl.gameObject.SetActive(false);
                listObstacleController.Add(obstacleControl);
            }

            return obstacleControl;
        }




        /// <summary>
        /// Get an inactive StaticKnifeController.
        /// </summary>
        /// <returns></returns>
        public StaticKnifeController GetStaticKnifeController()
        {
            //Find an inactive coin control
            StaticKnifeController staticKnifeController = listStaticKnifeController.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();
            if (staticKnifeController == null)
            {
                staticKnifeController = Instantiate(staticKnifeControllerPrefab, Vector3.zero, Quaternion.identity);
                staticKnifeController.gameObject.SetActive(false);
                listStaticKnifeController.Add(staticKnifeController);
            }

            return staticKnifeController;
        }



        /// <summary>
        /// Get an inactive DynamicKnifeController.
        /// </summary>
        /// <returns></returns>
        public DynamicKnifeController GetDynamicKnifeController()
        {
            //Find an inactive coin control
            DynamicKnifeController dynamicKnifeController = listDynamicKnifeController.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();
            if (dynamicKnifeController == null)
            {
                dynamicKnifeController = Instantiate(dynamicKnifeControllerPrefab, Vector3.zero, Quaternion.identity);
                dynamicKnifeController.gameObject.SetActive(false);
                listDynamicKnifeController.Add(dynamicKnifeController);
            }

            return dynamicKnifeController;
        }



        /// <summary>
        /// Get an inactive knife icon.
        /// </summary>
        /// <returns></returns>
        public Image GetKnifeIcon()
        {
            //Find an inactive knife icon
            Image knifeIcon = listKnifeIcon.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();
            if (knifeIcon == null)
            {
                knifeIcon = Instantiate(knifeIconPrefab, Vector3.zero, Quaternion.identity);
                knifeIcon.gameObject.SetActive(false);
                listKnifeIcon.Add(knifeIcon);
            }
            return knifeIcon;
        }
    }
}
