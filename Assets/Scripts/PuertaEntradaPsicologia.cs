using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PuertaEntradaPsicologia : MonoBehaviour
{
    private bool jugadorDentro = false;
    public TextMeshProUGUI textoAviso; 

    void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.capituloBibliotecaCompletado &&
                GameManager.capituloEnfermeriaCompletado &&
                GameManager.capituloBienestarCompletado)
            {
                EntrarPsicologia();
            }
            else
            {
                StartCoroutine(MostrarAviso());
            }
        }
    }

    System.Collections.IEnumerator MostrarAviso()
    {
        if (textoAviso != null)
        {
            textoAviso.text = "<color=#FF4444>¡Completa los otros departamentos primero!</color>";
            textoAviso.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            textoAviso.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) jugadorDentro = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) jugadorDentro = false;
    }

    public void EntrarPsicologia()
    {
        SceneManager.LoadScene("Capitulo_04_Psicologia");
    }
}