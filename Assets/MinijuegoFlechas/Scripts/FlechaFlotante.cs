using UnityEngine;

public class FlechaFlotante : MonoBehaviour
{
    public float velocidad = 2f;
    public float altura = 0.3f;
    private Vector3 posInicial;

    void Start()
    {
        posInicial = transform.position;
    }

    void Update()
    {
        transform.position = posInicial + Vector3.up * Mathf.Sin(Time.time * velocidad) * altura;
    }
}