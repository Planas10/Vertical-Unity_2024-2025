using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineManager : MonoBehaviour
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
                ButtonBase button = lastSeen.GetComponent<ButtonBase>();
                if (button != null)
                {
                    if (button.activated)
                    {
                        button.looked = false;
                    }
                    else
                    {
                        button.looked = true;
                    }
                }
            }
        }
        else
        {
            if (looking)
            {
                ButtonBase button = lastSeen.GetComponent<ButtonBase>();
                if (button != null) {
                    button.looked = false;
                }
            }
        }
    }
}
