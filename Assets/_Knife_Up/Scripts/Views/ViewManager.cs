using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OnefallGames
{
    public class ViewManager : MonoBehaviour
    {

        public static ViewManager Instance { get; private set; }

        [SerializeField] private HomeViewController homeViewControl = null;
        [SerializeField] private LoadingViewController loadingViewControl = null;
        [SerializeField] private IngameViewController ingameViewControl = null;
        [SerializeField] private CharacterViewController characterViewControl = null;

        public HomeViewController HomeViewController { get { return homeViewControl; } }
        public LoadingViewController LoadingViewController { get { return loadingViewControl; } }
        public IngameViewController IngameViewController { get { return ingameViewControl; } }
        public CharacterViewController CharacterViewController { get { return characterViewControl; } }
        public ViewType ActiveViewType { private set; get; }


        private void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }


        private IEnumerator CRMovingRect(RectTransform rect, Vector2 startPos, Vector2 endPos, float movingTime)
        {
            Vector2 currentPos = new Vector2(Mathf.RoundToInt(rect.anchoredPosition.x), Mathf.RoundToInt(rect.anchoredPosition.y));
            if (!currentPos.Equals(endPos))
            {
                rect.anchoredPosition = startPos;
                float t = 0;
                while (t < movingTime)
                {
                    t += Time.deltaTime;
                    float factor = EasyType.MatchedLerpType(LerpType.EaseInOutQuart, t / movingTime);
                    rect.anchoredPosition = Vector2.Lerp(startPos, endPos, factor);
                    yield return null;
                }
            }
        }

        private IEnumerator CRScalingRect(RectTransform rect, Vector2 startScale, Vector2 endScale, float scalingTime)
        {
            rect.localScale = startScale;
            float t = 0;
            while (t < scalingTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.EaseInOutQuart, t / scalingTime);
                rect.localScale = Vector2.Lerp(startScale, endScale, factor);
                yield return null;
            }
        }

        private IEnumerator CRFadingImage(Image image, float startAlpha, float endAlpha, float fadingTime)
        {
            Color startColor = image.color;
            startColor.a = startAlpha;
            image.color = startColor;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, endAlpha);
            float t = 0;
            while (t < fadingTime)
            {
                t += Time.deltaTime;
                float factor = EasyType.MatchedLerpType(LerpType.EaseInOutQuart, t / fadingTime);
                image.color = Color.Lerp(startColor, endColor, factor);
                yield return null;
            }
        }


        private IEnumerator CRLoadingScene(string sceneName, float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneLoader.SetTargetScene(sceneName);
            SceneManager.LoadScene("Loading");
        }

        /////////////////////////////////////////////////////////////////////////////////Public functions


        /// <summary>
        /// Play the sound effect when you click a button.
        /// </summary>
        public void PlayClickButtonSound()
        {
            ServicesManager.Instance.SoundManager.PlaySound(ServicesManager.Instance.SoundManager.button);
        }


        /// <summary>
        /// Move the given rect transform from startPos to endPos with movingTime,
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="movingTime"></param>
        public void MoveRect(RectTransform rect, Vector2 startPos, Vector2 endPos, float movingTime)
        {
            StartCoroutine(CRMovingRect(rect, startPos, endPos, movingTime));
        }


        /// <summary>
        /// Scale the given rect from startScale to endScale with scalingTime.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="startScale"></param>
        /// <param name="endScale"></param>
        /// <param name="scalingTime"></param>
        public void ScaleRect(RectTransform rect, Vector2 startScale, Vector2 endScale, float scalingTime)
        {
            StartCoroutine(CRScalingRect(rect, startScale, endScale, scalingTime));
        }

        /// <summary>
        /// Load Loading scene with a delay time then use LoadSceneAsync to load the given scene.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="delay"></param>
        public void LoadScene(string sceneName, float delay)
        {
            StartCoroutine(CRLoadingScene(sceneName, delay));
        }





        /// <summary>
        /// Show the correct view base on ViewType.
        /// </summary>
        /// <param name="sceneName"></param>
        public void OnShowView(ViewType viewType)
        {
            //Load HomeView
            if (viewType == ViewType.HOME_VIEW)
            {
                homeViewControl.gameObject.SetActive(true);
                homeViewControl.OnShow();
                ActiveViewType = ViewType.HOME_VIEW;

                //Hide all other views
                loadingViewControl.gameObject.SetActive(false);
                ingameViewControl.gameObject.SetActive(false);
                characterViewControl.gameObject.SetActive(false);
            }
            else if (viewType == ViewType.INGAME_VIEW)
            {
                ingameViewControl.gameObject.SetActive(true);
                ingameViewControl.OnShow();
                ActiveViewType = ViewType.INGAME_VIEW;

                //Hide all other views
                loadingViewControl.gameObject.SetActive(false);
                homeViewControl.gameObject.SetActive(false);
                characterViewControl.gameObject.SetActive(false);
            }
            else if (viewType == ViewType.CHARACTER_VIEW)
            {
                characterViewControl.gameObject.SetActive(true);
                characterViewControl.OnShow();
                ActiveViewType = ViewType.CHARACTER_VIEW;

                //Hide all other views
                loadingViewControl.gameObject.SetActive(false);
                homeViewControl.gameObject.SetActive(false);
                ingameViewControl.gameObject.SetActive(false);
            }
            else if (viewType == ViewType.LOADING_VIEW)
            {
                loadingViewControl.gameObject.SetActive(true);
                loadingViewControl.OnShow();
                ActiveViewType = ViewType.LOADING_VIEW;

                //Hide all other views
                ingameViewControl.gameObject.SetActive(false);
                homeViewControl.gameObject.SetActive(false);
                characterViewControl.gameObject.SetActive(false);
            }
        }
    }
}
