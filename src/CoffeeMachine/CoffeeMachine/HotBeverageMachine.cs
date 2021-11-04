using System.Collections.Generic;
using System.Linq;
using CoffeeMachine.Core.BeverageMachine;
using CoffeeMachine.Core.BeverageMachine.Beverage.Specification;
using CoffeeMachine.Core.BeverageMachine.DispenserUnit;
using CoffeeMachine.Core.BeverageMachine.Exceptions;
using CoffeeMachine.Core.BeverageMachine.LifeCycle;
using CoffeeMachine.Core.IngredientUnit;
using CoffeeMachine.Core.InputProcessing;
using CoffeeMachine.Core.Notifications;

namespace CoffeeMachine.CoffeeMachine
{
    public class HotBeverageMachine : IBeverageMachine
    {
        private readonly INotificationController _notificationController;

        private readonly List<BeverageSpecification> _supportedBeverageSpecifications =
            new List<BeverageSpecification>();

        private ILifeTime _lifeTime;

        public HotBeverageMachine(
            IIngredientUnit ingredientUnit,
            IInputCommandProcessor inputCommandProcessor,
            INotificationController notificationController,
            ILifeTime lifeTime,
            IDispenserUnit dispenserUnit)
        {
            _notificationController = notificationController;
            _lifeTime = lifeTime;
            this.DispenserUnit = dispenserUnit;
            IngredientUnit = ingredientUnit;
            InputCommandProcessor = inputCommandProcessor;
        }

        public IEnumerable<BeverageSpecification> SupportedBeverageSpecifications =>
            this._supportedBeverageSpecifications;

        public IIngredientUnit IngredientUnit { get; }
        public IInputCommandProcessor InputCommandProcessor { get; }
        public IDispenserUnit DispenserUnit { get; }

        public void Run()
        {
            this._notificationController.SpecialKeywordEntered += keyword =>
            {
                if (keyword == "off")
                    this._lifeTime.ShutDown();
            };

            ProcessContext processContext =
                new ProcessContext(this._supportedBeverageSpecifications, this.IngredientUnit);

            this.InputCommandProcessor.Run(processContext);
        }

        public void ConfigureBeverageSpecifications(params BeverageSpecification[] specifications)
        {
            foreach (var specification in specifications)
            {
                if (this._supportedBeverageSpecifications.Any(x => x.DisplayName == specification.DisplayName))
                    throw new ConfigureBeverageSpecificationException(
                        $"Beverage specification with name '{specification.DisplayName}' already configured");

                this._supportedBeverageSpecifications.Add(specification);
            }
        }

        public void ClearBeverageSpecifications() => this._supportedBeverageSpecifications.Clear();
    }
}