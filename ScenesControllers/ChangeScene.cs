using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesControllers
{
    public class ChangeScene : MonoBehaviour
    {
        public void MoveToScene(int sceneId)
        {
            SceneManager.LoadScene(sceneId);
        }
    }
}
