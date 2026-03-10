using UnityEngine;
using UnityEngine.AI;

public class MovimientoZaida : MonoBehaviour
{
    [Header("--- CONEXIONES DE RUTA ---")]
    public Transform puntoDestino; // Arrastra el dispensario aquí
    public float velocidadCaminar = 2f;

    [Header("--- ANIMACIONES ---")]
    public Animator anim;
    public string animCaminando = "Walk_N"; // El nombre que vi en tu captura
    public string animQuieta = "Idle";      // PON AQUÍ EL NOMBRE DE SU ANIMACIÓN DE ESTAR PARADA

    // Variables internas
    private NavMeshAgent agente;
    private bool tienePermisoParaMoverse = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        // Freno de mano al inicio
        agente.speed = velocidadCaminar;
        agente.isStopped = true;

        // Se queda quieta al inicio del juego
        if (anim != null) anim.Play(animQuieta);
    }

    void Update()
    {
        // Si no le hemos dado la orden, no hace nada
        if (!tienePermisoParaMoverse) return;

        // Revisar si ya llegó a la meta (con un pequeño margen de distancia)
        if (!agente.pathPending && agente.remainingDistance <= 0.5f)
        {
            // ¡LLEGÓ AL CENTRO MÉDICO!
            agente.isStopped = true;

            // Le decimos que pase a la animación de descanso
            if (anim != null) anim.Play(animQuieta);

            // Le quitamos el permiso para que no siga intentando caminar
            tienePermisoParaMoverse = false;
        }
    }

    // LA FUNCIÓN MÁGICA (Se llamará desde el Diálogo)
    public void IniciarViaje()
    {
        if (puntoDestino != null)
        {
            tienePermisoParaMoverse = true;
            agente.isStopped = false;
            agente.SetDestination(puntoDestino.position);

            // Cambia la animación a caminar
            if (anim != null) anim.Play(animCaminando);

            Debug.Log("Zaida: ¡Sígueme al dispensario!");
        }
    }
}