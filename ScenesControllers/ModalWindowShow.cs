using UnityEngine;

namespace ScenesControllers
{
    public class ModalWindowShow : MonoBehaviour
    {
        public GameObject modalWindow;

        public void ChangeVisibility()
        {
            modalWindow.SetActive(!modalWindow.activeSelf); 
        }

    }
}
