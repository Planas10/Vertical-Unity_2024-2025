using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject SettingsCanvas;
    [SerializeField] private GameObject CreditsCanvas;
    [SerializeField] private GameObject LevelSelector;


    private void Awake()
    {
        SettingsCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
        LevelSelector.SetActive(false);
    }
    public void StartLevel1() {
        SceneManager.LoadScene("Level1");
    }

    public void StartLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void StartSandbox()
    {
        SceneManager.LoadScene("Sandbox");
    }

    public void GoToLevelSelector() {
        MainMenuCanvas.SetActive(false);
        LevelSelector.SetActive(true);
    }

    public void GoToMM()
    {
        SettingsCanvas.SetActive(false);
        LevelSelector.SetActive(false);
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
