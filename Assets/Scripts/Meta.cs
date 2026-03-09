using UnityEngine;

public class Meta : MonoBehaviour
{
    public GameObject winPanel; // referencia al panel de victoria

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Usamos el nuevo método recomendado
            CronometroLaberinto cronometro = Object.FindFirstObjectByType<CronometroLaberinto>();
            if (cronometro != null)
            {
                cronometro.TerminarJuego(true);
            }

            // Desactivar el movimiento del jugador
            other.GetComponent<PlayerMovement>().enabled = false;

            // Activar el panel de victoria (solo si está asignado)
            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }

            // Destruir el objeto Meta para que no arrastre referencias a la siguiente escena
            Destroy(gameObject);
        }
    }
}
