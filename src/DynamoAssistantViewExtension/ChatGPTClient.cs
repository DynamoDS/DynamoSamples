using System;
using Newtonsoft.Json;
using RestSharp;

namespace DynamoAssistant
{
    public class ChatGPTClient
    {
        private readonly string _apiKey;
        private readonly RestClient _client;
        private string _conversationHistory = string.Empty;

        // Constructor that takes the API key as a parameter
        public ChatGPTClient(string apiKey)
        {
            _apiKey = apiKey;
            // Initialize the RestClient with the ChatGPT API endpoint
            _client = new RestClient("https://api.openai.com/v1/chat/completions");
        }

        // We'll add methods here to interact with the API.
        // Method to send a message to the ChatGPT API and return the response
        public string SendMessage(string message)
        {
            // Check for empty input
            if (string.IsNullOrWhiteSpace(message))
            {
                return "Sorry, I didn't receive any input. Please try again!";
            }

            try
            {
                // Update the conversation history with the user's message
                _conversationHistory += $"User: {message}\n";

                // Create a new POST request
                var request = new RestRequest("", Method.Post);
                // Set the Content-Type header
                // request.AddHeader("Content-Type", "application/json");
                // Set the Authorization header with the API key
                request.AddHeader("Authorization", $"Bearer {_apiKey}");

                // Create the request body with the message and other parameters
                var requestBody = new
                {
                    model = "tts-1",
                    prompt = message,
                    max_tokens = 100,
                    n = 1,
                    stop = (string)null,
                    temperature = 0.7,
                };

                // Add the JSON body to the request
                request.AddJsonBody(JsonConvert.SerializeObject(requestBody));

                // Execute the request and receive the response
                var response = _client.Execute(request);

                // Deserialize the response JSON content
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content ?? string.Empty);

                // Extract and return the chatbot's response text
                string chatbotResponse = jsonResponse?.choices[0]?.text?.ToString()?.Trim() ?? string.Empty;

                // Update the conversation history with the chatbot's response
                _conversationHistory += $"Chatbot: {chatbotResponse}\n";

                return chatbotResponse;
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the API request
                Console.WriteLine($"Error: {ex.Message}");
                return "Sorry, there was an error processing your request. Please try again later.";
            }

        }
    }
}
