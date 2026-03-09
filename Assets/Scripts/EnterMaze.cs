using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnterMaze : MonoBehaviour
{
    [Header("Configuración")]
    public string mazeSceneName = "MazeScene";

    [Header("UI")]
    public TMP_Text mensajeUI;
    public GameObject panelUI;

    private bool jugadorDentro = false;

    private void OnTriggerEnter(Collider other) // OJO: Collider (3D)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = true;

            if (mensajeUI != null)
            {
                mensajeUI.gameObject.SetActive(true);
                mensajeUI.text = "Presiona F para entrar al minijuego";
            }

            if (panelUI != null)
            {
                panelUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other) // OJO: Collider (3D)
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;

            if (mensajeUI != null)
                mensajeUI.gameObject.SetActive(false);

            if (panelUI != null)
                panelUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (jugadorDentro && Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(mazeSceneName);
        }
    }
}
