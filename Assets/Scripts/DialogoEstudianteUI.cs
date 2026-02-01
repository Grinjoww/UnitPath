using UnityEngine;
using TMPro;

public class DialogoEstudianteUI : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject panelDialogo;
    public TMP_Text textoDialogo;
    public MovimientoJugador jugador;

    private bool jugadorCerca = false;
    private bool dialogoActivo = false;

    void Update()
    {
        if (jugadorCerca && !dialogoActivo && Input.GetKeyDown(KeyCode.E))
        {
            AbrirDialogo();
        }
    }

    void AbrirDialogo()
    {
        dialogoActivo = true;
        panelDialogo.SetActive(true);
        textoDialogo.text =
            "¿Qué preguntas?\n\n" +
            "A) \"Disculpa, ¿dónde dejo estas muestras... biológicas?\"\n" +
            "B) \"Ehh... ¿el edificio de... salud?\"";
        Time.timeScale = 0f; // pausa el juego
    }

    public void OpcionA()
    {
        textoDialogo.text =
            "El estudiante mira la bolsa con incomodidad.\n" +
            "Te da la dirección exacta, pero fue incómodo.";
        jugador.adaptacionActual -= 10f;
        StartCoroutine(CerrarDialogoConRetraso());
    }
    System.Collections.IEnumerator CerrarDialogoConRetraso()
    {
        yield return new WaitForSecondsRealtime(2f); // Espera 2 seg reales, aunque el juego esté pausado
        CerrarDialogo();
    }

    public void OpcionB()
    {
        textoDialogo.text =
            "Te ignoran y te dan una dirección confusa.\n" +
            "Te sientes frustrado.";
        jugador.adaptacionActual -= 15f;
        Invoke(nameof(CerrarDialogo), 2f);
    }

    void CerrarDialogo()
    {
        panelDialogo.SetActive(false);
        dialogoActivo = false;
        Time.timeScale = 1f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = false;
    }
}
