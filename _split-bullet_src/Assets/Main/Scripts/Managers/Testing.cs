using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class Testing : MonoBehaviour
    {
        private InputControls _controls;
        private bool _pressed;
        public GameObject noirXFX;

        private void Awake()
        {
            _controls = new InputControls();
        }

        private void Update()
        {
            RenderSettings.fog = _pressed;
        }

        private void OnEnable()
        {
            _controls.Testing.NoirVFX.started += NoirVFX;
            _controls.Testing.Enable();
        }

        private void OnDisable()
        {
            _controls.Testing.NoirVFX.started -= NoirVFX;
            _controls.Testing.Disable();
        }

        private void NoirVFX(InputAction.CallbackContext obj)
        {
            if (!_pressed)
            {
                _pressed = true;
                noirXFX.SetActive(true);
            }
            else
            {
                _pressed = false;
                noirXFX.SetActive(false);
            }
        }
    }
}