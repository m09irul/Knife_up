using UnityEngine;
using UnityEngine.UI;


namespace OnefallGames
{
    public class LeaderboardItemController : MonoBehaviour
    {
        [SerializeField] private Text usernameTxt = null;
        [SerializeField] private Text levelTxt = null;


        public void OnSetup(int indexRank, PlayerLeaderboardData data)
        {
            transform.localScale = Vector3.one;
            usernameTxt.text = indexRank.ToString() + "." + " " + data.Name;
            levelTxt.text = "Level: " + data.Level.ToString();

        }
    }
}
