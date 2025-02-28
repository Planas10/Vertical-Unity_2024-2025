using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Button button;
    public Animator animator;

    private bool wasActivated;

    public AudioSource Open;
    public AudioSource Close;

    private bool canPlayOpen;
    private bool canPlayClose;

    private void Update()
    {
        StartAnimation();
    }

    private void StartAnimation() {
        if (button.activated)
        {
            if(!wasActivated)
                wasActivated = true;
            animator.SetBool("BTTactivated", button.activated);
            if (canPlayOpen)
            {
                canPlayOpen = false;
                canPlayClose = true;
                Open.Play();
            }
        }
        else {
            if (wasActivated) {
                animator.SetBool("BTTactivated", button.activated);
                if (canPlayClose) {
                    canPlayClose = false;
                    canPlayOpen = true;
                    Close.Play();
                }
                wasActivated = false;
            }
        }
    }
}
