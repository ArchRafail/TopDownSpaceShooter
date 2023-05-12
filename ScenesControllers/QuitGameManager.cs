using UnityEngine;

namespace ScenesControllers
{
    public class QuitGameManager : MonoBehaviour
    {
        public void QuitGame()
        {
            // EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
}