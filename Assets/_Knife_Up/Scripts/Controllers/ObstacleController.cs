using System.Collections;
using UnityEngine;

namespace OnefallGames
{
    public class ObstacleController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigid2D = null;


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


        /// <summary>
        /// Play flying effect and disable this ostacle.
        /// Only call this when player complete the level.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRPlayingFlyEffectAndDisableObject()
        {
            transform.parent = null;
            rigid2D.bodyType = RigidbodyType2D.Dynamic;
            if (IngameManager.Instance.LevelType == LevelType.NORMAL_LEVEL)
            {
                Vector2 forceDir = (transform.position - TargetController.Instance.transform.position).normalized;
                rigid2D.AddForceAtPosition(forceDir * 5f, TargetController.Instance.transform.position, ForceMode2D.Impulse);
                rigid2D.AddTorque(3f, ForceMode2D.Impulse);
            }
            else
            {
                Vector2 forceDir = (transform.position - BossController.Instance.transform.position).normalized;
                rigid2D.AddForceAtPosition(forceDir * 5f, BossController.Instance.transform.position, ForceMode2D.Impulse);
                rigid2D.AddTorque(3f, ForceMode2D.Impulse);
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
