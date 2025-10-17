using UnityEngine;

[ExecuteInEditMode]
public class BlackHoleWarpEffect2 : MonoBehaviour
{
    public float debugDistance = 100f;
    public Vector2 blackHolePos = new Vector2(0.5f, 0.5f); // in viewport coordinates (0..1)
    public Material material;
    [Range(0f, 1f)] public float warp = 0f;
    [Range(0.1f, 10f)] public float falloff = 3f;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material == null)
        {
            Graphics.Blit(src, dest);
            return;
        }

        SetForDebug();

        material.SetFloat("_Warp", warp);
        material.SetVector("_BlackHolePos", blackHolePos);
        material.SetFloat("_Falloff", falloff);

        Graphics.Blit(src, dest, material);
    }

    void SetForDebug()
    {
        float t = Mathf.Clamp01(1f - debugDistance / 100f);
        warp = Mathf.SmoothStep(0f, 1f, t);
    }
}
