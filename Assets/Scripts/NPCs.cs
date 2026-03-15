using UnityEngine;
using TMPro;
using StarterAssets;
public class NPC : MonoBehaviour
{
    [Header("Diálogo")]
    public Dialogo miDialogo;

    [Header("Interacción")]
    public float distanciaInteraccion = 2f;
    public TextMeshProUGUI textoInteraccion; // Texto "Presiona E para hablar"

    private SistemaDialogosV2 sistemaDialogos;
    private Transform player;
    private bool jugadorCerca = false;

    void Start()
    {
        // Buscar el sistema de diálogos en la escena
        sistemaDialogos = FindFirstObjectByType<SistemaDialogosV2>();

        // Buscar el jugador
        player = FindFirstObjectByType<StarterAssets.ThirdPersonController>()?.transform;

        if (textoInteraccion != null)
            textoInteraccion.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distancia = Vector3.Distance(transform.position, player.position);
        jugadorCerca = distancia < distanciaInteraccion;

        // ⭐ CAMBIO: Solo mostrar si está cerca Y el diálogo NO está activo
        if (textoInteraccion != null)
        {
            bool hayDialogo = sistemaDialogos != null && sistemaDialogos.panelDialogo != null && sistemaDialogos.panelDialogo.activeSelf;
            textoInteraccion.gameObject.SetActive(jugadorCerca && !hayDialogo);
        }

        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            IniciarDialogo();
        }
    }

    void IniciarDialogo()
    {
        if (sistemaDialogos != null && miDialogo != null)
        {
            if (textoInteraccion != null)
                textoInteraccion.gameObject.SetActive(false);
            GameObject luzNPC = GameObject.Find("LuzNPC");
            if (luzNPC != null) luzNPC.SetActive(false);
            sistemaDialogos.IniciarDialogo(miDialogo, this);
        }
    }

    // Para debugging: visualizar la distancia de interacción
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaInteraccion);
    }
}