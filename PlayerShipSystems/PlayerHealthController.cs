using System.Collections.Generic;
using UnityEngine;

namespace PlayerShipSystems
{
    public class PlayerHealthController : MonoBehaviour
    {
        public GameObject playerShip;
        public GameObject healthPlate;
        public GameObject healthCirclePrefab;
        public GameObject healthRingPrefab;

        public int HitCountsToProcess { get; set; }
        public int PlayerHealthRemained { get; set; }

        private GameObject[] _healthCircles;
        private GameObject[] _healthRings;
        private int _playerHealth;

        private void Start()
        {
            PlayerHealthRemained = _playerHealth = playerShip.GetComponent<PlayerShipController>().maxBodyHitsNumber;
            var healthCirclesList = new List<GameObject>();
            var healthRingsList = new List<GameObject>();
            for (int i = 0; i < _playerHealth; i++)
            {
                healthCirclesList.Add(Instantiate(healthCirclePrefab, new Vector3(0.2f * i, 0, 0), Quaternion.Euler(0, 0, 0), healthPlate.transform));
                var healthCirclePosition = healthCirclesList[i].transform.position;
                healthCirclesList[i].transform.position = new Vector2(healthCirclePosition.x + healthPlate.transform.position.x, healthCirclePosition.y + healthPlate.transform.position.y);
                healthRingsList.Add(Instantiate(healthRingPrefab, new Vector3(0.2f * i, 0, 0), Quaternion.Euler(0, 0, 0), healthPlate.transform));
                healthRingsList[i].transform.position = healthCirclesList[i].transform.position;
                healthRingsList[i].SetActive(false);
            }

            _healthCircles = healthCirclesList.ToArray();
            _healthRings = healthRingsList.ToArray();
        
            HitCountsToProcess = 0;
        }

        private void Update()
        {
            if (HitCountsToProcess > 0)
            {
                for (int i = _playerHealth - 1; i >= 0; i--)
                {
                    if (_healthCircles[i].activeSelf)
                    {
                        _healthCircles[i].SetActive(false);
                        _healthRings[i].SetActive(true);
                        HitCountsToProcess -= 1;
                        PlayerHealthRemained -= 1;
                        break;
                    }
                }
            }
        }
    }
}
