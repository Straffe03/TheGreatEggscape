using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

/**
 * IA bàsica d’un enemic: patrulla aleatòriament, persegueix el jugador si és a prop, i torna a patrullar si s'allunya.
 */
public class EnemyAI : MonoBehaviour
{
    public float visionRange = 10f;
    public float stopChasingDistance = 20f;
    public float patrolRadius = 15f;
    public float patrolWaitTime = 3f;
    private Transform player;

    private NavMeshAgent agent;
    private Vector3 initialPosition = Vector3.zero;
    private float patrolTimer;

    // Animació de caminar/idle
    private Animator animator;
    private enum State { Patrolling, Chasing }
    private State currentState;

    // Sonido de ladrido
    public AudioClip barkSound;
    public float audioDuration = 3f; // Duración del sonido de ladrido
    private AudioSource audioSource;
    private bool hasBarked = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        initialPosition = transform.position;
        currentState = State.Patrolling;
        patrolTimer = patrolWaitTime;

        PatrolToNewPoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager playerHealth = other.GetComponent<PlayerManager>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(25);
                GameManager.score -= 25; // Reduir puntuació del jugador
            }
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrolling:
                if (distanceToPlayer < visionRange)
                {
                    currentState = State.Chasing;

                    if (!hasBarked && barkSound != null)
                    {
                        Debug.Log("Barking at player!");
                        StartCoroutine(PlayBarkForSeconds(audioDuration));
                        hasBarked = true;
                    }
                }
                else if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    patrolTimer -= Time.deltaTime;
                    if (patrolTimer <= 0f)
                    {
                        PatrolToNewPoint();
                        patrolTimer = patrolWaitTime;
                    }
                }
                break;

            case State.Chasing:
                if (distanceToPlayer > stopChasingDistance)
                {
                    currentState = State.Patrolling;
                    hasBarked = false; 
                    PatrolToNewPoint();
                }
                else
                {
                    agent.SetDestination(player.position);
                }
                break;
        }
                float velocity = agent.velocity.magnitude;

        if (velocity < 0.1f)
        {
            animator.SetFloat("Vert", 0f);
        }
        else
        {
            animator.SetFloat("Vert", 0.85f); // Ajusta si vols fer que corri
        }

        animator.speed = 3f; // opcional, pots ajustar si vols animació més ràpida
    }

    void PatrolToNewPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += initialPosition;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    IEnumerator PlayBarkForSeconds(float duration)
    {
        audioSource.clip = barkSound;
        audioSource.time = 0f; // empieza desde el inicio
        audioSource.Play();
        yield return new WaitForSeconds(duration);
        audioSource.Stop();
    }
}
