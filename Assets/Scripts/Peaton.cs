using UnityEngine;
using UnityEngine.AI;

public class Peaton : MonoBehaviour
{
    public NavMeshAgent AI;
    public float Velocidad;
    public Transform[] Objetivos;
    Transform objetivo;
    public float Distancia;
    [Header("Animaciones")]
    public Animator Anim;
    public string CaminandoAnim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objetivo = Objetivos[Random.Range(0, Objetivos.Length)];

        Anim.Play(CaminandoAnim);
    }

    // Update is called once per frame
    void Update()
    {
        Distancia = Vector3.Distance(transform.position, objetivo.position);

        if (Distancia < 2f)
        {
            objetivo = Objetivos[Random.Range(0, Objetivos.Length)];
        }
        AI.destination = objetivo.position;

        AI.speed = Velocidad;
    }
}
