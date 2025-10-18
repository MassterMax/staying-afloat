using System.Collections;
using TMPro;
using UnityEngine;

public class BlinkingScript : MonoBehaviour
{
    private TextMeshProUGUI text;
    private float blinkInterval = 1f;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(Blinking());
    }

    private IEnumerator Blinking()
    {
        while (true)
        {
            // Переключаем видимость текста
            text.enabled = !text.enabled;

            // Ждём заданное время
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
