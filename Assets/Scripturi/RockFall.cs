using UnityEngine;

public class RockFall : MonoBehaviour
{
    [Header("Falling")]
    public float fallSpeed = 5f;
    public float startHeight = 10f;
    public float activationDistance = 3f;

    [Header("Rotation")]
    public float rotationSpeed = 30f;   // grade pe secundă

    private Vector2 startPosition;
    private bool shouldFall = false;
    private Transform player;

    void Start()
    {
        // poziția de start la înălțimea definită
        startPosition = new Vector2(transform.position.x, startHeight);
        transform.position = startPosition;

        // găsește jucătorul după tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!shouldFall)
        {
            // activează căderea când player-ul e suficient de aproape
            if (Vector2.Distance(transform.position, player.position) <= activationDistance)
                shouldFall = true;
        }

        if (shouldFall)
        {
            // 1) cade în jos (în spațiul mondial, ca să nu alunece diagonal odată cu rotirea)
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime, Space.World);

            // 2) se rotește încet în sensul acelor de ceasornic
            //    rotirea negativă pe axa Z e către sensul acelor de ceasornic
            transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);

            // 3) dacă a trecut de josul ecranului, se distruge
            if (transform.position.y < -5f)
                Destroy(gameObject);
        }
    }
}
