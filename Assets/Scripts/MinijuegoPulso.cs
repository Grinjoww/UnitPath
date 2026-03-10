using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MinijuegoPulso : MonoBehaviour
{
    [Header("--- CONFIGURACIÓN BÁSICA ---")]
    public float velocidadInicial = 200f;
    public int aciertosNecesarios = 3;

    [Header("--- SISTEMA DE VIDAS ---")]
    public int vidasMaximas = 5;
    private int vidasActuales;
    public TMP_Text textoVidas;

    [Header("--- LÍMITES DEL BRAZO (Ajusta aquí) ---")]
    public float limiteMovimiento = 300f;
    public float limiteZonaVerde = 250f;

    [Header("--- ANIMACIÓN PINCHAZO ---")]
    public float avanceDerecha = 50f;
    public float avanceArriba = 20f;
    public float tiempoClavada = 0.15f;

    [Header("--- CONEXIONES UI ---")]
    public RectTransform aguja;
    public RectTransform zonaSegura;
    public GameObject panelCompleto;

    [Header("--- FINAL Y TRANSICIÓN ---")]
    public GameObject pantallaVictoria;
    public GameObject bolsaMuestras;
    public float tiempoEsperaVictoria = 3f;
    // 👇 ESTA ES LA NUEVA MAGIA A PRUEBA DE FALLOS 👇
    public string nombreSiguienteEscena = "Capitulo_03 Castro";

    // Variables internas
    private float velocidadActual;
    private int aciertos = 0;
    private bool moviendoDerecha = true;
    private bool manosArriba = false;
    private bool estaPinchando = false;

    void Start()
    {
        velocidadActual = velocidadInicial;
    }

    void OnEnable()
    {
        aciertos = 0;
        vidasActuales = vidasMaximas;
        velocidadActual = velocidadInicial;
        manosArriba = false;
        estaPinchando = false;

        ActualizarTextoVidas();
        MoverZonaSegura();
    }

    void Update()
    {
        if (panelCompleto.activeInHierarchy == false) return;

        if (manosArriba == false)
        {
            if (!Input.GetKey(KeyCode.Space) && !Input.GetMouseButton(0))
            {
                manosArriba = true;
            }
            return;
        }

        if (!estaPinchando)
        {
            MoverAguja();
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !estaPinchando)
        {
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

    IEnumerator AnimacionPinchazoDiagonal()
    {
        estaPinchando = true;

        Vector2 posicionOriginal = aguja.anchoredPosition;
        Vector2 offsetDiagonal = new Vector2(avanceDerecha, avanceArriba);
        Vector2 posicionEnterrada = posicionOriginal + offsetDiagonal;

        aguja.anchoredPosition = posicionEnterrada;

        VerificarAcierto();

        yield return new WaitForSeconds(tiempoClavada);

        aguja.anchoredPosition = posicionOriginal;
        estaPinchando = false;
    }

    void VerificarAcierto()
    {
        float distancia = Mathf.Abs(aguja.anchoredPosition.x - zonaSegura.anchoredPosition.x);
        float margenError = zonaSegura.rect.width * 1.5f;

        if (distancia < margenError)
        {
            aciertos++;

            if (aciertos >= aciertosNecesarios)
            {
                GanarJuego();
            }
            else
            {
                velocidadActual += 100f;
                MoverZonaSegura();
            }
        }
        else
        {
            vidasActuales--;
            ActualizarTextoVidas();

            if (vidasActuales <= 0)
            {
                PerderJuego();
            }
            else
            {
                aciertos = 0;
                velocidadActual = velocidadInicial;
            }
        }
    }

    void MoverZonaSegura()
    {
        float nuevaPosX = Random.Range(-limiteZonaVerde, limiteZonaVerde);
        zonaSegura.anchoredPosition = new Vector2(nuevaPosX, zonaSegura.anchoredPosition.y);
    }

    void ActualizarTextoVidas()
    {
        if (textoVidas != null)
        {
            textoVidas.text = "Vidas: " + vidasActuales;
        }
    }

    void GanarJuego()
    {
        panelCompleto.SetActive(false);

        if (pantallaVictoria != null)
        {
            pantallaVictoria.SetActive(true);
        }

        if (bolsaMuestras != null)
        {
            bolsaMuestras.SetActive(false);
        }

        StartCoroutine(TransicionSiguienteEscena());
    }

    // --- AHORA BUSCAMOS LA ESCENA POR SU NOMBRE ---
    IEnumerator TransicionSiguienteEscena()
    {
        yield return new WaitForSeconds(tiempoEsperaVictoria);
        SceneManager.LoadScene(nombreSiguienteEscena);
    }

    void PerderJuego()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // <-- También modificado para recargar por nombre
    }
}