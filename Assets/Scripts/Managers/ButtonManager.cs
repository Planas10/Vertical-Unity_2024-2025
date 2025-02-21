using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public PlayerController controller;
    private GameObject lastSeen;
    private bool looking;

    private void Update()
    {
        if (controller.CheckInteractable())
        {
            if (controller.GetInteractable().name != "interactable return placeholder")
            {
                lastSeen = controller.GetInteractable();
                looking = true;
                if (lastSeen.GetComponent<Button>().activated) {
                    lastSeen.GetComponent<Button>().looked = false;
                }
                else { 
                    lastSeen.GetComponent<Button>().looked = true;
                }
            }
        }
        else {
            if(looking) {
                lastSeen.GetComponent<Button>().looked = false;
            }
        }
    }
}
