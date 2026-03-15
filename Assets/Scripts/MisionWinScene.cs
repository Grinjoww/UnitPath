using UnityEngine;

public class MisionWinScene : MonoBehaviour
{
    public GameObject panelMisionWinScene;       // Misión: Habla con el maestro
    public GameObject panelObjetivoSalirAula;    // Objetivo: Sal del Aula

    void Start()
    {
        if (panelMisionWinScene != null)
            panelMisionWinScene.SetActive(true);   // activo al inicio

        if (panelObjetivoSalirAula != null)
            panelObjetivoSalirAula.SetActive(false); // desactivado al inicio
    }

    public void CompletarMision()
    {
        Debug.Log("Misión WinScene completada");

        if (panelMisionWinScene != null)
            panelMisionWinScene.SetActive(false);

        if (panelObjetivoSalirAula != null)
            panelObjetivoSalirAula.SetActive(true); // activa el nuevo objetivo
    }

    public void CompletarObjetivoSalirAula()
    {
        Debug.Log("Objetivo salir del aula completado");

        if (panelObjetivoSalirAula != null)
            panelObjetivoSalirAula.SetActive(false);
    }
}
