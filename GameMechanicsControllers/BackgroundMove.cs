using UnityEngine;

namespace GameMechanicsControllers
{
    public class BackgroundMove : MonoBehaviour
    {
        public float speed;

        private void Update()
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
        }
    }
}
