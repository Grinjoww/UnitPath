using UnityEngine;

public class TriggerMisionMaestro : MonoBehaviour
{
    public MisionWinScene misionWinScene;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aquí puedes poner la lógica de diálogo
            Debug.Log("Jugador habló con el maestro");

            // Completar misión
            if (misionWinScene != null)
                misionWinScene.CompletarMision();
        }
    }
}
