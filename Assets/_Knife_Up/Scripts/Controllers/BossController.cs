using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OnefallGames
{
    public class BossController : MonoBehaviour
    {
        public static BossController Instance { private set; get; }

        [Header("Boss References")]
        [SerializeField] private SpriteRenderer spriteRenderer = null;
        [SerializeField] private BoxCollider2D boxCollider2D = null;
        [SerializeField] private CircleCollider2D circleCollider2D = null;
        [SerializeField] private Rigidbody2D[] brokenPieces = null;

        public CircleCollider2D CircleCollider2D { get { return circleCollider2D; } }


        private List<Vector2> listTargetPos = new List<Vector2>();


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
                Instance = null;
            }
        }


        private void OnDisable()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }


        /// <summary>
        /// Set this boss sprite by given sprite .
        /// </summary>
        /// <param name="sp"></param>
        public void SetSprite(Sprite sp)
        {
            spriteRenderer.sprite = sp;
        }


        /// <summary>
        /// Create objects such as coins, static knifes and obstacles.
        /// </summary>
        /// <param name="targetObjectData"></param>
        public void CreateObjects(TargetObjectData targetObjectData)
        {
            //Get radius
            float targetRadius = circleCollider2D.radius;
            //Left, right, up, down positions
            listTargetPos.Add((Vector2)transform.position + Vector2.up * targetRadius);
            listTargetPos.Add((Vector2)transform.position + Vector2.down * targetRadius);
            listTargetPos.Add((Vector2)transform.position + Vector2.left * targetRadius);
            listTargetPos.Add((Vector2)transform.position + Vector2.right * targetRadius);

            //up_Left, up_Right, down_Left, down_Right
            Vector2 up_Left = (Vector2.up + Vector2.left).normalized;
            Vector2 up_Right = (Vector2.up + Vector2.right).normalized;
            Vector2 down_Left = (Vector2.down + Vector2.left).normalized;
            Vector2 down_Right = (Vector2.down + Vector2.right).normalized;
            listTargetPos.Add((Vector2)transform.position + up_Left * targetRadius);
            listTargetPos.Add((Vector2)transform.position + up_Right * targetRadius);
            listTargetPos.Add((Vector2)transform.position + down_Left * targetRadius);
            listTargetPos.Add((Vector2)transform.position + down_Right * targetRadius);

            //left_UpLeft, up_UpLeft, right_UpRight, up_UpRight
            Vector2 left_UpLeft = (Vector2.left + up_Left).normalized;
            Vector2 up_UpLeft = (Vector2.up + up_Left).normalized;
            Vector2 right_UpRight = (Vector2.right + up_Right).normalized;
            Vector2 up_UpRight = (Vector2.up + up_Right).normalized;
            listTargetPos.Add((Vector2)transform.position + left_UpLeft * targetRadius);
            listTargetPos.Add((Vector2)transform.position + up_UpLeft * targetRadius);
            listTargetPos.Add((Vector2)transform.position + right_UpRight * targetRadius);
            listTargetPos.Add((Vector2)transform.position + up_UpRight * targetRadius);

            //left_DownLeft, down_DownLeft, right_DownRight, down_DownRight
            Vector2 left_DownLeft = (Vector2.left + down_Left).normalized;
            Vector2 down_DownLeft = (Vector2.down + down_Left).normalized;
            Vector2 right_DownRight = (Vector2.right + down_Right).normalized;
            Vector2 down_DownRight = (Vector2.down + down_Right).normalized;
            listTargetPos.Add((Vector2)transform.position + left_DownLeft * targetRadius);
            listTargetPos.Add((Vector2)transform.position + down_DownLeft * targetRadius);
            listTargetPos.Add((Vector2)transform.position + right_DownRight * targetRadius);
            listTargetPos.Add((Vector2)transform.position + down_DownRight * targetRadius);

            //Start create objects.
            StartCoroutine(CRCreatingObjects(targetObjectData));
        }


        /// <summary>
        /// Creating objects for this boss.
        /// </summary>
        /// <param name="targetObjectData"></param>
        /// <returns></returns>
        private IEnumerator CRCreatingObjects(TargetObjectData targetObjectData)
        {
            //Create coins
            for (int i = 0; i < targetObjectData.CoinNumber; i++)
            {
                int o = Random.Range(0, listTargetPos.Count);
                Vector2 targetVector = (listTargetPos[o] - (Vector2)transform.position).normalized;
                Vector3 angles = new Vector3(0, 0, Vector3.Angle(targetVector, Vector2.up));
                if (listTargetPos[o].x > transform.position.x)
                    angles = new Vector3(0, 0, 360 - angles.z);

                CoinController coinController = PoolManager.Instance.GetCoinController();
                coinController.transform.position = listTargetPos[o];
                coinController.transform.localEulerAngles = angles;
                coinController.transform.SetParent(transform);
                coinController.gameObject.SetActive(true);
                listTargetPos.RemoveAt(o);
                yield return null;
            }


            //Create static knifes
            for (int i = 0; i < targetObjectData.StaticKnifeNumber; i++)
            {
                int o = Random.Range(0, listTargetPos.Count);
                Vector2 targetVector = (listTargetPos[o] - (Vector2)transform.position).normalized;
                Vector3 angles = new Vector3(0, 0, Vector3.Angle(targetVector, Vector2.up));
                if (listTargetPos[o].x > transform.position.x)
                    angles = new Vector3(0, 0, 360 - angles.z);

                StaticKnifeController statiKnifeController = PoolManager.Instance.GetStaticKnifeController();
                statiKnifeController.transform.position = listTargetPos[o];
                statiKnifeController.transform.localEulerAngles = angles;
                statiKnifeController.transform.SetParent(transform);
                statiKnifeController.gameObject.SetActive(true);
                listTargetPos.RemoveAt(o);
                yield return null;
            }

            //Create obstacles
            for (int i = 0; i < targetObjectData.ObstacleNumber; i++)
            {
                int o = Random.Range(0, listTargetPos.Count);
                Vector2 targetVector = (listTargetPos[o] - (Vector2)transform.position).normalized;
                Vector3 angles = new Vector3(0, 0, Vector3.Angle(targetVector, Vector2.up));
                if (listTargetPos[o].x > transform.position.x)
                    angles = new Vector3(0, 0, 360 - angles.z);

                ObstacleController obstacleController = PoolManager.Instance.GetObstacleController();
                obstacleController.transform.position = listTargetPos[o];
                obstacleController.transform.localEulerAngles = angles;
                obstacleController.transform.SetParent(transform);
                obstacleController.gameObject.SetActive(true);
                listTargetPos.RemoveAt(o);
                yield return null;
            }
        }


        /// <summary>
        /// Start rotating this boss.
        /// </summary>
        public void StartRotating(TargetParameterData targetParameterData)
        {
            StartCoroutine(CRRotating(targetParameterData));
        }

        private IEnumerator CRRotating(TargetParameterData targetParameterData)
        {
            while (IngameManager.Instance.IngameState == IngameState.Playing && gameObject.activeInHierarchy)
            {
                Vector3 currentAngles = transform.eulerAngles;
                int randomZAngle = Random.Range(targetParameterData.MinRotatingAngle, targetParameterData.MaxRotatingAngle);
                Vector3 endAngles = Vector3.zero;
                if (Random.value <= 0.5f)
                    endAngles = new Vector3(0, 0, currentAngles.z + randomZAngle);
                else
                    endAngles = new Vector3(0, 0, currentAngles.z - randomZAngle);
                float rotatingTime = randomZAngle / Random.Range(targetParameterData.MinRotatingSpeed, targetParameterData.MaxRotatingSpeed);
                LerpType lerpType = targetParameterData.LerpTypes[Random.Range(0, targetParameterData.LerpTypes.Length)];

                float t = 0;
                while (t < rotatingTime)
                {
                    t += Time.deltaTime;
                    float factor = EasyType.MatchedLerpType(lerpType, t / rotatingTime);
                    transform.eulerAngles = Vector3.Lerp(currentAngles, endAngles, factor);
                    yield return null;
                    if (IngameManager.Instance.IngameState != IngameState.Playing)
                    {
                        yield break;
                    }
                }

                yield return null;
                if (IngameManager.Instance.IngameState != IngameState.Playing)
                {
                    yield break;
                }
            }
        }


        /// <summary>
        /// Play broken effect of this boss.
        /// </summary>
        public void PlayBrokenEffect()
        {
            StartCoroutine(CrPlayingBrokenEffect());
        }
        private IEnumerator CrPlayingBrokenEffect()
        {
            yield return new WaitForFixedUpdate();
            spriteRenderer.enabled = false;
            circleCollider2D.enabled = false;
            boxCollider2D.enabled = false;

            //Enable broken pieces
            foreach (Rigidbody2D o in brokenPieces)
            {
                o.gameObject.SetActive(true);
                o.transform.parent = null;

                Vector3 forceDirection = (o.transform.position - transform.position).normalized * 10f;
                o.AddForceAtPosition(forceDirection, o.GetComponent<SpriteRenderer>().bounds.center, ForceMode2D.Impulse);
                o.AddTorque(10, ForceMode2D.Impulse);
                Destroy(o.gameObject, 2f);
                yield return null;
            }
        }


        /// <summary>
        /// Boucing this boss up and down.
        /// </summary>
        public void Bounce()
        {
            StartCoroutine(CRBouncing());
        }
        private IEnumerator CRBouncing()
        {
            yield return null;
            float t = 0;
            Vector2 startPos = transform.position;
            Vector2 endPos = startPos + Vector2.up * 0.1f;
            float bouncingTime = 0.05f;
            while (t < bouncingTime)
            {
                t += Time.deltaTime;
                float factor = t / bouncingTime;
                transform.position = Vector2.Lerp(startPos, endPos, factor);
                yield return null;
            }
            t = 0;
            while (t < bouncingTime)
            {
                t += Time.deltaTime;
                float factor = t / bouncingTime;
                transform.position = Vector2.Lerp(endPos, startPos, factor);
                yield return null;
            }
        }
    }
}
