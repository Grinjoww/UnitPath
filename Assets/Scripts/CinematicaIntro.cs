using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CinematicaIntro : MonoBehaviour
{
    [Header("--- UI DE LA INTRO ---")]
    public GameObject panelIntro;
    public Image imagenVisualizador;
    public TMP_Text textoHistoria;

    // 👇 NUEVA CASILLA PARA TU TEXTO DE "ESPACIO" 👇
    public GameObject textoContinuar;

    [Header("--- LA HISTORIA ---")]
    public Sprite[] imagenes;
    [TextArea(3, 5)]
    public string[] textos;

    [Header("--- EFECTO TEXTO ---")]
    public float velocidadEscritura = 0.03f;
    private Coroutine corrutinaEscritura;
    private bool estaEscribiendo = false;

    [Header("--- AUDIO (Opcional) ---")]
    public AudioSource audioLatidos;
    public AudioSource audioAmbiente;

    [Header("--- CONEXIÓN AL JUGADOR ---")]
    public EstadisticasJugador statsJugador;

    public static bool introYaVista = false;
    private int indiceActual = 0;

    void Start()
    {
        if (introYaVista)
        {
            if (panelIntro != null) panelIntro.SetActive(false);
            if (statsJugador != null) statsJugador.BloquearMovimiento(false);

            GestorMisiones gestor = FindFirstObjectByType<GestorMisiones>();
            if (gestor != null) gestor.ActualizarObjetivo("Encuentra el Centro Médico (Pregunta a los estudiantes).");

            return;
        }

        if (panelIntro != null) panelIntro.SetActive(true);
        if (statsJugador != null) statsJugador.BloquearMovimiento(true);

        // Nos aseguramos de que el aviso de ESPACIO empiece apagado
        if (textoContinuar != null) textoContinuar.SetActive(false);

        if (audioLatidos != null) audioLatidos.Play();
        if (audioAmbiente != null) audioAmbiente.Play();

        indiceActual = 0;
        MostrarDiapositiva();
    }

    void Update()
    {
        if (panelIntro != null && !panelIntro.activeInHierarchy) return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (estaEscribiendo)
            {
                // Si el jugador da clic para saltar la animación:
                if (corrutinaEscritura != null) StopCoroutine(corrutinaEscritura);
                textoHistoria.text = textos[indiceActual];
                estaEscribiendo = false;

                // Prendemos el aviso de ESPACIO de inmediato
                if (textoContinuar != null) textoContinuar.SetActive(true);
            }
            else
            {
                AvanzarHistoria();
            }
        }
    }

    void MostrarDiapositiva()
    {
        if (indiceActual < imagenes.Length && imagenes[indiceActual] != null)
            imagenVisualizador.sprite = imagenes[indiceActual];

        // Apagamos el aviso de ESPACIO cada vez que cambiamos de foto
        if (textoContinuar != null) textoContinuar.SetActive(false);

        if (indiceActual < textos.Length)
        {
            if (corrutinaEscritura != null) StopCoroutine(corrutinaEscritura);
            corrutinaEscritura = StartCoroutine(EfectoMaquinaDeEscribir(textos[indiceActual]));
        }
    }

    IEnumerator EfectoMaquinaDeEscribir(string frase)
    {
        estaEscribiendo = true;
        textoHistoria.text = "";

        foreach (char letra in frase.ToCharArray())
        {
            textoHistoria.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        estaEscribiendo = false;

        // ¡Terminó de escribir sola! Prendemos el aviso de ESPACIO
        if (textoContinuar != null) textoContinuar.SetActive(true);
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
        introYaVista = true;

        if (panelIntro != null) panelIntro.SetActive(false);
        if (statsJugador != null) statsJugador.BloquearMovimiento(false);

        if (audioLatidos != null) audioLatidos.Stop();
        if (audioAmbiente != null) audioAmbiente.Stop();

        GestorMisiones gestor = FindFirstObjectByType<GestorMisiones>();
        if (gestor != null) gestor.ActualizarObjetivo("Encuentra el Centro Médico (Pregunta a los estudiantes).");
    }
}