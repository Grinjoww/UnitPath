using UnityEngine;

public class TriggerMision : MonoBehaviour
{
    [Header("--- LA NUEVA MISIÓN ---")]
    [TextArea]
    public string textoDeLaNuevaMision = "Entra al dispensario y entrega las muestras.";

    // Un candado para que no se actualice 20 veces si el jugador se queda parado ahí
    private bool yaSeActivo = false;

    private void OnTriggerEnter(Collider other)
    {
        // Revisamos si el que acaba de entrar al cubo invisible es Jaime/Jugador
        if (other.CompareTag("Player") && !yaSeActivo)
        {
            yaSeActivo = true; // Ponemos el candado

            // Llamamos a tu Gestor de Misiones mágico
            GestorMisiones gestor = FindFirstObjectByType<GestorMisiones>();
            if (gestor != null)
            {
                gestor.ActualizarObjetivo(textoDeLaNuevaMision);
            }

            // Opcional: Apagamos este cubo invisible para que ya no estorbe
            gameObject.SetActive(false);
        }
    }
}