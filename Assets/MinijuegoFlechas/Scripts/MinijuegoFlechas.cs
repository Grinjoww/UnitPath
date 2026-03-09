using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MinijuegoFlechas : MonoBehaviour
{
    [Header("Flechas")]
    public GameObject prefabFlecha;
    public RectTransform[] zonasPresionar;
    public Sprite[] spritesFlecha;
    public float velocidadFlecha = 300f;
    public float tolerancia = 60f;

    [Header("Spawn")]
    public float spawnY = 400f;
    public float[] spawnX;

    [Header("Vida")]
    public Image[] corazones;
    public Sprite corazonLleno;
    public Sprite corazonVacio;
    private int vidaActual;

    [Header("UI")]
    public TextMeshProUGUI textoScore;
    public GameObject panelGameOver;
    public TextMeshProUGUI textoScoreFinal;

    // ⭐ NUEVOS: Mejoras visuales
    public TextMeshProUGUI textoCombo; // Opcional
    public Image barraProgreso; // Opcional
    public AudioClip sonidoAcierto; // Opcional
    public AudioClip sonidoFallo; // Opcional

    private int score = 0;
    private int combo = 0; // ⭐ NUEVO
    private List<GameObject> flechasActivas = new List<GameObject>();
    private float temporizador;
    private bool juegoActivo = true;
    private StarterAssets.ThirdPersonController playerController;
    private AudioSource audioSource; // ⭐ NUEVO

    void OnEnable()
    {
        score = 0;
        combo = 0; // ⭐ NUEVO
        vidaActual = corazones.Length;
        juegoActivo = true;
        temporizador = 0f;

        // Limpiar flechas anteriores
        foreach (var f in flechasActivas)
            if (f != null) Destroy(f);
        flechasActivas.Clear();

        ActualizarCorazones();
        ActualizarScore();
        ActualizarCombo(); // ⭐ NUEVO

        if (panelGameOver != null) panelGameOver.SetActive(false);

        // Desactivar personaje
        playerController = FindFirstObjectByType<StarterAssets.ThirdPersonController>();
        var playerInput = FindFirstObjectByType<StarterAssets.StarterAssetsInputs>();
        if (playerController != null) playerController.enabled = false;
        if (playerInput != null) playerInput.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // ⭐ NUEVO: Obtener AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        if (spawnX == null || spawnX.Length == 0)
            spawnX = new float[] { -300f, -100f, 100f, 300f };
    }

    void Update()
    {
        if (!juegoActivo)
        {
            if (Input.GetKeyDown(KeyCode.R)) Reintentar();
            if (Input.GetKeyDown(KeyCode.Escape)) Salir();
            return;
        }

        temporizador -= Time.deltaTime;
        if (temporizador <= 0f)
        {
            SpawnFlecha(Random.Range(0, 4));
            temporizador = Random.Range(0.8f, 1.5f);
        }

        MoverFlechas();
        DetectarInput();
    }

    void SpawnFlecha(int dir)
    {
        GameObject obj = Instantiate(prefabFlecha, transform);
        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(spawnX[dir], spawnY);

        Image img = obj.GetComponent<Image>();
        if (img != null && spritesFlecha.Length > dir)
            img.sprite = spritesFlecha[dir];

        Flecha f = obj.GetComponent<Flecha>();
        f.Inicializar(dir, zonasPresionar[dir], velocidadFlecha);

        flechasActivas.Add(obj);
    }

    void MoverFlechas()
    {
        for (int i = flechasActivas.Count - 1; i >= 0; i--)
        {
            if (flechasActivas[i] == null) { flechasActivas.RemoveAt(i); continue; }

            Flecha f = flechasActivas[i].GetComponent<Flecha>();
            f.Mover();

            if (f.DistanciaAZona() < 5f)
            {
                // ⭐ NUEVO: Efecto visual de fallo
                CrearEfectoFallo(flechasActivas[i].GetComponent<RectTransform>());
                Destroy(flechasActivas[i]);
                flechasActivas.RemoveAt(i);
                PerderVida();
            }
        }
    }

    void DetectarInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) VerificarZona(0);
        if (Input.GetKeyDown(KeyCode.S)) VerificarZona(1);
        if (Input.GetKeyDown(KeyCode.A)) VerificarZona(2);
        if (Input.GetKeyDown(KeyCode.D)) VerificarZona(3);
    }

    void VerificarZona(int dir)
    {
        // ⭐ Declarar UNA sola vez aquí, afuera del for
        SistemaEmojis sistemaEmojis = FindFirstObjectByType<SistemaEmojis>();

        for (int i = flechasActivas.Count - 1; i >= 0; i--)
        {
            Flecha f = flechasActivas[i].GetComponent<Flecha>();
            if (f.direccion == dir && f.DistanciaAZona() < tolerancia)
            {
                // ⭐ Efecto visual de acierto
                CrearEfectoAcierto(flechasActivas[i].GetComponent<RectTransform>());
                AnimarZonaPresionada(zonasPresionar[dir]);

                Destroy(flechasActivas[i]);
                flechasActivas.RemoveAt(i);

                score += 10;
                combo++;

                ActualizarScore();
                ActualizarCombo();

                // ⭐ Mostrar emoji y mensaje de acierto
                if (sistemaEmojis != null)
                    sistemaEmojis.MostrarAcierto();

                PlaySound(sonidoAcierto);
                return;
            }
        }

        // ⭐ Si no aciertas, efecto de fallo
        CrearEfectoFallo(zonasPresionar[dir]);
        combo = 0;
        ActualizarCombo();

        // ⭐ Mostrar emoji y mensaje de fallo
        if (sistemaEmojis != null)
            sistemaEmojis.MostrarFallo();

        PlaySound(sonidoFallo);
        PerderVida();
    }

    // ⭐ NUEVO: Crear efecto visual de acierto
    void CrearEfectoAcierto(RectTransform posicion)
    {
        if (posicion == null) return;

        // Crear texto flotante
        GameObject textoObj = new GameObject("FloatingText");
        textoObj.transform.SetParent(transform);
        RectTransform textoRect = textoObj.AddComponent<RectTransform>();
        textoRect.anchoredPosition = posicion.anchoredPosition;

        TextMeshProUGUI tmp = textoObj.AddComponent<TextMeshProUGUI>();
        tmp.text = "+10";
        tmp.fontSize = 36;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.green;

        CanvasGroup cg = textoObj.AddComponent<CanvasGroup>();

        StartCoroutine(AnimarTextoFlotante(textoRect, cg, textoObj));

        // Parpadeo de la zona
        StartCoroutine(ParpadeoZona(posicion, Color.green));
    }

    // ⭐ NUEVO: Crear efecto visual de fallo
    void CrearEfectoFallo(RectTransform posicion)
    {
        if (posicion == null) return;
        StartCoroutine(ParpadeoZona(posicion, Color.red));
    }

    // ⭐ NUEVO: Animar texto flotante
    IEnumerator AnimarTextoFlotante(RectTransform rect, CanvasGroup cg, GameObject obj)
    {
        float tiempo = 0f;
        float duracion = 1f;
        Vector2 posInicial = rect.anchoredPosition;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / duracion;

            rect.anchoredPosition = posInicial + Vector2.up * (100f * progreso);
            cg.alpha = Mathf.Lerp(1f, 0f, progreso);

            yield return null;
        }

        Destroy(obj);
    }

    // ⭐ NUEVO: Parpadeo de zona
    IEnumerator ParpadeoZona(RectTransform zona, Color color)
    {
        Image img = zona.GetComponent<Image>();
        if (img == null) yield break;

        Color colorOriginal = img.color;
        float tiempo = 0f;
        float duracion = 0.3f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / duracion;
            img.color = Color.Lerp(color, colorOriginal, progreso);
            yield return null;
        }

        img.color = colorOriginal;
    }

    // ⭐ NUEVO: Animar zona presionada
    void AnimarZonaPresionada(RectTransform zona)
    {
        StartCoroutine(EscalaZona(zona));
    }

    // ⭐ NUEVO: Escala de zona
    IEnumerator EscalaZona(RectTransform zona)
    {
        Vector3 escalaOriginal = zona.localScale;
        float tiempo = 0f;
        float duracion = 0.1f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / duracion;
            zona.localScale = Vector3.Lerp(escalaOriginal, escalaOriginal * 1.15f, progreso);
            yield return null;
        }

        tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / duracion;
            zona.localScale = Vector3.Lerp(escalaOriginal * 1.15f, escalaOriginal, progreso);
            yield return null;
        }

        zona.localScale = escalaOriginal;
    }

    void PerderVida()
    {
        vidaActual--;
        ActualizarCorazones();
        if (vidaActual <= 0) GameOver();
    }

    void ActualizarCorazones()
    {
        for (int i = 0; i < corazones.Length; i++)
            corazones[i].sprite = i < vidaActual ? corazonLleno : corazonVacio;
    }

    void ActualizarScore()
    {
        if (textoScore != null)
            textoScore.text = "Score: " + score;

        // ⭐ NUEVO: Actualizar barra de progreso
        if (barraProgreso != null)
        {
            barraProgreso.fillAmount = Mathf.Min(score / 500f, 1f);
        }
    }

    // ⭐ NUEVO: Actualizar combo
    void ActualizarCombo()
    {
        if (textoCombo != null)
        {
            textoCombo.text = combo > 0 ? "Combo: " + combo : "";

            if (combo > 0)
            {
                StartCoroutine(AnimarCombo(textoCombo));
            }
        }
    }

    // ⭐ NUEVO: Animar combo
    IEnumerator AnimarCombo(TextMeshProUGUI texto)
    {
        Vector3 escalaOriginal = texto.transform.localScale;
        float tiempo = 0f;
        float duracion = 0.15f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / duracion;
            texto.transform.localScale = Vector3.Lerp(escalaOriginal, escalaOriginal * 1.2f, progreso);
            yield return null;
        }

        tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float progreso = tiempo / duracion;
            texto.transform.localScale = Vector3.Lerp(escalaOriginal * 1.2f, escalaOriginal, progreso);
            yield return null;
        }

        texto.transform.localScale = escalaOriginal;
    }

    void GameOver()
    {
        juegoActivo = false;

        // ⭐ NUEVO: Resetear emojis antes de mostrar game over
        SistemaEmojis sistemaEmojis = FindFirstObjectByType<SistemaEmojis>();
        if (sistemaEmojis != null)
            sistemaEmojis.Resetear();

        // Destruir flechas activas
        foreach (var f in flechasActivas)
            if (f != null) Destroy(f);
        flechasActivas.Clear();

        if (textoScoreFinal != null)
            textoScoreFinal.text = "Score final: " + score; // ⭐ MEJORADO: Mostrar combo también

        if (panelGameOver != null)
            panelGameOver.SetActive(true);
    }

    public void Reintentar()
    {
        Debug.Log("REINTENTAR PRESIONADO");
        score = 0;
        combo = 0; // ⭐ NUEVO
        vidaActual = corazones.Length;
        ActualizarCorazones();
        ActualizarScore();
        ActualizarCombo(); // ⭐ NUEVO
        juegoActivo = true;
        if (panelGameOver != null)
            panelGameOver.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // ⭐ NUEVO: Resetear emojis
        SistemaEmojis sistemaEmojis = FindFirstObjectByType<SistemaEmojis>();
        if (sistemaEmojis != null)
            sistemaEmojis.Resetear();
    }

    public void Salir()
    {
        // Reactivar personaje
        var playerController2 = FindFirstObjectByType<StarterAssets.ThirdPersonController>(FindObjectsInactive.Include);
        if (playerController2 != null)
            playerController2.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    // ⭐ NUEVO: Reproducir sonido
    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}