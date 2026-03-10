using UnityEngine;
using TMPro;

public class InteraccionMaestro : MonoBehaviour
{
    public GameObject interactMessage; // Texto TMP "Presione E"
    public TMP_Text maestroDialogo;    // Texto TMP para mostrar el diálogo
    public ZonaQuiz zonaQuiz;          // referencia a la banca/quiz

    private bool jugadorCerca = false;
    private bool yaInteractuo = false;

    void Update()
    {
        if (jugadorCerca && !yaInteractuo)
        {
            interactMessage.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                interactMessage.SetActive(false);
                yaInteractuo = true;

                // Mostrar diálogo del maestro en pantalla
                maestroDialogo.gameObject.SetActive(true);
                maestroDialogo.text = "Llegaste justo a tiempo, toma asiento. Vamos a hacer una prueba diagnóstica.";

                // Activar la banca/quiz
                if (zonaQuiz != null)
                    zonaQuiz.ActivarQuiz();
            }
        }
        else
        {
            interactMessage.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;

            // Ocultar el mensaje y el diálogo al salir del rango
            interactMessage.SetActive(false);
            maestroDialogo.gameObject.SetActive(false);
        }
    }
}
