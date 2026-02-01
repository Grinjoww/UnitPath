using UnityEngine;
using UnityEngine.SceneManagement; // <--- ESTA es la librería correcta para juegos

public class MainMenu : MonoBehaviour
{
    public GameObject optionMenu;
    public GameObject mainMenu;

    public void OpenOptionsPanel()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void OpenMainMenuPanel()
    {
        mainMenu.SetActive(true);
        optionMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        // Esto ayuda a ver si funciona mientras pruebas en el editor:
        Debug.Log("Saliendo del juego...");
    }

    public void PlayGame()
    {
        // IMPORTANTE: Asegúrate de que el tiempo no esté pausado por si acaso
        Time.timeScale = 1f;

        // ESTA es la forma correcta de cambiar de nivel jugando:
        SceneManager.LoadScene("Capitulo_02 Mariscal");
    }
}