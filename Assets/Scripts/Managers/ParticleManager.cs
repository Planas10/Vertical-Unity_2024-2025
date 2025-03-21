using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject hookImpactParticles;
    public PlayerController Player;

    public void HookImpactParticle(Vector3 position) {
        hookImpactParticles.transform.position = position;
        hookImpactParticles.GetComponent<ParticleSystem>().Play();
    }
}
