using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaEntradaBienestar : MonoBehaviour
{
    private bool jugadorDentro = false;

    void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(KeyCode.E))
        {
            EntrarBienestar();
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

    public void EntrarBienestar()
    {
        GameManager.capituloBienestarCompletado = true;
        SceneManager.LoadScene("Capitulo_04_Bienestar");
    }
}
