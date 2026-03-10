using UnityEngine;

public class ConexionCapitulosManager : MonoBehaviour
{
    public GameObject highlightBiblioteca;
    public GameObject highlightBienestar;
    public GameObject highlightEnfermeria;
    public GameObject highlightPsicologia;
    void Start()
    {
        if (GameManager.capituloBibliotecaCompletado && highlightBiblioteca != null)
            highlightBiblioteca.SetActive(false);

        if (GameManager.capituloBienestarCompletado && highlightBienestar != null)
            highlightBienestar.SetActive(false);

        if (GameManager.capituloEnfermeriaCompletado && highlightEnfermeria != null)
            highlightEnfermeria.SetActive(false);

        if (GameManager.capituloPsicologiaCompletado && highlightPsicologia != null)
            highlightPsicologia.SetActive(false);
    }
}
