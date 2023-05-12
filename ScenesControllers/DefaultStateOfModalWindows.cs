using UnityEngine;

namespace ScenesControllers
{
    public class DefaultStateOfModalWindows : MonoBehaviour
    {
        public GameObject controlElementsWindow; 
        public GameObject creditsWindow; 
            
        private void Start()
        {
            controlElementsWindow.SetActive(false);
            creditsWindow.SetActive(false);
        }
    }
}
