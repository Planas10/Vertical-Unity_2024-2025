using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void MainMenu() {
        SceneManager.LoadScene("MNainMenu");
    }

    public void Exit() {
        Application.Quit();
    }
}
