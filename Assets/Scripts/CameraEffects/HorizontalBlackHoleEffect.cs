using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class HorizontalBlackHoleEffect : MonoBehaviour
{
    public Material material;
    [Range(0, 1)] public float blackHoleX = 0.8f; // Положение чёрной дыры по X
    [Range(0, 1)] public float strengthX = 0.5f;   // Сила горизонтального всасывания
    [Range(0, 1)] public float strengthY = 0.3f;   // Сила вертикального искривления

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            material.SetFloat("_BlackHoleX", blackHoleX);
            material.SetFloat("_StrengthX", strengthX);
            material.SetFloat("_StrengthY", strengthY);
            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
