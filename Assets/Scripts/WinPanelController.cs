using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanelController : MonoBehaviour
{
    public void ContinuarJuego()
    {
        SceneManager.LoadScene("WinScene"); // nombre exacto
    }
}
