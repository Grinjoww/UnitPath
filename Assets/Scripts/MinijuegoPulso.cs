using System.Collections; // Necesario para la animación (Corrutina)
using UnityEngine;
using UnityEngine.UI;

public class MinijuegoPulso : MonoBehaviour
{
    [Header("--- CONFIGURACIÓN BÁSICA ---")]
    public float velocidadInicial = 200f;
    public int aciertosNecesarios = 3;

    [Header("--- LÍMITES DEL BRAZO (Ajusta aquí) ---")]
    [Tooltip("Hasta qué número X llega la jeringa antes de rebotar")]
    public float limiteMovimiento = 300f;
    [Tooltip("Rango máximo donde puede aparecer la zona verde (-X a +X)")]
    public float limiteZonaVerde = 250f;

    [Header("--- ANIMACIÓN PINCHAZO (Ajusta Diagonal) ---")]
    [Tooltip("Cuantos pixeles se mueve a la DERECHA (+X) al pinchar")]
    public float avanceDerecha = 50f; // <-- AJUSTA ESTO EN EL INSPECTOR
    [Tooltip("Cuantos pixeles se mueve hacia ARRIBA (+Y) al pinchar")]
    public float avanceArriba = 20f;  // <-- AJUSTA ESTO EN EL INSPECTOR
    [Tooltip("Tiempo (segundos) que la aguja se queda clavada")]
    public float tiempoClavada = 0.15f;

    [Header("--- CONEXIONES UI ---")]
    public RectTransform aguja;      // Arrastra tu inyección aquí
    public RectTransform zonaSegura; // Arrastra tu objetivo rojo/verde aquí
    public GameObject panelCompleto; // El objeto padre que apaga todo al ganar

    [Header("--- FINAL ---")]
    public GameObject pantallaVictoria;

    // Variables internas
    private float velocidadActual;
    private int aciertos = 0;
    private bool moviendoDerecha = true;
    private bool manosArriba = false;

    // VARIABLE MAGICA PARA LA ANIMACIÓN
    private bool estaPinchando = false;

    void Start()
    {
        velocidadActual = velocidadInicial;
    }

    void OnEnable()
    {
        // RESET TOTAL AL APARECER
        aciertos = 0;
        velocidadActual = velocidadInicial;
        manosArriba = false;
        estaPinchando = false;
        MoverZonaSegura();
    }

    void Update()
    {
        // 1. Si el panel está apagado o estamos pinchando, no hacer nada
        if (panelCompleto.activeInHierarchy == false) return;

        // 2. Filtro de seguridad (esperar a que suelte Space/Click)
        if (manosArriba == false)
        {
            if (!Input.GetKey(KeyCode.Space) && !Input.GetMouseButton(0))
            {
                manosArriba = true;
            }
            return;
        }

        // 3. Solo se mueve de lado a lado si NO está en medio del pinchazo
        if (!estaPinchando)
        {
            MoverAguja();
        }

        // 4. Detectar el intento de acierto (solo si no estamos ya pinchando)
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !estaPinchando)
        {
            // INICIAMOS LA PELÍCULA DE CÓDIGO (ANIMACIÓN)
            StartCoroutine(AnimacionPinchazoDiagonal());
        }
    }

    void MoverAguja()
    {
        if (moviendoDerecha)
        {
            aguja.anchoredPosition += Vector2.right * velocidadActual * Time.deltaTime;
            if (aguja.anchoredPosition.x >= limiteMovimiento) moviendoDerecha = false;
        }
        else
        {
            aguja.anchoredPosition += Vector2.left * velocidadActual * Time.deltaTime;
            if (aguja.anchoredPosition.x <= -limiteMovimiento) moviendoDerecha = true;
        }
    }

    // --- LA NUEVA MAGIA VISUAL: EL EFECTO DIAGONAL SATISFACTORIO ---
    IEnumerator AnimacionPinchazoDiagonal()
    {
        estaPinchando = true; // Congelamos el movimiento lateral

        // 1. Guardamos la posición original completa (Vector2 es mejor)
        Vector2 posicionOriginal = aguja.anchoredPosition;

        // 2. Calculamos el vector diagonal de movimiento (derecha y arriba)
        // (Al sumar offsetDiagonal a posicionOriginal, la movemos en diagonal)
        Vector2 offsetDiagonal = new Vector2(avanceDerecha, avanceArriba);
        Vector2 posicionEnterrada = posicionOriginal + offsetDiagonal;

        // 3. Movemos la inyección de golpe (el "pincho" satisfactorio)
        aguja.anchoredPosition = posicionEnterrada;

        // 4. Revisamos si atinaste o fallaste mientras está visualmente "adentro"
        VerificarAcierto();

        // 5. Esperamos para que el jugador la vea clavada
        yield return new WaitForSeconds(tiempoClavada);

        // 6. La devolvemos suavemente (o de golpe) a su posición original completa
        aguja.anchoredPosition = posicionOriginal;

        estaPinchando = false; // Descongelamos el movimiento lateral
    }

    void VerificarAcierto()
    {
        float distancia = Mathf.Abs(aguja.anchoredPosition.x - zonaSegura.anchoredPosition.x);

        // ¡Usamos el margen generoso!
        float margenError = zonaSegura.rect.width;

        if (distancia < margenError)
        {
            // ACIERTO
            aciertos++;
            Debug.Log("✅ Acierto: " + aciertos);

            if (aciertos >= aciertosNecesarios)
            {
                GanarJuego();
            }
            else
            {
                velocidadActual += 100f; // Más rápido
                MoverZonaSegura();       // Mover verde
            }
        }
        else
        {
            // FALLO
            Debug.Log("❌ FALLASTE - Reiniciando");
            aciertos = 0;
            velocidadActual = velocidadInicial;
        }
    }

    void MoverZonaSegura()
    {
        float nuevaPosX = Random.Range(-limiteZonaVerde, limiteZonaVerde);
        zonaSegura.anchoredPosition = new Vector2(nuevaPosX, zonaSegura.anchoredPosition.y);
    }

    void GanarJuego()
    {
        Debug.Log("🏆 ¡PRUEBA COMPLETADA!");
        panelCompleto.SetActive(false); // Cierra el minijuego completo

        if (pantallaVictoria != null)
        {
            pantallaVictoria.SetActive(true);
        }

        // Desbloquear a Jaime
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            EstadisticasJugador stats = player.GetComponent<EstadisticasJugador>();
            if (stats != null) stats.BloquearMovimiento(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}