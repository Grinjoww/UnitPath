using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovimientoJugador : MonoBehaviour
{
    [Header("--- 1. MOVIMIENTO ---")]
    public float velocidad = 5f;
    public float velocidadRotacion = 10f;
    public Animator animadorJaime;

    [Header("--- 2. BARRA DE ADAPTACIÓN ---")]
    public Slider barraAdaptacion;
    public GameObject panelDerrota;
    public float adaptacionActual = 50f;
    public float velocidadDesgaste = 1.5f;

    [Header("--- 3. INTERACCIÓN CON ZAIDA ---")]
    public Transform transformZaida;
    public float distanciaParaHablar = 3f;
    public float puntosRecuperacion = 20f;

    [Header("--- 4. SISTEMA DE DIÁLOGO ---")]
    public GameObject panelDialogo;
    public TMP_Text textoDialogo;
    [TextArea(3, 10)]
    public string[] listaDialogos;

    // VARIABLES INTERNAS
    private int indiceDialogo = 0;
    private bool hablando = false;
    private bool conversacionTerminada = false;

    private GameObject objetoAActivarAlFinal;
    private Vector3 movimientoInput;
    private Rigidbody rb;
    private Transform camaraTransform;

    void Start()
    {
        Time.timeScale = 1f;
        adaptacionActual = 50f;

        if (barraAdaptacion != null)
            barraAdaptacion.value = adaptacionActual;

        if (panelDerrota != null)
            panelDerrota.SetActive(false);

        if (panelDialogo != null)
            panelDialogo.SetActive(false);

        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        if (Camera.main != null)
            camaraTransform = Camera.main.transform;
    }

    void Update()
    {
        // --- MODO CONVERSACIÓN ---
        if (hablando)
        {
            // seguridad extra
            movimientoInput = Vector3.zero;
            if (animadorJaime != null)
                animadorJaime.SetBool("caminando", false);

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                SiguienteFrase();

            return;
        }

        // --- INPUT DE MOVIMIENTO ---
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (camaraTransform != null)
        {
            Vector3 camForward = camaraTransform.forward;
            Vector3 camRight = camaraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            movimientoInput = camForward * vertical + camRight * horizontal;
        }
        else
        {
            movimientoInput = new Vector3(horizontal, 0, vertical);
        }

        if (animadorJaime != null)
            animadorJaime.SetBool("caminando", movimientoInput.magnitude > 0);

        // --- BARRA DE ADAPTACIÓN ---
        adaptacionActual -= velocidadDesgaste * Time.deltaTime;

        // --- INTERACCIÓN CON ZAIDA ---
        if (transformZaida != null && !conversacionTerminada)
        {
            float distancia = Vector3.Distance(transform.position, transformZaida.position);

            if (distancia <= distanciaParaHablar && Input.GetKeyDown(KeyCode.E))
                EmpezarConversacion();

            if (distancia <= distanciaParaHablar)
                adaptacionActual += 5f * Time.deltaTime;
        }

        adaptacionActual = Mathf.Clamp(adaptacionActual, 0f, 100f);

        if (barraAdaptacion != null)
            barraAdaptacion.value = adaptacionActual;

        if (adaptacionActual <= 0)
            Morir();
    }

    void FixedUpdate()
    {
        if (hablando) return;

        if (movimientoInput != Vector3.zero)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(movimientoInput);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacionObjetivo,
                velocidadRotacion * Time.fixedDeltaTime
            );

            rb.MovePosition(
                rb.position + movimientoInput * velocidad * Time.fixedDeltaTime
            );
        }
    }

    void EmpezarConversacion()
    {
        hablando = true;

        // 🔴 CLAVE: detener movimiento y animación
        movimientoInput = Vector3.zero;
        if (animadorJaime != null)
            animadorJaime.SetBool("caminando", false);

        indiceDialogo = 0;
        panelDialogo.SetActive(true);
        MostrarFraseActual();
        adaptacionActual += puntosRecuperacion;
    }

    void MostrarFraseActual()
    {
        textoDialogo.text = listaDialogos[indiceDialogo];
    }

    void SiguienteFrase()
    {
        indiceDialogo++;
        if (indiceDialogo < listaDialogos.Length)
            MostrarFraseActual();
        else
            CerrarDialogo();
    }

    void CerrarDialogo()
    {
        hablando = false;
        conversacionTerminada = true;
        panelDialogo.SetActive(false);

        if (objetoAActivarAlFinal != null)
        {
            objetoAActivarAlFinal.SetActive(true);
            objetoAActivarAlFinal = null;
        }
    }

    public void IniciarDialogoExterno(string[] nuevosTextos, GameObject objetoFinal = null)
    {
        listaDialogos = nuevosTextos;
        objetoAActivarAlFinal = objetoFinal;

        indiceDialogo = 0;
        hablando = true;
        panelDialogo.SetActive(true);
        MostrarFraseActual();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
            adaptacionActual -= 10f;
    }

    void Morir()
    {
        if (panelDerrota != null)
        {
            panelDerrota.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
