using System;
using System.Collections;
using GameMechanicsControllers;
using UnityEngine;

namespace EnemyShipSystems
{
    public class EnemyShipController : MonoBehaviour
    {
        public int maxBodyHitsNumber;
        public GameObject bulletHit;
        public GameObject explosion;
        public float stepMovementTime;
        public float explosionTime;

        private const float TimeToDestroyShip = 0.5f;
        private const float TimeToDestroyExplosives = 0.2f;
        private const float DelayTimeBeforeMove = 0.8f;
    
        private int _hitsNumber;
        private bool _destroyed;
        private bool _move;
        private bool[] _movementPoints;
        private Vector2[] _movementPositionsInDifferences;
        private float[] _movementTiming;
        private bool _isMoving;
        private GameGuideController _gameController;
        
        public float TimerBeforeMove { get; set; }

        private int _hitsNumberOnStart;
        private bool _destroyedOnStart;
        private bool _moveOnStart;
        private bool _isMovingOnStart;
        
        private void Start()
        {
            _hitsNumber = _hitsNumberOnStart = 0;
            _destroyed = _destroyedOnStart = false;
            _move = _moveOnStart = true;
            _movementPoints = new bool[5];
            _movementPositionsInDifferences = new[]
            {
                new Vector2(-2f, -2.25f),
                new Vector2(3.5f, -1.3f),
                new Vector2(-3.3f, -1.5f),
                new Vector2(3f, -1.5f),
                new Vector2(-3.2f, -3.5f)
            };
            _movementTiming = new[]
            {
                stepMovementTime,
                stepMovementTime,
                stepMovementTime,
                stepMovementTime,
                stepMovementTime + 0.5f
            };
            _isMoving = _isMovingOnStart = false;
            TimerBeforeMove = 0.0f;
            _gameController = GameObject.Find("UI").gameObject.GetComponent<GameGuideController>();
        }

        private void Update()
        {
            TimerBeforeMove += Time.deltaTime;
            
            if (!_destroyed && _hitsNumber >= maxBodyHitsNumber)
            {
                _destroyed = true;
                _gameController.KilledEnemies += 1;
                StartCoroutine(ExplosionShow());
                Invoke(nameof(EnemyReset), TimeToDestroyShip);
            }

            if (_move && TimerBeforeMove > DelayTimeBeforeMove)
            {
                var indexOfMovement = -1;
                foreach (var movementPoint in _movementPoints)
                {
                    if (!movementPoint)
                    {
                        indexOfMovement = Array.IndexOf(_movementPoints, movementPoint);
                        break;
                    }
                }
                StartCoroutine(Move(indexOfMovement));
            }
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_destroyed && other.gameObject.CompareTag("PlayerBullet"))
            {
                var explosiveTransform = other.gameObject.transform;
                other.gameObject.SetActive(false);
                _hitsNumber += 1;
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
            var explosionOnEnemy = Instantiate(explosion, transform.position, transform.rotation);
            yield return new WaitForSeconds(explosionTime);
            if (explosionOnEnemy != null)
            {
                Destroy(explosionOnEnemy);
            }
        }
    
        private IEnumerator Move(int indexOfMovement)
        {
            if(_isMoving) yield break;
            _isMoving = true;

            var timeToReachTargetElapsed = 0.0f;
            var timeToReachTargetDuration = _movementTiming[indexOfMovement];
            Vector2 startPosition = transform.position;
            var targetPosition = new Vector2(startPosition.x + _movementPositionsInDifferences[indexOfMovement].x, startPosition.y + _movementPositionsInDifferences[indexOfMovement].y);
            while (timeToReachTargetElapsed < timeToReachTargetDuration)
            {
                transform.position = Vector2.Lerp(startPosition, targetPosition, timeToReachTargetElapsed / timeToReachTargetDuration);
                timeToReachTargetElapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPosition;
            _movementPoints[indexOfMovement] = true;

            if (indexOfMovement == 4 && _movementPoints[indexOfMovement])
            {
                _move = false;
            }

            _isMoving = false;
            
        }

        public void EnemyReset()
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
            _hitsNumber = _hitsNumberOnStart;
            _destroyed = _destroyedOnStart;
            _move = _moveOnStart;
            _isMoving = _isMovingOnStart;
            _movementPoints = new bool[5];
        }
    }
}
