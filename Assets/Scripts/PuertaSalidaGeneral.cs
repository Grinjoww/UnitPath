using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaSalidaGeneral : MonoBehaviour
{
    private bool jugadorDentro = false;

    [Tooltip("Nombre de la escena central a la que se regresa")]
    public string escenaConexion = "ConexionCapitulos";

    [Tooltip("Identificador del capítulo (ej: Biblioteca, Maze, Software, Redes)")]
    public string nombreEntrada = "Biblioteca";

    void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(KeyCode.E))
        {
            
            if (MisionHUD.instancia != null && !MisionHUD.instancia.EstaCompleta())
            {
                MisionHUD.instancia.MostrarAdvertencia();
                return;
            }
            VolverConexion();
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
    public void VolverConexion()
    {
        switch (nombreEntrada)
        {
            case "Biblioteca":
                GameManager.capituloBibliotecaCompletado = true;
                PlayerPrefs.SetInt("Completo_Biblioteca", 1);
                break;
            case "Enfermeria":
                GameManager.capituloEnfermeriaCompletado = true;
                PlayerPrefs.SetInt("Completo_Enfermeria", 1);
                break;
            case "Psicologia":
                GameManager.capituloPsicologiaCompletado = true;
                PlayerPrefs.SetInt("Completo_Psicologia", 1);
                break;
            case "Bienestar":
                GameManager.capituloBienestarCompletado = true;
                PlayerPrefs.SetInt("Completo_Bienestar", 1);
                break;
        }
        PlayerPrefs.Save();
        SceneManager.LoadScene(escenaConexion);
    }
}
