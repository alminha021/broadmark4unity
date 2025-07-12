using UnityEngine;

public class PhysicsModeController : MonoBehaviour
{
    //funcionamento da fisica nos obj para os cenarios, deve estar acoplado nos prefabs
    public enum MovementMode
    {
        Brownian,
        FreeFall,
        RandomGravity,
        RotatingGravity,
        Hurricane
    }

    [Header("Configurações Gerais")]
    public MovementMode mode = MovementMode.Brownian;
    public float speed = 5f;

    [Header("Rotating Gravity")]
    public float rotationInterval = 3f;
    public float rotatingGravityMultiplier = 3f;

    [Header("Hurricane")]
    public float hurricaneForce = 20f;
    public Vector3 hurricaneCenter = Vector3.zero;

    private Rigidbody rb;

    private Vector3[] directions = new Vector3[]
    {
        Vector3.down,
        Vector3.right,
        Vector3.up,
        Vector3.left
    };

    private int currentDirectionIndex = 0;
    private float rotationTimer;

    // Para RandomGravity
    private Vector3 currentRandomGravity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody não encontrado no objeto " + gameObject.name);
            enabled = false;
            return;
        }
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
                currentRandomGravity = Random.onUnitSphere * 9.81f;
                break;

            case MovementMode.RotatingGravity:
                rb.useGravity = false;
                currentDirectionIndex = 0;
                rotationTimer = rotationInterval;
                rb.velocity = directions[currentDirectionIndex] * speed * rotatingGravityMultiplier;
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
                Vector3 randomChange = Random.insideUnitSphere * 3f;
                rb.velocity += randomChange * Time.fixedDeltaTime;
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, speed);
                break;

            case MovementMode.RandomGravity:
                rb.AddForce(currentRandomGravity, ForceMode.Acceleration);
                break;

            case MovementMode.RotatingGravity:
                rotationTimer -= Time.fixedDeltaTime;
                if (rotationTimer <= 0f)
                {
                    currentDirectionIndex = (currentDirectionIndex + 1) % directions.Length;
                    rotationTimer = rotationInterval;
                }

                float localForce = speed * rotatingGravityMultiplier;
                rb.AddForce(directions[currentDirectionIndex] * localForce, ForceMode.Acceleration);
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, localForce * 2f);
                break;

            case MovementMode.Hurricane:
                Vector3 toCenter = hurricaneCenter - transform.position;
                Vector3 perpendicular = Vector3.Cross(toCenter.normalized, Vector3.up);
                Vector3 swirlForce = perpendicular * hurricaneForce + toCenter.normalized * (hurricaneForce * 0.5f);
                rb.AddForce(swirlForce, ForceMode.Acceleration);
                break;

            // FreeFall: nada a fazer aqui
        }
    }

    public void SetMode(MovementMode newMode)
    {
        mode = newMode;
        SetupMode();
    }
}
