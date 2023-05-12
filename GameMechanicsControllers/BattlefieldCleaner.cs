using System.Collections;
using UnityEngine;

namespace GameMechanicsControllers
{
    public class BattlefieldCleaner : MonoBehaviour
    {
        public GameObject enemySpawnManager;
        public GameObject playerShip;
    
        public bool NeedClean { get; set; }

        private const float TimeBeforeCleaning = 0.5f;
        private const float TimeToReturnPlayerShip = 2.5f;

        private EnemySpawnController _enemySpawnController;
        private Vector2 _playerGameStartPosition;
        private bool _isCleaning;
        private bool _isMoving;

        private void Start()
        {
            NeedClean = false;
            _enemySpawnController = enemySpawnManager.GetComponent<EnemySpawnController>();
            _playerGameStartPosition = playerShip.transform.position;
        }

        private void Update()
        {
            if (NeedClean)
            {
                NeedClean = false;
                StartCoroutine(Cleaning());
                if (playerShip != null)
                {
                    StartCoroutine(PlayerReturnToStartPosition());
                }
            }
        }
    
        private IEnumerator Cleaning()
        {
            if (_isCleaning) yield break;
            _isCleaning = true;
        
            yield return new WaitForSeconds(TimeBeforeCleaning);

            foreach (var enemy in _enemySpawnController.Enemies)
            {
                if (enemy != null)
                {
                    enemy.SetActive(false);
                }
            }
            var enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
            for (int i = 0; i < enemyBullets.Length; i++)
            {
                if (enemyBullets[i].activeSelf)
                {
                    enemyBullets[i].SetActive(false);
                }
            }
            _isCleaning = false;
        }

        private IEnumerator PlayerReturnToStartPosition()
        {
            if(_isMoving) yield break;
            _isMoving = true;
            
            yield return new WaitForSeconds(TimeBeforeCleaning * 2);
            var timeToReachTargetElapsed = 0.0f;
            var timeToReachTargetDuration = TimeToReturnPlayerShip;
            if (playerShip == null) yield break;
            Vector2 startPosition = playerShip.transform.position;
            Vector2 targetPosition = _playerGameStartPosition;
            while (timeToReachTargetElapsed < timeToReachTargetDuration)
            {
                playerShip.transform.position = Vector2.Lerp(startPosition, targetPosition, timeToReachTargetElapsed / timeToReachTargetDuration);
                timeToReachTargetElapsed += Time.deltaTime;
                yield return null;
            }
            playerShip.transform.position = targetPosition;
            
            _isMoving = false;
        }
    
    }
}
