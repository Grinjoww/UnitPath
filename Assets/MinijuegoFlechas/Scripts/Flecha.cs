using UnityEngine;
using UnityEngine.UI;

public class Flecha : MonoBehaviour
{
    public int direccion;
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
        if (rectTransform == null || zonaObjetivo == null) return;
        rectTransform.anchoredPosition = Vector2.MoveTowards(
            rectTransform.anchoredPosition,
            zonaObjetivo.anchoredPosition,
            velocidad * Time.deltaTime
        );
    }

    public float DistanciaAZona()
    {
        if (rectTransform == null || zonaObjetivo == null) return 0f;
        return Vector2.Distance(
            rectTransform.anchoredPosition,
            zonaObjetivo.anchoredPosition
        );
    }
}