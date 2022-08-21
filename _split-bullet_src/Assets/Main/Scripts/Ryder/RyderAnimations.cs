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
        private Animator _anim;
        private int _idle, _speed, _jump, _attack, _dodge;
        private bool _dodgeDone, _attackDone;
        public float dodgeCool = .7f, attackCool = 4;

        private void Awake()
        {
            _controls = new InputControls();
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _anim = GetComponent<Animator>();

            _idle = Animator.StringToHash("Idle");
            _speed = Animator.StringToHash("Speed");
            _jump = Animator.StringToHash("Jump");
            _attack = Animator.StringToHash("Attack");
            _dodge = Animator.StringToHash("Dodge");

            _anim.SetFloat(_speed, 0);
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
            CoolDown();
            SwitchAttackLayers();
        }

        private void LateUpdate()
        {
            _anim.SetFloat(_speed, _rb.velocity.magnitude * GetComponent<RyderProfiler>().maxSpeed);
        }

        private void JumpAction(InputAction.CallbackContext obj)
        {
            if(GetComponent<RyderProfiler>().grounded)
                _anim.SetTrigger(_jump);
        }

        private void AttackAction(InputAction.CallbackContext obj)
        {
            if (_dodgeDone && _attackDone) return;
            IdleReset(true);
            _anim.SetTrigger(_attack);
            _dodgeDone = true;
            attackCool = 4;
        }

        private void DodgeAction(InputAction.CallbackContext obj)
        {
            if (_dodgeDone && _attackDone) return;
            IdleReset(true);
            _anim.SetTrigger(_dodge);
            _dodgeDone = true;
            dodgeCool = .7f;
        }

        private void CoolDown()
        {
            // dodge
            if (_dodgeDone)
                dodgeCool -= Time.deltaTime;

            if (_dodgeDone && dodgeCool <= 0)
                _dodgeDone = false;

            // attack
            if (_attackDone)
                attackCool -= Time.deltaTime;

            if (_attackDone && attackCool <= 0)
                _attackDone = false;
        }

        private void SwitchAttackLayers()
        {
            // switch Layers depending on Movement
            if (!_controls.Profiler.Movement.IsPressed())
            {
                // StandardAttack
                _anim.SetLayerWeight(3, 1);
                _anim.SetLayerWeight(4, 0);
            }
            else
            {
                // AppliedAttack
                _anim.SetLayerWeight(3, 0);
                _anim.SetLayerWeight(4, 1);
            }
        }

        private void IdleReset(bool reset)
        {
            _anim.SetBool(_idle, reset);
            if (reset) Invoke(nameof(IdleFalse), 0.1f);
        }

        private void IdleFalse()
        {
            _anim.SetBool(_idle, false);
        }
    }
}