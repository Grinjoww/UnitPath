using StarterAssets;
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

    [Header("--- AUDIO AMBIENTE ---")] // <--- 1. NUEVA SECCIÓN DE AUDIO
    public AudioSource sonidoAmbiente;

    // Referencias internas
    private ThirdPersonController controladorMovimiento;
    private StarterAssetsInputs _input;
    private Animator _animator;

    // ESTADO
    private bool estaBloqueado = false;
    private float tiempoUltimoGolpe = 0f;

    void Start()
    {
        Time.timeScale = 1f;

        controladorMovimiento = GetComponent<ThirdPersonController>();
        _input = GetComponent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();

        if (controladorMovimiento != null) controladorMovimiento.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (barraAdaptacion != null) barraAdaptacion.value = adaptacionActual;
        if (panelDerrota != null) panelDerrota.SetActive(false);

        // 👇 EL CANDADO: Solo suena si la Intro ya se terminó o si el jugador revivió
        if (CinematicaIntro.introYaVista && sonidoAmbiente != null)
        {
            sonidoAmbiente.Play();
        }
    }

    void Update()
    {
        // --- BLOQUEO TOTAL DE INPUTS ---
        if (estaBloqueado && _input != null)
        {
            _input.move = Vector2.zero;
            _input.jump = false;
            _input.sprint = false;
        }

        // --- LÓGICA DE VIDA ---
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

            if (bloquear && _animator != null)
            {
                _animator.SetFloat("Speed", 0f);
                _animator.SetFloat("MotionSpeed", 1f);
            }

            controladorMovimiento.enabled = !bloquear;
        }

        if (!bloquear && _input != null)
        {
            _input.jump = false;
        }
    }

    // <--- 3. FUNCIONES PARA APAGAR Y PRENDER EL SONIDO DESDE OTROS SCRIPTS
    public void PausarSonidoAmbiente()
    {
        if (sonidoAmbiente != null) sonidoAmbiente.Pause();
    }

    public void ReanudarSonidoAmbiente()
    {
        if (sonidoAmbiente != null && !sonidoAmbiente.isPlaying) sonidoAmbiente.Play();
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
        PausarSonidoAmbiente(); // También apagamos el sonido si te mueres
    }
}