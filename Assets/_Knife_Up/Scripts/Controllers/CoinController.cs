using System.Collections;
using UnityEngine;

namespace OnefallGames
{
    public class CoinController : MonoBehaviour
    {
        [SerializeField] private LayerMask dynamicKnifeLayer = new LayerMask();
        [SerializeField] private Rigidbody2D rigid2D = null;
        [SerializeField] private SpriteRenderer spriteRenderer = null;
        [SerializeField] private Rigidbody2D[] coinPieceRigidbodies = null;


        private void OnEnable()
        {
            IngameManager.IngameStateChanged += IngameManager_IngameStateChanged;
        }
        private void OnDisable()
        {
            IngameManager.IngameStateChanged -= IngameManager_IngameStateChanged;
        }

        private void IngameManager_IngameStateChanged(IngameState obj)
        {
            if (obj == IngameState.CompletedLevel)
            {
                StartCoroutine(CRPlayingFlyEffectAndDisableObject());
            }
        }



        private void Update()
        {
            if (IngameManager.Instance.IngameState == IngameState.Playing)
            {
                Vector2 size = (Vector2)spriteRenderer.bounds.size - (Vector2.one * 0.15f);
                Collider2D collider2D = Physics2D.OverlapBox(transform.position, size, 0, dynamicKnifeLayer);
                if (collider2D != null)
                {
                    ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.hitCoin);
                    ServicesManager.Instance.CoinManager.AddTotalCoins(1);
                    EffectManager.Instance.CreateHitCoinEffect(transform.position);

                    transform.SetParent(null);
                    spriteRenderer.enabled = false;

                    //Force the pieces down
                    foreach (Rigidbody2D o in coinPieceRigidbodies)
                    {
                        o.gameObject.SetActive(true);
                        o.transform.SetParent(null);

                        Vector2 dir = (o.transform.position - transform.position).normalized * 5f;
                        o.AddForceAtPosition(dir, transform.position, ForceMode2D.Impulse);
                        o.AddTorque(10f, ForceMode2D.Impulse);
                        Destroy(o.gameObject, 2f);
                    }

                    Destroy(gameObject);
                }
            }
        }




        /// <summary>
        /// Play flying effect when Completed Level
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRPlayingFlyEffectAndDisableObject()
        {
            transform.parent = null;
            rigid2D.bodyType = RigidbodyType2D.Dynamic;
            if (IngameManager.Instance.LevelType == LevelType.NORMAL_LEVEL)
            {
                Vector2 forceDir = (transform.position - TargetController.Instance.transform.position).normalized;
                rigid2D.AddForceAtPosition(forceDir * 10f, TargetController.Instance.transform.position, ForceMode2D.Impulse);
                rigid2D.AddTorque(5f, ForceMode2D.Impulse);
            }
            else
            {
                Vector2 forceDir = (transform.position - BossController.Instance.transform.position).normalized;
                rigid2D.AddForceAtPosition(forceDir * 10f, BossController.Instance.transform.position, ForceMode2D.Impulse);
                rigid2D.AddTorque(5f, ForceMode2D.Impulse);
            }

            while (gameObject.activeInHierarchy)
            {
                Vector2 viewPortPos = Camera.main.WorldToViewportPoint(transform.position);
                if (viewPortPos.x >= 1.1f || viewPortPos.x <= -0.1f || viewPortPos.y <= -0.1f || viewPortPos.y >= 1.1f)
                {
                    Destroy(gameObject, 0.1f);
                    yield break;
                }
                yield return null;
            }
        }
    }
}
