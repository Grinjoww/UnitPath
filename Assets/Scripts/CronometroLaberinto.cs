using UnityEngine;
using TMPro; // Para usar TMP_Text

public class CronometroLaberinto : MonoBehaviour
{
    public float tiempoInicial = 10f; // segundos
    private float tiempoRestante;

    public TMP_Text cronometroText;   // Texto del cronómetro
    public GameObject winPanel;       // Panel de victoria
    public GameObject losePanel;      // Panel de derrota

    private bool juegoTerminado = false;

    void Start()
    {
        tiempoRestante = tiempoInicial;
        cronometroText.color = Color.black; // color inicial en negro
    }

    void Update()
    {
        if (juegoTerminado) return; // si ya terminó, no sigue contando

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0)
        {
            tiempoRestante = 0;
            TerminarJuego(false); // derrota
        }

        // Cambiar color si queda menos de 60 segundos
        if (tiempoRestante < 60f)
        {
            cronometroText.color = Color.red;
        }

        // Actualizar texto
        cronometroText.text = "Tiempo: " + Mathf.Ceil(tiempoRestante).ToString();
    }

    public void TerminarJuego(bool victoria)
    {
        juegoTerminado = true; // detiene el cronómetro

        if (victoria)
        {
            if (winPanel != null) winPanel.SetActive(true);
        }
        else
        {
            if (losePanel != null) losePanel.SetActive(true);

            // 🚫 Desactivar movimiento del jugador al perder
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerMovement movimiento = player.GetComponent<PlayerMovement>();
                if (movimiento != null)
                {
                    movimiento.enabled = false;
                }
            }
        }
    }
}
