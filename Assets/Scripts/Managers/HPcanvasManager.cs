using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPcanvasManager : MonoBehaviour
{
    public PlayerController _playerController;
    public Text _HPtext;
    private int hitpoints;

    private void Start()
    {
        hitpoints = _playerController.hitpoints;
        _HPtext.text = hitpoints.ToString();
    }

    private void Update()
    {
        if (hitpoints != _playerController.hitpoints) { 
            UpdateLife();
        }
    }

    public void UpdateLife() {
        _HPtext.text = _playerController.hitpoints.ToString();
    }
}
