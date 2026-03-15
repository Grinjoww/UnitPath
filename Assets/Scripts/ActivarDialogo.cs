using UnityEngine;
using TMPro;

public class ActivadorDialogo : MonoBehaviour
{
    [Header("--- UI DEL DIÁLOGO ---")]
    public GameObject panelDialogo;       // Arrastra aquí tu Panel de Diálogo
    public TMP_Text textoDialogo;         // Arrastra aquí el Texto TMP

    [Header("--- LA CONVERSACIÓN ---")]
    [TextArea(3, 5)]
    public string[] dialogoZona;          // Tus frases de la Doctora

    [Header("--- ACTIVAR AL FINAL ---")]
    public GameObject minijuegoParaActivar; // Arrastra aquí el Canvas del Minijuego de la Aguja

    // VARIABLES INTERNAS
    private int indiceFrase = 0;
    private bool hablando = false;
    private bool yaSeActivo = false;      // Para que no se repita el diálogo

    // Referencia al jugador para frenarlo
    private EstadisticasJugador statsJugador;

    void Start()
    {
        // Asegurarnos de que el minijuego empiece apagado (opcional)
        if (minijuegoParaActivar != null) minijuegoParaActivar.SetActive(false);
    }

    void Update()
    {
        // Si estamos hablando, escuchamos el Espacio o Click
        if (hablando)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                SiguienteFrase();
            }
        }
    }

    // 1. DETECTAR CUANDO ENTRAS A LA CÁPSULA
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaSeActivo)
        {
            statsJugador = other.GetComponent<EstadisticasJugador>();
            EmpezarConversacion();
        }
    }

    void EmpezarConversacion()
    {
        hablando = true;
        indiceFrase = 0;
        yaSeActivo = true; // Candado para que no ocurra 2 veces

        // Encender Panel
        if (panelDialogo != null) panelDialogo.SetActive(true);

        // Frenar a Jaime
        if (statsJugador != null) statsJugador.BloquearMovimiento(true);

        // Soltar mouse para leer tranquilo
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ActualizarTexto();
    }

    void SiguienteFrase()
    {
        indiceFrase++;
        if (indiceFrase < dialogoZona.Length)
        {
            ActualizarTexto();
        }
        else
        {
            TerminarConversacionYJugar();
        }
    }

    void ActualizarTexto()
    {
        if (textoDialogo != null)
            textoDialogo.text = dialogoZona[indiceFrase];
    }

    // --- AQUÍ OCURRE LA MAGIA DEL CAMBIO ---
    void TerminarConversacionYJugar()
    {
        hablando = false;

        // 1. Ocultar el diálogo
        if (panelDialogo != null) panelDialogo.SetActive(false);

        FindFirstObjectByType<GestorMisiones>().ApagarMisiones();

        // 2. ACTIVAR EL MINIJUEGO
        if (minijuegoParaActivar != null)
        {
            minijuegoParaActivar.SetActive(true);
            Debug.Log("💉 ¡Minijuego Activado!");

            // OJO: NO desbloqueamos el movimiento de Jaime aquí.
            // Porque si empieza el minijuego, quieres que Jaime siga quieto
            // mientras juega con el mouse.

            // El script de tu minijuego deberá encargarse de llamar a 
            // statsJugador.BloquearMovimiento(false) cuando Ganes o Pierdas.
        }
        else
        {
            // Si no hay minijuego, soltamos a Jaime (por seguridad)
            if (statsJugador != null) statsJugador.BloquearMovimiento(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}