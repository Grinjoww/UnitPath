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
    public GameObject avisoPresionarE;

    [Header("--- JUGADOR ---")]
    public Transform transformJugador; // <-- Volvemos a usar esto para la distancia
    public EstadisticasJugador scriptJugador;
    public float distanciaInteraccion = 3f; // Distancia para que aparezca la E

    [Header("--- CONFIGURACIÓN DE PREGUNTAS ---")]
    [TextArea] public string preguntaParaOpcionA = "Disculpa, ¿dónde dejo estas muestras biológicas?";
    public float penalizacionA = 10f;

    [TextArea] public string preguntaParaOpcionB = "Ehh... ¿el edificio de... salud?";
    public float penalizacionB = 15f;

    // ESTADOS INTERNOS
    private bool menuAbierto = false;
    private bool mostrandoResultado = false;
    private bool npcDesactivado = false;

    void Start()
    {
        if (avisoPresionarE != null) avisoPresionarE.SetActive(false);

        // Busca a Jaime automáticamente para medir la distancia
        if (transformJugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) transformJugador = playerObj.transform;
        }
    }

    void Update()
    {
        if (transformJugador == null) return;

        // 1. CERRAR LA VENTANA FINAL CON ESPACIO
        if (mostrandoResultado)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                CerrarTodo();
            }
            return;
        }

        // 2. MEDIR DISTANCIA (El método que SÍ funciona)
        float distancia = Vector3.Distance(transform.position, transformJugador.position);
        bool jugadorCerca = distancia <= distanciaInteraccion;

        // 3. LÓGICA DE LA "E" Y BLOQUEO PERMANENTE
        if (!npcDesactivado)
        {
            if (avisoPresionarE != null)
            {
                avisoPresionarE.SetActive(jugadorCerca && !menuAbierto);
            }

            if (jugadorCerca && Input.GetKeyDown(KeyCode.E) && !menuAbierto)
            {
                npcDesactivado = true; // 🔒 Muere para siempre
                if (avisoPresionarE != null) avisoPresionarE.SetActive(false);
                AbrirMenu();
            }
        }
    }

    void AbrirMenu()
    {
        menuAbierto = true;

        if (scriptJugador != null) scriptJugador.BloquearMovimiento(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        panelDialogo.SetActive(true);
        contenedorBotones.SetActive(true);
        if (textoPreguntaInicial != null) textoPreguntaInicial.SetActive(true);
        if (textoDondeHablaLaIA != null) textoDondeHablaLaIA.text = "";
    }

    public void SeleccionarOpcionA()
    {
        if (cerebroIA != null) cerebroIA.EnviarPregunta(preguntaParaOpcionA);
        EjecutarConsecuencia(penalizacionA);
    }

    public void SeleccionarOpcionB()
    {
        if (cerebroIA != null) cerebroIA.EnviarPregunta(preguntaParaOpcionB);
        EjecutarConsecuencia(penalizacionB);
    }

    void EjecutarConsecuencia(float daño)
    {
        menuAbierto = false;
        mostrandoResultado = true;

        contenedorBotones.SetActive(false);
        if (textoPreguntaInicial != null) textoPreguntaInicial.SetActive(false);

        if (scriptJugador != null) scriptJugador.adaptacionActual -= daño;
    }

    void CerrarTodo()
    {
        mostrandoResultado = false;
        menuAbierto = false;
        panelDialogo.SetActive(false);

        if (scriptJugador != null) scriptJugador.BloquearMovimiento(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GestorMisiones gestor = FindFirstObjectByType<GestorMisiones>();
        if (gestor != null)
        {
            gestor.ActualizarObjetivo("Sigue buscando el Centro Médico");
        }
    }
}