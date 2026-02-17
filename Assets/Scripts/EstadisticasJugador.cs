using StarterAssets; // Necesario para acceder a los Inputs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EstadisticasJugador : MonoBehaviour
{
    [Header("--- BARRA DE ADAPTACIÓN ---")]
    public Slider barraAdaptacion;
    public GameObject panelDerrota;
    public float adaptacionActual = 50f;
    public float velocidadDesgaste = 1.5f;

    // Referencias internas
    private ThirdPersonController controladorMovimiento;
    private StarterAssetsInputs _input; // <--- NUEVA REFERENCIA A LOS CONTROLES

    // ESTADO
    private bool estaBloqueado = false;
    private float tiempoUltimoGolpe = 0f;

    void Start()
    {
        Time.timeScale = 1f;

        // 2. Asegurar que el controlador esté encendido al nacer
        if (controladorMovimiento != null) controladorMovimiento.enabled = true;

        // 3. Bloquear el cursor de nuevo para que no se vea la flecha del mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controladorMovimiento = GetComponent<ThirdPersonController>();
        _input = GetComponent<StarterAssetsInputs>(); // Conectamos con los inputs

        if (barraAdaptacion != null) barraAdaptacion.value = adaptacionActual;
        if (panelDerrota != null) panelDerrota.SetActive(false);
    }

    void Update()
    {
        // --- 1. BLOQUEO TOTAL DE INPUTS (Anti-Saltos) ---
        // Si estamos hablando, forzamos a que el juego crea que NO estás tocando nada.
        if (estaBloqueado && _input != null)
        {
            _input.move = Vector2.zero; // Anula movimiento WASD
            _input.jump = false;        // Anula la Barra Espaciadora (Salto)
            _input.sprint = false;      // Anula el Shift (Correr)
        }

        // --- 2. LÓGICA DE VIDA ---
        if (!estaBloqueado) // Solo desgasta vida si no estás en pausa/dialogo (Opcional)
        {
            adaptacionActual -= velocidadDesgaste * Time.deltaTime;
        }

        // Actualizar barra
        if (barraAdaptacion != null) barraAdaptacion.value = adaptacionActual;

        // Morir
        if (adaptacionActual <= 0) Morir();
    }

    // --- CHOQUE CON ENEMIGOS ---
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("NPC"))
        {
            if (Time.time > tiempoUltimoGolpe + 1.0f)
            {
                adaptacionActual -= 10f;
                tiempoUltimoGolpe = Time.time;
                Debug.Log("¡Auch! Choque con NPC");
            }
        }
    }

    // --- UTILIDADES ---
    public void BloquearMovimiento(bool bloquear)
    {
        estaBloqueado = bloquear;

        if (controladorMovimiento != null)
        {
            controladorMovimiento.LockCameraPosition = bloquear;
            controladorMovimiento.enabled = !bloquear; // ← LA MAGIA
        }

        // Si desbloqueamos, asegurarnos de que no quede un salto guardado
        if (!bloquear && _input != null)
        {
            _input.jump = false;
        }
    }
    public void ReiniciarNivel()
    {
        // 1. Descongelar el tiempo (CRUCIAL para que no se quede tieso)
        Time.timeScale = 1f;

        // 2. Cargar la escena actual de nuevo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Morir()
    {
        if (panelDerrota != null) panelDerrota.SetActive(true);
        Time.timeScale = 0f;

        // Soltar mouse para poder reiniciar
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Bloquear controles para que no se mueva el cadáver
        BloquearMovimiento(true);
    }
}