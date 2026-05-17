using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class MultiplayerPlayerController : MonoBehaviour
    {
        private Vector2 _moveInput;
        private PlayerInput _playerInput;
    
        [SerializeField] private float moveSpeed = 5f;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        // This method will be hooked up to the PlayerInput component's Unity Event
        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        // Hook this up to the Jump event
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log($"Player #{_playerInput.playerIndex} Jumped!");
            }
        }

        private void Update()
        {
            // Calculate frame-rate independent movement
            Vector3 moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
            transform.Translate(moveDirection * (moveSpeed * Time.deltaTime), Space.World);
        }
    }
}