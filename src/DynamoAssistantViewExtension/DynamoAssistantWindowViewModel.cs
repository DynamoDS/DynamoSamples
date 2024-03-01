﻿using Dynamo.Core;
using Dynamo.Extensions;
using Dynamo.UI.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DynamoAssistant
{
    public class DynamoAssistantWindowViewModel : NotificationObject, IDisposable
    {
        private string userInput;
        private readonly ReadyParams readyParams;
        private readonly ChatGPTClient chatGPTClient;
        private static readonly string apikey = "Your API";

        /// <summary>
        /// 
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

        public ObservableCollection<string> Messages { get; set; } = new ObservableCollection<string>();

        public DynamoAssistantWindowViewModel(ReadyParams p)
        {
            readyParams = p;

            // Create a ChatGPTClient instance with the API key
            chatGPTClient = new ChatGPTClient(apikey);
            // Display a welcome message
            Messages.Add("Assistant:\nWelcome to Dynamo world and ask me anything to get started!");
        }

        internal void SendMessage(string msg)
        {
            // Send the user's input to the ChatGPT API and receive a response
            string response = chatGPTClient?.SendMessage(msg);
            // Display user message first
            Messages.Add("You:\n" + msg);
            // Display the chatbot's response
            Messages.Add("Assistant:\n" + response);
        }

        public void Dispose()
        {
            // Do nothing
        }

        private DelegateCommand enterCommand;

        public ICommand EnterCommand
        {
            get
            {
                if (enterCommand == null)
                {
                    enterCommand = new DelegateCommand(Enter);
                }

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
