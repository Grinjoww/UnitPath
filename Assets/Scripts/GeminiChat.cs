using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class GeminiChat : MonoBehaviour
{
    [Header("--- CONFIGURACIÓN ---")]
    [SerializeField] private string apiKey = "AQUI_PEGA_TU_CLAVE_API";

    [Header("--- UI (Arrastra aquí) ---")]
    public TMP_Text textoDialogoNPC; // El texto donde habla Zaida
    public GameObject objetoCargando; // (Opcional) Un icono que gira o texto "Pensando..."

    // URL de Gemini 1.5 Flash (Rápido y Gratis)
    private string apiUrl ="https://generativelanguage.googleapis.com/v1/models/gemini-2.5-flash:generateContent";





    void Start()
    {
        // Al iniciar, ocultamos el icono de carga si existe
        if (objetoCargando != null) objetoCargando.SetActive(false);
    }

    // ESTA FUNCIÓN ES LA QUE LLAMAS DESDE LOS BOTONES
    public void EnviarPregunta(string preguntaDelJugador)
    {
        StartCoroutine(LlamadaGemini(preguntaDelJugador));
    }

    IEnumerator LlamadaGemini(string inputUsuario)
    {
        // 1. Mostrar que está pensando
        if (objetoCargando != null) objetoCargando.SetActive(true);
        textoDialogoNPC.text = "..."; // Limpiamos el texto anterior

        // 2. PERSONALIDAD DE ZAIDA (System Prompt)
        // Aquí defines quién es y cómo responde.
        string personalidad = "Eres Sophie, una estudiante guía de la universidad UTEQ. " +
                              "Tu misión es ayudar a nuevos estudiantes. " +
                              "Responde de forma ÚTIL, AMABLE y MUY BREVE (máximo 25 palabras). " +
                              "No saludes siempre, ve al grano. " +
                              "Si preguntan algo fuera de la universidad, di que no sabes.";

        // 3. CREAR EL JSON (El mensaje para Google)
        // Escapamos las comillas (\") para que no rompan el formato.
        string promptCompleto = personalidad + " El usuario pregunta: " + inputUsuario;

        string jsonBody = "{ \"contents\": [{ \"parts\": [{ \"text\": \"" +
                          CleanString(promptCompleto) +
                          "\" }] }] }";

        // 4. PREPARAR EL ENVÍO
        string urlCompleta = apiUrl + "?key=" + apiKey.Trim();
        Debug.Log("URL QUE ESTOY USANDO: " + urlCompleta);
        using (UnityWebRequest request = new UnityWebRequest(urlCompleta, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // 5. ENVIAR Y ESPERAR
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // 6. PROCESAR RESPUESTA
                string respuestaJson = request.downloadHandler.text;
                string textoLimpio = ExtraerTextoDeJson(respuestaJson);
                textoDialogoNPC.text = textoLimpio;
            }
            else
            {
                Debug.LogError("Error Gemini: " + request.error);
                textoDialogoNPC.text = "¡Ups! Me quedé sin señal. (Error de conexión)";
            }
        }

        if (objetoCargando != null) objetoCargando.SetActive(false);
    }

    // Función auxiliar para limpiar el texto antes de enviarlo (evita errores con comillas)
    string CleanString(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        return s.Replace("\"", "'").Replace("\n", " ");
    }

    // Función "sucia" pero efectiva para leer el JSON sin plugins externos
    string ExtraerTextoDeJson(string json)
    {
        // Buscamos el campo "text": "
        string marcador = "\"text\": \"";
        int inicio = json.IndexOf(marcador);

        if (inicio != -1)
        {
            inicio += marcador.Length;
            int fin = json.IndexOf("\"", inicio); // Buscamos donde cierra la comilla
            if (fin != -1)
            {
                string resultado = json.Substring(inicio, fin - inicio);
                return resultado.Replace("\\n", "\n"); // Arreglar saltos de línea
            }
        }
        return "No entendí la respuesta del servidor.";
    }
}