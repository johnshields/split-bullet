using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /*
     * RyderProfiler
     * Main Script for Ryder's functionality and controls.
     * ref - https://youtu.be/WIl6ysorTE0
     */
    public class RyderProfiler : MonoBehaviour
    {
        // input
        private InputControls _controls;
        private InputAction _moveKeys;

        // movement
        public bool grounded, callDodge, dodgeActive, actionActive;
        public float movementForce = 1, jumpForce = 5, walk = 1.5f, run = 3, maxSpeed, dodge = 3;
        private Vector3 _forceDir = Vector3.zero;
        private Rigidbody _rb;
        private Camera _mainCam;
        private bool _runPressed, _runAction, _callJump, _actionNotCooled;
        private float _dodgeInit;

        // profiles
        public bool lowProfile, highProfile;

        private void Awake()
        {
            _controls = new InputControls();
            _mainCam = Camera.main;
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _dodgeInit = dodge;
        }

        private void OnEnable()
        {
            _moveKeys = _controls.Profiler.Movement;
            _controls.Profiler.Dodge.started += DodgeAction;
            _controls.Profiler.LowProfile.started += LowProfile;
            _controls.Profiler.HighProfile.started += HighProfile;
            _controls.Profiler.Enable();
        }

        private void OnDisable()
        {
            _controls.Profiler.Dodge.started -= DodgeAction;
            _controls.Profiler.LowProfile.started -= LowProfile;
            _controls.Profiler.HighProfile.started -= HighProfile;
            _controls.Profiler.Disable();
        }

        private void Update()
        {
            if (!grounded || callDodge)
                actionActive = true;
            else
                actionActive = false;
        }

        private void FixedUpdate()
        {
            MovementProfile();
            LookAt();

            if (!_controls.Profiler.Movement.IsPressed())
                CallDodge();
        }

        private void MovementProfile()
        {
            // Movement X & Y
            _forceDir += GetCameraRight(_mainCam) * (_moveKeys.ReadValue<Vector2>().x * movementForce);
            _forceDir += GetCameraForward(_mainCam) * (_moveKeys.ReadValue<Vector2>().y * movementForce);

            // cap Ryder's speed
            var hVelocity = _rb.velocity;
            hVelocity.y = 0;
            if (hVelocity.sqrMagnitude > maxSpeed * maxSpeed)
                _rb.velocity = hVelocity.normalized * maxSpeed + Vector3.up * _rb.velocity.y;

            // Movement force
            _rb.AddForce(_forceDir, ForceMode.Impulse);
            _forceDir = Vector3.zero;

            // if Run (Shift) IsPressed go to run
            maxSpeed = _controls.Profiler.Run.IsPressed() ? run : walk;
        }

        // Forward & Back.
        private Vector3 GetCameraForward(Component cam)
        {
            var forward = cam.transform.forward;
            forward.y = 0;
            return forward.normalized;
        }

        // Side to side.
        private Vector3 GetCameraRight(Component cam)
        {
            var right = cam.transform.right;
            right.y = 0;
            return right.normalized;
        }

        // Alter Ryder's look direction depending on input. 
        private void LookAt()
        {
            var direction = _rb.velocity;
            direction.y = 0;

            if (_moveKeys.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
                _rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
            else
                _rb.angularVelocity = Vector3.zero;
        }

        // Called by animation Event.
        public void CallJump()
        {
            // Ryder can only JumpAction when grounded
            if (!_callJump && !grounded) return;
            _rb.velocity = transform.TransformDirection(0, jumpForce, 0f);
            grounded = false;
            _callJump = false;
        }

        private void OnCollisionEnter()
        {
            grounded = true;
        }

        private void DodgeAction(InputAction.CallbackContext obj)
        {
            if (!callDodge && !_controls.Profiler.Movement.IsPressed()) callDodge = true;
        }

        private void CallDodge()
        {
            if (!callDodge) return;
            dodgeActive = true;
            dodge -= Time.deltaTime; // increase dodge
            _rb.velocity = transform.TransformDirection(0, 0, -dodge);
            Invoke(nameof(RestDodge), .8f);
        }

        private void RestDodge()
        {
            dodge = _dodgeInit;
            callDodge = false;
            dodgeActive = false;
        }

        // Input: 1
        private void LowProfile(InputAction.CallbackContext obj)
        {
            if (!lowProfile)
            {
                lowProfile = true;
                highProfile = false;
                GetComponent<ScottAndWalton>().EquipPistol(false);
                _mainCam.GetComponent<CinemachineFreeLook>().m_YAxisRecentering.m_enabled = true;
            }
        }

        // Input: 2
        private void HighProfile(InputAction.CallbackContext obj)
        {
            if (!highProfile)
            {
                lowProfile = false;
                highProfile = true;
                GetComponent<ScottAndWalton>().EquipPistol(true);
                _mainCam.GetComponent<CinemachineFreeLook>().m_YAxisRecentering.m_enabled = false;
                print($"HighProfile Status: LowProfile: {lowProfile} | HighProfile: {highProfile}");
            }
        }
    }
}