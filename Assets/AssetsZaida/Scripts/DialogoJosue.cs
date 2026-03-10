using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StarterAssets;

public class DialogoJosue : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelConversacion; // Panel Conversacion Josue
    public TextMeshProUGUI textoDialogo;  // Texto Dialogo
    public GameObject panelOpciones;     // Opciones Panel (HIJO del anterior)
    public Button[] botonesOpciones;
    public GameObject textoPresioneE;
    public TextMeshProUGUI[] textosOpciones;
    public TextMeshProUGUI textoMision;

    [Header("Jugador")]
    public GameObject jugador;

    [Header("Misiones y NPCs")]
    public GameObject npcMarta;

    private ThirdPersonController _controller;
    private StarterAssetsInputs _inputs;

    private bool jugadorCerca = false;
    private bool dialogoActivo = false;
    private bool dialogoTerminado = false; // <-- NUEVA VARIABLE DE CONTROL
    private int estadoDialogo = 0;

    void Start()
    {
        if (jugador != null)
        {
            _controller = jugador.GetComponent<ThirdPersonController>();
            _inputs = jugador.GetComponent<StarterAssetsInputs>();
        }

        panelConversacion.SetActive(false);
        panelOpciones.SetActive(false);
        textoPresioneE.SetActive(false);
    }

    void Update()
    {
        // Solo permite iniciar si NO ha terminado antes
        if (jugadorCerca && !dialogoActivo && !dialogoTerminado && Input.GetKeyDown(KeyCode.E))
        {
            IniciarDialogo();
        }
        else if (dialogoActivo && !panelOpciones.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            AvanzarDialogo();
        }
    }

    void IniciarDialogo()
    {
        estadoDialogo = 0;
        dialogoActivo = true;
        textoPresioneE.SetActive(false);
        panelConversacion.SetActive(true); // Activamos el padre

        SetPlayerControl(false); // Bloquea personaje, libera mouse
        MostrarEstado();
    }

    void AvanzarDialogo()
    {
        estadoDialogo++;
        MostrarEstado();
    }

    void MostrarEstado()
    {
        // IMPORTANTE: No desactivamos panelConversacion aquí para no matar a los hijos
        textoDialogo.gameObject.SetActive(true);

        switch (estadoDialogo)
        {
            case 0:
                textoDialogo.text = "Josué: Oye, ¿qué haces todavía aquí?";
                break;
            case 1:
                textoDialogo.text = "Jugador: <color=#FFFFCC><i>Nada… dando una vuelta antes de irme. \nCreo que voy a extrañar todo esto, ¿Y tú?</i></color>";
                break;
            case 2:
                textoDialogo.text = "Josué: Pienso lo mismo. \nTe cuento que me aceptaron en una universidad fuera de la ciudad.Todos me dicen que debería estar feliz y emocionado, pero no es tan simple. Estoy contento, sí... pero también un poco asustado, la verdad.";
                break;
            case 3:
                ConfigurarPrimeraEleccion();
                break;
            case 5:
                textoDialogo.text = "Josué: Me emociona la idea de irme, conocer gente nueva y todo eso… Pero también pienso en la sensación de despedirme de la vida que hice aquí. \nSupongo que ahora me toca empezar desde cero.";
                break;
            case 6:
                ConfigurarSegundaEleccion();
                break;
            case 8:
                textoDialogo.text = "Josué: Creo que nadie sale de aquí sabiendo realmente qué viene después. Supongo que solo nos queda ir descubriéndolo.";
                break;
            case 9:
                textoDialogo.text = "Jugador: <color=#FFFFCC><i>Tú tranquilo… de alguna forma vamos a aprender en el camino. Por ahora me despido. ¡Cuídate!</i></color>";
                break;
            case 10:
                // MONÓLOGO FINAL
                textoDialogo.text = "<color=#FFFFCC><i>“Creo que debería despedirme de la profe Marta, es muy sabia y la aprecio mucho, ¿Dónde estará?”</i></color>";
                break;
            case 11:
                FinalizarDialogo();
                break;
        }
    }

    void ConfigurarPrimeraEleccion()
    {
        textoDialogo.gameObject.SetActive(false); // Solo ocultamos el TEXTO, no el panel padre
        panelOpciones.SetActive(true);

        textosOpciones[0].text = "Creo que es algo normal sentirse así, es un cambio grande.";
        textosOpciones[1].text = "Sí... creo que a cualquiera le pasaría.";
        textosOpciones[2].text = "Hoy yo también me siento un poco raro.";

        AsignarBotones(() => SeleccionarRespuesta("Josué: Sí… nadie te dice esa parte.", 4),
                       () => SeleccionarRespuesta("Josué: Eso mismo pienso, pero cuando te toca vivirlo se siente distinto.", 4),
                       () => SeleccionarRespuesta("Josué: ¿Ves? Entonces no soy el único que se siente así", 4));
    }

    void ConfigurarSegundaEleccion()
    {
        textoDialogo.gameObject.SetActive(false);
        panelOpciones.SetActive(true);

        textosOpciones[0].text = "Yo creo que con el tiempo te vas a adaptar.";
        textosOpciones[1].text = "Debe dar un poco de miedo empezar así.";
        textosOpciones[2].text = "Al menos es un camino que tú elegiste.";

        AsignarBotones(() => SeleccionarRespuesta("Josué: Ojalá. Me gustaría pensar que sí.", 7),
                       () => SeleccionarRespuesta("Josué: Sí… bastante, pero imagino que es parte del proceso.", 7),
                       () => SeleccionarRespuesta("Josué: Sí, pensarlo así me da un poco de calma.", 7));
    }

    void AsignarBotones(UnityEngine.Events.UnityAction act1, UnityEngine.Events.UnityAction act2, UnityEngine.Events.UnityAction act3)
    {
        botonesOpciones[0].onClick.RemoveAllListeners();
        botonesOpciones[1].onClick.RemoveAllListeners();
        botonesOpciones[2].onClick.RemoveAllListeners();
        botonesOpciones[0].onClick.AddListener(act1);
        botonesOpciones[1].onClick.AddListener(act2);
        botonesOpciones[2].onClick.AddListener(act3);
    }

    void SeleccionarRespuesta(string respuestaJosue, int proximoEstado)
    {
        panelOpciones.SetActive(false);
        textoDialogo.gameObject.SetActive(true);
        textoDialogo.text = respuestaJosue;
        estadoDialogo = proximoEstado;
    }

    void SetPlayerControl(bool enabled)
    {
        if (_controller != null) _controller.enabled = enabled;
        if (_inputs != null)
        {
            _inputs.cursorLocked = enabled;
            _inputs.cursorInputForLook = enabled;
        }
        Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !enabled;
    }

    void FinalizarDialogo()
    {
        dialogoActivo = false;
        dialogoTerminado = true;
        panelConversacion.SetActive(false);
        SetPlayerControl(true);

        if (npcMarta != null) npcMarta.SetActive(true); // Aparece la profe

        if (textoMision != null)
        {
            textoMision.text = "<color=#57FF57><b>OBJETIVO ACTUALIZADO:</b></color>\nEncontrar a la profe Marta y hablar con ella";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Solo muestra el texto de "E" si el diálogo no ha terminado nunca
        if (other.CompareTag("Player") && !dialogoTerminado)
        {
            jugadorCerca = true;
            if (!dialogoActivo) textoPresioneE.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) { jugadorCerca = false; textoPresioneE.SetActive(false); } }
}