using System.Collections.Generic;
using CoffeeMachine.CoffeeMachine.Beverage.Makers;
using CoffeeMachine.CoffeeMachine.Beverage.Specifications;
using CoffeeMachine.CoffeeMachine.Ingredients;
using CoffeeMachine.CoffeeMachine.Processing.Cappuccino;
using CoffeeMachine.CoffeeMachine.Processing.Coffee;
using CoffeeMachine.CoffeeMachine.Processing.Latte;
using CoffeeMachine.Core.BeverageMachine;
using CoffeeMachine.Core.BeverageMachine.DispenserUnit;
using CoffeeMachine.Core.BeverageMachine.Factory;
using CoffeeMachine.Core.BeverageMachine.LifeCycle;
using CoffeeMachine.Core.BeverageMachine.Processing;
using CoffeeMachine.Core.IngredientUnit;
using CoffeeMachine.Core.InputProcessing;
using CoffeeMachine.Core.Notifications;
using CoffeeMachine.Core.Presentation.Console;

namespace CoffeeMachine.CoffeeMachine.Factory
{
    public class BeverageMachineFactory : IBeverageMachineFactory
    {
        public T Create<T>() where T : class, IBeverageMachine
        {
            if (typeof(T) == typeof(HotBeverageMachine))
            {
                IIngredientUnit ingredientUnit = new IngredientUnit();
                ingredientUnit.Add(new BeansIngredient(25));
                ingredientUnit.Add(new MilkIngredient(20));
                ingredientUnit.Add(new SugarIngredient(100));

                IConsoleWrapper consoleWrapper = new ConsoleWrapper();

                INotificationController notificationController =
                    new ConsoleNotificationController(consoleWrapper, new List<string> { "off" });

                IDispenserUnit dispenserUnit = new BeverageDispenserUnit(2);

                CoffeeMaker coffeeMaker = new CoffeeMaker(ingredientUnit);
                LatteMaker latteMaker = new LatteMaker(ingredientUnit);
                CappuccinoMaker cappuccinoMaker = new CappuccinoMaker(ingredientUnit);

                IInputCommandProcessor inputCommandProcessor = new InputCommandProcessor(notificationController);
                inputCommandProcessor.Register(new CheckDispenseUnitProcess(notificationController, dispenserUnit));
                inputCommandProcessor.Register(new MenuOptionsProcess(notificationController));
                inputCommandProcessor.Register(new CoffeeOptionsProcess(notificationController));
                inputCommandProcessor.Register(new LatteOptionsProcess(notificationController));
                inputCommandProcessor.Register(new CappuccinoOptionsProcess(notificationController));
                inputCommandProcessor.Register(new CreateCoffeeProcess(notificationController, coffeeMaker,
                    dispenserUnit));
                inputCommandProcessor.Register(new CreateLatteProcess(notificationController, latteMaker,
                    dispenserUnit));
                inputCommandProcessor.Register(new CreateCappuccinoProcess(notificationController, cappuccinoMaker,
                    dispenserUnit));

                ILifeTime lifeTime = new ConsoleApplicationLifeTime();

                HotBeverageMachine hotBeverageMachine =
                    new HotBeverageMachine(ingredientUnit, inputCommandProcessor, notificationController, lifeTime,
                        dispenserUnit);
                hotBeverageMachine.ConfigureBeverageSpecifications(
                    new CoffeeSpecification("Coffee"),
                    new LatteSpecification("Latte"),
                    new CappuccinoSpecification("Cappuccino")
                );

                return hotBeverageMachine as T;
            }

            return null;
        }
    }
}