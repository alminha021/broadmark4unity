using UnityEngine;

public class PhysicsModeController : MonoBehaviour
{
    public enum MovementMode
    {
        Brownian,
        FreeFall,
        RandomGravity
    }

    public MovementMode mode = MovementMode.Brownian;
    public float speed = 5f;

    private Rigidbody rb;
    private Vector3 randomGravity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody não encontrado no objeto " + gameObject.name);
            enabled = false;
            return;
        }

        switch (mode)
        {
            case MovementMode.Brownian:
                rb.useGravity = false;
                rb.velocity = Random.onUnitSphere * speed;
                break;

            case MovementMode.FreeFall:
                rb.useGravity = true;
                break;

            case MovementMode.RandomGravity:
                rb.useGravity = false;
                randomGravity = Random.onUnitSphere * 9.81f;
                break;
        }
    }

    void FixedUpdate()
    {
        if (mode == MovementMode.RandomGravity)
        {
            rb.AddForce(randomGravity, ForceMode.Acceleration);
        }
        else if (mode == MovementMode.Brownian)
        {
            // Pequenas variações na velocidade para movimento mais "aleatório"
            Vector3 randomChange = Random.insideUnitSphere * 0.5f;
            rb.velocity += randomChange * Time.fixedDeltaTime;
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
        }
    }
}
