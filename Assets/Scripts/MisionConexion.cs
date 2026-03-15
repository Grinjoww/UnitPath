using UnityEngine;
using TMPro;

public class MisionConexion : MonoBehaviour
{
    public static MisionConexion instancia;
    public TextMeshProUGUI textoMision;
    public GameObject highlightPsicologia;

    void Awake()
    {
        instancia = this;
    }
    void Start()
    {
        bool biblioteca = GameManager.capituloBibliotecaCompletado;
        bool enfermeria = GameManager.capituloEnfermeriaCompletado;
        bool bienestar = GameManager.capituloBienestarCompletado;
        bool tresPrimeros = biblioteca && enfermeria && bienestar;

        string texto = "<color=#FFCC00><b>OBJETIVO:</b></color>\nVisita todos los departamentos\n\n";
        texto += (biblioteca ? "<color=#57FF57>[LISTO]</color> " : "<color=#FF4444>[PENDIENTE]</color> ") + "Biblioteca\n";
        texto += (enfermeria ? "<color=#57FF57>[LISTO]</color> " : "<color=#FF4444>[PENDIENTE]</color> ") + "Centro médico\n";
        texto += (bienestar ? "<color=#57FF57>[LISTO]</color> " : "<color=#FF4444>[PENDIENTE]</color> ") + "Bienestar universitario\n";

        if (tresPrimeros)
            texto += "<color=#00BFFF>[DISPONIBLE]</color> Oficina de Psicología";
        else
            texto += "<color=#888888>[BLOQUEADO]</color> Oficina de Psicología";

        if (textoMision != null)
            textoMision.text = texto;

        if (highlightPsicologia != null)
            highlightPsicologia.SetActive(tresPrimeros);
    }
    void ActualizarTexto()
    {
        bool biblioteca = PlayerPrefs.GetInt("Completo_Biblioteca") == 1;
        bool enfermeria = PlayerPrefs.GetInt("Completo_Enfermeria") == 1;
        bool bienestar = PlayerPrefs.GetInt("Completo_Bienestar") == 1;
        bool tresPrimeros = biblioteca && enfermeria && bienestar;

        string texto = "<color=#FFCC00><b>OBJETIVO:</b></color>\nVisita todos los departamentos\n\n";
        texto += (biblioteca ? "<color=#57FF57>[LISTO]</color> " : "<color=#FF4444>[PENDIENTE]</color> ") + "Biblioteca\n";
        texto += (enfermeria ? "<color=#57FF57>[LISTO]</color> " : "<color=#FF4444>[PENDIENTE]</color> ") + "Enfermería\n";
        texto += (bienestar ? "<color=#57FF57>[LISTO]</color> " : "<color=#FF4444>[PENDIENTE]</color> ") + "Bienestar\n";

        if (tresPrimeros)
            texto += "<color=#00BFFF>[DISPONIBLE]</color> Psicología";
        else
            texto += "<color=#888888>[BLOQUEADO]</color> Psicología";

        if (textoMision != null)
            textoMision.text = texto;

        if (highlightPsicologia != null)
            highlightPsicologia.SetActive(tresPrimeros);
    }
}