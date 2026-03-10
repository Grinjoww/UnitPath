using UnityEngine;

public class FloatEffect : MonoBehaviour
{
    public float amplitud = 10f;
    public float velocidad = 2f;
    private Vector3 posicionInicial;
    public bool estaSiendoArrastrado = false; // Nueva variable

    void Start() { posicionInicial = transform.localPosition; }

    void Update()
    {
        // Si lo estamos arrastrando, NO aplicamos el efecto de flotado
        if (estaSiendoArrastrado) return;

        float nuevaY = posicionInicial.y + Mathf.Sin(Time.time * velocidad) * amplitud;
        transform.localPosition = new Vector3(transform.localPosition.x, nuevaY, transform.localPosition.z);
    }

    // Método para que el DraggableThought pueda resetear la base del flotado
    public void ActualizarPosicionBase(Vector3 nuevaPos) { posicionInicial = nuevaPos; }
}