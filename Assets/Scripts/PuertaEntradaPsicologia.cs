using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaEntradaPsicologia : MonoBehaviour
{
    private bool jugadorDentro = false;

    void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(KeyCode.E))
        {
            EntrarPsicologia();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorDentro = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorDentro = false;
    }

    public void EntrarPsicologia()
    {
        GameManager.capituloPsicologiaCompletado = true;
        SceneManager.LoadScene("Capitulo_04_Psicologia");
    }
}
