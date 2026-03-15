using UnityEngine;
using TMPro;

public class GestorMisiones1 : MonoBehaviour
{
    [Header("--- UI DE MISIONES ---")]
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
        if (textoMision != null)
        {
            // Usamos formato HTML para que el título salga naranja y negrita, y el texto normal abajo
            textoMision.text = "<color=#FFA500><b>NUEVO OBJETIVO:</b></color>\n" + nuevoTexto;
        }
    }
}