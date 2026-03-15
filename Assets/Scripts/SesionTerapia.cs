using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SesionTerapia : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textoPsicologa;
    public Button[] botones;
    public TextMeshProUGUI[] textoBotones;
    public Image rellenoBarra;
    public Image spriteEmocion;
    public TextMeshProUGUI textoEmocion;
    public Sprite[] spritesEmociones; // 0=feliz, 1=neutral, 2=incomodo

    [Header("Pantalla Final")]
    public GameObject panelFinal;
    public TextMeshProUGUI textoReflexion;

    private float estres = 1f;
    private int rondaActual = 0;

    // Datos de cada ronda
    private string[] preguntasPsicologa = {
        "¿Cómo te sientes al estar aquí hoy?",
        "¿Cómo ha sido para ti la universidad últimamente?",
        "Cuando las cosas se ponen difíciles… ¿qué pasa por tu cabeza?",
        "¿Qué haces normalmente cuando te sientes así?",
        "Cierra los ojos si quieres… inhala lento… y suelta."
    };

    private string[][] opciones = {
        new string[]{ "La verdad, un poco nervioso/a", "No sé bien qué decir", "Cansado/a… más que nada" },
        new string[]{ "Siento que todo se me acumula", "Me esfuerzo, pero no es suficiente", "Prefiero no pensar mucho en eso" },
        new string[]{ "Que no voy a poder con todo", "Que los demás pueden y yo no", "Un ruido constante que no me deja pensar" },
        new string[]{ "Me lo guardo y sigo", "Me distraigo para no pensar", "A veces intento hablarlo" },
        new string[]{ "Intentarlo", "Me cuesta, pero lo intento igual", "" }
    };

    private string[][] respuestasPsicologa = {
        new string[]{
            "Tiene mucho sentido. Hablar con alguien que no conoces no es fácil. Que estés aquí dice bastante de ti.",
            "Eso pasa más de lo que crees. Podemos ir despacio.",
            "Ese cansancio no siempre es solo físico. Muchas veces viene de sostener demasiadas cosas."
        },
        new string[]{
            "Cuando todo se acumula, la mente entra en modo supervivencia. No porque seas débil.",
            "Dar lo mejor y sentir que no alcanza puede desgastar bastante la autoestima.",
            "Evitar pensar es una forma de protegerte. Solo una señal de que algo pesa más de lo que parece."
        },
        new string[]{
            "Ese pensamiento suele aparecer cuando estamos agotados. No siempre refleja la realidad.",
            "Compararse es muy duro. Desde afuera nunca vemos las batallas internas de los demás.",
            "Cuando la mente no se calla, el cuerpo tampoco descansa. Es una señal de estrés sostenido."
        },
        new string[]{
            "Aguantar en silencio cansa más de lo que parece. Nadie debería cargar todo solo.",
            "Distraerse ayuda por momentos, pero no siempre hay otro lugar donde soltar lo que pesa.",
            "Hablar, aunque sea un poco, suele aliviar. No siempre soluciona, pero permite respirar."
        },
        new string[]{
            "No importa cómo salga. Lo importante es darte permiso para bajar el ritmo.",
            "No importa cómo salga. Lo importante es darte permiso para bajar el ritmo.",
            ""
        }
    };

    // Cuánto baja el estrés por opción 
    private float[][] bajadaEstres = {
        new float[]{ 0.15f, 0.10f, 0.12f },
        new float[]{ 0.15f, 0.12f, 0.08f },
        new float[]{ 0.12f, 0.15f, 0.10f },
        new float[]{ 0.08f, 0.10f, 0.15f },
        new float[]{ 0.15f, 0.12f, 0f }
    };

    // Emoción por opción 
    private int[][] emociones = {
        new int[]{ 1, 1, 0 },
        new int[]{ 2, 1, 0 },
        new int[]{ 1, 2, 0 },
        new int[]{ 0, 1, 2 },
        new int[]{ 2, 1, 0 }
    };

    private string[] textosEmocion = { "¡Bien!", "Okay...", "Hmm..." };

    void OnEnable()
    {
        estres = 1f;
        rondaActual = 0;
        ActualizarBarra();
        MostrarRonda();

        if (panelFinal != null) panelFinal.SetActive(false);

        // Desactivar personaje
        var playerController = FindFirstObjectByType<StarterAssets.ThirdPersonController>();
        var playerInput = FindFirstObjectByType<StarterAssets.StarterAssetsInputs>();
        if (playerController != null) playerController.enabled = false;
        if (playerInput != null) playerInput.enabled = false;
        var playerInputComponent = FindFirstObjectByType<StarterAssets.ThirdPersonController>()
    ?.GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if (playerInputComponent != null) playerInputComponent.DeactivateInput();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private bool esperandoContinuar = false;

    void Update()
    {
        if (esperandoContinuar)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                esperandoContinuar = false;
                rondaActual++;
                if (rondaActual >= preguntasPsicologa.Length)
                    MostrarFinal();
                else
                {
                    foreach (var b in botones) b.gameObject.SetActive(true);
                    foreach (var b in botones) b.interactable = true;
                    MostrarRonda();
                }
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && botones[0].gameObject.activeSelf) ElegirOpcion(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && botones[1].gameObject.activeSelf) ElegirOpcion(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) && botones[2].gameObject.activeSelf) ElegirOpcion(2);
    }
    void MostrarRonda()
    {
        textoPsicologa.text = preguntasPsicologa[rondaActual];

        // Ocultar emoción
        if (spriteEmocion != null) spriteEmocion.gameObject.SetActive(false);
        if (textoEmocion != null) textoEmocion.gameObject.SetActive(false);

        for (int i = 0; i < botones.Length; i++)
        {
            string opcion = opciones[rondaActual][i];
            if (string.IsNullOrEmpty(opcion))
            {
                botones[i].gameObject.SetActive(false);
            }
            else
            {
                botones[i].gameObject.SetActive(true);
                textoBotones[i].text = (i + 1) + ". " + opcion;
            }
        }
    }

    public void ElegirOpcion(int indice)
    {
        // Bajar estrés
        estres -= bajadaEstres[rondaActual][indice];
        estres = Mathf.Clamp01(estres);
        ActualizarBarra();

        // Mostrar respuesta del psicólogo
        textoPsicologa.text = respuestasPsicologa[rondaActual][indice];

        // Mostrar emoción
        if (spriteEmocion != null)
        {
            spriteEmocion.gameObject.SetActive(true);
            int emocionIndex = emociones[rondaActual][indice];
            if (spritesEmociones != null && spritesEmociones.Length > emocionIndex)
                spriteEmocion.sprite = spritesEmociones[emocionIndex];
        }
        if (textoEmocion != null)
        {
            textoEmocion.gameObject.SetActive(true);
            textoEmocion.text = textosEmocion[emociones[rondaActual][indice]];
        }

        foreach (var b in botones) b.interactable = false;
        StartCoroutine(SiguienteRonda());
    }

    IEnumerator SiguienteRonda()
    {
        yield return new WaitForSeconds(0.1f);
        esperandoContinuar = true;
        foreach (var b in botones) b.gameObject.SetActive(false);
        textoPsicologa.text += "\n\n<size=35><color=yellow>[ 'ESPACIO' para continuar ]</color></size>";
    }

    void ActualizarBarra()
    {
        if (rellenoBarra != null)
            rellenoBarra.fillAmount = estres;
    }

    void MostrarFinal()
    {
        if (panelFinal != null) panelFinal.SetActive(true);

        string reflexion;
        if (estres > 0.65f)
            reflexion = "Aún cargas mucho.\nEstá bien pedir ayuda más seguido.\nEl primer paso ya lo diste.";
        else if (estres > 0.35f)
            reflexion = "Vas soltando poco a poco.\nLa terapia es un proceso,\nno una solución inmediata.";
        else
            reflexion = "Lograste abrirte.\nHablar de lo que sientes\nes uno de los actos más valientes.";

        if (textoReflexion != null)
            textoReflexion.text = reflexion;

        StartCoroutine(EsperarYCargarOutro());
    }

    IEnumerator EsperarYCargarOutro()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        if (MisionHUD.instancia != null)
            MisionHUD.instancia.ActualizarMision("<color=#57FF57><b>OBJETIVO ACTUALIZADO:</b></color>\nAbandona la oficina de psicología");
        if (MisionHUD.instancia != null)
            MisionHUD.instancia.CompletarMision();
        SceneManager.LoadScene("Outro");
    }
}