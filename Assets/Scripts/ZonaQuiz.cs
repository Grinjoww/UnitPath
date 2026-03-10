using UnityEngine;

public class ZonaQuiz : MonoBehaviour
{
    public GameObject quizPanel;
    public GameObject interactMessage;
    public MonoBehaviour playerMovementScript;
    public GameObject highlightObject; // aquí va SM_Teleport_Indicator

    private bool jugadorCerca = false;
    private bool quizUsado = false;
    private bool quizActivo = false;

    void Update()
    {
        if (jugadorCerca && !quizUsado && quizActivo)
        {
            interactMessage.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                quizPanel.SetActive(true);
                quizPanel.GetComponent<QuizManager>().IniciarQuiz();

                if (playerMovementScript != null)
                    playerMovementScript.enabled = false;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                interactMessage.SetActive(false);
                quizUsado = true;

                // Ocultar el objeto guía
                if (highlightObject != null)
                    highlightObject.SetActive(false);
            }
        }
        else
        {
            interactMessage.SetActive(false);
        }
    }

    public void ActivarQuiz()
    {
        quizActivo = true;

        // Mostrar el objeto guía
        if (highlightObject != null)
            highlightObject.SetActive(true);

        Debug.Log("La banca está ahora activa y señalada visualmente.");
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
        }
    }
}
