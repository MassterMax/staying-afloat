using UnityEngine;

public class DestroyOnEndAnim : MonoBehaviour
{
    public void DestroyOnEnd()
    {
        if (!Application.isPlaying)
            return;
        Destroy(gameObject);
    }
}
