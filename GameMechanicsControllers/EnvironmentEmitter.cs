using UnityEngine;

namespace GameMechanicsControllers
{
    public class EnvironmentEmitter : MonoBehaviour
    {
        public GameObject backgroundPrefab;
        public Transform backgroundWrapper;

        private const float BackgroundSwapGap = 1f;
        private GameObject _currentBackground;
        private GameObject _previousBackground;
        private float _cameraMinY;
        private float _cameraMaxY;
        private float _backgroundHeight;

        private void Start()
        {
            _currentBackground = Instantiate(backgroundPrefab, backgroundWrapper);
            _cameraMinY = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y;
            _cameraMaxY = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).y;

            _backgroundHeight = backgroundPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.y *
                                backgroundPrefab.transform.localScale.y;
        }

        private void Update()
        {
            ProcessBackground();
        }

        private void ProcessBackground()
        {
            var backgroundTopY = _currentBackground.transform.position.y + _backgroundHeight;

            if (_cameraMaxY + BackgroundSwapGap >= backgroundTopY)
            {
                _previousBackground = _currentBackground;
                _currentBackground = Instantiate(backgroundPrefab,
                    new Vector2(backgroundPrefab.transform.position.x, backgroundTopY),
                    backgroundPrefab.transform.rotation, backgroundWrapper);
            }

            if (_previousBackground != null)
            {
                var previousBackgroundTopY = _previousBackground.transform.position.y + _backgroundHeight;

                if (_cameraMinY - BackgroundSwapGap >= previousBackgroundTopY)
                {
                    Destroy(_previousBackground);
                    _previousBackground = null;
                }
            }
        }
    }
}
