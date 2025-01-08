using UnityEngine;

public class ShaderController : MonoBehaviour
{
    public Material cloudMaterial; // Assign your material in the inspector

    void Update()
    {
        if (cloudMaterial != null)
        {
            cloudMaterial.SetFloat("_DeltaTime", Time.deltaTime);
        }
    }
}
