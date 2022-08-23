using UnityEngine;

namespace Characters
{
    public class DummyProfiler : MonoBehaviour
    {
        public int dummyHealth;
        public bool knight;
        private const int maxHealth = 6;
        private bool _shot;
        private GameObject _dummy;

        private void Start()
        {
            dummyHealth = maxHealth;
        }

        private void Update()
        {
            if(_shot)
                DummyDead();
        }

        public void DummyHit(GameObject dummy)
        {
            _dummy = dummy;
            _shot = true;
            if (knight)
            {
                dummy.GetComponentInParent<Animator>().SetTrigger($"Hit");
                dummy.GetComponentInParent<ParticleSystem>().Play();
                dummy.GetComponentInParent<DummyProfiler>().dummyHealth -= 1;
            }
            else
            {
                dummy.GetComponent<Animator>().SetTrigger($"Hit");
                dummy.GetComponent<ParticleSystem>().Play();
                dummy.GetComponent<DummyProfiler>().dummyHealth -= 1;
            }
        }

        private void DummyDead()
        {
            if (!knight) return;
            if (_dummy.GetComponentInParent<DummyProfiler>().dummyHealth <= 1)
                _dummy.GetComponentInParent<Animator>().SetTrigger($"Dead");

            if (knight) return;
            if (_dummy.GetComponent<DummyProfiler>().dummyHealth <= 1)
                _dummy.GetComponent<Animator>().SetTrigger($"Dead");
        }
    }
}