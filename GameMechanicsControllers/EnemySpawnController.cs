using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using EnemyShipSystems;
using UnityEngine;

namespace GameMechanicsControllers
{
    public class EnemySpawnController : MonoBehaviour
    {
        public GameObject uiGame;
        public GameObject enemyShip;
        public int enemiesZones;
        public float spawnTime;
        public float exhaustOffset;
    
        private const float DefaultOffset = 0.05f;

        private GameGuideController _gameController;
        private Vector2 _cameraMax;
        private Vector2[] _spawnPoints;
        private float _halfWidth;
        private float _halfHeight;
        private float _widthOfEnemyZone;
        private bool[] _occupiedZones;
        private bool _isSpawning;
    
        public bool SpawnEnemy { get; set; }
        public bool AllEnemySpawned { get; private set; }
        public bool CheckIfAllEnemySpawned { get; set; }
        public GameObject[] Enemies { get; set; }

        private void Start()
        {
            _gameController = uiGame.GetComponent<GameGuideController>();
        
            var sprite = enemyShip.GetComponent<SpriteRenderer>().sprite;
            _halfWidth = sprite.bounds.size.x / 2 * enemyShip.transform.localScale.x;
            _halfHeight = sprite.bounds.size.y / 2 * enemyShip.transform.localScale.y;
            _cameraMax = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
            _cameraMax.x -= _halfWidth + DefaultOffset;
            var spawnPointsY = _cameraMax.y - _halfHeight - exhaustOffset;

            _widthOfEnemyZone = _cameraMax.x * 2 / enemiesZones;
            _widthOfEnemyZone = (float)Math.Round(_widthOfEnemyZone, 1);
        
            var spawnPointsList = new List<Vector2>();
        
            var enemiesZoneQuantityBeforeZero = enemiesZones / 2;
            if (enemiesZones % 2 == 0)
            {
                for (int i = 0; i < enemiesZoneQuantityBeforeZero; i++)
                {
                    spawnPointsList.Add(new Vector2(0 - (_widthOfEnemyZone / 2 + _widthOfEnemyZone * (enemiesZoneQuantityBeforeZero - i - 1)), spawnPointsY));
                }
                for (int i = 0; i < enemiesZoneQuantityBeforeZero; i++)
                {
                    spawnPointsList.Add(new Vector2(_widthOfEnemyZone / 2 + _widthOfEnemyZone * i, spawnPointsY));
                }
            }
            else
            {
                for (int i = 0; i < enemiesZoneQuantityBeforeZero; i++)
                {
                    spawnPointsList.Add(new Vector2(0 - (_widthOfEnemyZone * (enemiesZoneQuantityBeforeZero - i)), spawnPointsY));
                }
                spawnPointsList.Add(new Vector2(0, spawnPointsY));
                for (int i = 0; i < enemiesZoneQuantityBeforeZero; i++)
                {
                    spawnPointsList.Add(new Vector2(_widthOfEnemyZone * (i + 1), spawnPointsY));
                }
            }
        
            _spawnPoints = spawnPointsList.ToArray();
            _occupiedZones = new bool[enemiesZones];
            Enemies = new GameObject[enemiesZones];
            SpawnEnemy = false;
            AllEnemySpawned = false;
            CheckIfAllEnemySpawned = false;
        }

        private void Update()
        {
            var indexesOfFreeZones = new List<int>();
            foreach (var occupiedZone in _occupiedZones)
            {
                if (!occupiedZone)
                {
                    indexesOfFreeZones.Add(Array.IndexOf(_occupiedZones, occupiedZone));
                }
            }
            if (indexesOfFreeZones.Count != 0 && SpawnEnemy)
            {
                SpawnEnemy = false;
                var spawnPointerZone = indexesOfFreeZones[UnityEngine.Random.Range(0, indexesOfFreeZones.Count)];
                StartCoroutine(SpawnEnemyShip(spawnPointerZone));
            }
            for (int i = 0; i < Enemies.Length; i++)
            {
                if (Enemies[i]!= null && !Enemies[i].activeSelf)
                {
                    _occupiedZones[i] = false;
                    Enemies[i] = null;
                }
            }
            if (CheckIfAllEnemySpawned)
            {
                var occupiedZonesEmpty = true;
                foreach (var occupiedZone in _occupiedZones)
                {
                    if (!occupiedZone) continue;
                    occupiedZonesEmpty = false;
                    break;
                }
                if (occupiedZonesEmpty)
                {
                    AllEnemySpawned = true;
                    foreach (var enemy in Enemies)
                    {
                        if (enemy == null) continue;
                        AllEnemySpawned = false;
                        break;
                    }
                }
                if (AllEnemySpawned)
                {
                    CheckIfAllEnemySpawned = false;
                }
            }
        }
    
        private IEnumerator SpawnEnemyShip(int index)
        {
            if (_isSpawning) yield break;
            _isSpawning = true;
        
            yield return new WaitForSeconds(spawnTime);
            
            var enemy = GetComponent<ObjectPoller>().GetPooledObject();

            if (enemy == null)
            {
                _isSpawning = false;
                yield break;
            }
            _occupiedZones[index] = true;
            enemy.transform.position = _spawnPoints[index];
            enemy.transform.rotation = Quaternion.Euler(0, 0, 180);
            var enemyShipController = enemy.GetComponent<EnemyShipController>();
            enemyShipController.EnemyReset();
            var enemyLeftBlasterController = enemy.transform.Find("Blasters").gameObject.transform.Find("BlasterLeft").gameObject.GetComponent<EnemyBlasterController>();
            var enemyRightBlasterController = enemy.transform.Find("Blasters").gameObject.transform.Find("BlasterRight").gameObject.GetComponent<EnemyBlasterController>();
            enemyLeftBlasterController.AllowFire = true;
            enemyRightBlasterController.AllowFire = true;
            enemy.SetActive(true);
            enemyShipController.TimerBeforeMove = 0.0f;
            enemyLeftBlasterController.PassedDelayInFire = 0.0f;
            enemyRightBlasterController.PassedDelayInFire = 0.0f;
            Enemies[index] = enemy;
            _gameController.totalEnemiesQuantity -= 1;
        
            _isSpawning = false;
        }
    }
}
