using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidad = 50f;   // velocidad fija del jugador

    private Rigidbody2D rb;
    private Vector2 direccion;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Captura la entrada del teclado (Flechas o WASD)
        direccion.x = Input.GetAxisRaw("Horizontal");
        direccion.y = Input.GetAxisRaw("Vertical");

        // Rotar la flecha en la dirección del movimiento
        if (direccion != Vector2.zero)
        {
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angulo - 90);
        }
    }

    void FixedUpdate()
    {
        // Usar linearVelocity en lugar de velocity
        rb.linearVelocity = direccion.normalized * velocidad;
    }
}
