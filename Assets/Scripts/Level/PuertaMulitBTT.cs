using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaMulitBTT : MonoBehaviour
{
    private List<bool> estadoBotones;

    public int cantidadBotones;

    public Animator animator;
    public AudioSource Open;
    public AudioSource Close;

    private bool canPlayOpen;

    private void Start()
    {
        estadoBotones = new List<bool>();
        for (int i = 0; i < cantidadBotones; i++)
        {
            estadoBotones.Add(false);
        }
    }

    public void BotonActivado(int indiceBTT) {
        estadoBotones[indiceBTT] = true;
        if (estadoBotones.Contains(false))
        {
            return;
        }
        else {
            animator.SetBool("BTTactivated", true);
            Open.Play();
        }
    }
}
