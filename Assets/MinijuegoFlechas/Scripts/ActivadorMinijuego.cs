using UnityEngine;
using TMPro;
using StarterAssets;
using UnityEngine.InputSystem;

public class ActivadorMinijuego : MonoBehaviour
{
    public GameObject canvasMinijuego;
    public TextMeshProUGUI textoInteraccion;

    private bool jugadorCerca = false;
    private ThirdPersonController playerController;
    private StarterAssetsInputs playerInput;

    void Start()
    {
        playerController = FindFirstObjectByType<ThirdPersonController>();
        playerInput = FindFirstObjectByType<StarterAssetsInputs>();
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            textoInteraccion.gameObject.SetActive(false);

            // Desactivar el objeto raíz del personaje
            var playerArmature = playerController.gameObject;
            playerArmature.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canvasMinijuego.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detectado: " + other.name + " tag: " + other.tag);
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            textoInteraccion.gameObject.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            textoInteraccion.gameObject.SetActive(false);
        }
    }
}