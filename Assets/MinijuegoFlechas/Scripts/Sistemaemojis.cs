using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SistemaEmojis : MonoBehaviour
{
    [Header("Emojis")]
    public Image imagenEmoji;
    public Sprite emojiAcierto; // Carita feliz 😊
    public Sprite emojiiFallo; // Carita estresada 😰

    [Header("Mensajes")]
    public TextMeshProUGUI textoMensaje;

    // Mensajes que rotan en aciertos
    private string[] mensajesAcierto = new string[]
    {
        "¡Bien!",
        "¡Enfocado!",
        "¡Genial!",
        "¡Excelente!",
        "¡Concentrado!",
        "¡Perfecto!"
    };

    // Mensajes que rotan en fallos
    private string[] mensajesFallo = new string[]
    {
        "Distracción",
        "Estrés",
        "Perdiste enfoque",
        "Sin concentración",
        "¡Cuidado!",
        "Desenfocado"
    };

    private int indiceAcierto = 0;
    private int indiceFallo = 0;

    // Llamar desde MinijuegoFlechas cuando aciertes
    public void MostrarAcierto()
    {
        try
        {
            // Verificar que existan
            if (imagenEmoji == null || emojiAcierto == null || textoMensaje == null)
                return;

            // Cambiar emoji a feliz
            imagenEmoji.sprite = emojiAcierto;

            // Rotar mensaje de acierto
            textoMensaje.text = mensajesAcierto[indiceAcierto];
            textoMensaje.color = Color.green;

            indiceAcierto = (indiceAcierto + 1) % mensajesAcierto.Length;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Error en MostrarAcierto: " + e.Message);
        }
    }

    // Llamar desde MinijuegoFlechas cuando falles
    public void MostrarFallo()
    {
        try
        {
            // Verificar que existan
            if (imagenEmoji == null || emojiiFallo == null || textoMensaje == null)
                return;

            // Cambiar emoji a estresado
            imagenEmoji.sprite = emojiiFallo;

            // Rotar mensaje de fallo
            textoMensaje.text = mensajesFallo[indiceFallo];
            textoMensaje.color = Color.red;

            indiceFallo = (indiceFallo + 1) % mensajesFallo.Length;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Error en MostrarFallo: " + e.Message);
        }
    }

    // Resetear cuando reintentas
    public void Resetear()
    {
        try
        {
            if (imagenEmoji == null || emojiAcierto == null || textoMensaje == null)
                return;

            indiceAcierto = 0;
            indiceFallo = 0;

            imagenEmoji.sprite = emojiAcierto;
            textoMensaje.text = "";
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Error en Resetear: " + e.Message);
        }
    }
}