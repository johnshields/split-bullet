using System;
using Characters;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class ScottAndWalton : MonoBehaviour
    {
        private InputControls _controls;
        private GameObject _pistol, _crosshair, _dummy;
        private Camera _mainCam, _barrel;
        private bool _fired, _reloadingChamber;
        private int _chamber = 7;

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
            ReloadingChamber();
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
                Chamber();
                GetComponent<RyderSFX>().PistolFire(.6f);

                var forward = _mainCam.transform.forward;
                forward.y = 0;
                forward.Normalize();
                Aim(forward);

                var center = new Vector2(Screen.width / 2, Screen.height / 2);
                var ray = _barrel.ScreenPointToRay(center);

                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2);

                if (Physics.Raycast(ray, out var hit))
                {
                    print($"Pistol hit: {hit.transform.name}");
                    // testing
                    if (hit.transform.CompareTag("Dummy"))
                    {
                        _dummy.GetComponentInParent<DummyProfiler>().DummyHit(hit.transform.gameObject);
                        _dummy.GetComponentInParent<DummyProfiler>().DummyDead(hit.transform.gameObject);
                    }
                }

                Invoke(nameof(Fired), .5f);
            }
            else
                print($"Ryder in HighProfile: {GetComponent<RyderProfiler>().highProfile}");
        }

        private void Aim(Vector3 target)
        {
            transform.LookAt(transform.position + target);
        }

        private void Chamber()
        {
            _fired = true;
            _chamber -= 1;
            print($"Bullets in chamber: {_chamber}");
        }

        private void Fired()
        {
            _fired = false;
        }
        
        private void ReloadingChamber()
        {
            if (_chamber <= 0 && !_reloadingChamber)
            {
                _reloadingChamber = true;
                GetComponent<Animator>().SetTrigger($"Reload");
                GetComponent<RyderSFX>().PistolChamberReload(.6f);
                Invoke(nameof(ChamberReloaded), 3.25f);
            }
        }

        private void ChamberReloaded()
        {
            _chamber += 7;
            _reloadingChamber = false;
        }
    }
}