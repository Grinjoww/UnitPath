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
    private StarterAssetsInputs _input;
    private Animator _animator; // <--- 1. AGREGAMOS EL ANIMATOR AQUÍ

    // ESTADO
    private bool estaBloqueado = false;
    private float tiempoUltimoGolpe = 0f;

    void Start()
    {
        Time.timeScale = 1f;

        controladorMovimiento = GetComponent<ThirdPersonController>();
        _input = GetComponent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>(); // <--- 2. LO CONECTAMOS AQUÍ

        if (controladorMovimiento != null) controladorMovimiento.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (barraAdaptacion != null) barraAdaptacion.value = adaptacionActual;
        if (panelDerrota != null) panelDerrota.SetActive(false);
    }

    void Update()
    {
        // --- 1. BLOQUEO TOTAL DE INPUTS (Anti-Saltos) ---
        if (estaBloqueado && _input != null)
        {
            _input.move = Vector2.zero;
            _input.jump = false;
            _input.sprint = false;
        }

        // --- 2. LÓGICA DE VIDA ---
        if (!estaBloqueado)
        {
            adaptacionActual -= velocidadDesgaste * Time.deltaTime;
        }

        if (barraAdaptacion != null) barraAdaptacion.value = adaptacionActual;

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

            // --- 3. AQUÍ LE DECIMOS A LA ANIMACIÓN QUE SE DETENGA ---
            if (bloquear && _animator != null)
            {
                _animator.SetFloat("Speed", 0f);
                _animator.SetFloat("MotionSpeed", 1f);
            }

            controladorMovimiento.enabled = !bloquear; // LA MAGIA
        }

        if (!bloquear && _input != null)
        {
            _input.jump = false;
        }
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Morir()
    {
        if (panelDerrota != null) panelDerrota.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        BloquearMovimiento(true);
    }
}