using System;
using Characters;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class ScottAndWalton : MonoBehaviour
    {
        private InputControls _controls;
        private GameObject _pistol, _crosshair, _counter, _counterUI, _dummy;
        private Camera _mainCam;
        private bool _fired, _reloadingChamber;
        private RaycastHit _hit;
        public int chamber = 7, bullets, maxBullets = 36;
        public bool fillUpBullets;
        public Transform bullet, bulletTrans;
        public ParticleSystem[] flashVFX;

        private void Awake()
        {
            _controls = new InputControls();
        }

        private void Start()
        {
            bullets = maxBullets;
            chamber = bullets / 6 + 1;
            _pistol = GameObject.FindGameObjectWithTag("Pistol");
            _crosshair = GameObject.FindGameObjectWithTag("Crosshair");
            _counter =  GameObject.FindGameObjectWithTag("BulletCounter");
            _counterUI =  GameObject.FindGameObjectWithTag("CounterUI");
            _dummy = GameObject.FindGameObjectWithTag("Dummy");
            _mainCam = Camera.main;
            EquipPistol(false);
        }

        private void Update()
        {
            ReloadingChamber();

            if (bullets > maxBullets)
                bullets = maxBullets;
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
            _counterUI.SetActive(equip);
        }

        // ref - https://youtu.be/dqfVlSxOXv8
        private void FirePistol(InputAction.CallbackContext obj)
        {
            if (GetComponent<RyderProfiler>().actionActive) return;
            if (GetComponent<RyderProfiler>().highProfile && !_fired && chamber != 0 && bullets != 0)
            {
                Chamber();
                if (chamber > 0)
                {
                    GetComponent<RyderSFX>().PistolFire(.6f);
                    MuzzleFlash();
                }

                // gets the center point of the screen
                var center = new Vector2(Screen.width / 2, Screen.height / 2);
                var ray = _mainCam.ScreenPointToRay(center);

                // testing
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2);

                // if ray (bullet) hits a transform
                if (Physics.Raycast(ray, out var hit))
                {
                    _hit = hit;
                    print($"Pistol hit: {hit.transform.name}");
                    // testing
                    if (hit.transform.CompareTag("Dummy") && chamber > 0)
                        _dummy.GetComponentInParent<DummyProfiler>().DummyHit(hit.transform.gameObject);
                }

                var aimDir = (_hit.point - bulletTrans.position).normalized;
                ShootBullet(aimDir);

                Invoke(nameof(Fired), .5f);
            }
            else
                print($"Ryder in HighProfile: {GetComponent<RyderProfiler>().highProfile}");

            if (GetComponent<RyderProfiler>().highProfile && bullets == 0)
                GetComponent<RyderSFX>().PistolDryFire(.6f);
        }

        private void Chamber()
        {
            _fired = true;
            chamber -= 1;
        }

        private void MuzzleFlash()
        {
            flashVFX[0].Play();
            flashVFX[1].Play();
            flashVFX[2].Play();
        }

        private void ShootBullet(Vector3 aim)
        {
            Instantiate(bullet, bulletTrans.position, Quaternion.LookRotation(aim, Vector3.up));
            var forward = _mainCam.transform.forward;
            forward.y = 0;
            forward.Normalize();
            Aim(forward);
        }


        private void Aim(Vector3 target)
        {
            transform.LookAt(transform.position + target);
        }

        private void Fired()
        {
            _fired = false;
        }

        private void ReloadingChamber()
        {
            if (chamber <= 0 && bullets != 0 && !_reloadingChamber)
            {
                bullets -= 6;
                if (bullets == 0) return;
                _reloadingChamber = true;
                GetComponent<Animator>().SetTrigger($"Reload");
                GetComponent<RyderSFX>().PistolChamberReload(.6f);
                Invoke(nameof(ChamberReloaded), 3.25f);
            }
        }

        private void ChamberReloaded()
        {
            chamber += 7;
            _reloadingChamber = false;
        }

        public void FillUpBullets(int amount)
        {
            if (!fillUpBullets) return;
            fillUpBullets = false;
            bullets += amount;
        }
    }
}