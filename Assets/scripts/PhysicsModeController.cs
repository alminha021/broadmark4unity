using UnityEngine;

public class PhysicsModeController : MonoBehaviour
{
    public enum MovementMode
    {
        Brownian,
        FreeFall,
        RandomGravity,
        RotatingGravity,
        Hurricane
    }

    public MovementMode mode = MovementMode.Brownian;
    public float speed = 5f;
    public float rotationInterval = 3f;
    public float hurricaneForce = 20f;
    public Vector3 hurricaneCenter = Vector3.zero;

    private Rigidbody rb;
    private Vector3 currentGravity;
    private float rotationTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody não encontrado no objeto " + gameObject.name);
            enabled = false;
            return;
        }

        SetupMode();
    }

    void SetupMode()
    {
        switch (mode)
        {
            case MovementMode.Brownian:
                rb.useGravity = false;
                rb.velocity = Random.insideUnitSphere * speed;
                break;

            case MovementMode.FreeFall:
                rb.useGravity = true;
                break;

            case MovementMode.RandomGravity:
                rb.useGravity = false;
                currentGravity = Random.onUnitSphere * 9.81f;
                break;

            case MovementMode.RotatingGravity:
                rb.useGravity = false;
                currentGravity = Vector3.down * 9.81f;
                rotationTimer = rotationInterval;
                break;

            case MovementMode.Hurricane:
                rb.useGravity = false;
                break;
        }
    }

    void FixedUpdate()
    {
        switch (mode)
        {
            case MovementMode.Brownian:
                Vector3 randomChange = Random.insideUnitSphere * 3f; // mais forte
                rb.velocity += randomChange * Time.fixedDeltaTime;
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
                break;

            case MovementMode.RandomGravity:
                rb.AddForce(currentGravity, ForceMode.Acceleration);
                break;

            case MovementMode.RotatingGravity:
                rotationTimer -= Time.fixedDeltaTime;
                if (rotationTimer <= 0f)
                {
                    currentGravity = Random.onUnitSphere * 9.81f;
                    rotationTimer = rotationInterval;
                }
                rb.AddForce(currentGravity, ForceMode.Acceleration);
                break;

            case MovementMode.Hurricane:
                Vector3 toCenter = hurricaneCenter - transform.position;
                Vector3 perpendicular = Vector3.Cross(toCenter.normalized, Vector3.up);
                Vector3 swirlForce = perpendicular * hurricaneForce + toCenter.normalized * (hurricaneForce * 0.5f);

                rb.AddForce(swirlForce, ForceMode.Acceleration);
                break;
        }
    }

    // Se quiser trocar modo via script externo
    public void SetMode(MovementMode newMode)
    {
        mode = newMode;
        SetupMode();
    }
}
