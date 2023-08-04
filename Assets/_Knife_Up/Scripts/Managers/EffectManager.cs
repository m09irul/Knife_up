using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace OnefallGames
{
    public class EffectManager : MonoBehaviour
    {

        public static EffectManager Instance { private set; get; }

        [SerializeField] private ParticleSystem hitCoinEffectPrefab = null;
        [SerializeField] private ParticleSystem hitTargetEffectPrefab = null;

        private List<ParticleSystem> listHitCoinEffect = new List<ParticleSystem>();
        private List<ParticleSystem> listHitTargetEffect = new List<ParticleSystem>();

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


        /// <summary>
        /// Play the given particle then disable it 
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



        /// <summary>
        /// Create a explosion effect at given position.
        /// </summary>
        /// <returns></returns>
        public void CreateHitTargetEffect(Vector2 pos)
        {
            ParticleSystem hitTargetEffect = listHitTargetEffect.Where(a => !a.gameObject.activeInHierarchy).FirstOrDefault();
            if (hitTargetEffect == null)
            {
                hitTargetEffect = Instantiate(hitTargetEffectPrefab, Vector3.zero, Quaternion.identity);
                listHitTargetEffect.Add(hitTargetEffect);
            }
            hitTargetEffect.gameObject.SetActive(true);
            hitTargetEffect.transform.position = pos;
            StartCoroutine(CRPlayParticle(hitTargetEffect));
        }
    }
}
