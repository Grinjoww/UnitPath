using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ControladorIntro : MonoBehaviour
{
    [Header("UI")]
    public Image imagenPanel;
    public TextMeshProUGUI textoNarracion;
    public TextMeshProUGUI textoContinuar;

    [Header("Paneles")]
    public Sprite[] imagenes;
    public string[] textos;

    [Header("Configuración")]
    public float velocidadTexto = 0.05f;
    public string nombreEscenaSiguiente = "Capitulo_01";

    private int panelActual = 0;
    private bool escribiendo = false;
    private bool puedeAvanzar = false;

    void Start()
    {
        textoContinuar.gameObject.SetActive(false);
        MostrarPanel(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (escribiendo)
            {
                // Si está escribiendo, muestra todo el texto de golpe
                StopAllCoroutines();
                textoNarracion.text = textos[panelActual];
                escribiendo = false;
                puedeAvanzar = true;
                textoContinuar.gameObject.SetActive(true);
            }
            else if (puedeAvanzar)
            {
                panelActual++;
                if (panelActual >= imagenes.Length)
                    SceneManager.LoadScene(nombreEscenaSiguiente);
                else
                    MostrarPanel(panelActual);
            }
        }
    }

    void MostrarPanel(int indice)
    {
        imagenPanel.sprite = imagenes[indice];
        textoContinuar.gameObject.SetActive(false);
        puedeAvanzar = false;
        StartCoroutine(EscribirTexto(textos[indice]));
    }

    IEnumerator EscribirTexto(string texto)
    {
        escribiendo = true;
        textoNarracion.text = "";
        foreach (char letra in texto)
        {
            textoNarracion.text += letra;
            yield return new WaitForSeconds(velocidadTexto);
        }
        escribiendo = false;
        puedeAvanzar = true;
        textoContinuar.gameObject.SetActive(true);
    }
}