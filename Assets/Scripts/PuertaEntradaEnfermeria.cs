using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaEntradaEnfermeria : MonoBehaviour
{
    private bool jugadorDentro = false;

    void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(KeyCode.E))
        {
            EntrarEnfermeria();
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

    public void EntrarEnfermeria()
    {
        GameManager.capituloEnfermeriaCompletado = true;
        SceneManager.LoadScene("Capitulo_04_Enfermeria");
    }
}
