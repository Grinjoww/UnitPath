using UnityEngine;

public class IndicadorSeguirJugador : MonoBehaviour
{
    public Transform jugador;   // referencia al Player
    public Transform destino;   // coordenadas objetivo
    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2; // aseguramos que tenga 2 puntos
    }

    void LateUpdate() // LateUpdate para que se actualice después del movimiento del Player
    {
        if (jugador != null && destino != null)
        {
            // Punto inicial pegado al Player
            lr.SetPosition(0, jugador.position);

            // Punto final en el destino
            lr.SetPosition(1, destino.position);
        }
    }
}
