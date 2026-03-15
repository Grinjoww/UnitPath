using UnityEngine;

public class ControlMisionesCap3 : MonoBehaviour
{
    public GameObject panelMision1; // Panel con "Misión: Entra a la Universidad"
    public GameObject panelMision2; // Panel con "Misión: Sigue la flecha hasta tu destino"

    void Start()
    {
        // Estado inicial
        if (panelMision1 != null) panelMision1.SetActive(true);
        if (panelMision2 != null) panelMision2.SetActive(false);
    }

    public void ActivarMision2()
    {
        Debug.Log("Activando Misión 2"); // Mensaje de prueba en consola

        if (panelMision1 != null) panelMision1.SetActive(false);
        if (panelMision2 != null) panelMision2.SetActive(true);
    }
}
