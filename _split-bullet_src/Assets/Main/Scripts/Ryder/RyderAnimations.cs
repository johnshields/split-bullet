using System;
using System.Collections;
using System.Collections.Generic;
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
        private int _speed, _jump, _attack;

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

            _animator.SetFloat(_speed, 0);
        }

        private void OnEnable()
        {
            _controls.Profiler.Jump.started += JumpAction;
            _controls.Profiler.Attack.started += AttackAction;
            _controls.Profiler.Enable();
        }

        private void OnDisable()
        {
            _controls.Profiler.Jump.started -= JumpAction;
            _controls.Profiler.Attack.started -= AttackAction;
            _controls.Profiler.Disable();
        }

        private void LateUpdate()
        {
            _animator.SetFloat(_speed, _rb.velocity.magnitude * GetComponent<RyderProfiler>().maxSpeed);
        }

        private void JumpAction(InputAction.CallbackContext obj)
        {
            print("Jump");
            _animator.SetTrigger(_jump);
        }

        private void AttackAction(InputAction.CallbackContext obj)
        {
            print("Attack");
            _animator.SetTrigger(_attack);
        }
    }
}