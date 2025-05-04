using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CapitalizeFirstLetter : MonoBehaviour
{
    private Text textComponent;

    void Awake()
    {
        textComponent = GetComponent<Text>();
        CapitalizeFirstLetterInText();
    }

    void OnValidate()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<Text>();
        }
        CapitalizeFirstLetterInText();
    }

    void CapitalizeFirstLetterInText()
    {
        if (!string.IsNullOrEmpty(textComponent.text))
        {
            textComponent.text = char.ToUpper(textComponent.text[0]) + textComponent.text.Substring(1);
        }
    }
}