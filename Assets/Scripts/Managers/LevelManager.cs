using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Spawnpoints
public class LevelManager : MonoBehaviour
{
    public List<Transform> spawnPositions;
    private int currentSpawnIndex;

    public CharacterController characterController;

    private void Start()
    {
        currentSpawnIndex = 0;
    }

    public void SetSpawn(int spawn) {
        if (currentSpawnIndex < spawn)
        {
            currentSpawnIndex = spawn;
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


}