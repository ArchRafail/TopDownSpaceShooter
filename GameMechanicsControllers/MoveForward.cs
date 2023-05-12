using UnityEngine;

namespace GameMechanicsControllers
{
    public class MoveForward : MonoBehaviour
    {
        public float speed;

        private void Update()
        {
            transform.Translate(speed * Time.deltaTime * Vector2.up);    
        }
    }
}
