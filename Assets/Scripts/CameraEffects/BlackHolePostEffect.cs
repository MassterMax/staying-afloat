using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class BlackHolePostEffect : MonoBehaviour
{
    public Material material;
    public float debugDistance = 100f;
    public Vector2 blackHolePos = new Vector2(0.5f, 0.5f); // in viewport coordinates (0..1)
    [Range(0f, 1f)] public float distortionStrength = 0.5f;
    [Range(0f, 1f)] public float collapse = 0f;
    [Range(0.01f, 1f)] public float edgeSoftness = 0.2f;

    // центр дыры в мировых координатах — назначай объект дыры или позицию
    public Transform blackHoleWorldTransform;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material == null)
        {
            Graphics.Blit(src, dest);
            return;
        }
        SetForDebug();

        // вычисляем позицию черной дыры в нормализованных координатах Viewport (0..1)
        // Vector2 bhScreen = new Vector2(0.5f, 0.5f);
        // if (blackHoleWorldTransform != null && Camera.main != null)
        // {
        //     Vector3 vp = Camera.main.WorldToViewportPoint(blackHoleWorldTransform.position);
        //     bhScreen = new Vector2(vp.x, vp.y);
        // }

        material.SetFloat("_Distortion", distortionStrength);
        material.SetFloat("_Collapse", collapse);
        material.SetVector("_BlackHolePos", blackHolePos);
        material.SetFloat("_EdgeSoftness", edgeSoftness);

        Graphics.Blit(src, dest, material);
    }

    void SetForDebug()
    {
        float t = Mathf.Clamp01(1f - debugDistance / 100f); // 0..1

        distortionStrength = Mathf.Lerp(0f, 1f, t);
        collapse = Mathf.Pow(t, 2f); // быстрее увеличивается ближе к нулю
        edgeSoftness = Mathf.Lerp(0.25f, 0.05f, t);
    }
}
