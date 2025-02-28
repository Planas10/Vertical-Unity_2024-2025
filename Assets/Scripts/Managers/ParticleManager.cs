using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject hookImpact;
    public PlayerController Player;

    public void HookImpactParticle(Vector3 position) {
        hookImpact.transform.position = position;
        hookImpact.GetComponent<ParticleSystem>().Play();
    }
}
