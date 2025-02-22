using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject SettingsCanvas;
    [SerializeField] private GameObject CreditsCanvas;

    public AudioSource _buttonSound;


    private void Awake()
    {
        SettingsCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }
    public void StartLevel1() {
        _buttonSound.Play();
        SceneManager.LoadScene("Scene1");
    }

    public void StartSandbox()
    {
        _buttonSound.Play();
        SceneManager.LoadScene("Sandbox");
    }

    public void GoToMM()
    {
        _buttonSound.Play();
        SettingsCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }

    public void GoToSettings()
    {
        _buttonSound.Play();
        SettingsCanvas.SetActive(true);
        MainMenuCanvas.SetActive(false);
    }

    public void GoToCredits()
    {
        _buttonSound.Play();
        CreditsCanvas.SetActive(true);
        MainMenuCanvas.SetActive(false);
    }

    public void Exit()
    {
        _buttonSound.Play();
        Application.Quit();
    }
}
