using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class SideBlackHoleEffect : MonoBehaviour
{
    public float debugDistance = 100f;
    public Vector2 blackHolePos = new Vector2(0.5f, 0.5f); // in viewport coordinates (0..1)
    public Material material;
    // public Transform blackHole; // твой объект дыры (спрайт)
    // public Transform player;

    [Range(0f, 1f)] public float distortion = 0f;
    [Range(0f, 1f)] public float collapse = 0f;
    [Range(0f, 1f)] public float blackHoleEdge = 0.5f;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material == null)
        {
            Graphics.Blit(src, dest);
            return;
        }

        float t = Mathf.Clamp01(1f - debugDistance / 100f);

        // Чем ближе — тем сильнее искажение и схлопывание
        distortion = Mathf.SmoothStep(0f, 1f, t * 1.2f);
        collapse = Mathf.SmoothStep(0f, 1f, t * 1.5f);
        blackHoleEdge = Mathf.Lerp(0.3f, 0.7f, t);

        material.SetFloat("_Distortion", distortion);
        material.SetFloat("_Collapse", collapse);
        material.SetFloat("_BlackHoleEdge", blackHoleEdge);

        Graphics.Blit(src, dest, material);
    }
}
