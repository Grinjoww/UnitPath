using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterMaze : MonoBehaviour   // 👈 IMPORTANTE: hereda de MonoBehaviour
{
    public string mazeSceneName = "MazeScene";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(mazeSceneName);
        }
    }
}
