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
            Open.Play();
        }
        else {
            if (wasActivated) {
                animator.SetBool("BTTactivated", button.activated);
                Close.Play();
                wasActivated = false;
            }
        }
    }
}
