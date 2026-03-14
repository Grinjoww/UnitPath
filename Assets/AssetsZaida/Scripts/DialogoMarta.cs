using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StarterAssets;

public class DialogoMarta : MonoBehaviour
{
    [Header("Referencias UI (Reutiliza las de Josue)")]
    public GameObject panelConversacion;
    public TextMeshProUGUI textoDialogo;
    public GameObject panelOpciones;
    public Button[] botonesOpciones;
    public GameObject textoPresioneE;
    public Image imagenPerfil;
    public Sprite miSprite;
    public TextMeshProUGUI textoNombreUI; // <--- NUEVA: Referencia al componente de texto del nombre
    public string nombreNPC;

    [Header("Jugador")]
    public GameObject jugador;

    private ThirdPersonController _controller;
    private StarterAssetsInputs _inputs;

    [Header("Referencias Minijuego")]
    public GameObject minigameCanvas;

    private bool jugadorCerca = false;
    private bool dialogoActivo = false;
    private bool dialogoTerminado = false;
    private int estadoDialogo = 0;
        public void ReiniciarDialogo()
    {
        estadoDialogo = 0;
        dialogoActivo = true;
        gameObject.SetActive(true); // Se asegura de que el NPC o el panel se activen
        
    }

    void Start()
    {
        if (jugador != null)
        {
            _controller = jugador.GetComponent<ThirdPersonController>();
            _inputs = jugador.GetComponent<StarterAssetsInputs>();
        }

        // Aseguramos que todo empiece apagado
        panelConversacion.SetActive(false);
        panelOpciones.SetActive(false);
        textoPresioneE.SetActive(false);
    }

    void Update()
    {
        if (jugadorCerca && !dialogoActivo && !dialogoTerminado && Input.GetKeyDown(KeyCode.E))
        {
            IniciarDialogo();
        }
        else if (dialogoActivo && !panelOpciones.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            AvanzarDialogo();
        }
    }

    void IniciarDialogo()
    {
        imagenPerfil.sprite = miSprite;
        textoNombreUI.text = nombreNPC;
        estadoDialogo = 0;
        dialogoActivo = true;
        textoPresioneE.SetActive(false);
        panelConversacion.SetActive(true);

        SetPlayerControl(false);
        MostrarEstado();
    }

    void AvanzarDialogo()
    {
        estadoDialogo++;
        MostrarEstado();
    }

    void MostrarEstado()
    {
        textoDialogo.gameObject.SetActive(true);

        switch (estadoDialogo)
        {
            case 0:
                textoDialogo.text = "Ah… así que sigues por aquí. Supongo que hoy nadie se quiere ir tan rápido. \nLlegó el último día, ¿no?";
                break;
            case 1:
                textoDialogo.text = "<i>Hola profe… \nLa verdad no pensé que este día llegaría tan rápido.</i>";
                break;
            case 2:
                ConfigurarEleccionMarta();
                break;
            case 4: // Respuesta tras elección
                break;
            case 5:
                textoDialogo.text = "No tienes que demostrar nada ahora. Esta etapa no se trata de acertar a la primera, sino de ir entendiendo qué te gusta, qué no y qué puedes mejorar. \nCréeme que eso toma tiempo.";
                break;
            case 6:
                textoDialogo.text = "Organiza tus ideas, ve paso a paso. Y recuerda esto: pedir ayuda también es parte de aprender. \nToma, llévate estos globos… para que recuerdes esta etapa con cariño.";
                break;
            case 7:
                FinalizarDialogo();
                break;
        }
    }

    void ConfigurarEleccionMarta()
    {
        textoDialogo.gameObject.SetActive(false);
        panelOpciones.SetActive(true);

        // Textos de las opciones
        panelOpciones.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = "A veces siento que todos saben qué hacer… menos yo.";
        panelOpciones.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "Pienso que voy a perder el tiempo, si elijo una carrera que al final no sea para mí.";
        panelOpciones.transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = "No sé si soy capaz de enfrentar con todo lo que implica crecer.";

        AsignarBotones(
            () => SeleccionarRespuesta("Te voy a decir algo con sinceridad: muchos de los que parecen seguros, solo están fingiendo un poco. \nDudar no es algo malo. Significa que te importa decidir bien.", 4),
            () => SeleccionarRespuesta("Equivocarse no es perder tiempo. Es aprender algo que no se aprende leyendo ni escuchando a otros. \nEl tiempo no se pierde cuando estás aprendiendo sobre ti.", 4),
            () => SeleccionarRespuesta("Nadie se siente capaz todo el tiempo. \nLa universidad no es para los que “pueden con todo”, es para los que aprenden en el camino.", 4)
        );
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

    void SeleccionarRespuesta(string respuesta, int proximoEstado)
    {
        panelOpciones.SetActive(false);
        textoDialogo.gameObject.SetActive(true);
        textoDialogo.text = respuesta;
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
        SetPlayerControl(false);

        if (minigameCanvas != null)
        {
            minigameCanvas.SetActive(true);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dialogoTerminado)
        {
            jugadorCerca = true;
            if (!dialogoActivo)
            {
                textoPresioneE.SetActive(true);
                // Cambiamos el texto del panel de interacción dinámicamente
                textoPresioneE.GetComponentInChildren<TextMeshProUGUI>().text = "Presione \"E\" para interactuar con la Profe Marta";
            }
        }
    }
        private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) { jugadorCerca = false; textoPresioneE.SetActive(false); }
    }
}