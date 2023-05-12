using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesControllers
{
    public class RestartSceneManager : MonoBehaviour
    {
        public GameObject uiWinText;
        public GameObject uiLosingText;
        public GameObject uiButtonSet;

        public void RestartScene ()
        {
            var allUiItems = new [] { uiWinText, uiLosingText, uiButtonSet };
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            foreach (var uiItem in allUiItems)
            {
                if (uiItem.activeSelf)
                {
                    uiItem.SetActive(false);
                }
            }
        }
    }
}
