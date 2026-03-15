using UnityEngine;
using TMPro;

public class GestorMisiones : MonoBehaviour
{
    [Header("--- UI DE MISIONES ---")]
    public GameObject contenedorMisiones;
    public TMP_Text textoMision;

    void Start()
    {
        // Apenas cargue el juego (durante tu intro), nos aseguramos de que esto esté invisible
        if (contenedorMisiones != null) contenedorMisiones.SetActive(false);
    }

    public void ActualizarObjetivo(string nuevoTexto)
    {
        // Cuando llamemos a esta función, se prenderá automáticamente
        if (contenedorMisiones != null) contenedorMisiones.SetActive(true);

        if (textoMision != null)
        {
            textoMision.text = "<color=#FFA500><b>NUEVO OBJETIVO:</b></color>\n" + nuevoTexto;
        }
    }

    public void ApagarMisiones()
    {
        if (contenedorMisiones != null) contenedorMisiones.SetActive(false);
    }
}