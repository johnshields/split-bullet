using UnityEngine;

namespace Characters
{
    public class DummyProfiler : MonoBehaviour
    {
        public int dummyHealth;
        public bool knight;
        private const int maxHealth = 6;

        private void Start()
        {
            dummyHealth = maxHealth;
        }

        public void DummyHit(GameObject dummy)
        {
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

        public void DummyDead(GameObject dummy)
        {
            if (dummy.GetComponentInParent<DummyProfiler>().dummyHealth <= 1 && knight)
                dummy.GetComponentInParent<Animator>().SetTrigger($"Dead");

            if (knight) return;
            if (dummy.GetComponent<DummyProfiler>().dummyHealth <= 1)
                dummy.GetComponent<Animator>().SetTrigger($"Dead");
        }
    }
}