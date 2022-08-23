using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /*
     * RyderAnimations
     * Script that controls Ryder's animations.
     * ref - https://youtu.be/WIl6ysorTE0
     */
    public class RyderAnimations : MonoBehaviour
    {
        private InputControls _controls;
        private Rigidbody _rb;
        private Animator _anim;
        private int _speed, _jump, _runJump, _attack, _dodge, _dodgeFwd;
        private bool _dodgeDone, _attackDone, _jumpDone;
        private float _attackInit, _jumpInit;
        public float attackCool = 2, jumpCool = 3;

        private void Awake()
        {
            _controls = new InputControls();
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _anim = GetComponent<Animator>();
            
            _speed = Animator.StringToHash("Speed");
            _jump = Animator.StringToHash("Jump");
            _runJump = Animator.StringToHash("RunJump");
            _attack = Animator.StringToHash("Attack");
            _dodge = Animator.StringToHash("Dodge");
            _dodgeFwd = Animator.StringToHash("DodgeFwd");

            _attackInit = attackCool;
            _jumpInit = jumpCool;
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
            SwitchProfiles();
        }

        private void FixedUpdate()
        {
            _anim.SetFloat(_speed, _rb.velocity.magnitude * GetComponent<RyderProfiler>().maxSpeed);
        }

        private void JumpAction(InputAction.CallbackContext obj)
        {
            if (!GetComponent<RyderProfiler>().grounded || _jumpDone) return;
            if (!_controls.Profiler.Movement.IsPressed() && !_controls.Profiler.Run.IsPressed())
                _anim.SetTrigger(_jump);
            else
                _anim.SetTrigger(_runJump);
            _jumpDone = true;
        }

        private void AttackAction(InputAction.CallbackContext obj)
        {
            if (_attackDone) return;
            _anim.SetTrigger(_attack);
            _attackDone = true;
        }

        private void CoolDown()
        {
            // jump
            if (_jumpDone)
                jumpCool -= Time.deltaTime;
            else
                jumpCool = _jumpInit;

            if (_jumpDone && jumpCool <= 0)
                _jumpDone = false;

            // attack
            if (_attackDone)
                attackCool -= Time.deltaTime;
            else
                attackCool = _attackInit;

            if (_attackDone && attackCool <= 0)
                _attackDone = false;
        }

        private void DodgeAction(InputAction.CallbackContext obj)
        {
            if (_dodgeDone) return;
            _anim.SetTrigger(!_controls.Profiler.Movement.IsPressed() ? _dodge : _dodgeFwd);
            _dodgeDone = true;
            GetComponent<RyderProfiler>().dodgeDone = true;
            Invoke(nameof(RestDodge), 1.5f);
        }

        private void RestDodge()
        {
            _dodgeDone = false;
            GetComponent<RyderProfiler>().dodgeDone = false;
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

        private void SwitchProfiles()
        {
            if (_controls.Profiler.LowProfile.IsPressed() && GetComponent<RyderProfiler>().lowProfile)
            {
                _anim.SetLayerWeight(0, 1);
                _anim.SetLayerWeight(1, 0);
            }
            else if (_controls.Profiler.HighProfile.IsPressed() && GetComponent<RyderProfiler>().highProfile)
            {
                _anim.SetLayerWeight(0, 0);
                _anim.SetLayerWeight(1, 1);
            }
        }
    }
}