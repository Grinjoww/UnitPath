using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanelController : MonoBehaviour
{
    public void ContinuarJuego()
    {
        // Ocultar el cursor antes de cambiar de escena
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Cargar la escena de victoria
        SceneManager.LoadScene("WinScene");
    }
}
