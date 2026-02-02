using UnityEngine;

public class EmotionChoice : MonoBehaviour
{
    public GameObject panelUI;

    void Start()
    {
        ShowUI();
    }

    void ShowUI()
    {
        panelUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f; // pausa el juego
    }

    void HideUI()
    {
        panelUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f; // reanuda el juego
    }

    public void ChooseAnsioso()
    {
        Debug.Log("Elección: Ansioso");
        HideUI();
    }

    public void ChooseEmocionado()
    {
        Debug.Log("Elección: Emocionado");
        HideUI();
    }

    public void ChooseConfundido()
    {
        Debug.Log("Elección: Confundido");
        HideUI();
    }

}
