using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonReintentar : MonoBehaviour
{
    public void ReiniciarJuego()
    {
        // Reinicia la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
