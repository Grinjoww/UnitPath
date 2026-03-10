using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class Pregunta
{
    public string enunciado;          // Texto de la pregunta
    public string[] opciones;         // Opciones A, B, C
    public int indiceCorrecto;        // Índice de la opción correcta (0=A, 1=B, 2=C)
}

public class QuizManager : MonoBehaviour
{
    [Header("Referencias principales")]
    public GameObject quizPanel;
    public GameObject resultadoPanel;
    public TMP_Text resultadoText;
    public MonoBehaviour playerMovementScript;

    [Header("Preguntas")]
    public List<Pregunta> preguntas;
    public TMP_Text preguntaText;
    public TMP_Text[] opcionTexts;

    private int preguntaActual = 0;
    private int respuestasCorrectas = 0;

    [Header("Tiempo")]
    public float tiempoPorPregunta = 15f;   // segundos por pregunta
    private float tiempoRestante;
    public TMP_Text cronometroText;         // TMP_Text para mostrar el tiempo

    [Header("Finalización")]
    public GameObject finalMessagePanel;
    public GameObject puertaHighlight;

    private bool quizActivo = false;

    public void IniciarQuiz()
    {
        quizActivo = true;
        respuestasCorrectas = 0;
        preguntaActual = 0;

        MostrarPregunta();
    }

    void Update()
    {
        if (quizActivo)
        {
            tiempoRestante -= Time.deltaTime;
            if (cronometroText != null)
                cronometroText.text = "Tiempo: " + Mathf.Ceil(tiempoRestante).ToString();

            if (tiempoRestante <= 0f)
            {
                preguntaActual++;
                MostrarPregunta();
            }
        }
    }

    void MostrarPregunta()
    {
        if (preguntaActual < preguntas.Count)
        {
            Pregunta p = preguntas[preguntaActual];
            preguntaText.text = p.enunciado;

            for (int i = 0; i < opcionTexts.Length; i++)
            {
                opcionTexts[i].text = p.opciones[i];
            }

            tiempoRestante = tiempoPorPregunta; // reinicia cronómetro
        }
        else
        {
            TerminarQuiz();
        }
    }

    public void Responder(int indiceElegido)
    {
        if (preguntaActual < preguntas.Count)
        {
            if (indiceElegido == preguntas[preguntaActual].indiceCorrecto)
                respuestasCorrectas++;
        }

        preguntaActual++;
        MostrarPregunta();
    }

    public void TerminarQuiz()
    {
        quizActivo = false;
        resultadoPanel.SetActive(true);
        resultadoText.text = "Respuestas correctas: " + respuestasCorrectas + "/" + preguntas.Count;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void FinalizarQuiz()
    {
        if (resultadoPanel != null) resultadoPanel.SetActive(false);
        if (quizPanel != null) quizPanel.SetActive(false);

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        // Cursor libre para poder hacer clic en el mensaje final
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (finalMessagePanel != null)
            finalMessagePanel.SetActive(true);
    }

    public void CerrarFinalMessage()
    {
        if (finalMessagePanel != null) finalMessagePanel.SetActive(false);
        if (resultadoPanel != null) resultadoPanel.SetActive(false);
        if (quizPanel != null) quizPanel.SetActive(false);

        if (puertaHighlight != null)
            puertaHighlight.SetActive(true);

        // Cursor bloqueado de nuevo para regresar al juego
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EntrarPuerta()
    {
        SceneManager.LoadScene("ConexionCapitulos");
    }
}
