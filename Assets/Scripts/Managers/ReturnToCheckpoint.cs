using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToCheckpoint : MonoBehaviour
{
    public LevelManager levelmanager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            levelmanager.ResetPlayer();
        }
    }
}
