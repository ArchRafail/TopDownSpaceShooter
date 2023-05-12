using System.Collections;
using Common;
using UnityEngine;

namespace EnemyShipSystems
{
    public class EnemyBlasterController : MonoBehaviour
    {
        public Transform firePositionTransform;
        public float fireRateInMinute;

        public float PassedDelayInFire { get; set; }
        public bool AllowFire { get; set; }

        private AudioSource _audioSource;
        private float _fireDelay;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _fireDelay = 60 / fireRateInMinute;
            AllowFire = true;
            PassedDelayInFire = 0.0f;
        }

        private void Update()
        {
            PassedDelayInFire += Time.deltaTime;
            if (AllowFire && PassedDelayInFire > _fireDelay)
            {
                StartCoroutine(Shoot());
            }
        }

        private IEnumerator Shoot()
        {
            AllowFire = false;
            
            var bullet = GameObject.Find("EnemyAmmunitionSupplyManager").GetComponent<ObjectPoller>().GetPooledObject();

            if (bullet == null)
            {
                yield return new WaitForSeconds(_fireDelay);
                AllowFire = true;
                yield break;
            };

            bullet.transform.position = firePositionTransform.position;
            bullet.transform.rotation = firePositionTransform.rotation;
            _audioSource.Play();
            bullet.SetActive(true);
            yield return new WaitForSeconds(_fireDelay);
            AllowFire = true;
        }
    
    }
}
