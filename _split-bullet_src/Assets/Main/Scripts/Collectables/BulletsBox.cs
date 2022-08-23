using Managers;
using Player;
using UnityEngine;

namespace Collectables
{
    public class BulletsBox : MonoBehaviour
    {
        private AudioSource _audio;
        private GameObject _randoAudio;

        private void Start()
        {
            _audio = GetComponent<AudioSource>();
            _randoAudio = GameObject.FindGameObjectWithTag("RandoAudio");
        }
        
        private void OnTriggerEnter(Collider obj)
        {
            if (obj.CompareTag("Player") &&
                obj.GetComponent<ScottAndWalton>().bullets < obj.GetComponent<ScottAndWalton>().maxBullets)
            {
                obj.GetComponent<ScottAndWalton>().fillUpBullets = true;
                obj.GetComponent<ScottAndWalton>().FillUpBullets(6);
                BulletsPickup(.6f);
            }
        }

        private void OnTriggerExit(Collider obj)
        {
            if (obj.CompareTag("Player"))
                obj.GetComponent<ScottAndWalton>().fillUpBullets = false;
        }
        
        private void BulletsPickup(float vol)
        {
            _audio.Stop();
            _audio.PlayOneShot(_randoAudio.GetComponent<RandoAudio>().GetRandomClip("ScottAndWalton/BulletsPickup"), vol);
        }
    }
}