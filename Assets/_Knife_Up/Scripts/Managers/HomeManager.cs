using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OnefallGames
{
    public class HomeManager : MonoBehaviour
    {

        [SerializeField] private Transform targetTrans = null;
        [SerializeField] private CoinRewardController coinCoinRewardControllerPrefab = null;
        [SerializeField] private ParticleSystem hitCoinEffectPrefab = null;
        [SerializeField] private SpriteRenderer[] knivesRenders = null;


        private List<CoinRewardController> listCoinCoinRewardController = new List<CoinRewardController>();
        private List<ParticleSystem> listHitCoinEffect = new List<ParticleSystem>();
        private void Start()
        {
            Application.targetFrameRate = 60;
            ViewManager.Instance.OnShowView(ViewType.HOME_VIEW);

            //Replace the sprite of main character with selected one
            CharacterInforController selectedcharacterInfor = ServicesManager.Instance.CharacterContainer.CharacterInforControllers[ServicesManager.Instance.CharacterContainer.SelectedCharacterIndex];
            foreach (SpriteRenderer o in knivesRenders)
            {
                o.sprite = selectedcharacterInfor.Sprite;
            }

            //Report level to leaderboard
            string username = PlayerPrefs.GetString(PlayerPrefsKeys.PPK_SAVED_USER_NAME);
            if (!string.IsNullOrEmpty(username))
            {
                ServicesManager.Instance.LeaderboardManager.SetPlayerLeaderboardData();
            }

        }

        private void Update()
        {
            targetTrans.localEulerAngles += Vector3.forward * 50f * Time.deltaTime;
        }


        /// <summary>
        /// //Creating coins for reward effect.
        /// </summary>
        /// <param name="number"></param>
        public void CreateRewardedCoins(int number)
        {
            StartCoroutine(CRCreatingCoins(number));
        }
        private IEnumerator CRCreatingCoins(int coinNumber)
        {
            yield return new WaitForSeconds(0.25f);

            float leftX = Camera.main.ViewportToWorldPoint(new Vector2(0.4f, 0f)).x;
            float rightX = Camera.main.ViewportToWorldPoint(new Vector2(0.9f, 0f)).x;
            float y = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0f)).y;
            for (int i = 0; i < coinNumber; i++)
            {
                float x = Random.Range(leftX, rightX);
                CoinRewardController coinCoinRewardController = GetCoinRewardController();
                coinCoinRewardController.gameObject.SetActive(true);
                coinCoinRewardController.transform.position = new Vector2(x, y);
                coinCoinRewardController.PlayRewardCoinsEffect();
                ServicesManager.Instance.CoinManager.AddTotalCoins(1);
                listCoinCoinRewardController.Add(coinCoinRewardController);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
            ViewManager.Instance.HomeViewController.OnSubViewClose();
        }


        /// <summary>
        /// Get an inactive CoinRewardController object.
        /// </summary>
        /// <returns></returns>
        private CoinRewardController GetCoinRewardController()
        {
            //Find an inactive coin control
            CoinRewardController coinControl = listCoinCoinRewardController.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();
            if (coinControl == null)
            {
                coinControl = Instantiate(coinCoinRewardControllerPrefab, Vector3.zero, Quaternion.identity);
                coinControl.gameObject.SetActive(false);
                listCoinCoinRewardController.Add(coinControl);
            }

            return coinControl;
        }


        /// <summary>
        /// Play the particle and disable that particle.
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        private IEnumerator CRPlayParticle(ParticleSystem par)
        {
            par.Play();
            yield return new WaitForSeconds(par.main.startLifetimeMultiplier);
            par.gameObject.SetActive(false);
        }



        /// <summary>
        /// Create a explosion effect at given position.
        /// </summary>
        /// <returns></returns>
        public void CreateHitCoinEffect(Vector2 pos)
        {
            ParticleSystem hitCoinEffect = listHitCoinEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();
            if (hitCoinEffect == null)
            {
                hitCoinEffect = Instantiate(hitCoinEffectPrefab, Vector3.zero, Quaternion.identity);
                listHitCoinEffect.Add(hitCoinEffect);
            }
            hitCoinEffect.gameObject.SetActive(true);
            hitCoinEffect.transform.position = pos;
            StartCoroutine(CRPlayParticle(hitCoinEffect));
        }
    }
}
