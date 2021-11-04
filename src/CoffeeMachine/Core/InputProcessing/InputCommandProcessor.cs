using System;
using System.Collections.Generic;
using System.Linq;
using CoffeeMachine.Core.BeverageMachine.Beverage.Specification;
using CoffeeMachine.Core.InputProcessing.Exceptions;
using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.Core.InputProcessing
{
    public class InputCommandProcessor : IInputCommandProcessor
    {
        private readonly List<Process> _processes = new List<Process>();
        private readonly INotificationController _notificationController;
        private bool _stopProcessing;

        public InputCommandProcessor(INotificationController notificationController)
        {
            _notificationController = notificationController;
        }

        public IEnumerable<Process> Processes => this._processes;

        public void Run(ProcessContext processContext)
        {
            if (processContext == null)
                throw new ArgumentNullException(nameof(processContext));

            
            this._notificationController.SpecialKeywordEntered += keyword =>
            {
                if (keyword == "off")
                    this._stopProcessing = true;
            };

            while (true)
                if (!ContinueRunning(processContext))
                    break;
        }

        public void Register(Process process)
        {
            if (process == null)
                throw new ArgumentNullException(nameof(process));

            if (this._processes.Any(x => x.Name == process.Name))
                throw new Exception($"Process with name '{process.Name}' already configured.");

            this._processes.Add(process);
        }

        private bool ContinueRunning(ProcessContext processContext)
        {
            bool result = true;

            foreach (var command in this._processes)
            {
                try
                {
                    command.Execute(processContext);

                    if (this._stopProcessing)
                    {
                        result = false;
                        break;
                    }
                }
                catch (TerminateProcessException)
                {
                    processContext.SelectedOption = new NoSpecification();
                    break;
                }
            }

            return result;
        }
    }
}