using UnityEngine;

namespace OnefallGames
{
    public class StaticKnifeController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigid2D = null;
        [SerializeField] private BoxCollider2D boxCollider2D = null;

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
                transform.parent = null;
                boxCollider2D.enabled = false;

                if (IngameManager.Instance.LevelType == LevelType.NORMAL_LEVEL)
                {
                    Vector2 forceDir = (transform.position - TargetController.Instance.transform.position).normalized * 8f;
                    rigid2D.bodyType = RigidbodyType2D.Dynamic;
                    rigid2D.AddForceAtPosition(forceDir, TargetController.Instance.transform.position, ForceMode2D.Impulse);
                    rigid2D.AddTorque(7f, ForceMode2D.Impulse);
                    Destroy(gameObject, 2f);
                }
                else
                {
                    Vector2 forceDir = (transform.position - BossController.Instance.transform.position).normalized * 8f;
                    rigid2D.bodyType = RigidbodyType2D.Dynamic;
                    rigid2D.AddForceAtPosition(forceDir, BossController.Instance.transform.position, ForceMode2D.Impulse);
                    rigid2D.AddTorque(7f, ForceMode2D.Impulse);
                    Destroy(gameObject, 2f);
                }
            }
        }
    }
}
