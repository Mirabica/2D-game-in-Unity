using System.Collections.Generic;
using UnityEngine;

public class OwlAIController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag your Player GameObject here")]
    public Transform        player;
    [Tooltip("Optional: your HealthController (not used here)")]
    public HealthController healthController;

    [Header("Flight Settings")]
    [Tooltip("World‐space Y coordinate at which the owl flies")]
    public float flightHeight = 3f;

    [Header("Detection & Zig-Zag Chase")]
    [Tooltip("How close the player must be to trigger the chase")]
    public float detectionRadius = 6f;
    [Tooltip("Base horizontal speed when chasing")]
    public float chaseSpeed      = 3f;
    [Tooltip("Vertical range of the zig-zag motion")]
    public float zigZagAmplitude = 1f;
    [Tooltip("How many zig-zag oscillations per second")]
    public float zigZagFrequency = 1f;

    [Header("Patrol (Timed)")]
    [Tooltip("Seconds to fly left (phase 0)")]
    public float patrolLeftTime   = 2f;
    [Tooltip("Seconds to fly right (phase 1)")]
    public float patrolRightTime  = 2f;
    [Tooltip("Seconds to fly right again (phase 2)")]
    public float patrolRight2Time = 3f;
    [Tooltip("Seconds to fly left back (phase 3)")]
    public float patrolLeft2Time  = 3f;
    [Tooltip("Speed during patrol")]
    public float patrolSpeed      = 1.5f;

    private Node _root;
    private Rigidbody2D _rb;

    void Start()
    {
        // get components
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f; // owl should not fall

        if (healthController == null)
            healthController = FindObjectOfType<HealthController>();

        // 1) Chase sequence
        var seePlayer   = new IsPlayerInRange(transform, player, detectionRadius);
        var zigZagChase = new ZigZagChaseNode(
            transform,
            player,
            _rb,
            chaseSpeed,
            zigZagAmplitude,
            zigZagFrequency,
            flightHeight
        );
        var chaseSeq    = new Sequence(new List<Node> { seePlayer, zigZagChase });

        // 2) Patrol sequence
        var patrol = new OwlPatrolNode(
            transform,
            _rb,
            patrolSpeed,
            patrolLeftTime,
            patrolRightTime,
            patrolRight2Time,
            patrolLeft2Time,
            flightHeight
        );

        // 3) Build the root selector: chase first, otherwise patrol
        _root = new Selector(new List<Node> {
            chaseSeq,
            patrol
        });
    }

    void Update()
    {
        _root?.Evaluate();
    }
}
