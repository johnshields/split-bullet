using Managers;
using Player;
using UnityEngine;

public class RyderSFX : MonoBehaviour
{
    private InputControls _controls;
    private AudioSource _audio, _stepsAudio;
    private GameObject _randoAudio;

    private void Awake()
    {
        _controls = new InputControls();
    }

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _randoAudio = GameObject.FindGameObjectWithTag("RandoAudio");
        _stepsAudio = GameObject.FindGameObjectWithTag("Footsteps").GetComponent<AudioSource>();
        PistolFire(0);
        PistolChamberReload(0);
        PistolDryFire(0);
        Steps(0);
        Jump(0);
    }

    private void OnEnable()
    {
        _controls.Profiler.Enable();
    }

    private void OnDisable()
    {
        _controls.Profiler.Disable();
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

    private void Steps(float vol)
    {
        if (_controls.Profiler.Movement.IsPressed() && !_controls.Profiler.Jump.IsPressed() &&
            GetComponent<RyderProfiler>().grounded)
        {
            _stepsAudio.Stop();
            _stepsAudio.PlayOneShot(_randoAudio.GetComponent<RandoAudio>().GetRandomClip("Ryder/Steps"), vol);
        }
    }

    private void Jump(float vol)
    {
        _audio.Stop();
        _audio.PlayOneShot(_randoAudio.GetComponent<RandoAudio>().GetRandomClip("Ryder/Grunts"), vol);
    }
}