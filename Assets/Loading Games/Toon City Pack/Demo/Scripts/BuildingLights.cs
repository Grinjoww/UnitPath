using UnityEngine;

public class BuildingLights : MonoBehaviour
{
    public int windowMaterialIndex;
    public Color lightColor;
    public bool areLightsOn;

    private Color defaultColor;
    private MeshRenderer mr;
    private SpriteRenderer sr;

    private void Start()
    {
        // Intentamos obtener MeshRenderer
        mr = GetComponent<MeshRenderer>();
        // Intentamos obtener SpriteRenderer
        sr = GetComponent<SpriteRenderer>();

        if (mr != null)
        {
            defaultColor = mr.materials[windowMaterialIndex].color;
            SetLights(areLightsOn);
        }
        else if (sr != null)
        {
            defaultColor = sr.color;
            SetLights(areLightsOn);
        }
        else
        {
            Debug.LogError("El objeto no tiene ni MeshRenderer ni SpriteRenderer. Añade uno en el Inspector.");
        }
    }

    public void SetLights(bool isOn)
    {
        if (mr != null)
        {
            mr.materials[windowMaterialIndex].shader = isOn ? Shader.Find("Unlit/Color") : Shader.Find("Standard");
            mr.materials[windowMaterialIndex].color = isOn ? lightColor : defaultColor;
        }
        else if (sr != null)
        {
            sr.color = isOn ? lightColor : defaultColor;
        }
    }
}
