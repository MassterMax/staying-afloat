using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class SideBlackHoleWarpOnlyEffect : MonoBehaviour
{
    public Material material;
    public float debugDistance = 100f;

    [Range(0f, 1f)] public float distortion = 0f;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material == null)
        {
            Graphics.Blit(src, dest);
            return;
        }

        // Расстояние от корабля до дыры
        distortion = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(1f - debugDistance / 100f));

        material.SetFloat("_Distortion", distortion);

        Graphics.Blit(src, dest, material);
    }
}
