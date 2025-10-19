using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimplePage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] Button nextButton;
    [SerializeField] TextMeshProUGUI nextButtonText;

    public void SetText(string text, string _nextButtonText = null)
    {
        content.text = text;
        if (_nextButtonText != null)
        {
            nextButtonText.text = _nextButtonText;
        }
        else
        {
            nextButtonText.text = (ReplicasController.Get("next"));
        }
    }
}
