using UnityEngine;
using TMPro;

public class LogicaNPC : MonoBehaviour
{
    [Header("--- CONEXIÓN CON IA ---")]
    public GeminiChat cerebroIA;
    public TMP_Text textoDondeHablaLaIA;

    [Header("--- UI GENERAL ---")]
    public GameObject panelDialogo;
    public GameObject contenedorBotones;
    public GameObject textoPreguntaInicial;

    [Header("--- JUGADOR (CAMBIO IMPORTANTE) ---")]
    // Antes era: public MovimientoJugador scriptJugador;
    // Ahora es:
    public EstadisticasJugador scriptJugador;

    [Header("--- CONFIGURACIÓN DE PREGUNTAS ---")]
    [TextArea] public string preguntaParaOpcionA = "Disculpa, ¿dónde dejo estas muestras biológicas?";
    public float penalizacionA = 10f;

    [TextArea] public string preguntaParaOpcionB = "Ehh... ¿el edificio de... salud?";
    public float penalizacionB = 15f;

    // ESTADOS INTERNOS
    private bool jugadorCerca = false;
    private bool menuAbierto = false;
    private bool mostrandoResultado = false;

    void Update()
    {
        // 1. ABRIR EL MENÚ
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E) && !menuAbierto && !mostrandoResultado)
        {
            AbrirMenu();
        }

        // 2. CERRAR EL RESULTADO CON ESPACIO
        if (mostrandoResultado)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
            {
                CerrarTodo();
            }
        }
    }

    void AbrirMenu()
    {
        menuAbierto = true;

        // --- CAMBIO: USAMOS EL NUEVO SISTEMA DE BLOQUEO ---
        if (scriptJugador != null)
        {
            // Llamamos a la función que frena al Starter Assets
            scriptJugador.BloquearMovimiento(true);
        }

        // --- IMPORTANTE: LIBERAR EL MOUSE ---
        // Starter Assets atrapa el cursor. Necesitamos soltarlo para dar clic a los botones.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        panelDialogo.SetActive(true);
        contenedorBotones.SetActive(true);
        if (textoPreguntaInicial != null) textoPreguntaInicial.SetActive(true);

        // Limpiamos el texto anterior de la IA
        if (textoDondeHablaLaIA != null) textoDondeHablaLaIA.text = "";
    }

    // --- INTERACCIÓN CON GEMINI ---

    public void SeleccionarOpcionA()
    {
        if (cerebroIA != null)
            cerebroIA.EnviarPregunta(preguntaParaOpcionA);

        EjecutarConsecuencia(penalizacionA);
    }

    public void SeleccionarOpcionB()
    {
        if (cerebroIA != null)
            cerebroIA.EnviarPregunta(preguntaParaOpcionB);

        EjecutarConsecuencia(penalizacionB);
    }

    void EjecutarConsecuencia(float daño)
    {
        menuAbierto = false;
        mostrandoResultado = true;

        contenedorBotones.SetActive(false);
        if (textoPreguntaInicial != null) textoPreguntaInicial.SetActive(false);

        // Aplicamos daño (Esto sigue funcionando igual porque EstadisticasJugador tiene esa variable)
        if (scriptJugador != null) scriptJugador.adaptacionActual -= daño;
    }

    void CerrarTodo()
    {
        mostrandoResultado = false;
        menuAbierto = false;

        panelDialogo.SetActive(false);

        // --- CAMBIO: DESBLOQUEAR ---
        if (scriptJugador != null)
        {
            scriptJugador.BloquearMovimiento(false); // Jaime puede caminar de nuevo
        }

        // VOLVER A ATRAPAR EL MOUSE (Para poder girar la cámara)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Starter Assets usa el tag "Player" en el objeto padre, así que esto funcionará bien
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            // Buscamos el nuevo script en el objeto que entró
            if (scriptJugador == null) scriptJugador = other.GetComponent<EstadisticasJugador>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (menuAbierto || mostrandoResultado) CerrarTodo();
        }
    }
}