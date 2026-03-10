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
        // Marcar bandera según el capítulo
        switch (nombreEntrada)
        {
            case "Biblioteca":
                GameManager.capituloBibliotecaCompletado = true;
                break;
                // Aquí añadiremos más casos para otros capítulos
        }

        // Regresar a la escena central
        SceneManager.LoadScene(escenaConexion);
    }
}
