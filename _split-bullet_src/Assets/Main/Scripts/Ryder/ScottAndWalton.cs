using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class ScottAndWalton : MonoBehaviour
    {
        private InputControls _controls;
        private GameObject _pistol, _crosshair, _dummy;
        private Camera _mainCam, _barrel;
        private bool _fired;
        private int _chamber = 6;

        private void Awake()
        {
            _controls = new InputControls();
        }

        private void Start()
        {
            _pistol = GameObject.FindGameObjectWithTag("Pistol");
            _crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            _barrel = GameObject.FindGameObjectWithTag("Barrel").GetComponent<Camera>();
            _dummy = GameObject.FindGameObjectWithTag("Dummy");
            _mainCam = Camera.main;
            EquipPistol(false);
        }

        private void Update()
        {
            if (_chamber == 0)
            {
                
            }
        }

        private void OnEnable()
        {
            _controls.Profiler.FirePistol.started += FirePistol;
            _controls.Profiler.Enable();
        }

        private void OnDisable()
        {
            _controls.Profiler.Jump.started -= FirePistol;
            _controls.Profiler.Disable();
        }

        public void EquipPistol(bool equip)
        {
            _pistol.SetActive(equip);
            _crosshair.SetActive(equip);
        }

        private void FirePistol(InputAction.CallbackContext obj)
        {
            if (GetComponent<RyderProfiler>().highProfile && !_fired && _chamber != 0)
            {
                _fired = true;
                _chamber -= 1;
                GetComponent<RyderSFX>().PistolFire(.6f);
                
                var forward = _mainCam.transform.forward;
                forward.y = 0;
                forward.Normalize();
                Aim(forward);

                var center = new Vector2(Screen.width / 2, Screen.height / 2);
                var ray = _barrel.ScreenPointToRay(center);

                Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue, 2);

                if (Physics.Raycast(ray, out var hit))
                {
                    print($"Pistol hit: {hit.transform.name}");
                    if (hit.transform.CompareTag("Dummy"))
                    {
                        _dummy.GetComponentInParent<Animator>().SetTrigger($"Hit"); 
                        _dummy.GetComponentInParent<ParticleSystem>().Play();
                    }
                }
                Invoke(nameof(Fired), .5f);
            }
            else
                print($"Ryder is in not in HighProfile: {GetComponent<RyderProfiler>().highProfile}");
        }

        private void Aim(Vector3 target)
        {
            transform.LookAt(transform.position + target);
        }

        private void Fired()
        {
            _fired = false;
        }
    }
}