using UnityEngine;

[ExecuteInEditMode]
public class BlackHolePostEffect : MonoBehaviour
{
    public Material material; // шейдер с искажением
    [Range(0f, 1f)] public float distortionStrength = 0.5f;
    public Vector2 blackHoleScreenPos = new Vector2(0.0f, 0.5f); // 0..1 по X и Y

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null && distortionStrength > 0f)
        {
            material.SetFloat("_DistortionStrength", distortionStrength);
            material.SetVector("_BlackHolePos", blackHoleScreenPos);
            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
