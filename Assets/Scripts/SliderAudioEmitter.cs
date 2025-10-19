using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderAudioEmitter : MonoBehaviour, IPointerDownHandler
{
    [Tooltip("Sound name passed to GameStateManager.Play(...)")]
    [SerializeField] string soundName = "engine";

    public void OnPointerDown(PointerEventData eventData)
    {
        // если нужен звук при нажатии — раскомментируйте
        if (GameStateManager.Instance != null) GameStateManager.Instance.Play(soundName);
    }

    // public void OnPointerUp(PointerEventData eventData)
    // {
    //     if (GameStateManager.Instance != null)
    //     {
    //         GameStateManager.Instance.Play(soundName);
    //     }
    //     else
    //     {
    //         Debug.LogWarning("GameStateManager.Instance is null — cannot play sound: " + soundName);
    //     }
    // }
}