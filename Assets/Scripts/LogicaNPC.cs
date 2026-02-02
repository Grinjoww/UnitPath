using UnityEngine;
using TMPro;

public class LogicaNPC : MonoBehaviour
{
    [Header("--- UI GENERAL ---")]
    public GameObject panelDialogo;
    public GameObject contenedorBotones;
    public GameObject textoPreguntaInicial; // Opcional

    [Header("--- OBJETOS DE RESPUESTA ---")]
    public GameObject objetoRespuestaA;
    public GameObject objetoRespuestaB;

    [Header("--- JUGADOR ---")]
    public MovimientoJugador scriptJugador;

    [Header("--- CONFIGURACIÓN ---")]
    public float penalizacionA = 10f;
    public float penalizacionB = 15f;

    // ESTADOS INTERNOS
    private bool jugadorCerca = false;
    private bool menuAbierto = false;       // Estamos eligiendo A o B
    private bool mostrandoResultado = false; // Ya elegimos y estamos leyendo la respuesta

    void Update()
    {
        // 1. ABRIR EL MENÚ (Solo si no estamos haciendo nada más)
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E) && !menuAbierto && !mostrandoResultado)
        {
            AbrirMenu();
        }

        // 2. CERRAR EL RESULTADO CON ESPACIO (Lo que pediste)
        if (mostrandoResultado)
        {
            // Si presionas Espacio O Enter, se cierra al instante
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
            {
                CerrarTodo();
            }
        }
    }

    void AbrirMenu()
    {
        menuAbierto = true;

        // IMPORTANTE: No usamos Time.timeScale = 0.
        // En su lugar, bloqueamos a Jaime usando la variable que hiciste pública.
        if (scriptJugador != null)
        {
            scriptJugador.hablando = true; // Jaime deja de caminar, pero respira
            scriptJugador.GetComponent<Rigidbody>().linearVelocity = Vector3.zero; // Frenado en seco
        }

        panelDialogo.SetActive(true);
        contenedorBotones.SetActive(true);
        if (textoPreguntaInicial != null) textoPreguntaInicial.SetActive(true);

        // Asegurar que las respuestas estén apagadas
        if (objetoRespuestaA != null) objetoRespuestaA.SetActive(false);
        if (objetoRespuestaB != null) objetoRespuestaB.SetActive(false);
    }

    public void SeleccionarOpcionA()
    {
        EjecutarConsecuencia(objetoRespuestaA, penalizacionA);
    }

    public void SeleccionarOpcionB()
    {
        EjecutarConsecuencia(objetoRespuestaB, penalizacionB);
    }

    void EjecutarConsecuencia(GameObject respuestaAActivar, float daño)
    {
        // Cambiamos de fase: de Menú a Resultado
        menuAbierto = false;
        mostrandoResultado = true;

        // Ocultar botones
        contenedorBotones.SetActive(false);
        if (textoPreguntaInicial != null) textoPreguntaInicial.SetActive(false);

        // Mostrar la respuesta elegida
        if (respuestaAActivar != null) respuestaAActivar.SetActive(true);

        // Aplicar daño
        if (scriptJugador != null) scriptJugador.adaptacionActual -= daño;

        // YA NO usamos Corrutina de tiempo.
        // Ahora esperamos en el Update() a que presiones Espacio.
    }

    void CerrarTodo()
    {
        mostrandoResultado = false;
        menuAbierto = false;

        panelDialogo.SetActive(false);
        if (objetoRespuestaA != null) objetoRespuestaA.SetActive(false);
        if (objetoRespuestaB != null) objetoRespuestaB.SetActive(false);

        // Liberamos a Jaime para que camine de nuevo
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
            // Si te alejas a media charla, cerramos todo por seguridad
            if (menuAbierto || mostrandoResultado) CerrarTodo();
        }
    }
}