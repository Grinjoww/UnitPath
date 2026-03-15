using UnityEngine;

public class TriggerDestino : MonoBehaviour
{
    public GameObject indicadorCamino;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            indicadorCamino.SetActive(false); // desactiva el indicador
        }
    }
}
