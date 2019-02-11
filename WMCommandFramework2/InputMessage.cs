using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace WMCommandFramework
{
    public class InputMessage
    {
        private CommandProcessor processor;
        private List<Message> messages = null;

        public InputMessage()
        {
            messages = new List<Message>();
        }

        public InputMessage(params Message[] messages)
        {
            //Check if not null or empty.
            if (!(messages == null || messages.Length == 0 || messages == new Message[0]))
                AddMessages(messages);
        }

        internal void SetProcessor(CommandProcessor processor)
        {
            this.processor = processor;
        }

        private void AddMessage(Message message)
        {
            if (!(message == null))
                messages.Add(message);
        }

        public void AddMessages(params Message[] messages)
        {
            foreach (Message m in messages)
                AddMessage(m);
        }

        public Message[] GetMessages() => messages.ToArray();

        CommandProcessor GetProcessor() => processor;

        public static InputMessage EMPTY => new InputMessage();

        public sealed class Message
        {
            private static InputMessage inputMessage = null;
            private ConsoleColor color = ConsoleColor.White;
            private string text = "";

            public Message(string message)
            {
                text = message;
                color = inputMessage.GetProcessor().DefaultForegroundColor; 
            }

            public Message(string message, ConsoleColor color)
            {
                text = message;
                this.color = color;
            }

            internal void SetInputMessage(InputMessage message)
            {
                inputMessage = message;
            }

            public string GetMessage() => text;
            public ConsoleColor GetColor() => color;

            /// <summary>
            /// The new line message.
            /// </summary>
            /// <value>The new line.</value>
            public static Message NewLine => new Message("/n");
            /// <summary>
            /// Sets the message color to the default color.
            /// </summary>
            /// <value>The message containing the default console color.</value>
            public static Message ResetColor => new Message("", inputMessage.GetProcessor().DefaultForegroundColor);
        }
    }
}
