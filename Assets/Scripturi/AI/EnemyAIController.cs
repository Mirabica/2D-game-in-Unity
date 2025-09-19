using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    [Header("References")]
    public Transform        player;
    public HealthController healthController;

    [Header("Detection")]
    public float detectionRadius = 5f;

    [Header("Chase Speeds")]
    public float normalChaseSpeed = 2f;
    public float fastChaseSpeed   = 4f;

    [Header("Periodic Jump (Chase only)")]
    [Tooltip("Seconds between jumps while chasing")]
    public float jumpInterval = 5f;
    [Tooltip("Upward force applied on each jump")]
    public float jumpForce    = 5f;

    [Header("Timed Patrol Settings")]
    public float durationLeft1   = 2f;
    public float durationRight1  = 2f;
    public float durationRight2  = 3f;
    public float durationLeft2   = 3f;
    public float patrolSpeed     = 1.5f;

    private Node _root;

    void Start()
    {
        if (healthController == null)
            healthController = FindObjectOfType<HealthController>();

        var rb = GetComponent<Rigidbody2D>();

        // --- JUMP DURING CHASE SEQUENCE ---
        var seeForJump     = new IsPlayerInRange(transform, player, detectionRadius);
        var periodicJump   = new PeriodicJumpNode(rb, jumpForce, jumpInterval);
        var jumpChaseSeq   = new Sequence(new List<Node> { seeForJump, periodicJump });

        // --- FAST & NORMAL CHASE SEQUENCES ---
        var seeForChase    = new IsPlayerInRange(transform, player, detectionRadius);
        var lowHealth      = new IsPlayerLowHealth(healthController, 1);
        var chaseNormal    = new ChasePlayer(transform, player, rb, normalChaseSpeed);
        var chaseFast      = new ChasePlayer(transform, player, rb, fastChaseSpeed);
        var fastChaseSeq   = new Sequence(new List<Node> { seeForChase, lowHealth, chaseFast });
        var normalChaseSeq = new Sequence(new List<Node> { seeForChase, chaseNormal });

        // --- PATROL ---
        var timedPatrol = new TimedPatrolNode(
            transform,
            rb,
            patrolSpeed,
            durationLeft1,
            durationRight1,
            durationRight2,
            durationLeft2
        );

        // --- ROOT: jump→fast→normal→patrol ---
        _root = new Selector(new List<Node> {
            jumpChaseSeq,
            fastChaseSeq,
            normalChaseSeq,
            timedPatrol
        });
    }

    void Update()
    {
        _root?.Evaluate();
    }
}
