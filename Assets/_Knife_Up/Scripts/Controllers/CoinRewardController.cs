using System.Collections;
using UnityEngine;

namespace OnefallGames
{
    public class CoinRewardController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigid2D = null;
        [SerializeField] private SpriteRenderer spriteRenderer = null;
        [SerializeField] private Rigidbody2D[] coinPieces = null;


        private Vector2 coinPiece_0_Pos = Vector2.zero;
        private Vector3 coinPiece_0_Angles = Vector3.zero;
        private Vector2 coinPiece_1_Pos = Vector2.zero;
        private Vector3 coinPiece_1_Angles = Vector3.zero;

        /// <summary>
        /// Play reward coins effect (only for reward coins)
        /// </summary>
        public void PlayRewardCoinsEffect()
        {
            transform.SetParent(null);
            coinPiece_0_Pos = coinPieces[0].transform.localPosition;
            coinPiece_0_Angles = coinPieces[0].transform.localEulerAngles;
            coinPiece_1_Pos = coinPieces[1].transform.localPosition;
            coinPiece_1_Angles = coinPieces[1].transform.localEulerAngles;
            rigid2D.bodyType = RigidbodyType2D.Dynamic;
            float upForce = Random.Range(60f, 80f);
            rigid2D.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
            rigid2D.AddTorque(3f, ForceMode2D.Impulse);
            StartCoroutine(CRRewardingCoinsEffect());
        }
        private IEnumerator CRRewardingCoinsEffect()
        {
            while (rigid2D.velocity.y > 0)
            {
                yield return null;
            }

            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.hitCoin);
            FindObjectOfType<HomeManager>().CreateHitCoinEffect(transform.position);
            spriteRenderer.enabled = false;

            foreach (Rigidbody2D o in coinPieces)
            {
                o.gameObject.SetActive(true);
                o.transform.SetParent(null);

                Vector2 dir = (o.transform.position - transform.position).normalized * 8f;
                o.AddForceAtPosition(dir, transform.position, ForceMode2D.Impulse);
                o.AddTorque(4f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(2f);
            foreach (Rigidbody2D o in coinPieces)
            {
                o.gameObject.SetActive(false);
                o.transform.SetParent(transform);
            }
            yield return null;
            coinPieces[0].transform.localPosition = coinPiece_0_Pos;
            coinPieces[0].transform.localEulerAngles = coinPiece_0_Angles;
            coinPieces[1].transform.localPosition = coinPiece_1_Pos;
            coinPieces[1].transform.localEulerAngles = coinPiece_1_Angles;
            spriteRenderer.enabled = true;
            gameObject.SetActive(false);
        }
    }
}
