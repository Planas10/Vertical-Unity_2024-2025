using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLvlBTT : ButtonBase
{
    public PlayerController pController;
    public Outline outlineScript;

    public MeshRenderer thisMesh;

    private void Update()
    {
        AlternateOutline();
    }

    private void AlternateOutline()
    {
        if (looked) { outlineScript.OutlineWidth = 10; }
        else { outlineScript.OutlineWidth = 0; }
    }

    public bool CheckActivated() {
        if (activated) {
            return true;
        }
        return false;
    }
}
