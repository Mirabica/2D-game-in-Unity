// Assets/Scripts/AI/Controllers/FrogAIController.cs
using System.Collections.Generic;
using UnityEngine;

public class FrogAIController : MonoBehaviour
{
    [Header("References")]
    public Transform        player;
    public HealthController healthController;

    [Header("Detection")]
    public float detectionRadius = 5f;

    [Header("Chase (Leap) Settings")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundRadius  = 0.1f;
    public float chaseHForce   = 2f;
    public float chaseVForce   = 4f;

    [Header("Patrol Jumping Settings")]
    public float patrolHForce       = 1.5f;  // horizontal jump strength
    public float patrolVForce       = 3f;    // vertical jump strength
    public float patrolJumpInterval = 1.5f;  // seconds between jumps
    public float patrolLeftDuration = 3f;    // seconds jump-left
    public float patrolRightDuration= 3f;    // seconds jump-right
    [Tooltip("-1 = start jumping left, +1 = start jumping right")]
    public int   patrolInitialDirection = -1;

    [Header("One–Shot Rage Settings")]
    public float rageRadius    = 1f;
    public float rageJumpForce = 5f;
    public float rageInterval  = 0.8f;
    public float rageDuration  = 5f;  // active window in seconds

    private Node _root;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (healthController == null)
            healthController = FindObjectOfType<HealthController>();

        // 1) See + leap-chase sequence
        var seePlayer = new IsPlayerInRange(transform, player, detectionRadius);
        var jumpChase = new JumpChaseNode(
            transform, player, _rb,
            groundCheck, groundLayer, groundRadius,
            chaseHForce, chaseVForce
        );
        var chaseSeq  = new Sequence(new List<Node> { seePlayer, jumpChase });

         // 1) Single‐use rage node
        var rageNode = new SingleUseRageNode(
            transform,
            player,
            healthController,
            _rb,
            rageRadius,
            rageJumpForce,
            rageInterval,
            rageDuration
        );

        // 3) Frog-style timed patrol (left/right)
        var frogPatrol = new FrogPatrolNode(
            transform, _rb,
            patrolHForce, patrolVForce,
            patrolJumpInterval,
            patrolLeftDuration,
            patrolRightDuration,
            patrolInitialDirection
        );

        // 4) Root: chase if seen, otherwise timed patrol
        _root = new Selector(new List<Node> {
            rageNode,
            chaseSeq,
            frogPatrol
        });
    }

    void Update()
    {
        _root.Evaluate();
    }
}
