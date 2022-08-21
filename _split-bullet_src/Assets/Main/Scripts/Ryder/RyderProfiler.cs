using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /*
     * RyderProfiler
     * Main Script for Ryder functionality and controls.
     * ref - https://youtu.be/WIl6ysorTE0
     */
    public class RyderProfiler : MonoBehaviour
    {
        // input
        private InputControls _controls;
        private InputAction _moveKeys;

        // movement
        public float movementForce = 1, jumpForce = 5, walk = 1.5f, run = 3, maxSpeed, dodge = 1, cooldown = 4, delay;
        private Vector3 _forceDir = Vector3.zero;
        private Rigidbody _rb;
        private Camera _mainCam;
        private bool _grounded, _runPressed, _runAction, _callJump;
        private bool _actionNotCooled;
        private bool _dodgeDone;

        private void Awake()
        {
            _controls = new InputControls();
            _mainCam = Camera.main;
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _moveKeys = _controls.Profiler.Movement;
            _controls.Profiler.Dodge.started += DodgeAction;
            _controls.Profiler.Enable();
        }

        private void OnDisable()
        {
            _controls.Profiler.Dodge.started -= DodgeAction;
            _controls.Profiler.Disable();
        }
        
        
        private void Update()
        {
            CallDodge();
        }

        private void FixedUpdate()
        {
            // movement X & Y
            _forceDir += GetCameraRight(_mainCam) * (_moveKeys.ReadValue<Vector2>().x * movementForce);
            _forceDir += GetCameraForward(_mainCam) * (_moveKeys.ReadValue<Vector2>().y * movementForce);

            // cap Ryder's speed
            var hVelocity = _rb.velocity;
            hVelocity.y = 0;
            if (hVelocity.sqrMagnitude > maxSpeed * maxSpeed)
                _rb.velocity = hVelocity.normalized * maxSpeed + Vector3.up * _rb.velocity.y;

            LookAt();
            MovementProfile();
        }

        private Vector3 GetCameraForward(Component cam)
        {
            var forward = cam.transform.forward;
            forward.y = 0;
            return forward.normalized;
        }

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


        private void MovementProfile()
        {
            _rb.AddForce(_forceDir, ForceMode.Impulse);
            _forceDir = Vector3.zero;

            // if Run (Shift) is trigger go to run
            maxSpeed = _controls.Profiler.Run.IsPressed() ? run : walk;
        }

        // Called by animation Event.
        public void CallJump()
        {
            // Ryder can only JumpAction when Grounded().
            if (!_callJump && !_grounded) return;
            _rb.velocity = transform.TransformDirection(0, jumpForce, 0f);
            _grounded = false;
            _callJump = false;
        }

        private void OnCollisionEnter()
        {
            _grounded = true;
        }
        
        private void DodgeAction(InputAction.CallbackContext obj)
        {
            if (_dodgeDone) return;
            _dodgeDone = true;
        }

        private void CallDodge()
        {
            if (_dodgeDone)
            {
                cooldown -= Time.deltaTime;
                dodge += 0.3f;  // dodges force increases
                _rb.AddForce(transform.forward * -dodge, ForceMode.Impulse);   
            }

            if (_dodgeDone && cooldown <= 0)
            {
                dodge = 0;
                _dodgeDone = false;
                cooldown = .7f;
            }
        }
    }
}