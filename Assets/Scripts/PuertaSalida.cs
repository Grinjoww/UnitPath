using UnityEngine;

public class PuertaSalida : MonoBehaviour
{
    public QuizManager quizManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            quizManager.EntrarPuerta();
        }
    }
}
