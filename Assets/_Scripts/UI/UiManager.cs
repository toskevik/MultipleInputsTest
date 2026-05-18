using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace UI
{
    public class UiManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private GameObject pauseMenu;
        
        [SerializeField] private InputActionMap _uiMap;
        private bool _isPaused = false;

        private void Awake()
        {
            _uiMap = inputActions.FindActionMap("UI");            
        }

        // Update is called once per frame
        private void Update()
        {
            // Listen for pause from ANY controller
            if (_uiMap.FindAction("Pause").triggered)
            {
                TogglePause();
            }
        }

        private void TogglePause()
        {
            _isPaused = !_isPaused;
            pauseMenu.SetActive(_isPaused);
        }
    }
}
