using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class EmotionChoice : MonoBehaviour
{
    public GameObject panelUI;
    public TextMeshProUGUI textoMision;

    [Header("Elementos UI iniciales")]
    public GameObject textMonologo;
    public GameObject textPregunta;
    public GameObject opcionesPanel;

    [Header("Textos")]
    public TextMeshProUGUI resultadoText;
    public GameObject continuarText;

    private PlayerInput playerInput;
    private bool esperandoContinuar = false;

    void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        ShowUI();

        // Estado inicial
        resultadoText.gameObject.SetActive(false);
        continuarText.SetActive(false);
    }

    void Update()
    {
        if (esperandoContinuar && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            HideUI();
        }
    }

    void ShowUI()
    {
        // 🔽 APAGAR textos al iniciar
        resultadoText.gameObject.SetActive(false);
        continuarText.SetActive(false);

        if (playerInput != null)
            playerInput.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HideUI()
    {
        panelUI.SetActive(false);

        textoMision.text = "<color=#FFCC00><b>NUEVO OBJETIVO:</b></color>\nHablar con Josué";

        if (playerInput != null)
            playerInput.enabled = true;

        if (playerInput != null)
            playerInput.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // -------- ELECCIONES --------

    public void ChooseAnsioso()
    {
        MostrarResultado(
            "Siento un nudo en el pecho… todo cambia tan rápido.\n¿Y si no estoy listo para lo que viene?"
        );
    }

    public void ChooseEmocionado()
    {
        MostrarResultado(
            "Siento mariposas en el pecho.\nTodo lo que viene me da un poco de miedo… pero también muchas ganas."
        );
    }

    public void ChooseConfundido()
    {
        MostrarResultado(
            "No sé si estoy feliz o nervioso.\nSupongo que es un poco de todo."
        );
    }

    // -------- LÓGICA --------

    void MostrarResultado(string texto)
    {
        textMonologo.SetActive(false);
        textPregunta.SetActive(false);
        opcionesPanel.SetActive(false);


        resultadoText.text = texto;
        resultadoText.gameObject.SetActive(true);
        continuarText.SetActive(true);

        esperandoContinuar = true;
    }
}
