using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocidadNormal = 6f;   // velocidad caminando
    public float velocidadCorrer = 2*30f;  // velocidad corriendo
    private float velocidadActual;

    private Rigidbody2D rb;
    private Vector2 direccion;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocidadActual = velocidadNormal;
    }

    void Update()
    {
        // Captura la entrada del teclado (flechas o WASD)
        direccion.x = Input.GetAxisRaw("Horizontal");
        direccion.y = Input.GetAxisRaw("Vertical");

        // Cambia la velocidad si se presiona Shift
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            velocidadActual = velocidadCorrer;
        }
        else
        {
            velocidadActual = velocidadNormal;
        }

        // Opcional: rotar la flecha en la dirección del movimiento
        if (direccion != Vector2.zero)
        {
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angulo - 90);
        }
    }

    void FixedUpdate()
    {
        // Mueve el personaje con física
        rb.MovePosition(rb.position + direccion * velocidadActual * Time.fixedDeltaTime);
    }
}
