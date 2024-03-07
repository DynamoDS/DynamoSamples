using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Dynamo.Core;
using Dynamo.Extensions;
using Dynamo.Models;
using Dynamo.UI.Commands;
using Dynamo.ViewModels;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace DynamoAssistant
{
    public class DynamoAssistantWindowViewModel : NotificationObject, IDisposable
    {
        private string userInput;
        private readonly ReadyParams readyParams;
        internal DynamoViewModel dynamoViewModel;

        // Chat GPT related fields
        private readonly OpenAIAPI chatGPTClient;
        private readonly Conversation conversation;
        private static readonly string apikey = "Your API Key";

        /// <summary>
        /// User input to the Copilot
        /// </summary>
        public string UserInput
        {
            get { return userInput; }
            set
            {
                if (value != null)
                {
                    // Set the value of the MessageText property
                    userInput = value;
                    // Raise the PropertyChanged event
                    RaisePropertyChanged(nameof(UserInput));
                }
            }
        }

        /// <summary>
        /// Dynamo Model getter
        /// </summary>
        internal DynamoModel dynamoModel => dynamoViewModel.Model;

        public ObservableCollection<string> Messages { get; set; } = new ObservableCollection<string>();

        public DynamoAssistantWindowViewModel(ReadyParams p)
        {
            readyParams = p;

            // Create a ChatGPTClient instance with the API key
            chatGPTClient = new OpenAIAPI(new APIAuthentication(apikey));
            // ChatGPT lets you start a new chat. 
            conversation = chatGPTClient.Chat.CreateConversation();
            conversation.Model = Model.GPT4_Turbo;
            // Adjust this value for more or less "creativity" in the response
            conversation.RequestParameters.Temperature = 0.2;
            // Display a welcome message
            Messages.Add("Copilot:\nWelcome to Dynamo world and ask me anything to get started!\n");
        }

        internal async void SendMessage(string msg)
        {
            // Send the user's input to the ChatGPT API and receive a response
            conversation?.AppendUserInput(msg);
            // Display user message first
            Messages.Add("You:\n" + msg + "\n");
            string response = await conversation.GetResponseFromChatbotAsync();
            // Display the chatbot's response
            Messages.Add("Copilot:\n" + response + "\n");

            var responseToLower = response.ToLower();
            if (responseToLower.Contains("python script") || responseToLower.Contains("python node"))
            {
                CreatePythonNode(response);
            }

            // create a Dynamo note example
            // CreateNote((new Guid()).ToString(), "This is a sample Note.", 0, 0, true);
        }

        /// <summary>
        /// Create a python node in Dynamo, use latest Nuget package for this
        /// </summary>
        /// <param name="response"></param>
        internal void CreatePythonNode(string response)
        {
            string pythonScript = string.Empty;
            if (response.Contains("```python"))
            {
                pythonScript = response.Split("```python")[1];
                if(pythonScript.Contains("```"))
                {
                    pythonScript = pythonScript.Split("```")[0];
                }
            }
            else return;

            var pythonNode = new PythonNodeModels.PythonNode
            {
                Script = pythonScript
            };
            dynamoModel.ExecuteCommand(new DynamoModel.CreateNodeCommand(pythonNode, 0, 0, true, false));
            Messages.Add("Copilot:\nThe Python node including the code above has been created for you!\n");
        }

        internal void CreateNote(string nodeId, string noteText, double x, double y, bool defaultPosition)
        {
            dynamoModel.ExecuteCommand(new DynamoModel.CreateNoteCommand(nodeId, noteText, x, y, defaultPosition));
            Messages.Add("Copilot:\nYour note has been created!\n");
        }

        /// <summary>
        /// Dispose function
        /// </summary>
        public void Dispose()
        {
            // Do nothing
        }

        private DelegateCommand enterCommand;

        public ICommand EnterCommand
        {
            get
            {
                enterCommand ??= new DelegateCommand(Enter);

                return enterCommand;
            }
        }

        private void Enter(object commandParameter)
        {
            SendMessage(commandParameter as string);
            // Raise event to update the UI
            UserInput = string.Empty;
        }
    }
}
