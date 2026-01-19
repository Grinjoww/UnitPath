using UnityEngine;
using UnityEngine.UI;

public class MinijuegoPulso : MonoBehaviour
{
    [Header("Configuración Básica")]
    public float velocidadInicial = 200f; // La velocidad con la que empieza
    public int aciertosNecesarios = 3; // Cuántos para ganar

    [Header("Conexiones UI")]
    public RectTransform aguja;    // La imagen de la aguja (roja)
    public RectTransform zonaSegura; // La imagen de la zona verde
    public GameObject panelCompleto; // El Canvas/Panel del minijuego

    [Header("Final")]
    public GameObject pantallaVictoria;

    // Variables internas
    private float velocidadActual;
    private int aciertos = 0;
    private bool moviendoDerecha = true;
    private bool manosArriba = false; // Filtro de seguridad para el clic

    void Start()
    {
        // Guardamos la velocidad original
        velocidadActual = velocidadInicial;
    }

    void OnEnable()
    {
        // --- RESET TOTAL AL APARECER ---
        aciertos = 0;
        velocidadActual = velocidadInicial; // Volvemos a la velocidad lenta
        manosArriba = false; // Activamos el filtro de seguridad

        // Movemos la zona verde a un lugar aleatorio para empezar
        MoverZonaSegura();

    }

    void Update()
    {
        // 1. Si el panel está apagado, no hacer nada
        if (panelCompleto.activeInHierarchy == false) return;

        // 2. FILTRO DE SEGURIDAD (Manos Arriba)
        if (manosArriba == false)
        {
            // Esperamos hasta que el jugador NO toque nada
            if (!Input.GetKey(KeyCode.Space) && !Input.GetMouseButton(0))
            {
                manosArriba = true;
                Debug.Log("🙌 ¡Listo para jugar!");
            }
            return; // Mientras siga tocando, no avanzamos
        }

        // 3. Mover la aguja de lado a lado
        MoverAguja();

        // 4. Detectar el intento de acierto
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            VerificarAcierto();
        }
    }

    void MoverAguja()
    {
        float limite = 240f; // Ajusta este límite según el ancho de tu barra gris

        if (moviendoDerecha)
        {
            aguja.anchoredPosition += Vector2.right * velocidadActual * Time.deltaTime;
            if (aguja.anchoredPosition.x >= limite) moviendoDerecha = false;
        }
        else
        {
            aguja.anchoredPosition += Vector2.left * velocidadActual * Time.deltaTime;
            if (aguja.anchoredPosition.x <= -limite) moviendoDerecha = true;
        }
    }

    void VerificarAcierto()
    {
        // Calculamos la distancia entre la aguja y el centro de la zona verde
        float distancia = Mathf.Abs(aguja.anchoredPosition.x - zonaSegura.anchoredPosition.x);
        float margenError = zonaSegura.rect.width / 2;

        if (distancia < margenError)
        {
            // --- ¡ACIERTO! ---
            aciertos++;
            Debug.Log("✅ Acierto: " + aciertos);

            if (aciertos >= aciertosNecesarios)
            {
                GanarJuego();
            }
            else
            {
                // Si acertó pero aún no gana, ponemos más dificultad:
                velocidadActual += 400f; // Aumentamos la velocidad
                MoverZonaSegura();       // Cambiamos la posición verde
            }
        }
        else
        {
            // --- FALLO ---
            Debug.Log("❌ FALLASTE - Reiniciando");
            aciertos = 0; // Castigo: Reiniciar contador
            velocidadActual = velocidadInicial; // Castigo: Velocidad vuelve a ser lenta
        }
    }

    void MoverZonaSegura()
    {
        // Mueve la zona verde a una posición X aleatoria entre -120 y 120
        // (Ajusta estos números para que no se salga de la barra gris)
        float nuevaPosX = Random.Range(-200f, 200f);
        zonaSegura.anchoredPosition = new Vector2(nuevaPosX, zonaSegura.anchoredPosition.y);
    }

    void GanarJuego()
    {
        Debug.Log("🏆 ¡PRUEBA COMPLETADA!");
        panelCompleto.SetActive(false); // Cierra el minijuego

        // --- ACTIVAR VICTORIA ---
        if (pantallaVictoria != null)
        {
            pantallaVictoria.SetActive(true); // ¡MUESTRA EL CARTEL!
        }
    }
}