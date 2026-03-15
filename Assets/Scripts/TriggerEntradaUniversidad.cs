using UnityEngine;

public class TriggerEntradaUniversidad : MonoBehaviour
{
    public GameObject indicadorCamino;         // referencia al objeto IndicadorCamino
    public ControlMisionesCap3 controlMisionesCap3; // referencia al script de misiones

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (indicadorCamino != null)
                indicadorCamino.SetActive(true);

            if (controlMisionesCap3 != null)
                controlMisionesCap3.ActivarMision2();
        }
    }
}
