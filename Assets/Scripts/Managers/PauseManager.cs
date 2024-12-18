using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public PlayerController _playercontroller;

    [SerializeField] private GameObject PauseCanvas;
    [SerializeField] private GameObject PauseSettingsCanvas;

    public bool gameIsPaused = false;

    private void Update()
    {
        if (_playercontroller._inputs.actions["Pause"].WasPressedThisFrame()) {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        PauseCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        gameIsPaused = false;
        PauseCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseSettingsButton()
    {
        PauseSettingsCanvas.SetActive(true);
        PauseCanvas.SetActive(false);
    }

    public void PauseSettingsReturn()
    {
        PauseSettingsCanvas.SetActive(false);
        PauseCanvas.SetActive(true);
    }

    public void PauseMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
