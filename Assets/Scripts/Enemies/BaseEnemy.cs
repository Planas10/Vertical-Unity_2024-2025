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

    public float attackDistance;

    public AudioSource _detectAudio;
    public Animator _anim;

    public float attackSpeed;
    public float attackTimer = 0;

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
        if (attackTimer > 0) {
            attackTimer -= Time.deltaTime;
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, PlayerPos.position - transform.position, out hit, DetectDistance))
        {
            Debug.Log(hit.collider.gameObject.tag);
            if (hit.collider.gameObject.CompareTag("Obstacle"))
            {
                _IA.isStopped = false;
                return false;
            }
        }
        if (CheckAngle()) {
            if (CheckDistance()) {
                if (Vector3.Distance(transform.position, PlayerPos.position) <= attackDistance)
                {
                    _IA.isStopped = true;
                    if (attackTimer <= 0)
                    {
                        playerController.TakeDamage();
                        attackTimer = attackSpeed;
                    }
                }
                else {
                    _IA.isStopped = false;
                }
                return true;
            }
        }
        _IA.isStopped = false;
        return false;
    }

    //comprovar la distancia entre el jugador y el enemigo asi como la diferencia de altura
    private bool CheckDistance()
    {
        return Vector3.Distance(transform.position, PlayerPos.position) <= DetectDistance && Mathf.Abs(transform.position.y - PlayerPos.position.y) < DetectHeight;
    }

    public virtual void PlayerSpotted() {
        _detectAudio.Play();
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
