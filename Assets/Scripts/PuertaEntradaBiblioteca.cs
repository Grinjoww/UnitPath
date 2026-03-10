using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaEntradaBiblioteca : MonoBehaviour
{
    private bool jugadorDentro = false;

    void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(KeyCode.E))
        {
            EntrarBiblioteca();
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

    public void EntrarBiblioteca()
    {
        // Marcar bandera
        GameManager.capituloBibliotecaCompletado = true;

        // Cargar la escena de la Biblioteca
        SceneManager.LoadScene("Capitulo_04_Biblioteca");
    }
}
