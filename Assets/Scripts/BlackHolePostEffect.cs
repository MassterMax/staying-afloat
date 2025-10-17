using UnityEngine;

[ExecuteInEditMode]
public class BlackHolePostEffect : MonoBehaviour
{
    public Material material; // шейдер с искажением
    [Range(0f, 1f)] public float distortionStrength = 0.1f;
    public Vector2 blackHoleScreenPos = new Vector2(1f, 0.5f); // 0..1 по X и Y

    public float collapse = 0f;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            material.SetFloat("_Collapse", collapse);
            material.SetFloat("_Distortion", distortionStrength);
            material.SetVector("_BlackHolePos", blackHoleScreenPos);
            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
