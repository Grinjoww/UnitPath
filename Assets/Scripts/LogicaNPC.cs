using UnityEngine;
using TMPro; // Aunque no usemos strings directos, mantenemos esto por si acaso

public class LogicaNPC : MonoBehaviour
{
    [Header("--- UI GENERAL ---")]
    public GameObject panelDialogo;
    public GameObject contenedorBotones;   // El grupo con los botones A y B
    public GameObject textoPreguntaInicial; // (Opcional) El texto que dice "¿Qué quieres hacer?" al inicio

    [Header("--- OBJETOS DE RESPUESTA (Arrastra tus Textos aquí) ---")]
    // Aquí arrastras los objetos de texto que YA creaste y acomodaste en Unity
    public GameObject objetoRespuestaA;
    public GameObject objetoRespuestaB;

    [Header("--- JUGADOR ---")]
    public MovimientoJugador scriptJugador;

    [Header("--- CONSECUENCIAS (Daño) ---")]
    public float penalizacionA = 10f;
    public float penalizacionB = 15f;

    private bool jugadorCerca = false;
    private bool dialogoActivo = false;

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E) && !dialogoActivo)
        {
            AbrirMenu();
        }
    }

    void AbrirMenu()
    {
        dialogoActivo = true;
        Time.timeScale = 0f;
        panelDialogo.SetActive(true);

        // 1. Mostrar botones y la pregunta inicial
        contenedorBotones.SetActive(true);
        if (textoPreguntaInicial != null) textoPreguntaInicial.SetActive(true);

        // 2. Asegurarnos que las respuestas estén APAGADAS al inicio
        if (objetoRespuestaA != null) objetoRespuestaA.SetActive(false);
        if (objetoRespuestaB != null) objetoRespuestaB.SetActive(false);
    }

    public void SeleccionarOpcionA()
    {
        // Pasamos el OBJETO A
        EjecutarConsecuencia(objetoRespuestaA, penalizacionA);
    }

    public void SeleccionarOpcionB()
    {
        // Pasamos el OBJETO B
        EjecutarConsecuencia(objetoRespuestaB, penalizacionB);
    }

    // Ahora esta función recibe un GameObject, no un string
    void EjecutarConsecuencia(GameObject respuestaAActivar, float daño)
    {
        // 1. Ocultar botones y la pregunta inicial
        contenedorBotones.SetActive(false);
        if (textoPreguntaInicial != null) textoPreguntaInicial.SetActive(false);

        // 2. ENCENDER el texto de respuesta correcto (el que ya acomodaste bonito)
        if (respuestaAActivar != null)
        {
            respuestaAActivar.SetActive(true);
        }

        // 3. Aplicar daño
        if (scriptJugador != null)
        {
            scriptJugador.adaptacionActual -= daño;
        }

        // 4. Cerrar todo en 3 segundos
        StartCoroutine(CerrarDialogoDelay());
    }

    System.Collections.IEnumerator CerrarDialogoDelay()
    {
        yield return new WaitForSecondsRealtime(4f); // Les damos 4 segundos para leer bien

        // Apagamos todo para limpiar
        panelDialogo.SetActive(false);
        if (objetoRespuestaA != null) objetoRespuestaA.SetActive(false);
        if (objetoRespuestaB != null) objetoRespuestaB.SetActive(false);

        Time.timeScale = 1f;
        dialogoActivo = false;
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
        if (other.CompareTag("Player")) jugadorCerca = false;
    }
}