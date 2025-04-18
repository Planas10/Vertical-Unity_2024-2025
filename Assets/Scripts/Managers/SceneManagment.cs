using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{
    public CanvasManager _canvasManager;

    public GameObject _winLevelCanvas;

    public AudioSource _buttonSound;

    public void WinCanvas() {
        _winLevelCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        _canvasManager.gameIsPaused = true;
    }

    public void GoToSandbox() {
        _buttonSound.Play();
        SceneManager.LoadScene("Sandbox");
    }

    public void GoToMM() {
        _buttonSound.Play();
        SceneManager.LoadScene("MainMenu");
    }
    public void Exit() {
        _buttonSound.Play();
        Application.Quit();
    }
}
