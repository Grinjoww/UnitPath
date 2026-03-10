using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableThought : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public string contenidoPensamiento;
    public bool esPiedra;
    private Vector3 posicionInicial;
    private IdeaMinigameManager manager;
    private FloatEffect efectoFlotar;

    void Start()
    {
        posicionInicial = transform.position;
        manager = FindObjectOfType<IdeaMinigameManager>();
        efectoFlotar = GetComponent<FloatEffect>();
    }

    public void ResetPosition() { transform.position = posicionInicial; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (efectoFlotar != null) efectoFlotar.estaSiendoArrastrado = true;

    }

    public void OnDrag(PointerEventData eventData)
    {
        // Movimiento libre total siguiendo el cursor
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (efectoFlotar != null) efectoFlotar.estaSiendoArrastrado = false;

        // Buscamos si soltamos sobre la cabeza usando Raycast
        GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

        if (hitObject != null && hitObject.CompareTag("Cabeza"))
        {
            // EXITO: Se soltó sobre la cabeza
            string mensaje = esPiedra ? "Esto pesa demasiado..." : "Poco a poco aprenderé.";
            manager.ProcesarPensamiento(esPiedra, mensaje);
            gameObject.SetActive(false);
            Debug.Log(" Colisión detectada con la cabeza al soltar.");
        }
        else
        {
            // FALLO: Regresa a su lugar original
            transform.position = posicionInicial;
            if (efectoFlotar != null) efectoFlotar.ActualizarPosicionBase(transform.localPosition);
        }
    }
}