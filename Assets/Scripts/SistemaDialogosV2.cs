using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class OpcionDialogo
{
    public string texto;
    public string[] respuesta;
}

[System.Serializable]
public class Dialogo
{
    public string nombre;
    public Sprite retrato;
    public string[] lineasInicio;

    [Header("Opciones (dejar vacío si es diálogo lineal)")]
    public OpcionDialogo[] opciones;
}

public class SistemaDialogosV2 : MonoBehaviour
{
    [Header("UI Diálogo")]
    public GameObject panelDialogo;
    public Image imagenNPC;
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoDialogo;
    public Button botonSiguiente;
    public TextMeshProUGUI textoBoton;

    [Header("UI Opciones")]
    public GameObject panelOpciones;
    public Button[] botonesOpciones;
    public TextMeshProUGUI[] textosOpciones;

    [Header("Velocidad")]
    public float velocidadTexto = 0.05f;
    private StarterAssets.ThirdPersonController playerController;

    private Dialogo dialogoActual;
    private int indiceLinea = 0;
    private bool escribiendo = false;
    private Coroutine corrutina;
    private bool mostrandoOpciones = false;
    private string[] respuestaActual;
    private NPC npcActual; // ⭐ NUEVO: Guardar referencia al NPC

    void Start()
    {
        if (panelDialogo != null)
            panelDialogo.SetActive(false);

        if (panelOpciones != null)
            panelOpciones.SetActive(false);

        playerController = FindFirstObjectByType<StarterAssets.ThirdPersonController>();
    }
    void Update()
    {
        if (dialogoActual != null)
        {
            // ⭐ Bloquear salto mientras hay diálogo
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Input.ResetInputAxes(); // Resetear input

                if (!mostrandoOpciones)
                {
                    SiguienteLinea();
                }
            }
        }
    }
  
    public void IniciarDialogo(Dialogo dialogo, NPC npc = null)
    {
        dialogoActual = dialogo;
        indiceLinea = 0;
        respuestaActual = null;
        mostrandoOpciones = false;
        npcActual = npc;

        if (panelDialogo != null)
            panelDialogo.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // ⭐ Desactivar COMPLETAMENTE el player
        if (playerController != null)
            playerController.enabled = false;

        MostrarLinea();
    }
    void MostrarLinea()
    {
        if (dialogoActual == null)
        {
            TerminarDialogo();
            return;
        }

        if (respuestaActual != null)
        {
            if (indiceLinea >= respuestaActual.Length)
            {
                respuestaActual = null;
                indiceLinea = 0;
                VolverAlInicio();
                return;
            }
        }
        else
        {
            if (indiceLinea >= dialogoActual.lineasInicio.Length)
            {
                if (dialogoActual.opciones != null && dialogoActual.opciones.Length > 0)
                {
                    MostrarOpciones();
                    return;
                }
                else
                {
                    TerminarDialogo();
                    return;
                }
            }
        }

        if (imagenNPC != null && dialogoActual.retrato != null)
            imagenNPC.sprite = dialogoActual.retrato;

        if (textoNombre != null)
            textoNombre.text = dialogoActual.nombre;

        string lineaActual = respuestaActual != null ?
            respuestaActual[indiceLinea] :
            dialogoActual.lineasInicio[indiceLinea];

        if (escribiendo && corrutina != null)
            StopCoroutine(corrutina);

        corrutina = StartCoroutine(EscribirTexto(lineaActual));

        indiceLinea++;

        if (botonSiguiente != null)
        {
            int totalLineas = respuestaActual != null ?
                respuestaActual.Length :
                dialogoActual.lineasInicio.Length;

            if (indiceLinea >= totalLineas)
            {
                if (textoBoton != null)
                    textoBoton.text = respuestaActual != null ? "Atrás" : "Cerrar";
            }
            else
            {
                if (textoBoton != null)
                    textoBoton.text = "Siguiente";
            }
        }
    }

    void MostrarOpciones()
    {
        if (panelOpciones == null || dialogoActual.opciones == null)
            return;

        mostrandoOpciones = true;

        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            if (i < dialogoActual.opciones.Length)
            {
                botonesOpciones[i].gameObject.SetActive(true);
                textosOpciones[i].text = dialogoActual.opciones[i].texto;
                Debug.Log("Botón " + i + " visible y clickeable");
            }
            else
            {
                botonesOpciones[i].gameObject.SetActive(false);
            }
        }

        panelOpciones.SetActive(true);
        panelOpciones.transform.SetAsLastSibling();
    }
    public void SeleccionarOpcion(int indice)
    {
        if (indice >= dialogoActual.opciones.Length)
            return;

        // ⭐ Si es opción 4, terminar
        if (indice == 3)
        {
            respuestaActual = null;
            indiceLinea = 0;
            TerminarDialogo();
            return;
        }

        respuestaActual = dialogoActual.opciones[indice].respuesta;
        indiceLinea = 0;

        if (panelOpciones != null)
            panelOpciones.SetActive(false);

        mostrandoOpciones = false;

        MostrarLinea();
    }

    void VolverAlInicio()
    {
        respuestaActual = null;
        indiceLinea = 0;
        mostrandoOpciones = true;

        if (panelOpciones != null)
            panelOpciones.SetActive(true);

        MostrarLinea();
    }

    IEnumerator EscribirTexto(string texto)
    {
        escribiendo = true;
        textoDialogo.text = "";

        foreach (char letra in texto)
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadTexto);
        }

        escribiendo = false;
    }

    void SiguienteLinea()
    {
        if (escribiendo)
        {
            StopCoroutine(corrutina);
            if (respuestaActual != null)
            {
                textoDialogo.text = respuestaActual[indiceLinea - 1];
            }
            else
            {
                textoDialogo.text = dialogoActual.lineasInicio[indiceLinea - 1];
            }
            escribiendo = false;
        }
        else
        {
            MostrarLinea();
        }
    }

    void TerminarDialogo()
    {
        if (panelDialogo != null)
            panelDialogo.SetActive(false);

        if (panelOpciones != null)
            panelOpciones.SetActive(false);

        indiceLinea = 0;
        respuestaActual = null;
        dialogoActual = null;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // ⭐ Reactivar player
        if (playerController != null)
            playerController.enabled = true;

        if (npcActual != null && npcActual.textoInteraccion != null)
            npcActual.textoInteraccion.gameObject.SetActive(true);
    }
    public void CerrarDialogo()
    {
        TerminarDialogo();
    }
}