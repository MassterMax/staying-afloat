using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class BlackHoleWarpEffect : MonoBehaviour
{
    public Material material;

    float distance = 100f;
    float strength = 0.266f;
    float glitchIntensity = 0.082f;
    Vector2 center;
    Vector2 radius = new Vector2(0.5f, 2f);
    Vector2 direction = new Vector2(1f, 0f);
    float offset = 80f;

    // public float _debugDistance;
    // public float _debugOffset;

    void Start()
    {
        CalculateCenter(100f);
        material.SetVector("_Center", center);
        material.SetVector("_Radius", radius);
        material.SetFloat("_Strength", strength);
        material.SetVector("_Direction", direction);
        material.SetFloat("_GlitchIntensity", glitchIntensity);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // material.SetVector("_Center", center);
        // material.SetVector("_Radius", radius);
        // material.SetFloat("_Strength", strength);
        // material.SetVector("_Direction", direction);
        // material.SetFloat("_GlitchIntensity", glitchIntensity);
        // center = new Vector2((_debugOffset + _debugDistance) / 100f, 0.5f);
        material.SetVector("_Center", center);
        material.SetVector("_Radius", radius);
        material.SetFloat("_Strength", strength);
        material.SetVector("_Direction", direction);
        material.SetFloat("_GlitchIntensity", glitchIntensity);
        center = new Vector2((offset + distance) / 100f, 0.5f);
        material.SetVector("_Center", center);

        Graphics.Blit(src, dest, material);
    }

    void CalculateCenter(float _distance)
    {
        center = new Vector2((offset + _distance) / 100f, 0.5f);
    }

    public void UpdateDistance(float _distance)
    {
        distance = _distance;
        // CalculateCenter(distance);
        // material.SetVector("_Center", center);
    }
}
