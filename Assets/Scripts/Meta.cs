using UnityEngine;

public class Meta : MonoBehaviour
{
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
        }
    }
}
