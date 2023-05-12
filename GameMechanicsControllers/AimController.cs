using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace GameMechanicsControllers
{
    public class AimController : MonoBehaviour
    {
        public float rotateSpeed;
        public GameObject gameLogic;

        private bool _endGame;

        private void Start()
        {
            _endGame = false;
        }

        private void Update()
        {
            _endGame = gameLogic.GetComponent<GameGuideController>().EndGame;
            if (_endGame) return;
        
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        }

    }
}
