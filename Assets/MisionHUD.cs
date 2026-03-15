using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MisionHUD : MonoBehaviour
{
    public static MisionHUD instancia;
    public TextMeshProUGUI textoMision;
    public string misionInicial; 

    private bool misionCompleta = false;
    private string textoActual = "";

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        ActualizarMision("<color=#FFCC00><b>NUEVO OBJETIVO:</b></color>\n" + misionInicial);
    }

    public void ActualizarMision(string texto)
    {
        textoActual = texto;
        if (textoMision != null)
            textoMision.text = texto;
    }

    public void CompletarMision()
    {
        misionCompleta = true;
    }

    public bool EstaCompleta()
    {
        return misionCompleta;
    }

    public void MostrarAdvertencia()
    {
        StartCoroutine(Advertencia());
    }

    IEnumerator Advertencia()
    {
        if (textoMision != null)
            textoMision.text = "<color=#FF4444><b>¡Completa el objetivo primero!</b></color>";
        yield return new WaitForSeconds(2f);
        if (textoMision != null)
            textoMision.text = textoActual;
    }
}