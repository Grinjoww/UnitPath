using UnityEngine;
using TMPro;

public class LogicaNPC : MonoBehaviour
{
    [Header("--- CONEXIÓN CON IA (NUEVO) ---")]
    public GeminiChat cerebroIA;         // Arrastra aquí al objeto "GeminiManager"
    public TMP_Text textoDondeHablaLaIA; // El cuadro de texto del Canvas donde saldrá la respuesta

    [Header("--- UI GENERAL (IGUAL QUE ANTES) ---")]
    public GameObject panelDialogo;
    public GameObject contenedorBotones;
    public GameObject textoPreguntaInicial;

    // YA NO NECESITAMOS "objetoRespuestaA" NI "B" PORQUE EL TEXTO ES DINÁMICO
    // public GameObject objetoRespuestaA; // Borrado
    // public GameObject objetoRespuestaB; // Borrado

    [Header("--- JUGADOR (IGUAL) ---")]
    public MovimientoJugador scriptJugador;

    [Header("--- CONFIGURACIÓN DE PREGUNTAS (NUEVO) ---")]
    // Aquí escribes lo que quieres que el botón le pregunte a la IA
    [TextArea] public string preguntaParaOpcionA = "Disculpa, ¿dónde dejo estas muestras biológicas?";
    public float penalizacionA = 10f;

    [TextArea] public string preguntaParaOpcionB = "Ehh... ¿el edificio de... salud?";
    public float penalizacionB = 15f;

    // ESTADOS INTERNOS (IGUAL)
    private bool jugadorCerca = false;
    private bool menuAbierto = false;
    private bool mostrandoResultado = false;

    void Update()
    {
        // 1. ABRIR EL MENÚ (Igual que tu script)
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E) && !menuAbierto && !mostrandoResultado)
        {
            AbrirMenu();
        }

        // 2. CERRAR EL RESULTADO CON ESPACIO (Igual que tu script)
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

        // BLOQUEO DE JAIME (Tu lógica intacta)
        if (scriptJugador != null)
        {
            scriptJugador.hablando = true;
            // Nota: Si usas Unity viejo es .velocity, si usas Unity 6 es .linearVelocity.
            // Dejo el que tú tenías:
            if (scriptJugador.GetComponent<Rigidbody>() != null)
                scriptJugador.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }

        panelDialogo.SetActive(true);
        contenedorBotones.SetActive(true);
        if (textoPreguntaInicial != null) textoPreguntaInicial.SetActive(true);

        // Limpiamos el texto para que no salga la respuesta anterior
        if (textoDondeHablaLaIA != null) textoDondeHablaLaIA.text = "";
    }

    // --- AQUÍ ESTÁ LA MAGIA DE LA IA ---

    public void SeleccionarOpcionA()
    {
        // 1. Mandamos la pregunta A a Gemini
        if (cerebroIA != null)
            cerebroIA.EnviarPregunta(preguntaParaOpcionA);

        // 2. Ejecutamos consecuencias (Daño y UI)
        EjecutarConsecuencia(penalizacionA);
    }

    public void SeleccionarOpcionB()
    {
        // 1. Mandamos la pregunta B a Gemini
        if (cerebroIA != null)
            cerebroIA.EnviarPregunta(preguntaParaOpcionB);

        // 2. Ejecutamos consecuencias
        EjecutarConsecuencia(penalizacionB);
    }

    void EjecutarConsecuencia(float daño)
    {
        menuAbierto = false;
        mostrandoResultado = true;

        // Ocultar botones y pregunta inicial
        contenedorBotones.SetActive(false);
        if (textoPreguntaInicial != null) textoPreguntaInicial.SetActive(false);

        // NOTA: Ya no activamos objetoRespuestaA/B porque Gemini escribirá
        // directamente en "textoDondeHablaLaIA".

        // Aplicar daño (Tu lógica intacta)
        if (scriptJugador != null) scriptJugador.adaptacionActual -= daño;
    }

    void CerrarTodo()
    {
        mostrandoResultado = false;
        menuAbierto = false;

        panelDialogo.SetActive(false);

        // Liberamos a Jaime (Tu lógica intacta)
        if (scriptJugador != null) scriptJugador.hablando = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (scriptJugador == null) scriptJugador = other.GetComponent<MovimientoJugador>();
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