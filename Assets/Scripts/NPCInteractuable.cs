using UnityEngine;

public class NPCInteractuable : MonoBehaviour
{
    public GameObject panelDialogoEstudiante;
    private bool jugadorCerca;

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            panelDialogoEstudiante.SetActive(true);
            Time.timeScale = 0f; // pausa el juego
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = false;
    }
}

