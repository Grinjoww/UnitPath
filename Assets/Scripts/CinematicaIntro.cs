using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CinematicaIntro : MonoBehaviour
{
    [Header("--- UI DE LA INTRO ---")]
    public GameObject panelIntro;
    public Image imagenVisualizador;
    public TMP_Text textoHistoria;

    [Header("--- LA HISTORIA ---")]
    public Sprite[] imagenes;
    [TextArea(3, 5)]
    public string[] textos;

    [Header("--- AUDIO (Opcional) ---")]
    public AudioSource audioLatidos;
    public AudioSource audioAmbiente;

    [Header("--- CONEXIÓN AL JUGADOR ---")]
    public EstadisticasJugador statsJugador;

    // 👇 LA MAGIA ESTÁ AQUÍ 👇
    // Al ser 'static', esta variable no se borra cuando reinicias el nivel al perder.
    public static bool introYaVista = false;

    private int indiceActual = 0;

    void Start()
    {
        // 1. REVISAMOS SI YA VIMOS LA INTRO ANTES DE MORIR
        if (introYaVista)
        {
            // Apagamos el panel negro de inmediato para que veas el juego
            if (panelIntro != null) panelIntro.SetActive(false);

            // Nos aseguramos de que el jugador no esté bloqueado
            if (statsJugador != null) statsJugador.BloquearMovimiento(false);

            // Cortamos la ejecución aquí, no hacemos nada de lo de abajo
            return;
        }

        // --- SI ES LA PRIMERA VEZ QUE ABRIMOS EL JUEGO, HACEMOS LO NORMAL ---

        // Mostrar el panel y frenar al jugador
        if (panelIntro != null) panelIntro.SetActive(true);
        if (statsJugador != null) statsJugador.BloquearMovimiento(true);

        // Encender los sonidos de fondo
        if (audioLatidos != null) audioLatidos.Play();
        if (audioAmbiente != null) audioAmbiente.Play();

        // Mostrar la primera foto y texto
        indiceActual = 0;
        MostrarDiapositiva();
    }

    void Update()
    {
        if (panelIntro != null && !panelIntro.activeInHierarchy) return;

        // Avanzar al presionar Espacio o Clic
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            AvanzarHistoria();
        }
    }

    void MostrarDiapositiva()
    {
        if (indiceActual < imagenes.Length && imagenes[indiceActual] != null)
            imagenVisualizador.sprite = imagenes[indiceActual];

        if (indiceActual < textos.Length)
            textoHistoria.text = textos[indiceActual];
    }

    void AvanzarHistoria()
    {
        indiceActual++;

        if (indiceActual < imagenes.Length)
        {
            MostrarDiapositiva();
        }
        else
        {
            TerminarIntro();
        }
    }

    void TerminarIntro()
    {
        // 👇 GUARDAMOS QUE YA LA VIMOS PARA FUTUROS REINICIOS 👇
        introYaVista = true;

        // Apagar el panel y soltar al jugador
        if (panelIntro != null) panelIntro.SetActive(false);
        if (statsJugador != null) statsJugador.BloquearMovimiento(false);

        // Apagar los sonidos de tensión
        if (audioLatidos != null) audioLatidos.Stop();
        if (audioAmbiente != null) audioAmbiente.Stop();

        Debug.Log("🎬 ¡Fin de la intro! A jugar.");
    }
}