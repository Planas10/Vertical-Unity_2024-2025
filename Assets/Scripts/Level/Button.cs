using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Button : ButtonBase
{
    public PlayerController pController;
    public Outline outlineScript;

    public AudioSource activateBTT;

    public MeshRenderer thisMesh;

    public float timerBTT;

    private bool canPlay = true;

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
            thisMesh.material.color = Color.green;
            thisMesh.material.SetColor("_EmissionColor", Color.green);
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
        thisMesh.material.color = Color.red;
        thisMesh.material.SetColor("_EmissionColor", Color.red);
        activated = false;
        canPlay = true;
    }
}
