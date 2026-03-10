using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class IdeaMinigameManager : MonoBehaviour
{
    [Header("Configuración Inicial")]
    public GameObject[] corazones;
    public TextMeshProUGUI textoPensamientoInterno;
    public Image flashDano; // Arrastra el objeto "FlashDano" aquí

    [Header("Pantallas Finales")]
    public GameObject panelFinal;
    public TextMeshProUGUI textoFinal;

    [Header("Variables")]
    public int vidas = 3;
    public int globosNecesarios = 4;
    private int globosActuales = 0;
    private bool esperandoReintento = false;

    void Start() { ReiniciarVariables(); }

    void Update()
    {
        if (esperandoReintento && Input.GetKeyDown(KeyCode.Space))
        {
            ReiniciarMinijuego();
        }
    }

    public void ProcesarPensamiento(bool esPesado, string mensaje)
    {
        textoPensamientoInterno.text = mensaje;
        StopAllCoroutines();
        StartCoroutine(LimpiarPensamiento());

        if (esPesado)
        {
            vidas--;
            ActualizarVidas();
            StartCoroutine(EfectoPantallaRoja());
            if (vidas <= 0) Derrota();
        }
        else
        {
            globosActuales++;
            if (globosActuales >= globosNecesarios) Victoria();
        }
    }

    void ActualizarVidas()
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            corazones[i].SetActive(i < vidas);
        }
    }

    IEnumerator EfectoPantallaRoja()
    {
        flashDano.color = new Color(1, 0, 0, 0.4f); // Rojo semi-transparente
        yield return new WaitForSeconds(0.1f);
        flashDano.color = new Color(1, 0, 0, 0); // Invisible
    }

    IEnumerator LimpiarPensamiento()
    {
        yield return new WaitForSeconds(4f); // Más tiempo para leer
        textoPensamientoInterno.text = "";
    }

    void Derrota()
    {
        panelFinal.SetActive(true);
        textoFinal.text = "<b><size=140%><color=#B22222>Inténtalo de nuevo</color></size></b>\nA veces intentamos cargar con pensamientos que nos abruman.\nNo todo tiene que ir contigo ahora.\n\n<size=30><color=#333333>Presiona ESPACIO para intentar de nuevo</color></size>";
        esperandoReintento = true;
    }

    void ReiniciarMinijuego()
    {
        esperandoReintento = false;
        panelFinal.SetActive(false);
        ReiniciarVariables();
        // Reactiva todos los pensamientos que fueron desactivados
        DraggableThought[] todos = Resources.FindObjectsOfTypeAll<DraggableThought>();
        foreach (var t in todos) { t.gameObject.SetActive(true); t.ResetPosition(); }
    }

    void ReiniciarVariables()
    {
        vidas = 3;
        globosActuales = 0;
        ActualizarVidas();
        panelFinal.SetActive(false);
    }

    void Victoria()
    {
        panelFinal.SetActive(true);
        textoFinal.text = "<b><size=140%><color=#2E8B57>¡Lo lograste!</color></size></b>\nNo siempre se puede controlar los pensamientos…\nPero sí se puede elegir qué nos ayuda a seguir.";
    }
}