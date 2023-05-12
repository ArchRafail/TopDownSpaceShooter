using System.Collections;
using PlayerShipSystems;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameMechanicsControllers
{
    public class GameGuideController : MonoBehaviour
    {
        public GameObject enemySpawnManager;
        public GameObject cleanManager;
        public GameObject playerHealthManager;
    
        public int totalEnemiesQuantity;
        public int killedEnemiesToWin;
    
        public GameObject uiWinText;
        public GameObject uiLosingText;
        public GameObject uiButtonSet;

        private const float DelayForUiShowing = 1f;
    
        private EnemySpawnController _spawnerEnemies;
        private BattlefieldCleaner _battlefieldCleaner;
        private PlayerHealthController _playerHealthController;
        private bool _isShowing;
        private bool _requestToSpawnerEnemiesSent;

        public int KilledEnemies { get; set; }
        public bool EndGame { get; private set; }
        public bool EmptyPlayerHealth { get; set; }

        private void Start()
        {
            _spawnerEnemies = enemySpawnManager.GetComponent<EnemySpawnController>();
            _battlefieldCleaner = cleanManager.GetComponent<BattlefieldCleaner>();
            _playerHealthController = playerHealthManager.GetComponent<PlayerHealthController>();

            totalEnemiesQuantity = totalEnemiesQuantity < 1 ? 1 : totalEnemiesQuantity;
            killedEnemiesToWin = killedEnemiesToWin > totalEnemiesQuantity ? totalEnemiesQuantity : killedEnemiesToWin;
            
            KilledEnemies = 0;
            EndGame = false;
            EmptyPlayerHealth = false;
            _requestToSpawnerEnemiesSent = false;
        
            uiWinText.SetActive(false);
            uiLosingText.SetActive(false);
            uiButtonSet.SetActive(false);
        }

        private void Update()
        {
            if (totalEnemiesQuantity > 0)
            {
                _spawnerEnemies.SpawnEnemy = true;
            }

            if (!EndGame && KilledEnemies == killedEnemiesToWin)
            {
                _spawnerEnemies.SpawnEnemy = false;
                EndGame = true;
                totalEnemiesQuantity = 0;
                _battlefieldCleaner.NeedClean = true;
                StartCoroutine(EndGameUiShow(uiWinText));
            }

            if (!EndGame && totalEnemiesQuantity == 0)
            {
                if (!_requestToSpawnerEnemiesSent)
                {
                    _spawnerEnemies.CheckIfAllEnemySpawned = true;
                    _requestToSpawnerEnemiesSent = true;
                }
                if (!_spawnerEnemies.CheckIfAllEnemySpawned && _spawnerEnemies.AllEnemySpawned)
                {
                    EndGame = true;
                    _battlefieldCleaner.NeedClean = true;
                    StartCoroutine(EndGameUiShow(uiLosingText));
                }
            }
        
            if (!EndGame && _playerHealthController.PlayerHealthRemained == 0)
            {
                _spawnerEnemies.SpawnEnemy = false;
                EndGame = true;
                totalEnemiesQuantity = 0;
                _battlefieldCleaner.NeedClean = true;
                StartCoroutine(EndGameUiShow(uiLosingText));
            }

            if (Input.GetButton("Cancel"))
            {
                EndGame = true;
                Invoke(nameof(ForceBackToMenu), 0.3f);
            }

        }

        private IEnumerator EndGameUiShow(GameObject uiText)
        {
            if (_isShowing) yield break;
            _isShowing = true;

            yield return new WaitForSeconds(DelayForUiShowing);
            uiText.SetActive(true);
            uiButtonSet.SetActive(true);
            gameObject.GetComponent<AudioSource>().Stop();
        
            _isShowing = true;
        }

        private void ForceBackToMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
