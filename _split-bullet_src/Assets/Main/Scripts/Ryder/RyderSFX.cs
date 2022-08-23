using Managers;
using UnityEngine;

public class RyderSFX : MonoBehaviour
{
    private AudioSource _audio;
    private GameObject _randoAudio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _randoAudio = GameObject.FindGameObjectWithTag("RandoAudio");
        PistolFire(0);
        PistolChamberReload(0);
        PistolDryFire(0);
        BulletsPickup(0);
    }
    
    public void PistolFire(float vol)
    {
        _audio.Stop();
        _audio.PlayOneShot(_randoAudio.GetComponent<RandoAudio>().GetRandomClip("ScottAndWalton/Fire"), vol);
    }
    
    public void PistolChamberReload(float vol)
    {
        _audio.Stop();
        _audio.PlayOneShot(_randoAudio.GetComponent<RandoAudio>().GetRandomClip("ScottAndWalton/Reload"), vol);
    }
    
    public void PistolDryFire(float vol)
    {
        _audio.Stop();
        _audio.PlayOneShot(_randoAudio.GetComponent<RandoAudio>().GetRandomClip("ScottAndWalton/DryFire"), vol);
    }
    
    public void BulletsPickup(float vol)
    {
        _audio.Stop();
        _audio.PlayOneShot(_randoAudio.GetComponent<RandoAudio>().GetRandomClip("ScottAndWalton/BulletsPickup"), vol);
    }
}
