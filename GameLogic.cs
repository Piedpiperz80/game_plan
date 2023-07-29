/*
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using OpenAI;
using UnityEngine.UI;
using System.Collections;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private TMP_Text gameText;
    [SerializeField] private TMP_InputField userInput;

    private OpenAIApi openai = new OpenAIApi();

    private string sceneDescription;
    private string userChoice;
    [SerializeField] private StoryBlock storyBlockAsset; // Reference to the ScriptableObject

    private StoryBlock currentStoryBlock;
    private List<ChatMessage> messages = new List<ChatMessage>();
    private bool isAIActive = false;  // Keep track of when AI is active
    private Coroutine processingAnimation;

    private void Start()
    {
        // Set up the initial scene description from the ScriptableObject
        if (storyBlockAsset != null)
        {
            currentStoryBlock = storyBlockAsset;
            UpdateStory();
        }
        else
        {
            Debug.LogError("No StoryBlockAsset assigned.");
            return;
        }

        // Add event listener for the end edit event of the input field
        userInput.onEndEdit.AddListener(OnUserInputEndEdit);

        // Select the input field when the game starts
        userInput.Select();
        userInput.ActivateInputField();
    }

    private void UpdateStory()
    {
        sceneDescription = currentStoryBlock.GetStoryText();
        gameText.text = sceneDescription;

        // Adjust user input placeholder based on the number of nextStoryBlocks
        var nextStoryBlocks = currentStoryBlock.GetNextStoryBlock();
        if (nextStoryBlocks.Length == 1 && nextStoryBlocks[0] == null)
        {
            // Activate the AI and change input prompt
            isAIActive = true;
            userInput.placeholder.GetComponent<TMP_Text>().text = "Please input your next action...";
        }
        else if (nextStoryBlocks.Length == 1)
        {
            userInput.placeholder.GetComponent<TMP_Text>().text = "Press any key to continue...";
        }
        else
        {
            userInput.placeholder.GetComponent<TMP_Text>().text = "Please input your choice...";
        }

        // Always enable the user input
        userInput.gameObject.SetActive(true);
    }

    private void OnUserInputEndEdit(string value)
    {
        userInput.text = ""; // Clear the userInput field
        userInput.Select();  // Select the userInput field
        userInput.ActivateInputField(); // Activate the userInput field

        if (isAIActive)
        {
            // Get the user's input choice
            userChoice = value;

            // Start processing animation
            processingAnimation = StartCoroutine(ProcessingAnimation());

            // Send the user's input to ChatGPT
            SendRequestToChatbot(sceneDescription, userChoice);
        }
        else if (currentStoryBlock.GetNextStoryBlock().Length == 1)
        {
            currentStoryBlock = currentStoryBlock.GetNextStoryBlock()[0];
            if (currentStoryBlock != null)
            {
                UpdateStory();
            }
            else
            {
                Debug.LogWarning("Next story block is null.");
            }
        }
        else if (currentStoryBlock.GetNextStoryBlock().Length > 1)
        {
            if (int.TryParse(value, out int index) && index > 0 && index <= currentStoryBlock.GetNextStoryBlock().Length)
            {
                index--; // Adjust for 0-based index
                currentStoryBlock = currentStoryBlock.GetNextStoryBlock()[index];
                UpdateStory();
            }
            else
            {
                Debug.LogWarning("Invalid user input.");
            }
        }
    }

    private async void SendRequestToChatbot(string scene, string userMessage)
    {
        var systemMessage = new ChatMessage { Role = "system", Content = "You are a helpful assistant." };
        var userSceneMessage = new ChatMessage { Role = "user", Content = scene };
        var userInputMessage = new ChatMessage { Role = "user", Content = userMessage };

        messages.Add(systemMessage);
        messages.Add(userSceneMessage);
        messages.Add(userInputMessage);

        // Send the request to the OpenAI API
        var response = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-3.5-turbo",
            Messages = messages
        });

        // Stop processing animation
        if (processingAnimation != null)
        {
            StopCoroutine(processingAnimation);
            processingAnimation = null;
        }

        // Get the chatbot's response
        var completion = response.Choices.Count > 0 ? response.Choices[0].Message.Content : null;

        if (!string.IsNullOrEmpty(completion))
        {
            // Update the scene description with the chatbot's response
            sceneDescription = completion;

            // Display the updated scene description
            gameText.text = sceneDescription;
        }
        else
        {
            Debug.LogError("Chatbot response is empty.");
        }
    }

    private IEnumerator ProcessingAnimation()
    {
        while (true)
        {
            gameText.text = ".";
            yield return new WaitForSeconds(0.5f);
            gameText.text = "..";
            yield return new WaitForSeconds(0.5f);
            gameText.text = "...";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
*/
/*using System.Collections.Generic;
using UnityEngine;
using TMPro;
using OpenAI;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private TMP_Text gameText;
    [SerializeField] private TMP_InputField userInput;

    private OpenAIApi openai = new OpenAIApi();

    private string sceneDescription;
    private string userChoice;
    [SerializeField] private StoryBlock storyBlockAsset; // Reference to the ScriptableObject

    private StoryBlock currentStoryBlock;
    private List<ChatMessage> messages = new List<ChatMessage>();
    private bool isAIActive = false;  // Keep track of when AI is active
    private Coroutine processingAnimation;

    private void Start()
    {
        InitStory();
        userInput.onEndEdit.AddListener(OnUserInputEndEdit);
        ActivateInputField();
    }

    private void InitStory()
    {
        if (storyBlockAsset != null)
        {
            currentStoryBlock = storyBlockAsset;
            UpdateStory();
        }
        else
        {
            Debug.LogError("No StoryBlockAsset assigned.");
        }
    }

    private void UpdateStory()
    {
        sceneDescription = currentStoryBlock.GetStoryText();
        gameText.text = sceneDescription;

        AdjustUserInputPlaceholder();
        userInput.gameObject.SetActive(true);
    }

    private void AdjustUserInputPlaceholder()
    {
        var nextStoryBlocks = currentStoryBlock.GetNextStoryBlock();
        if (nextStoryBlocks.Length == 1 && nextStoryBlocks[0] == null)
        {
            isAIActive = true;
            SetInputFieldPlaceholder("Please input your next action...");
        }
        else if (nextStoryBlocks.Length == 1)
        {
            SetInputFieldPlaceholder("Press any key to continue...");
        }
        else
        {
            SetInputFieldPlaceholder("Please input your choice...");
        }
    }

    private void SetInputFieldPlaceholder(string text)
    {
        userInput.placeholder.GetComponent<TMP_Text>().text = text;
    }

    private void OnUserInputEndEdit(string value)
    {
        ResetInputField();
        ActivateInputField();

        if (isAIActive)
        {
            HandleAIActive(value);
        }
        else if (currentStoryBlock.GetNextStoryBlock().Length == 1)
        {
            HandleSingleNextStoryBlock();
        }
        else if (currentStoryBlock.GetNextStoryBlock().Length > 1)
        {
            HandleMultipleNextStoryBlocks(value);
        }
    }

    private void ResetInputField()
    {
        userInput.text = "";
    }

    private void ActivateInputField()
    {
        userInput.Select();
        userInput.ActivateInputField();
    }

    private void HandleAIActive(string value)
    {
        userChoice = value;
        processingAnimation = StartCoroutine(ProcessingAnimation());
        SendRequestToChatbot(sceneDescription, userChoice);
    }

    private void HandleSingleNextStoryBlock()
    {
        currentStoryBlock = currentStoryBlock.GetNextStoryBlock()[0];
        if (currentStoryBlock != null)
        {
            UpdateStory();
        }
        else
        {
            Debug.LogWarning("Next story block is null.");
        }
    }

    private void HandleMultipleNextStoryBlocks(string value)
    {
        if (int.TryParse(value, out int index) && index > 0 && index <= currentStoryBlock.GetNextStoryBlock().Length)
        {
            index--; // Adjust for 0-based index
            currentStoryBlock = currentStoryBlock.GetNextStoryBlock()[index];
            UpdateStory();
        }
        else
        {
            Debug.LogWarning("Invalid user input.");
        }
    }

    private async void SendRequestToChatbot(string scene, string userMessage)
    {
        PrepareMessagesForChatbot(scene, userMessage);
        var response = await GetChatbotResponse();

        HandleChatbotResponse(response);
    }

    private void PrepareMessagesForChatbot(string scene, string userMessage)
    {
        var systemMessage = new ChatMessage { Role = "system", Content = "You are a helpful assistant." };
        var userSceneMessage = new ChatMessage { Role = "user", Content = scene };
        var userInputMessage = new ChatMessage { Role = "user", Content = userMessage };

        messages.Add(systemMessage);
        messages.Add(userSceneMessage);
        messages.Add(userInputMessage);
    }

    private async Task<CreateChatCompletionResponse> GetChatbotResponse()
    {
        return await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-3.5-turbo",
            Messages = messages
        });
    }

    private void HandleChatbotResponse(CreateChatCompletionResponse response)
    {
        if (processingAnimation != null)
        {
            StopCoroutine(processingAnimation);
            processingAnimation = null;
        }

        var completion = response.Choices.Count > 0 ? response.Choices[0].Message.Content : null;
        if (!string.IsNullOrEmpty(completion))
        {
            sceneDescription = completion;
            gameText.text = sceneDescription;
        }
        else
        {
            Debug.LogError("Chatbot response is empty.");
        }
    }

    private IEnumerator ProcessingAnimation()
    {
        while (true)
        {
            gameText.text = ".";
            yield return new WaitForSeconds(0.5f);
            gameText.text = "..";
            yield return new WaitForSeconds(0.5f);
            gameText.text = "...";
            yield return new WaitForSeconds(0.5f);
        }
    }
}*/

using UnityEngine;
using TMPro;
using System.Collections;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private TMP_Text gameText;
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private StoryBlock storyBlockAsset; // Reference to the ScriptableObject

    private Coroutine processingAnimation;

    private ChatbotManager chatbotManager = new ChatbotManager();
    private StoryManager storyManager;
    private UserInputManager userInputManager;

    private void Start()
    {
        storyManager = new StoryManager(storyBlockAsset);
        userInputManager = new UserInputManager(userInput);
        UpdateStory();
        userInput.onEndEdit.AddListener(OnUserInputEndEdit);
        userInputManager.ResetAndActivateInputField();
    }

    private void UpdateStory()
    {
        gameText.text = storyManager.GetCurrentStoryText();

        if (storyManager.GetNextStoryBlockCount() == 0)
        {
            userInputManager.SetInputFieldPlaceholder("Press any key to continue...");
        }
        else if (storyManager.GetNextStoryBlockCount() > 1)
        {
            userInputManager.SetInputFieldPlaceholder("Please input your choice...");
        }
        else
        {
            userInputManager.SetInputFieldPlaceholder("Please input your next action...");
        }
    }

    private async void OnUserInputEndEdit(string value)
    {
        userInputManager.SetUserInput(""); // Clear input field

        if (storyManager.GetNextStoryBlockCount() == 0)
        {
            // If the story manager has no next blocks, simply move to the next one without any user input processing
            storyManager.MoveToNextStoryBlock(0);
            UpdateStory();
            userInputManager.ResetAndActivateInputField();
        }
        else
        {
            if (storyManager.GetNextStoryBlockCount() > 1) // In a choice block
            {
                int choice;

                if (int.TryParse(value, out choice) && choice > 0 && choice <= storyManager.GetNextStoryBlockCount())
                {
                    // Subtract 1 from choice to make it 0-based
                    storyManager.MoveToNextStoryBlock(choice - 1);
                    UpdateStory();
                }
                else
                {
                    Debug.LogWarning("Invalid user input.");
                }

                userInputManager.ResetAndActivateInputField();
            }
            else // In an AI block
            {
                processingAnimation = StartCoroutine(ProcessingAnimation());

                string completion = await chatbotManager.SendRequestToChatbot(storyManager.GetCurrentStoryText(), value);
                StopCoroutine(processingAnimation);
                storyManager.MoveToNextStoryBlock(0);
                storyManager.SetCurrentStoryText(completion);
                UpdateStory();

                userInputManager.ResetAndActivateInputField();
            }
        }
    }

    private IEnumerator ProcessingAnimation()
    {
        while (true)
        {
            gameText.text += ".";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
