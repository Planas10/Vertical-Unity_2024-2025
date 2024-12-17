using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject SettingsCanvas;
    [SerializeField] private GameObject CreditsCanvas;

    public void StartGame() {
        SceneManager.LoadScene("Sandbox");
    }

    public void StartSandbox()
    {
        SceneManager.LoadScene("Sandbox");
    }

    public void GoToMM()
    {
        SettingsCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }

    public void GoToSettings()
    {
        SettingsCanvas.SetActive(true);
        MainMenuCanvas.SetActive(false);
    }

    public void GoToCredits()
    {
        CreditsCanvas.SetActive(true);
        MainMenuCanvas.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
