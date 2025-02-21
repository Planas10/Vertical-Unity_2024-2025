using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Button button;
    public Animator animator;

    private bool wasActivated;

    private void Update()
    {
        StartAnimation();
    }

    private void StartAnimation() {
        if (button.activated)
        {
            if(!wasActivated)
                wasActivated = true;
            Debug.Log("OpenDoor");
            animator.SetBool("BTTactivated", button.activated);
        }
        else {
            if (wasActivated) {
                Debug.Log("CloseDoor");
                animator.SetBool("BTTactivated", button.activated);
                wasActivated = false;
            }
        }
    }
}
