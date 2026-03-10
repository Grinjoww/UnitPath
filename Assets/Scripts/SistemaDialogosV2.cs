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

        if (botonSiguiente != null)
            botonSiguiente.onClick.AddListener(SiguienteLinea);

        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            int indice = i;
            botonesOpciones[i].onClick.AddListener(() => SeleccionarOpcion(indice));
        }
    }

    void Update()
    {
        if (dialogoActual != null)
        {
            if (!mostrandoOpciones && Input.GetKeyDown(KeyCode.Return))
            {
                SiguienteLinea();
            }

            if (mostrandoOpciones)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) && dialogoActual.opciones.Length > 0)
                    SeleccionarOpcion(0);

                if (Input.GetKeyDown(KeyCode.Alpha2) && dialogoActual.opciones.Length > 1)
                    SeleccionarOpcion(1);

                if (Input.GetKeyDown(KeyCode.Alpha3) && dialogoActual.opciones.Length > 2)
                    SeleccionarOpcion(2);

                if (Input.GetKeyDown(KeyCode.Alpha4) && dialogoActual.opciones.Length > 3)
                    SeleccionarOpcion(3);
            }
        }
    }

    // ⭐ CAMBIO: Ahora recibe el NPC que llama
    public void IniciarDialogo(Dialogo dialogo, NPC npc = null)
    {
        dialogoActual = dialogo;
        indiceLinea = 0;
        respuestaActual = null;
        mostrandoOpciones = false;
        npcActual = npc; // ⭐ Guardar el NPC

        if (panelDialogo != null)
            panelDialogo.SetActive(true);

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
            }
            else
            {
                botonesOpciones[i].gameObject.SetActive(false);
            }
        }

        panelOpciones.SetActive(true);
    }

    void SeleccionarOpcion(int indice)
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

        // ⭐ Mostrar "Presiona E" del NPC que inició el diálogo
        if (npcActual != null && npcActual.textoInteraccion != null)
            npcActual.textoInteraccion.gameObject.SetActive(true);
    }

    public void CerrarDialogo()
    {
        TerminarDialogo();
    }
}