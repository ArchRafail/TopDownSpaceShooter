using UnityEngine;

namespace Common
{
    public class DestroyerPollerObject : MonoBehaviour
    {
        public float additionalUpperComponentHeight = 0.0f;
        public float additionalLowerComponentHeight = 0.0f;
        public float additionalLeftComponentWidth = 0.0f;
        public float additionalRightComponentWidth = 0.0f;
        public float defaultOffsetAllDirection;

        private float _halfHeight;
        private float _halfWidth;
        private Vector2 _min;
        private Vector2 _max;
        
        private void Start()
        {
            var sprite = GetComponent<SpriteRenderer>().sprite;
            _halfWidth = sprite.bounds.size.x / 2 * transform.localScale.x;
            _halfHeight = sprite.bounds.size.y / 2 * transform.localScale.y;
        
            _min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            _max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

            _min.x -= _halfWidth + additionalRightComponentWidth + defaultOffsetAllDirection;
            _max.x += _halfWidth + additionalLeftComponentWidth + defaultOffsetAllDirection;
            _min.y -= _halfHeight + additionalUpperComponentHeight + defaultOffsetAllDirection;
            _max.y += _halfHeight + additionalLowerComponentHeight + defaultOffsetAllDirection;
        }

        private void Update()
        {
            if (transform.position.x < _min.x || transform.position.x > _max.x
                || transform.position.y < _min.y || transform.position.y > _max.y)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
