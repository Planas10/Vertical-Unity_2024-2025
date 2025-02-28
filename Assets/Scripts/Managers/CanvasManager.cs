using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public PlayerController _playercontroller;

    public GameObject PauseCanvas;
    public GameObject PauseSettingsCanvas;
    public GameObject HowToPlayCanvas;

    public AudioSource _buttonSound;

    public CanvasGroup damageCanvas;

    public bool gameIsPaused = false;

    private void Awake()
    {
        PauseCanvas.SetActive(false);
        PauseSettingsCanvas.SetActive(false);
        HowToPlayCanvas.SetActive(false);
    }

    private void Update()
    {
        if (_playercontroller._inputs.actions["Pause"].WasPressedThisFrame())
        {
            PauseGame();
        }
        if (_playercontroller._inputs.actions["HowToPlay"].WasPressedThisFrame())
        {
            HowToPlay();
        }
    }

    public void HowToPlay() {
        _buttonSound.Play();
        HowToPlayCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        gameIsPaused = true;
    }

    public void PauseGame()
    {
        _buttonSound.Play();
        PauseCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        gameIsPaused = true;
    }

    public void CloseHTP() {
        _buttonSound.Play();
        HowToPlayCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        gameIsPaused = false;
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

    public void PauseMainMenu()
    {
        _buttonSound.Play();
        SceneManager.LoadScene("MainMenu");
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
