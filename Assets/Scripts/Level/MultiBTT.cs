using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBTT : ButtonBase
{
    public PlayerController pController;
    public Outline outlineScript;

    public AudioSource activateBTT;

    public PuertaMulitBTT puerta;

    public int indice;

    public MeshRenderer thisMesh;

    private bool canPlay = true;


    private void Update()
    {
        AlternateOutline();
        if (activated) {
            thisMesh.material.color = Color.green;
            thisMesh.material.SetColor("_EmissionColor", Color.green);
            if (canPlay)
            {
                canPlay = false;
                activateBTT.Play();
            }
            puerta.BotonActivado(indice);
            activated = false;
        }
    }

    private void AlternateOutline()
    {
        if (looked) { outlineScript.OutlineWidth = 10; }
        else { outlineScript.OutlineWidth = 0; }
    }
}
