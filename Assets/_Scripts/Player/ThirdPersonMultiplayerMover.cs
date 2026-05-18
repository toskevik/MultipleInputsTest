using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class ThirdPersonMultiplayerMover : MonoBehaviour
    {
        [SerializeField] private float speed = 6f;
        [SerializeField] private float rotationSpeed = 720f;
    
        private PlayerInput _linkedInput;
        private Vector2 _moveInput;
        private CharacterController _controller;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        public void LinkInputDevice(PlayerInput input)
        {
            _linkedInput = input;
        
            // Dynamic event binding via standard Action asset maps
            _linkedInput.actions["Move"].performed += OnMovePerformed;
            _linkedInput.actions["Move"].canceled += OnMoveCanceled;
        }

        private void OnDestroy()
        {
            if (_linkedInput)
            {
                _linkedInput.actions["Move"].performed -= OnMovePerformed;
                _linkedInput.actions["Move"].canceled -= OnMoveCanceled;
            }
        }

        private void OnMovePerformed(InputAction.CallbackContext context) => _moveInput = context.ReadValue<Vector2>();
        private void OnMoveCanceled(InputAction.CallbackContext context) => _moveInput = Vector2.zero;

        private void Update()
        {
            if (!_linkedInput) return;

            // Process movement relative to standard world coordinates for local multiplayer clarity
            Vector3 movement = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;

            if (movement.magnitude >= 0.1f)
            {
                // Rotate character toward movement direction smoothly
                float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
                float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                _controller.Move(movement * (speed * Time.deltaTime));
            }
        }
    }
}