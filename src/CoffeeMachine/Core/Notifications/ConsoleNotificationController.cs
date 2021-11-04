using System;
using System.Collections.Generic;
using System.Linq;
using CoffeeMachine.Core.Presentation.Console;

namespace CoffeeMachine.Core.Notifications
{
    public class ConsoleNotificationController : INotificationController
    {
        private readonly IConsoleWrapper _consoleWrapper;
        private readonly List<string> _specialKeyWords;

        public ConsoleNotificationController(IConsoleWrapper consoleWrapper, List<string> specialKeyWords)
        {
            _consoleWrapper = consoleWrapper;
            _specialKeyWords = specialKeyWords;
        }

        public event Action<string> SpecialKeywordEntered; 

        public void Notify(string message) => this._consoleWrapper.WriteLine(message);

        public void Notify<TNotification>() where TNotification : NotificationBase, new() =>
            this._consoleWrapper.WriteLine(new TNotification().Message);

        public void Prompt<T>(string message, T[] options,  Action<T> onOptionSelected) where T: IIdentifiable
        {
            bool continuePrompting = true;

            do
            {
                this._consoleWrapper.WriteLine(message);

                for (int i = 0; i < options.Count(); i++)
                {
                    this._consoleWrapper.WriteLine($"[{i}] {options[i].Name}");
                }

                string input = this._consoleWrapper.ReadLine();

                if (this._specialKeyWords.Contains(input))
                {
                    this.SpecialKeywordEntered?.Invoke(input);
                    return;
                }

                if (!int.TryParse(input, out var selectedOptionIndex) || selectedOptionIndex >= options.Length)
                {
                    this._consoleWrapper.WriteLine("Invalid choice");
                    continue;
                }

                onOptionSelected(options[selectedOptionIndex]);

                continuePrompting = false;
            } while (continuePrompting);
        }

        public void Prompt(string message, string[] options, Action<string> onOptionSelected)
        {
            bool continuePrompting = true;

            do
            {
                this._consoleWrapper.WriteLine(message);

                for (int i = 0; i < options.Count(); i++)
                {
                    this._consoleWrapper.WriteLine($"[{i}] {options[i]}");
                }

                string input = this._consoleWrapper.ReadLine();

                if (this._specialKeyWords.Contains(input))
                {
                    this.SpecialKeywordEntered?.Invoke(input);
                    return;
                }

                if (!int.TryParse(input, out var selectedOptionIndex) || selectedOptionIndex >= options.Length)
                {
                    this._consoleWrapper.WriteLine("Invalid choice");
                    continue;
                }

                onOptionSelected(options[selectedOptionIndex]);

                continuePrompting = false;
            } while (continuePrompting);
        }

        public void Notify(NotificationBase notification)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));
            
            this._consoleWrapper.WriteLine(notification.Message);
        }
    }
}