using System.Collections;
using GameMechanicsControllers;
using UnityEngine;

namespace PlayerShipSystems
{
    public class PlayerShipController : MonoBehaviour
    {
        public GameObject playerHealthManager;
        public float speed;
        public float exhaustOffset;
        public int maxBodyHitsNumber;
        public GameObject bulletHit;
        public GameObject explosion;
        public GameObject uiGame;

        private const float DefaultOffset = 0.05f;
        private const float TimeToDestroyShip = 0.5f;
        private const float TimeToDestroyExplosives = 0.2f;

        private PlayerHealthController _playerHealthController;
        private Vector2 _cameraMin;
        private Vector2 _cameraMax;
        private float _halfWidth;
        private float _halfHeight;
        private int _hitsNumber;
        private bool _destroyed;
        private bool _endGame;

        private void Start()
        {
            var sprite = GetComponent<SpriteRenderer>().sprite;
            _halfWidth = sprite.bounds.size.x / 2 * transform.localScale.x;
            _halfHeight = sprite.bounds.size.y / 2 * transform.localScale.y;
        
            _cameraMin = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            _cameraMax = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

            _cameraMin.x += _halfWidth + DefaultOffset;
            _cameraMax.x -= _halfWidth + DefaultOffset;
            _cameraMin.y += _halfHeight + exhaustOffset;
            _cameraMax.y -= _halfHeight + DefaultOffset;
        
            _hitsNumber = 0;
            _destroyed = false;
            _endGame = false;

            _playerHealthController = playerHealthManager.GetComponent<PlayerHealthController>();
        }

        private void Update()
        {
            _endGame = uiGame.GetComponent<GameGuideController>().EndGame;
        
            if (!_destroyed && _hitsNumber >= maxBodyHitsNumber)
            {
                _destroyed = true;
                StartCoroutine(ExplosionShow());
                Destroy(gameObject, TimeToDestroyShip);
            }
        
            if (!_endGame)
            {
                Move();
            }
        }


        private void Move()
        {
            var x = Input.GetAxisRaw("Horizontal");
            var y = Input.GetAxisRaw("Vertical");

            var userMadeInput = x != 0 || y != 0;

            if (userMadeInput)
            {
                Vector2 position = transform.position;
                Vector2 direction = new Vector2(x, y).normalized;

                position += speed * Time.deltaTime * direction;

                position.x = Mathf.Clamp(position.x, _cameraMin.x, _cameraMax.x);
                position.y = Mathf.Clamp(position.y, _cameraMin.y, _cameraMax.y);
            
                transform.position = position;
            }
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_destroyed && other.gameObject.CompareTag("EnemyBullet"))
            {
                var explosiveTransform = other.gameObject.transform;
                other.gameObject.SetActive(false);
                _hitsNumber += 1;
                _playerHealthController.HitCountsToProcess += 1;
                var bulletHitObject = Instantiate(bulletHit, explosiveTransform.position, explosiveTransform.rotation);
                StartCoroutine(DestroyTargetHit(bulletHitObject));
            }
        }
    
        private IEnumerator DestroyTargetHit(GameObject bulletHitObject)
        {
            if (bulletHitObject != null)
            {
                yield return new WaitForSeconds(TimeToDestroyExplosives);
                Destroy(bulletHitObject);
            }
        }
    
        private IEnumerator ExplosionShow()
        {
            yield return new WaitForSeconds(TimeToDestroyShip - 0.1f);
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }
}
