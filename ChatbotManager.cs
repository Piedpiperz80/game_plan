using OpenAI;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ChatbotManager
{
    private OpenAIApi openai = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();

    public async Task<string> SendRequestToChatbot(string scene, string userMessage)
    {
        PrepareMessagesForChatbot(scene, userMessage);
        var response = await GetChatbotResponse();

        return HandleChatbotResponse(response);
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

    private string HandleChatbotResponse(CreateChatCompletionResponse response)
    {
        var completion = response.Choices.Count > 0 ? response.Choices[0].Message.Content : null;
        return !string.IsNullOrEmpty(completion) ? completion : null;
    }
}
