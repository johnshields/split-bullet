using UnityEngine;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody _rb;
        public float speed = 10;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _rb.velocity = transform.forward * speed;
            Invoke(nameof(BulletHit), 1);
        }

        private void BulletHit()
        {
            Destroy(gameObject);
        }
    }
}