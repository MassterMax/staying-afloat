using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HyperJumpController : MonoBehaviour
{
    [SerializeField] GameObject ship;
    [SerializeField] Image jumpPanel;
    void Start()
    {
        jumpPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void DoHyperJump(Action onComplete = null, bool keepPanel = true)
    {
        StartCoroutine(HyperJump(onComplete, keepPanel));
    }

    private IEnumerator HyperJump(Action onComplete, bool keepPanel)
    {
        float duration = 0.3f;
        float timer = 0f;
        Vector3 startScale = ship.transform.localScale;
        Vector3 endScale = new Vector3(startScale.x * 5f, startScale.y * 0.3f, 1f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            ship.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        // Вспышка (можно через ParticleSystem или спрайт)
        // Потом выключаем/телепортируем корабль
        ship.SetActive(false);
        jumpPanel.gameObject.SetActive(true);

        for (float t = 0; t < 1f; t += Time.deltaTime * 4f)
        {
            jumpPanel.color = new Color(jumpPanel.color.r, jumpPanel.color.g, jumpPanel.color.b, Mathf.Lerp(0f, 1f, t));
            yield return null;
        }
        // for (float t = 0; t < 1f; t += Time.deltaTime * 2f)
        // {
        //     jumpPanel.color = new Color(jumpPanel.color.r, jumpPanel.color.g, jumpPanel.color.b, Mathf.Lerp(1f, 0f, t));
        //     yield return null;
        // }
        jumpPanel.gameObject.SetActive(keepPanel);
        onComplete?.Invoke();
    }


}
