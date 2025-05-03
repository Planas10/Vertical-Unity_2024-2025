using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Spawnpoints
public class LevelManager : MonoBehaviour
{
    public List<Transform> spawnPositions;
    private int currentSpawnIndex;

    public CharacterController characterController;
    public SceneManagment scenemanagment;

    public CanvasGroup CheckpointCanvas;

    public AudioSource CheckpointAudio;

    public NextLvlBTT nextLvlBTT;

    private void Update()
    {
        if (nextLvlBTT.CheckActivated()) {
            NextLevel();
        }
    }

    private void Start()
    {
        currentSpawnIndex = 0;
    }

    public void SetSpawn(int spawn) {
        if (currentSpawnIndex < spawn)
        {
            currentSpawnIndex = spawn;
            CheckpointAudio.Play();
            CheckpointCanvas.alpha = 1;
            StartCoroutine(CheckpointFade());
        }
    }

    public IEnumerator CheckpointFade() {
        float currentValue = 1;
        while (currentValue > 0)
        {
            currentValue -= Time.deltaTime;
            CheckpointCanvas.alpha = currentValue;
            yield return null;
        }
    }

    public Vector3 GetCurrentSpawnPosition() {
        return spawnPositions[currentSpawnIndex].position;
    }
    public void ResetPlayer() {
        characterController.Move(new Vector3(0, 0, 0));
        characterController.enabled = false;
        characterController.gameObject.transform.position = GetCurrentSpawnPosition();
        characterController.enabled = true;
    }

    public void NextLevel() {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            SceneManager.LoadScene("Level2");
        }
        else
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}