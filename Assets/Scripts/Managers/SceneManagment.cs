using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{
    public PlayerController _playerController;
    public CanvasManager _canvasManager;

    public GameObject _winLevelCanvas;

    private void Update()
    {
        if (_playerController.win)
        {
            WinCanvas();
        }
    }

    private void WinCanvas() {
        _winLevelCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        _canvasManager.gameIsPaused = true;
    }

    public void GoToSandbox() {
        SceneManager.LoadScene("Sandbox");
    }

    public void GoToMM() {
        SceneManager.LoadScene("MainMenu");
    }
    public void Exit() {
        Application.Quit();
    }
}
