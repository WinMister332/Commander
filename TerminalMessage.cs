using System;
using System.Collections.Generic;
using System.Text;

namespace Commander
{
    public class TerminalMessage
    {
        private static CommandProcessor processor = null;
        private static List<Message> message = null;

        internal TerminalMessage()
        {
            message = new List<Message>();
            SetProcessor(CommandProcessor.DEFAULT_INSTANCE);
        }

        public TerminalMessage(params Message[] text)
        {
            message = new List<Message>(text.Length);
            if (!(text == null || text.Length == 0 || text == new Message[0]))
                foreach (Message m in text)
                    if (!(m == null) && !message.Contains(m))
                        message.Add(m);
        }

        public Returnable<Message> GetMessage()
        {
            if (!(message == null))
                return new Returnable<Message>(message);
            return new Returnable<Message>(new Message[0]);
        }

        internal static CommandProcessor GetProcessor() => processor;

        internal void SetProcessor(CommandProcessor cmdProcessor)
        {
            //Check to make sure 'processor' is not null, if so get the default instance.
            if (processor == null || cmdProcessor == null)
                processor = CommandProcessor.DEFAULT_INSTANCE;
            processor = cmdProcessor;
        }

        public static TerminalMessage EMPTY
            => new TerminalMessage();

        public static TerminalMessage FORMAT(TerminalMessage currentMessage, params Message[] text)
        {
            List<Message> mx = new List<Message>(currentMessage.GetMessage().ToArray().Length + text.Length);
            foreach (Message m in currentMessage.GetMessage().ToArray())
            {
                if (!(mx == null && m == null))
                    mx.Add(m);
            }
            foreach (Message m in text)
            {
                if (!(mx == null && m == null))
                    mx.Add(m);
            }
            if (!(mx == null || mx == new List<Message>(0)))
                return new TerminalMessage(mx.ToArray());
            else
                return EMPTY;
        }

        public sealed class Message
        {
            private ConsoleColor textColor = ConsoleColor.White;
            private string message = "";

            public Message(string message)
            {
                this.message = message;
                textColor = GetProcessor().CurrentFGColor;
            }

            public Message(ConsoleColor color, string message)
            {
                this.message = message;
                textColor = color;
            }

            public ConsoleColor GetColor() => textColor;
            public string GetMessage() => message;

            public static Message EMPTY
                => new Message("");

            public static Message RESET_COLOR
                => new Message(GetProcessor().DefaultFGColor, "");

            public static Message WHITESPACE
                => new Message(" ");

            public static Message NEWLINE
                => new Message("\n");
        }
    }
}
