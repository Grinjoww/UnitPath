using UnityEngine;
using TMPro;

public class GestorMisiones : MonoBehaviour
{
    [Header("--- UI DE MISIONES ---")]
    public GameObject contenedorMisiones; // <-- NUEVO: Para apagar todo el grupo
    public TMP_Text textoMision;

    [Header("--- MISIÓN INICIAL ---")]
    [TextArea]
    public string misionAlEmpezar = "Encuentra el dispensario médico (Pregunta a los estudiantes con [E]).";

    void Start()
    {
        // Al darle Play, ponemos la primera misión automáticamente
        ActualizarObjetivo(misionAlEmpezar);
    }

    // Esta es la función mágica que otros scripts van a llamar
    public void ActualizarObjetivo(string nuevoTexto)
    {
        // Si estaba apagado, lo volvemos a prender automáticamente
        if (contenedorMisiones != null) contenedorMisiones.SetActive(true);

        if (textoMision != null)
        {
            textoMision.text = "<color=#FFA500><b>NUEVO OBJETIVO:</b></color>\n" + nuevoTexto;
        }
    }

    // 👇 LA NUEVA FUNCIÓN PARA DESAPARECER LA MISIÓN 👇
    public void ApagarMisiones()
    {
        if (contenedorMisiones != null) contenedorMisiones.SetActive(false);
    }
}