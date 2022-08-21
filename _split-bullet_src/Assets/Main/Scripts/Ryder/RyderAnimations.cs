using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /*
     * RyderAnimations
     * Script that controls the Ryder animations.
     * ref - https://youtu.be/WIl6ysorTE0
     */
    public class RyderAnimations : MonoBehaviour
    {
        private InputControls _controls;
        private Rigidbody _rb;
        private Animator _animator;
        private int _speed, _jump, _attack, _dodge;
        private bool _actionDone;
        public float cooldown = 4;

        private void Awake()
        {
            _controls = new InputControls();
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();

            _speed = Animator.StringToHash("Speed");
            _jump = Animator.StringToHash("Jump");
            _attack = Animator.StringToHash("Attack");
            _dodge = Animator.StringToHash("Dodge");

            _animator.SetFloat(_speed, 0);
        }

        private void OnEnable()
        {
            _controls.Profiler.Jump.started += JumpAction;
            _controls.Profiler.Attack.started += AttackAction;
            _controls.Profiler.Dodge.started += DodgeAction;
            _controls.Profiler.Enable();
        }

        private void OnDisable()
        {
            _controls.Profiler.Jump.started -= JumpAction;
            _controls.Profiler.Attack.started -= AttackAction;
            _controls.Profiler.Dodge.started -= DodgeAction;
            _controls.Profiler.Disable();
        }
        
        private void Update()
        {
            if (_actionDone)
                cooldown -= Time.deltaTime;

            if (_actionDone && cooldown <= 0)
                _actionDone = false;
        }

        private void LateUpdate()
        {
            _animator.SetFloat(_speed, _rb.velocity.magnitude * GetComponent<RyderProfiler>().maxSpeed);
        }

        private void JumpAction(InputAction.CallbackContext obj)
        {
            _animator.SetTrigger(_jump);
        }

        private void AttackAction(InputAction.CallbackContext obj)
        {
            _animator.SetTrigger(_attack);
        }

        private void DodgeAction(InputAction.CallbackContext obj)
        {
            if (_actionDone) return;
            _actionDone = true;
            _animator.SetTrigger(_dodge);
            cooldown = .7f;
        }
    }
}