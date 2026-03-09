using UnityEngine;
using UnityEngine.UI;
public class Flecha : MonoBehaviour
{
    public int direccion; // 0=arriba, 1=abajo, 2=izquierda, 3=derecha
    private RectTransform rectTransform;
    private RectTransform zonaObjetivo;
    private float velocidad;
    public void Inicializar(int dir, RectTransform zona, float vel)
    {
        direccion = dir;
        zonaObjetivo = zona;
        velocidad = vel;
        rectTransform = GetComponent<RectTransform>();
    }
    public void Mover()
    {
        rectTransform.anchoredPosition = Vector2.MoveTowards(
            rectTransform.anchoredPosition,
            zonaObjetivo.anchoredPosition,
            velocidad * Time.deltaTime
        );
    }
    public float DistanciaAZona()
    {
        return Vector2.Distance(
            rectTransform.anchoredPosition,
            zonaObjetivo.anchoredPosition
        );
    }
}
