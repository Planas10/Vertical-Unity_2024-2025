using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public PlayerController playerController;
    public LevelManager levelManager;

    public Transform PlayerPos;
    public NavMeshAgent _IA;

    private float detectionAngle = 45f;

    public bool PlayerDetected;
    public float DetectDistance;
    public float DetectHeight;

    public float attackDistance = 1f;

    public AudioSource _detectAudio;
    public Animator _anim;

    //calcular el angulo de vision
    private bool CheckAngle() {

        Vector3 Direction = transform.forward;
        Vector3 Distance = PlayerPos.position - transform.position;

        var lookPercentage = Vector3.Angle(Direction, Distance);

        if (lookPercentage > detectionAngle)
        {
            return false;
        }
        else {
            return true;
        }
    }

    //comprovar si el jugador esta dentro del rango de vision
    protected bool CheckPlayer() {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, PlayerPos.position - transform.position, out hit, DetectDistance, 3))
        {
            if (hit.collider.gameObject.CompareTag("Obstacle") || playerController.hidden)
            {
                return false;
            }
        }
        if (CheckAngle()) {
            if (CheckDistance()) {
                if(Vector3.Distance(transform.position, PlayerPos.position) <= attackDistance){
                    levelManager.ResetPlayer();
                }
            }
        }
        return true;
    }

    //comprovar la distancia entre el jugador y el enemigo asi como la diferencia de altura
    private bool CheckDistance()
    {
        return Vector3.Distance(transform.position, PlayerPos.position) <= DetectDistance && Mathf.Abs(transform.position.y - PlayerPos.position.y) < DetectHeight;
    }

    public virtual void PlayerSpotted() {
        _IA.SetDestination(PlayerPos.position);
        PlayerDetected = true;
        _anim.SetBool("PlayerDetected", true);
    }

    //persecucion del jugador
    public IEnumerator ChasePlayer() {
        yield return null;
        while (PlayerDetected) {
            yield return new WaitForSeconds(0.2f);
            if (CheckDistance())
            {
                if (Vector3.Distance(transform.position, PlayerPos.position) < 1)
                {
                
                }
                else { 
                    _IA.SetDestination(PlayerPos.position);
                }
            }
        }

    }
}
