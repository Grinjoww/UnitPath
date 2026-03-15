using UnityEngine;
using TMPro;

public class ZaidaGuia : MonoBehaviour
{
    [Header("--- UI DEL DIÁLOGO ---")]
    public GameObject panelDialogo;
    public TMP_Text textoDialogo;

    // 👇 AQUÍ ESTÁ LA NUEVA CASILLA PARA EL TEXTO "E" 👇
    public GameObject avisoPresionarE;

    [Header("--- JUGADOR (ARRASTRA AQUÍ A TU PLAYER) ---")]
    public EstadisticasJugador statsJugador;

    [Header("--- MOVIMIENTO DE ZAIDA ---")]
    public MovimientoZaida scriptMovimiento;

    [Header("--- LOS DIÁLOGOS ---")]
    [TextArea(3, 5)]
    public string[] frasesDeZaida;

    [Header("--- CONFIGURACIÓN ---")]
    public float distanciaParaHablar = 3f;
    public float puntosCuracionConversacion = 20f;
    public float regeneracionPorSegundo = 5f;

    // VARIABLES INTERNAS
    private int indiceFrase = 0;
    private bool hablando = false;
    private bool conversacionCompletada = false;

    void Start()
    {
        if (panelDialogo != null) panelDialogo.SetActive(false);
        if (avisoPresionarE != null) avisoPresionarE.SetActive(false); // Empieza apagado
    }

    void Update()
    {
        if (statsJugador == null) return;

        float distancia = Vector3.Distance(transform.position, statsJugador.transform.position);
        bool jugadorCerca = distancia <= distanciaParaHablar;

        // 👇 CONTROL AUTOMÁTICO DE LA "E" 👇
        if (avisoPresionarE != null)
        {
            // Solo se muestra si estás cerca, NO estás hablando, y NO has terminado la charla
            avisoPresionarE.SetActive(jugadorCerca && !hablando && !conversacionCompletada);
        }

        if (jugadorCerca)
        {
            // REGENERACIÓN
            statsJugador.adaptacionActual += regeneracionPorSegundo * Time.deltaTime;
            if (statsJugador.adaptacionActual > 100f) statsJugador.adaptacionActual = 100f;

            // DIÁLOGO
            if (!hablando && !conversacionCompletada && Input.GetKeyDown(KeyCode.E))
            {
                EmpezarConversacion();
            }
            else if (hablando && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
            {
                SiguienteFrase();
            }
        }
        else
        {
            if (hablando) CancelarConversacion();
        }
    }

    void EmpezarConversacion()
    {
        hablando = true;
        indiceFrase = 0;

        // Apagamos la E manualmente por seguridad al empezar a hablar
        if (avisoPresionarE != null) avisoPresionarE.SetActive(false);

        if (panelDialogo != null) panelDialogo.SetActive(true);

        if (statsJugador != null) statsJugador.BloquearMovimiento(true);

        ActualizarTexto();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void SiguienteFrase()
    {
        indiceFrase++;
        if (indiceFrase < frasesDeZaida.Length)
        {
            ActualizarTexto();
        }
        else
        {
            TerminarConversacionConExito();
        }
    }

    void ActualizarTexto()
    {
        if (textoDialogo != null)
            textoDialogo.text = frasesDeZaida[indiceFrase];
    }

    void TerminarConversacionConExito()
    {
        hablando = false;
        conversacionCompletada = true;

        if (panelDialogo != null) panelDialogo.SetActive(false);

        FindFirstObjectByType<GestorMisiones>().ActualizarObjetivo("Sigue a Zaida, ella te guiará al centro médico.");

        if (statsJugador != null)
        {
            statsJugador.BloquearMovimiento(false);
            statsJugador.adaptacionActual += puntosCuracionConversacion;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (scriptMovimiento != null)
        {
            scriptMovimiento.IniciarViaje();
        }
    }

    void CancelarConversacion()
    {
        hablando = false;
        if (panelDialogo != null) panelDialogo.SetActive(false);

        if (statsJugador != null) statsJugador.BloquearMovimiento(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaParaHablar);
    }
}