using System.Collections;
using UnityEngine;

namespace OnefallGames
{
    public class DynamicKnifeController : MonoBehaviour
    {

        [SerializeField] private Rigidbody2D rigid2D = null;
        [SerializeField] private BoxCollider2D boxCollider2D = null;


        public bool IsReadyToShoot { private set; get; }

        private float normalKnifeForceUp = 8f;
        private float lastKnifeForceUp = 18f;
        private float lastKnideTorque = 15f;
        private float normalKnifeTorque = 13f;
        private float forceUp = 0;
        private float torque = 0;


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
        /// Shoot this knife up to the target.
        /// </summary>
        /// <param name="speed"></param>
        public void ShootUp(float speed)
        {
            IsReadyToShoot = false;
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(boxCollider2D.size.x, boxCollider2D.size.x), 0, Vector2.up, 10f);
            while (hit.collider == null)
            {
                Physics2D.BoxCast(transform.position, new Vector2(boxCollider2D.size.x, boxCollider2D.size.x), 0, Vector2.up, 10f);
            }
            if (hit.collider.CompareTag("Finish"))
            {
                //Hit a knife that already stick on to the target
                StartCoroutine(CRMovingUpAndForceDown(speed, hit.point));
            }
            else
            {
                //Hit the target
                StartCoroutine(CRMovingToTarget(speed));
            }
        }



        /// <summary>
        /// Moving this knife to the target.
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        private IEnumerator CRMovingToTarget(float speed)
        {
            Vector2 startPos = transform.position;
            Vector2 endPos = Vector2.zero;
            if (IngameManager.Instance.LevelType == LevelType.NORMAL_LEVEL)
            {
                endPos = (Vector2)TargetController.Instance.transform.position + Vector2.down * TargetController.Instance.CircleCollider2D.radius;
            }
            else
            {
                endPos = (Vector2)BossController.Instance.transform.position + Vector2.down * BossController.Instance.CircleCollider2D.radius;
            }


            float movingTime = Vector3.Distance(transform.position, endPos) / speed;
            float t = 0;
            while (t < movingTime)
            {
                t += Time.deltaTime;
                float factor = t / movingTime;
                transform.position = Vector3.Lerp(startPos, endPos, factor);
                yield return null;
            }

            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.hitTarget);
            EffectManager.Instance.CreateHitTargetEffect(endPos);


            if (IngameManager.Instance.LevelType == LevelType.NORMAL_LEVEL)
            {
                transform.SetParent(TargetController.Instance.transform);
                TargetController.Instance.BounceTarget();
            }
            else
            {
                transform.SetParent(BossController.Instance.transform);
                BossController.Instance.Bounce();
            }

            if (!IngameManager.Instance.IsOutOfKnives()) //Still have knives left
            {
                //Move another dynamic knife to ready position 
                IngameManager.Instance.MoveNextKnifeToReadyPosition();

                forceUp = normalKnifeForceUp;
                torque = normalKnifeTorque;
            }
            else //User threw all knifes
            {
                forceUp = lastKnifeForceUp;
                torque = lastKnideTorque;
                IngameManager.Instance.CompletedLevel();
            }
        }


        /// <summary>
        /// Moving this knife up and force it fall down.
        /// Only call this when hit a knife and not the target.
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="hitPoint"></param>
        /// <returns></returns>
        private IEnumerator CRMovingUpAndForceDown(float speed, Vector2 hitPoint)
        {
            Vector2 startPos = transform.position;
            Vector2 endPos = hitPoint + (Vector2.down * boxCollider2D.size.y / 2f);
            float movingTime = Vector3.Distance(transform.position, endPos) / speed;
            float t = 0;
            while (t < movingTime)
            {
                t += Time.deltaTime;
                float factor = t / movingTime;
                transform.position = Vector3.Lerp(startPos, endPos, factor);
                yield return null;
            }

            //Capture screenshot
            ServicesManager.Instance.ShareManager.CreateScreenshot();

            //Play sound effect
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.hitKnife);

            boxCollider2D.enabled = false;
            rigid2D.bodyType = RigidbodyType2D.Dynamic;
            rigid2D.AddTorque(10, ForceMode2D.Impulse);

            while (gameObject.activeInHierarchy)
            {
                Vector2 viewPortPos = Camera.main.WorldToViewportPoint(transform.position);
                if (viewPortPos.y <= 0f)
                {
                    IngameManager.Instance.HandleGameOver();
                    IsReadyToShoot = false;
                    rigid2D.bodyType = RigidbodyType2D.Kinematic;
                    transform.localEulerAngles = Vector3.zero;
                    gameObject.SetActive(false);
                    yield break;
                }
                yield return null;
            }
        }


        /// <summary>
        /// Playing fly up effect when the last knife hit the target.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRPlayingFlyEffectAndDisableObject()
        {
            transform.parent = null;
            boxCollider2D.enabled = false;

            if (IngameManager.Instance.LevelType == LevelType.NORMAL_LEVEL)
            {
                Vector2 forceDir = (forceUp == lastKnifeForceUp) ? Vector2.up : (Vector2)(transform.position - TargetController.Instance.transform.position).normalized;
                forceDir *= forceUp;
                rigid2D.bodyType = RigidbodyType2D.Dynamic;
                rigid2D.AddForceAtPosition(forceDir, TargetController.Instance.transform.position, ForceMode2D.Impulse);
                rigid2D.AddTorque(torque, ForceMode2D.Impulse);
            }
            else
            {
                Vector2 forceDir = (forceUp == lastKnifeForceUp) ? Vector2.up : (Vector2)(transform.position - BossController.Instance.transform.position).normalized;
                forceDir *= forceUp;
                rigid2D.bodyType = RigidbodyType2D.Dynamic;
                rigid2D.AddForceAtPosition(forceDir, BossController.Instance.transform.position, ForceMode2D.Impulse);
                rigid2D.AddTorque(torque, ForceMode2D.Impulse);
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



        /// <summary>
        /// Move this knife to the ready position.
        /// </summary>
        public void MoveToReadyPosition()
        {
            IsReadyToShoot = false;
            StartCoroutine(CRSnapingToPos());
        }


        /// <summary>
        /// Snaping to the ready position.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CRSnapingToPos()
        {
            Vector2 startPos = transform.position;
            Vector2 endPos = (Vector2)Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.125f)) + Vector2.up * (PoolManager.Instance.GetDynamicKnifeSize() / 2f);
            float movingTime = 0.15f;
            float t = 0;
            while (t < movingTime)
            {
                t += Time.deltaTime;
                float factor = t / movingTime;
                transform.position = Vector2.Lerp(startPos, endPos, factor);
                yield return null;
            }
            yield return null;
            IsReadyToShoot = true;
        }

    }
}
