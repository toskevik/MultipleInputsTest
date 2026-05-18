using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class LocalPlayerSetup : MonoBehaviour
    {
        public int PlayerIndex { get; private set; }
        public bool IsReady { get; private set; }
        public string ControlScheme { get; private set; }

        private LobbyManager _lobbyManager;
        private PlayerInput _playerInput;

        public void Initialize(int index, LobbyManager manager, string scheme)
        {
            PlayerIndex = index;
            _lobbyManager = manager;
            ControlScheme = scheme;
            _playerInput = GetComponent<PlayerInput>();
            
            // Make sure UI is selected as the default input scheme
            _playerInput.SwitchCurrentActionMap("UI");
        }

        // Input System Action Message (Requires PlayerInput Behavior: Send Messages)
        private void OnSubmit(InputValue value)
        {
            // If we are already in gameplay mode, ignore UI submits
            if (_playerInput.currentActionMap.name != "UI") return;

            if (!IsReady)
            {
                IsReady = true;
                _lobbyManager.SetPlayerReady(PlayerIndex);
            }
        }

        private void OnPause(InputValue value)
        {
            // Toggle pause from anywhere if the gameplay has started
            if (_lobbyManager.IsGameActive)
            {
                _lobbyManager.TogglePause();
            }
        }
        
        public void SwitchToGameplayMode()
        {
            _playerInput.SwitchCurrentActionMap("Player");
        }
    }
}
