using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public PlayerController pController;
    public Outline outlineScript;

    public bool looked;
    public bool activated;

    public float timerBTT;

    private void Update()
    {
        AlternateOutline();
        ActivatedTime();
    }

    private void AlternateOutline() {
        if (looked) { outlineScript.OutlineWidth = 10; }
        else { outlineScript.OutlineWidth = 0; }
    }

    public void ActivatedTime() {
        if (activated) {
            Debug.Log("Start Timer");
            StartCoroutine(DoorTimer());
        }
    }

    public IEnumerator DoorTimer() {
        yield return new WaitForSeconds(timerBTT);
        activated = false;
    }
}
