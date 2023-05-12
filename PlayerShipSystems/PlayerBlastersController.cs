using Common;
using GameMechanicsControllers;
using UnityEngine;

namespace PlayerShipSystems
{
    public class PlayerBlastersController : MonoBehaviour
    {
        public KeyCode fireButton;
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
            _charge += Time.deltaTime;
        
            if (!_endGame && Input.GetKeyDown(fireButton))
            {
                if (_charge > _fireDelay)
                {
                    InvokeRepeating(nameof(Fire), 0, _fireDelay);
                }
            }

            if (Input.GetKeyUp(fireButton))
            {
                CancelInvoke(nameof(Fire));
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
