﻿using System;
using System.Collections.ObjectModel;
using System.IO;
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
        private static readonly string apikey = "Your API";

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

        /// <summary>
        /// Is Gopilot waiting for input, this boolean dominates certain UX aspects
        /// </summary>
        public bool IsWaitingForInput = true;

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
            if (string.IsNullOrEmpty(msg)) return;

            // Set Dynamo file location
            string filePath = @"filepath";

            // A set of instructions to prepare GBT to analyze Dynamo nodes better
            string preInfo = "Given a JSON file representing a Dynamo for Revit project, perform a comprehensive analysis focusing on the graph's node structure. Your tasks include:\r\n\r\nReview Node Connections: Ensure each node is connected correctly according to Dynamo's expected data types and functionalities. Identify any instances where inputs may be receiving incorrect data types or where outputs are not utilized efficiently.\r\n\r\nData Type Validation: For each node input and output, validate that the data types are compatible with their intended functions. Highlight mismatches, such as a string data type connected to a numeric input without appropriate conversion.\r\n\r\nIdentify Unnecessary Nodes: Detect nodes that do not contribute to the final output or create redundant processes within the graph. This includes nodes with default values that never change or intermediary nodes that could be bypassed without altering the graph's outcome.\r\n\r\nOptimization Recommendations: Based on your analysis, recommend specific changes to the node structure. This might involve reordering nodes for logical flow, changing node types for efficiency, or altering connections to ensure data type compatibility.\r\n\r\nUpdate JSON Structure: Apply the optimization recommendations to the JSON file. Directly modify the \"Nodes\" and \"Connectors\" sections to reflect the optimized graph layout. Ensure that all other elements of the JSON file, such as \"Uuid\", \"Description\", \"ElementResolver\", and metadata, remain unchanged to preserve the file's integrity and additional context.\r\n\r\nOutput an Optimized JSON: Provide a revised JSON file, focusing exclusively on an updated node structure that reflects your analysis and optimizations. This file should retain all original details except for the modifications to nodes and their connections to address identified issues and enhance efficiency.";

            //Read the file 
            string jsonData = PickFile(filePath);

            IsWaitingForInput = false;

            // Display user message first
            Messages.Add("You:\n" + msg + "\n");

            msg = msg + "This is my Dynamo project JSON structure." + jsonData;
            // Send the user's input to the ChatGPT API and receive a response
            conversation?.AppendUserInput(preInfo + msg);
            string response = await conversation.GetResponseFromChatbotAsync();
            // Display the chatbot's response
            Messages.Add("Copilot:\n" + response + "\n");
            File.WriteAllText(filePath, response);

            var responseToLower = response.ToLower();
            if (responseToLower.Contains("python script") || responseToLower.Contains("python node"))
            {
                CreatePythonNode(response);
            }

            // create a Dynamo note example
            // CreateNote((new Guid()).ToString(), "This is a sample Note.", 0, 0, true);
            IsWaitingForInput = true;
        }
        internal string PickFile(string filePath)
        {
            // Read the file into a string
            var jsonData = File.ReadAllText(filePath);
            return jsonData;
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
                if (pythonScript.Contains("```"))
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
