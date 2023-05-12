using Common;
using GameMechanicsControllers;
using UnityEngine;

namespace PlayerShipSystems
{
    public class PlayerTurretsController : MonoBehaviour
    {
        public Transform fireStartTransform;
        public float fireRateInMinute;
        public GameObject gameLogic;

        private AudioSource _audioSource;
        private float _fireDelay;
        private float _charge;
        private bool _endGame;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _fireDelay = 60 / fireRateInMinute;
            _charge = 0.0f;
            _endGame = false;
        }

        private void Update()
        {
            _endGame = gameLogic.GetComponent<GameGuideController>().EndGame;
            if (!_endGame && Input.GetButton("Fire1"))
            {
                _charge += Time.deltaTime;

                if (_charge > _fireDelay)
                {
                    Fire();
                    _charge = 0.0f;
                }
            }
            else
            {
                _charge = 0.0f;
            }
        }

        private void Fire()
        {
            var bullet = GameObject.Find("PlayerAmmunitionSupply").GetComponent<ObjectPoller>().GetPooledObject();
            
            if (bullet == null) return;

            bullet.transform.position = fireStartTransform.position;
            bullet.transform.rotation = fireStartTransform.rotation;
            _audioSource.Play();
            bullet.SetActive(true);
        }
    }
}
