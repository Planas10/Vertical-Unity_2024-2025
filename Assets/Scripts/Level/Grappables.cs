using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappables : MonoBehaviour
{
    public Outline outlineScript;
    public bool looked;

    private void LateUpdate()
    {
        AlternateOutline();
        looked = false;
    }

    private void AlternateOutline()
    {
        if (looked) { 
            outlineScript.OutlineMode = Outline.Mode.OutlineAll;
            outlineScript.OutlineColor = Color.green;
        }
        else {
            outlineScript.OutlineMode = Outline.Mode.OutlineVisible;
            outlineScript.OutlineColor = Color.white;
        }
    }
}
