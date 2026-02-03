using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PuertaInteractiva : MonoBehaviour
{
    [Header("--- CONFIGURACIÓN ---")]
    public float velocidad = 2.0f;
    public float anguloAbierto = -90.0f;
    public float anguloCerrado = 0.0f;

    [Header("--- AUDIO ---")]
    public AudioClip sonidoAbrir;
    public AudioClip sonidoCerrar;

    [Header("--- AYUDA VISUAL (Opcional) ---")]
    public GameObject textoPresioneE; // Arrastra tu UI aquí si quieres

    // VARIABLES PRIVADAS
    private bool estaAbierta = false;
    private bool jugadorCerca = false;
    private AudioSource miAudio;
    private Quaternion rotacionInicial;

    void Start()
    {
        miAudio = GetComponent<AudioSource>();
        rotacionInicial = transform.localRotation; // Recordamos cómo empieza

        if (textoPresioneE != null)
            textoPresioneE.SetActive(false);
    }

    void Update()
    {
        // 1. INPUT: Si estoy cerca y presiono F
        if (jugadorCerca && Input.GetKeyDown(KeyCode.F))
        {
            CambiarEstadoPuerta();
        }

        // 2. ANIMACIÓN: Mover la puerta suavemente
        float anguloDestino = estaAbierta ? anguloAbierto : anguloCerrado;

        // Calculamos la rotación objetivo basándonos en la rotación original
        Quaternion rotacionObjetivo = Quaternion.Euler(0, anguloDestino, 0);

        // Slerp hace la magia de moverlo suave
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotacionObjetivo, Time.deltaTime * 5 * velocidad);
    }

    void CambiarEstadoPuerta()
    {
        estaAbierta = !estaAbierta;

        // Sonido
        if (miAudio != null)
        {
            miAudio.Stop(); // Parar sonido anterior si spameas la tecla
            miAudio.clip = estaAbierta ? sonidoAbrir : sonidoCerrar;
            miAudio.Play();
        }
    }

    // DETECTAR ENTRADA
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            if (textoPresioneE != null) textoPresioneE.SetActive(true);
        }
    }

    // DETECTAR SALIDA
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            if (textoPresioneE != null) textoPresioneE.SetActive(false);
        }
    }
}