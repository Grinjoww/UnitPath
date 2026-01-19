using UnityEngine;

public class ActivadorDialogo : MonoBehaviour
{
    [Header("Configuración del Diálogo")]
    [TextArea(3, 10)]
    public string[] dialogoZona;

    [Header("¿Qué pasa al terminar de hablar?")]
    public GameObject minijuegoParaActivar; // <--- Aquí arrastrarás el PanelMinijuego

    private bool yaSeActivo = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !yaSeActivo)
        {
            MovimientoJugador jaime = other.GetComponent<MovimientoJugador>();

            if (jaime != null)
            {
                // Le pasamos el texto Y TAMBIÉN el minijuego
                jaime.IniciarDialogoExterno(dialogoZona, minijuegoParaActivar);
                yaSeActivo = true;
            }
        }
    }
}