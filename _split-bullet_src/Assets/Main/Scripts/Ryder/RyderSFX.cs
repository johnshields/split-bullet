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
}
