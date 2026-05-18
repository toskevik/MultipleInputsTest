using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class LobbyManager : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject lobbyPanel;
        [SerializeField] private GameObject leftHalfUI;
        [SerializeField] private GameObject rightHalfUI;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private TextMeshProUGUI menuHeaderText;
        [SerializeField] private TextMeshProUGUI promptText;

        [Header("Spawn Settings")]
        [SerializeField] private GameObject gameplayAvatarPrefab; // The physical 3D character
        [SerializeField] private Transform leftSpawnPoint;
        [SerializeField] private Transform rightSpawnPoint;

        private LocalPlayerSetup[] _players = new LocalPlayerSetup[2];
        private int _playerCount = 0;
    
        public bool IsGameActive { get; private set; } = false;
        private bool _isPaused = false;

        private void OnEnable()
        {
            GetComponent<PlayerInputManager>().onPlayerJoined += OnPlayerJoined;
        }

        private void OnDisable()
        {
            if (TryGetComponent<PlayerInputManager>(out var manager))
            {
                manager.onPlayerJoined -= OnPlayerJoined;
            }
        }

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            if (_playerCount >= 2) return;

            LocalPlayerSetup setup = playerInput.GetComponent<LocalPlayerSetup>();
            _players[_playerCount] = setup;
        
            setup.Initialize(_playerCount, this, playerInput.currentControlScheme);

            if (_playerCount == 0)
            {
                leftHalfUI.SetActive(true);
                promptText.text = "Player 1 Connected! Player 2: Press any key to join...";
            }
            else if (_playerCount == 1)
            {
                rightHalfUI.SetActive(true);
                promptText.text = "Both players connected! Confirm Ready status to begin.";
                GetComponent<PlayerInputManager>().DisableJoining(); // Lock the lobby
            }

            _playerCount++;
        }

        public void SetPlayerReady(int playerIndex)
        {
            Debug.Log($"Player {playerIndex + 1} is READY!");
        
            // Update visual indicators on screen halves here
            // (e.g., change panel background color to green)

            if (_playerCount == 2 && _players[0].IsReady && _players[1].IsReady)
            {
                StartGameplay();
            }
        }

        private void StartGameplay()
        {
            IsGameActive = true;
            lobbyPanel.SetActive(false);

            // Spawn actual 3D avatars at designated offset locations
            SpawnAvatar(0, leftSpawnPoint);
            SpawnAvatar(1, rightSpawnPoint);
        }

        private void SpawnAvatar(int playerIndex, Transform spawnPoint)
        {
            GameObject avatar = Instantiate(gameplayAvatarPrefab, spawnPoint.position, spawnPoint.rotation);
        
            // Pass control over to the live avatar controller
            ThirdPersonMultiplayerMover mover = avatar.GetComponent<ThirdPersonMultiplayerMover>();
            mover.LinkInputDevice(_players[playerIndex].GetComponent<PlayerInput>());
        
            _players[playerIndex].SwitchToGameplayMode();
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;
            menuHeaderText.text = _isPaused ? "Pause - press cancel to exit" : "Resume";
            pausePanel.SetActive(_isPaused);
            Time.timeScale = _isPaused ? 0f : 1f;
        }
    }
}