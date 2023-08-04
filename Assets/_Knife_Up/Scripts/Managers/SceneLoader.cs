using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace OnefallGames
{
    public class SceneLoader : MonoBehaviour
    {

        private static string targetScene = string.Empty;

        private void Start()
        {
            StartCoroutine(LoadingScene());
            ViewManager.Instance.OnShowView(ViewType.LOADING_VIEW);
        }
        private IEnumerator LoadingScene()
        {
            AsyncOperation asyn = SceneManager.LoadSceneAsync(targetScene);
            while (!asyn.isDone)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        /// <summary>
        /// Set target scene.
        /// </summary>
        /// <param name="sceneName"></param>
        public static void SetTargetScene(string sceneName)
        {
            targetScene = sceneName;
        }
    }
}
