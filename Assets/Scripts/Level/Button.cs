using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : ButtonBase
{
    public PlayerController pController;
    public Outline outlineScript;

    public AudioSource activateBTT;

    public MeshRenderer thisMesh;

    public float timerBTT;

    private bool canPlay;

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
            thisMesh.material.color = Color.red;
            if (canPlay)
            {
                canPlay = false;
                activateBTT.Play();
            }
            StartCoroutine(DoorTimer());
        }
    }

    public IEnumerator DoorTimer() {
        yield return new WaitForSeconds(timerBTT);
        thisMesh.material.color = Color.green;
        activated = false;
        canPlay = true;
    }
}
