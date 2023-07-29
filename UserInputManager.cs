using TMPro;
using UnityEngine;

public class UserInputManager
{
    private TMP_InputField userInput;

    public UserInputManager(TMP_InputField userInputField)
    {
        userInput = userInputField;
    }

    public void ResetAndActivateInputField()
    {
        userInput.text = "";
        userInput.Select();
        userInput.ActivateInputField();
    }

    public void SetUserInput(string input)
    {
        userInput.text = input;
    }

    public void SetInputFieldPlaceholder(string placeholderText)
    {
        userInput.placeholder.GetComponent<TMP_Text>().text = placeholderText;
    }
}
