using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] Button button1;
    [SerializeField] Button button2;
    [SerializeField] TextMeshProUGUI button1Text;
    [SerializeField] TextMeshProUGUI button2Text;

    public void SetText(string text, string option1, string option2)
    {
        content.text = text;
        button1Text.text = option1;
        button2Text.text = option2;
    }
}
