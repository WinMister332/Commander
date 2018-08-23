using System;
using System.Collections.Generic;
using System.Text;

namespace WMCommandFramework.NETStandard
{
    public class InputMessage
    {
        private List<MessageText> text;

        public InputMessage()
        {
            text = new List<MessageText>(0);
        }

        /// <summary>
        /// The message to append to 'InputMessage'.
        /// </summary>
        /// <param name="text"></param>
        public InputMessage(params MessageText[] text)
        {
            this.text = new List<MessageText>(text.Length);
            foreach (MessageText mt in text)
            {
                this.text.Add(mt);
            }
        }

        /// <summary>
        /// Get the entire message.
        /// </summary>
        /// <returns>The entire 'InputMessage'.</returns>
        public MessageText[] GetMessage()
        {
            return text.ToArray();
        }

        /// <summary>
        /// Gets a specific portion of the 'InputMessage' based off the index of the array it was applied to.
        /// </summary>
        /// <param name="index">The index of the message in the array.</param>
        /// <returns>Return the message based on the array.</returns>
        public MessageText GetMessageText(int index)
        {
            var x = GetMessage();
            return x[index];
        }

        /// <summary>
        /// Gets the index of a specific message from the 'InputMessage' array.
        /// </summary>
        /// <param name="messageText">The message in the array.</param>
        /// <returns>The index of the message in the parent array.</returns>
        public int GetIndex(MessageText messageText)
        {
            return text.IndexOf(messageText);
        }

        public void AppendMessage(MessageText text)
        {
            var a = this.text;
            a.Add(text);
        }

        public static InputMessage AppendMessage(InputMessage input, MessageText text)
        {
            input.AppendMessage(text);
            return input;
        }

        public static InputMessage Empty()
        {
            return new InputMessage();
        }

        public static InputMessage SimpleHelp()
        {
            return new InputMessage(new MessageText("[Type \"help\" for help.]"), MessageText.WhiteSpace());
        }
    }

    public class MessageText
    {
        private bool colorSet = false;
        private ConsoleColor textColor = ConsoleColor.White;
        private string text = "";

        /// <summary>
        /// Creates a new instance of the 'MessageText' class, while setting the values for the 'MessageText' specified.
        /// </summary>
        /// <param name="text">The text.</param>
        public MessageText(string text)
        {
            this.text = text;
            colorSet = false;
            textColor = Console.ForegroundColor;
        }

        /// <summary>
        /// Creates a new instance of the 'MessageText' class, while setting the values for the 'MessageText' specified.
        /// </summary>
        /// <param name="color">The color of this specific portion of the text in the overall message.</param>
        /// <param name="text">The text.</param>
        public MessageText(ConsoleColor color, string text)
        {
            this.text = text;
            textColor = color;
            colorSet = true;
        }

        /// <summary>
        /// The color of the text set in the constructor.
        /// </summary>
        /// <returns>The consolecolor of the text by the constructor.</returns>
        public ConsoleColor GetColor()
        {
            if (colorSet)
                return textColor;
            else
                return Console.ForegroundColor;
        }

        /// <summary>
        /// The text.
        /// </summary>
        /// <returns>The text.</returns>
        public string GetText()
        {
            if (!(text == "" || text == null))
                return text;
            else return "";
        }

        public static MessageText CurrentColor()
        {
            return new MessageText(Console.ForegroundColor, "");
        }

        public static MessageText NewLine()
        {
            return new MessageText("\n");
        }

        public static MessageText Empty()
        {
            return new MessageText("");
        }

        public static MessageText WhiteSpace()
        {
            return new MessageText(" ");
        }
    }
}
