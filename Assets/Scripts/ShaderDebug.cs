using UnityEngine;

public class ShaderDebug : MonoBehaviour
{
    public Renderer targetRenderer;

    void Start()
    {
        Material mat = targetRenderer.material;

        Debug.Log("Shader: " + mat.shader.name);

        int count = mat.shader.GetPropertyCount();

        for (int i = 0; i < count; i++)
        {
            Debug.Log(mat.shader.GetPropertyName(i));
        }
    }
}