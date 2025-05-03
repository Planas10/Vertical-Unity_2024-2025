using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public PlayerController _playercontroller;

    public GameObject PauseCanvas;
    public GameObject PauseSettingsCanvas;

    public AudioSource _buttonSound;

    public CanvasGroup damageCanvas;

    public bool gameIsPaused = false;

    private void Awake()
    {
        PauseCanvas.SetActive(false);
        PauseSettingsCanvas.SetActive(false);
    }

    private void Update()
    {
        if (_playercontroller._inputs.actions["Pause"].WasPressedThisFrame())
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        _buttonSound.Play();
        PauseCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        _buttonSound.Play();
        gameIsPaused = false;
        PauseCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseSettingsButton()
    {
        _buttonSound.Play();
        PauseSettingsCanvas.SetActive(true);
        PauseCanvas.SetActive(false);
    }

    public void PauseSettingsReturn()
    {
        _buttonSound.Play();
        PauseSettingsCanvas.SetActive(false);
        PauseCanvas.SetActive(true);
    }

    public void ShowDamage() {
        damageCanvas.alpha = 1;
        StartCoroutine(ResetDamageCanvas());
    }

    public IEnumerator ResetDamageCanvas() {
        float currentValue = 1;
        while (currentValue > 0) {
            currentValue -= Time.deltaTime;
            damageCanvas.alpha = currentValue;
            yield return null;
        }
    }
}
