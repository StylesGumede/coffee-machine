using System.Linq;
using CoffeeMachine.CoffeeMachine.Beverage;
using CoffeeMachine.CoffeeMachine.Beverage.Specifications;
using CoffeeMachine.CoffeeMachine.Ingredients;
using CoffeeMachine.CoffeeMachine.Notifications;
using CoffeeMachine.Core.BeverageMachine.Beverage.Specification;
using CoffeeMachine.Core.BeverageMachine.Exceptions;
using CoffeeMachine.Core.Presentation.Console;
using CoffeeMachine.Tests.Resources;
using Moq;
using NUnit.Framework;

namespace CoffeeMachine.Tests
{
    [TestFixture]
    public class HotBeverageMachineTests
    {
        public class ConfigureSupportedBeverageSpecifications : HotBeverageMachineTests
        {
            private Mock<IConsoleWrapper> _consoleWrapperMock;

            [SetUp]
            public void SetupTest()
            {
                this._consoleWrapperMock = new Mock<IConsoleWrapper>();
            }

            [Test]
            public void MustFailAddingDuplicateSpecification()
            {
                var beverageMachine = HotBeverageMachineBuilder.CreateMachine(_consoleWrapperMock);
                beverageMachine.ClearBeverageSpecifications();

                Assert.Throws<ConfigureBeverageSpecificationException>(() =>
                    beverageMachine.ConfigureBeverageSpecifications(
                        new CoffeeSpecification("Coffee"),
                        new CoffeeSpecification("Coffee")
                    ));
            }

            [Test]
            public void MustSuccessfullyAddSpecifications()
            {
                var beverageMachine = HotBeverageMachineBuilder.CreateMachine(_consoleWrapperMock);
                beverageMachine.ClearBeverageSpecifications();

                var expectedSpecifications = new BeverageSpecification[]
                {
                    new CoffeeSpecification("Coffee"),
                    new LatteSpecification("Latte"),
                    new CappuccinoSpecification("Cappuccino")
                };

                beverageMachine.ConfigureBeverageSpecifications(expectedSpecifications);

                var specifications = beverageMachine.SupportedBeverageSpecifications.ToArray();

                Assert.AreEqual(3, beverageMachine.SupportedBeverageSpecifications.Count());
                Assert.AreEqual(expectedSpecifications[0], specifications[0]);
                Assert.AreEqual(expectedSpecifications[1], specifications[1]);
                Assert.AreEqual(expectedSpecifications[2], specifications[2]);
            }
        }

        public class Run : HotBeverageMachineTests
        {
            public class CreateCoffeeBeverage : Run
            {
                private string beverageName = "Coffee";

                /// <summary>
                /// User selects Coffee option -> Confirms they would like some milk ->
                /// Confirms they would like some sugar -> Specifies 3 units of sugar
                /// </summary>
                [Test]
                [TestCase("0", "0", "0", "2")]
                public void MustCreateCoffeeWithMilkAndSpecifiedSugarUnits(
                    string beverageOption,
                    string milkConfirmation,
                    string sugarConfirmation,
                    string sugarUnitsRequired)
                {
                    var consoleWrapperMock = new Mock<IConsoleWrapper>(MockBehavior.Strict);
                    var consoleWrapperMockSequence = new MockSequence();

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence,
                        beverageOption);

                    UserInteractions.PromptForMilk(consoleWrapperMock, consoleWrapperMockSequence, milkConfirmation,
                        beverageName);

                    UserInteractions.PromptForSugar(consoleWrapperMock, consoleWrapperMockSequence, sugarConfirmation,
                        beverageName);

                    UserInteractions.PromptForUnitsOfSugar(consoleWrapperMock, consoleWrapperMockSequence,
                        sugarUnitsRequired, beverageName);

                    UserInteractions.ConfirmBeverageNotifications(consoleWrapperMock, consoleWrapperMockSequence);

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence, "off");

                    var beverageMachine = HotBeverageMachineBuilder.CreateMachine(consoleWrapperMock);
                    beverageMachine.Run();

                    Assert.AreEqual(1, beverageMachine.DispenserUnit.Tray.Count());

                    var beverage = beverageMachine.DispenserUnit.Tray.First();

                    Assert.AreEqual(typeof(Coffee), beverage.GetType());
                    Assert.AreEqual(2, beverage.BeverageSpecification.BeanUnitsRequired);
                    Assert.AreEqual(1, beverage.BeverageSpecification.MilkUnitsRequired);
                    Assert.AreEqual(3, beverage.BeverageSpecification.SugarUnitsRequired);
                }

                [Test]
                [TestCase("0", "1", "0", "2")]
                public void MustCreateCoffeeWithSpecifiedSugarUnitsWithNoMilk(
                    string beverageOption,
                    string milkConfirmation,
                    string sugarConfirmation,
                    string sugarUnitsRequired)
                {
                    var consoleWrapperMock = new Mock<IConsoleWrapper>(MockBehavior.Strict);
                    var consoleWrapperMockSequence = new MockSequence();

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence,
                        beverageOption);

                    UserInteractions.PromptForMilk(consoleWrapperMock, consoleWrapperMockSequence, milkConfirmation,
                        beverageName);

                    UserInteractions.PromptForSugar(consoleWrapperMock, consoleWrapperMockSequence, sugarConfirmation,
                        beverageName);

                    UserInteractions.PromptForUnitsOfSugar(consoleWrapperMock, consoleWrapperMockSequence,
                        sugarUnitsRequired, beverageName);

                    UserInteractions.ConfirmBeverageNotifications(consoleWrapperMock, consoleWrapperMockSequence);

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence, "off");

                    var beverageMachine = HotBeverageMachineBuilder.CreateMachine(consoleWrapperMock);
                    beverageMachine.Run();

                    Assert.AreEqual(1, beverageMachine.DispenserUnit.Tray.Count());

                    var beverage = beverageMachine.DispenserUnit.Tray.First();

                    Assert.AreEqual(typeof(Coffee), beverage.GetType());
                    Assert.AreEqual(2, beverage.BeverageSpecification.BeanUnitsRequired);
                    Assert.AreEqual(0, beverage.BeverageSpecification.MilkUnitsRequired);
                    Assert.AreEqual(3, beverage.BeverageSpecification.SugarUnitsRequired);
                }

                [Test]
                [TestCase("0", "0", "0", "2")]
                public void MustNotifyUserThatTheresNotEnoughSugarToCreateCoffeeWithMilkAndSpecifiedSugarUnits(
                    string beverageOption,
                    string milkConfirmation,
                    string sugarConfirmation,
                    string sugarUnitsRequired)
                {
                    var consoleWrapperMock = new Mock<IConsoleWrapper>(MockBehavior.Strict);
                    var consoleWrapperMockSequence = new MockSequence();


                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence,
                        beverageOption);

                    UserInteractions.PromptForMilk(consoleWrapperMock, consoleWrapperMockSequence, milkConfirmation,
                        beverageName);

                    UserInteractions.PromptForSugar(consoleWrapperMock, consoleWrapperMockSequence, sugarConfirmation,
                        beverageName);

                    UserInteractions.PromptForUnitsOfSugar(consoleWrapperMock, consoleWrapperMockSequence,
                        sugarUnitsRequired, beverageName);

                    consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                        .Setup(x => x.WriteLine(It.IsAny<string>()))
                        .Callback<string>(x => { Assert.AreEqual(new NotEnoughSugarNotification().Message, x); });

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence, "off");

                    var beverageMachine = HotBeverageMachineBuilder.CreateMachine(
                        consoleWrapperMock,
                        onIngredientUnitInitialized: unit =>
                        {
                            unit.Use<SugarIngredient>(100); //Use all of it
                        });

                    beverageMachine.Run();

                    Assert.AreEqual(0, beverageMachine.DispenserUnit.Tray.Count());
                }

                [Test]
                [TestCase("0", "0", "0", "2")]
                public void MustNotifyUserThatTheresNotEnoughMilkToCreateCoffeeWithMilkAndSpecifiedSugarUnits(
                    string beverageOption,
                    string milkConfirmation,
                    string sugarConfirmation,
                    string sugarUnitsRequired)
                {
                    var consoleWrapperMock = new Mock<IConsoleWrapper>(MockBehavior.Strict);
                    var consoleWrapperMockSequence = new MockSequence();

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence,
                        beverageOption);

                    UserInteractions.PromptForMilk(consoleWrapperMock, consoleWrapperMockSequence, milkConfirmation,
                        beverageName);

                    consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                        .Setup(x => x.WriteLine(It.IsAny<string>()))
                        .Callback<string>(x => { Assert.AreEqual(new NotEnoughMilkNotification().Message, x); });

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence, "off");

                    var beverageMachine = HotBeverageMachineBuilder.CreateMachine(
                        consoleWrapperMock,
                        onIngredientUnitInitialized: unit =>
                        {
                            unit.Use<MilkIngredient>(20); //Use all of it
                        });

                    beverageMachine.Run();

                    Assert.AreEqual(0, beverageMachine.DispenserUnit.Tray.Count());
                }

                [Test]
                [TestCase("0", "0", "0", "2")]
                public void MustNotifyUserThatTheresNotEnoughBeansToCreateCoffeeWithMilkAndSpecifiedSugarUnits(
                    string beverageOption,
                    string milkConfirmation,
                    string sugarConfirmation,
                    string sugarUnitsRequired)
                {
                    var consoleWrapperMock = new Mock<IConsoleWrapper>(MockBehavior.Strict);
                    var consoleWrapperMockSequence = new MockSequence();

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence,
                        beverageOption);

                    consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                        .Setup(x => x.WriteLine(It.IsAny<string>()))
                        .Callback<string>(x => { Assert.AreEqual(new NotEnoughBeansNotification().Message, x); });

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence, "off");

                    var beverageMachine = HotBeverageMachineBuilder.CreateMachine(
                        consoleWrapperMock,
                        onIngredientUnitInitialized: unit =>
                        {
                            unit.Use<BeansIngredient>(25); //Use all of it
                        });

                    beverageMachine.Run();

                    Assert.AreEqual(0, beverageMachine.DispenserUnit.Tray.Count());
                }
            }

            public class CreateLatteBeverage : Run
            {
                private string beverageName = "Latte";

                [Test]
                [TestCase("1", "0", "2")]
                public void MustCreateLatteWithMilkAndSpecifiedSugarUnits(
                    string beverageOption,
                    string sugarConfirmation,
                    string sugarUnitsRequired)
                {
                    var consoleWrapperMock = new Mock<IConsoleWrapper>(MockBehavior.Strict);
                    var consoleWrapperMockSequence = new MockSequence();

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence,
                        beverageOption);

                    UserInteractions.PromptForSugar(consoleWrapperMock, consoleWrapperMockSequence, sugarConfirmation,
                        beverageName);

                    UserInteractions.PromptForUnitsOfSugar(consoleWrapperMock, consoleWrapperMockSequence,
                        sugarUnitsRequired, beverageName);

                    UserInteractions.ConfirmBeverageNotifications(consoleWrapperMock, consoleWrapperMockSequence);

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence, "off");

                    var beverageMachine = HotBeverageMachineBuilder.CreateMachine(consoleWrapperMock);
                    beverageMachine.Run();

                    Assert.AreEqual(1, beverageMachine.DispenserUnit.Tray.Count());

                    var beverage = beverageMachine.DispenserUnit.Tray.First();

                    Assert.AreEqual(typeof(Latte), beverage.GetType());
                    Assert.AreEqual(3, beverage.BeverageSpecification.BeanUnitsRequired);
                    Assert.AreEqual(2, beverage.BeverageSpecification.MilkUnitsRequired);
                    Assert.AreEqual(3, beverage.BeverageSpecification.SugarUnitsRequired);
                }

                [Test]
                [TestCase("1", "0", "2")]
                public void MustNotifyUserThatTheresNotEnoughSugarToCreateLatteWithMilkAndSpecifiedSugarUnits(
                    string beverageOption,
                    string sugarConfirmation,
                    string sugarUnitsRequired)
                {
                    var consoleWrapperMock = new Mock<IConsoleWrapper>(MockBehavior.Strict);
                    var consoleWrapperMockSequence = new MockSequence();

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence,
                        beverageOption);

                    UserInteractions.PromptForSugar(consoleWrapperMock, consoleWrapperMockSequence, sugarConfirmation,
                        beverageName);

                    UserInteractions.PromptForUnitsOfSugar(consoleWrapperMock, consoleWrapperMockSequence,
                        sugarUnitsRequired, beverageName);

                    consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                        .Setup(x => x.WriteLine(It.IsAny<string>()))
                        .Callback<string>(x => { Assert.AreEqual(new NotEnoughSugarNotification().Message, x); });

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence, "off");

                    var beverageMachine = HotBeverageMachineBuilder.CreateMachine(
                        consoleWrapperMock,
                        onIngredientUnitInitialized: unit =>
                        {
                            unit.Use<SugarIngredient>(100); //Use all of it
                        });

                    beverageMachine.Run();

                    Assert.AreEqual(0, beverageMachine.DispenserUnit.Tray.Count());
                }

                [Test]
                [TestCase("1", "0", "2")]
                public void MustNotifyUserThatTheresNotEnoughMilkToCreateLatteWithMilkAndSpecifiedSugarUnits(
                    string beverageOption,
                    string sugarConfirmation,
                    string sugarUnitsRequired)
                {
                    var consoleWrapperMock = new Mock<IConsoleWrapper>(MockBehavior.Strict);
                    var consoleWrapperMockSequence = new MockSequence();

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence,
                        beverageOption);

                    consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                        .Setup(x => x.WriteLine(It.IsAny<string>()))
                        .Callback<string>(x => { Assert.AreEqual(new NotEnoughMilkNotification().Message, x); });

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence, "off");

                    var beverageMachine = HotBeverageMachineBuilder.CreateMachine(
                        consoleWrapperMock,
                        onIngredientUnitInitialized: unit =>
                        {
                            unit.Use<MilkIngredient>(20); //Use all of it
                        });

                    beverageMachine.Run();

                    Assert.AreEqual(0, beverageMachine.DispenserUnit.Tray.Count());
                }

                [Test]
                [TestCase("1", "0", "2")]
                public void MustNotifyUserThatTheresNotEnoughBeansToCreateLatteWithMilkAndSpecifiedSugarUnits(
                    string beverageOption,
                    string sugarConfirmation,
                    string sugarUnitsRequired)
                {
                    var consoleWrapperMock = new Mock<IConsoleWrapper>(MockBehavior.Strict);
                    var consoleWrapperMockSequence = new MockSequence();

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence,
                        beverageOption);

                    consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                        .Setup(x => x.WriteLine(It.IsAny<string>()))
                        .Callback<string>(x => { Assert.AreEqual(new NotEnoughBeansNotification().Message, x); });

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence, "off");

                    var beverageMachine = HotBeverageMachineBuilder.CreateMachine(
                        consoleWrapperMock,
                        onIngredientUnitInitialized: unit =>
                        {
                            unit.Use<BeansIngredient>(25); //Use all of it
                        });

                    beverageMachine.Run();

                    Assert.AreEqual(0, beverageMachine.DispenserUnit.Tray.Count());
                }
            }

            public class CreateCappuccinoBeverage : Run
            {
                private string beverageName = "Cappuccino";

                [Test]
                [TestCase("2", "2")]
                public void MustCreateCappuccinoWithMilkAndSpecifiedSugarUnits(
                    string beverageOption,
                    string sugarUnitsRequired)
                {
                    var consoleWrapperMock = new Mock<IConsoleWrapper>(MockBehavior.Strict);
                    var consoleWrapperMockSequence = new MockSequence();

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence,
                        beverageOption);

                    UserInteractions.PromptForUnitsOfSugar(consoleWrapperMock, consoleWrapperMockSequence,
                        sugarUnitsRequired, beverageName);

                    UserInteractions.ConfirmBeverageNotifications(consoleWrapperMock, consoleWrapperMockSequence);

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence, "off");

                    var beverageMachine = HotBeverageMachineBuilder.CreateMachine(consoleWrapperMock);
                    beverageMachine.Run();

                    Assert.AreEqual(1, beverageMachine.DispenserUnit.Tray.Count());

                    var beverage = beverageMachine.DispenserUnit.Tray.First();

                    Assert.AreEqual(typeof(Cappuccino), beverage.GetType());
                    Assert.AreEqual(5, beverage.BeverageSpecification.BeanUnitsRequired);
                    Assert.AreEqual(3, beverage.BeverageSpecification.MilkUnitsRequired);
                    Assert.AreEqual(3, beverage.BeverageSpecification.SugarUnitsRequired);
                }

                [Test]
                [TestCase("2", "2")]
                public void MustNotifyUserThatTheresNotEnoughSugarToCreateCappuccinoWithMilkAndSpecifiedSugarUnits(
                    string beverageOption,
                    string sugarUnitsRequired)
                {
                    var consoleWrapperMock = new Mock<IConsoleWrapper>(MockBehavior.Strict);
                    var consoleWrapperMockSequence = new MockSequence();

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence,
                        beverageOption);

                    UserInteractions.PromptForUnitsOfSugar(consoleWrapperMock, consoleWrapperMockSequence,
                        sugarUnitsRequired, beverageName);

                    consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                        .Setup(x => x.WriteLine(It.IsAny<string>()))
                        .Callback<string>(x => { Assert.AreEqual(new NotEnoughSugarNotification().Message, x); });

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence, "off");

                    var beverageMachine = HotBeverageMachineBuilder.CreateMachine(
                        consoleWrapperMock,
                        onIngredientUnitInitialized: unit =>
                        {
                            unit.Use<SugarIngredient>(100); //Use all of it
                        });

                    beverageMachine.Run();

                    Assert.AreEqual(0, beverageMachine.DispenserUnit.Tray.Count());
                }

                [Test]
                [TestCase("1", "2")]
                public void MustNotifyUserThatTheresNotEnoughMilkToCreateCappuccinoWithMilkAndSpecifiedSugarUnits(
                    string beverageOption,
                    string sugarUnitsRequired)
                {
                    var consoleWrapperMock = new Mock<IConsoleWrapper>(MockBehavior.Strict);
                    var consoleWrapperMockSequence = new MockSequence();

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence,
                        beverageOption);

                    consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                        .Setup(x => x.WriteLine(It.IsAny<string>()))
                        .Callback<string>(x => { Assert.AreEqual(new NotEnoughMilkNotification().Message, x); });

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence, "off");

                    var beverageMachine = HotBeverageMachineBuilder.CreateMachine(
                        consoleWrapperMock,
                        onIngredientUnitInitialized: unit =>
                        {
                            unit.Use<MilkIngredient>(20); //Use all of it
                        });

                    beverageMachine.Run();

                    Assert.AreEqual(0, beverageMachine.DispenserUnit.Tray.Count());
                }

                [Test]
                [TestCase("1", "2")]
                public void MustNotifyUserThatTheresNotEnoughBeansToCreateCappuccinoWithMilkAndSpecifiedSugarUnits(
                    string beverageOption,
                    string sugarUnitsRequired)
                {
                    var consoleWrapperMock = new Mock<IConsoleWrapper>(MockBehavior.Strict);
                    var consoleWrapperMockSequence = new MockSequence();

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence,
                        beverageOption);

                    consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                        .Setup(x => x.WriteLine(It.IsAny<string>()))
                        .Callback<string>(x => { Assert.AreEqual(new NotEnoughBeansNotification().Message, x); });

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence, "off");

                    var beverageMachine = HotBeverageMachineBuilder.CreateMachine(
                        consoleWrapperMock,
                        onIngredientUnitInitialized: unit =>
                        {
                            unit.Use<BeansIngredient>(25); //Use all of it
                        });

                    beverageMachine.Run();

                    Assert.AreEqual(0, beverageMachine.DispenserUnit.Tray.Count());
                }
            }

            public class CreateTwoBeverages : Run
            {
                [Test]
                public void MustNotifyTrayIsFullWhenAttemptingToCreateMoreBeveragesThenDispenserCapacity()
                {
                    var consoleWrapperMock = new Mock<IConsoleWrapper>(MockBehavior.Strict);
                    var consoleWrapperMockSequence = new MockSequence();

                    var beverageName = "Coffee";

                    SetupBeverage(
                        consoleWrapperMock: consoleWrapperMock,
                        consoleWrapperMockSequence: consoleWrapperMockSequence,
                        beverageName: beverageName,
                        milkConfirmation: "0",
                        sugarConfirmation: "0",
                        sugarUnitsRequired: "2");

                    SetupBeverage(
                        consoleWrapperMock: consoleWrapperMock,
                        consoleWrapperMockSequence: consoleWrapperMockSequence,
                        beverageName: beverageName,
                        milkConfirmation: "0",
                        sugarConfirmation: "0",
                        sugarUnitsRequired: "1"); //Different sugar

                    //Extra one
                    consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                        .Setup(x => x.WriteLine(It.IsAny<string>()))
                        .Callback<string>(x => { Assert.AreEqual("Checking dispense unit...", x); });

                    consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                        .Setup(x => x.WriteLine(It.IsAny<string>()))
                        .Callback<string>(x =>
                        {
                            Assert.AreEqual(
                                "Dispensing unit is full, please remove one or more of your beverages", x);
                        });

                    consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                        .Setup(x => x.WriteLine(It.IsAny<string>()))
                        .Callback<string>(x =>
                        {
                            Assert.AreEqual(
                                "[0] Coffee", x);
                        });

                    consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                        .Setup(x => x.WriteLine(It.IsAny<string>()))
                        .Callback<string>(x =>
                        {
                            Assert.AreEqual(
                                "[1] Coffee", x);
                        });

                    consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                        .Setup(x => x.ReadLine())
                        .Returns("0");

                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence, "off",
                        ignoreDispenseUnitAssert: true);

                    var beverageMachine = HotBeverageMachineBuilder.CreateMachine(consoleWrapperMock);
                    beverageMachine.Run();

                    Assert.AreEqual(1, beverageMachine.DispenserUnit.Tray.Count());

                    var expectedBeverageOne = beverageMachine.DispenserUnit.Tray.First();

                    Assert.AreEqual(typeof(Coffee), expectedBeverageOne.GetType());
                    Assert.AreEqual(2, expectedBeverageOne.BeverageSpecification.BeanUnitsRequired);
                    Assert.AreEqual(1, expectedBeverageOne.BeverageSpecification.MilkUnitsRequired);
                    Assert.AreEqual(2, expectedBeverageOne.BeverageSpecification.SugarUnitsRequired);
                }

                private static void SetupBeverage(
                    Mock<IConsoleWrapper> consoleWrapperMock,
                    MockSequence consoleWrapperMockSequence,
                    string beverageName,
                    string milkConfirmation,
                    string sugarConfirmation,
                    string sugarUnitsRequired)
                {
                    UserInteractions.ShowBeverageOptions(consoleWrapperMock, consoleWrapperMockSequence,
                        "0");

                    UserInteractions.PromptForMilk(consoleWrapperMock, consoleWrapperMockSequence, milkConfirmation,
                        beverageName);

                    UserInteractions.PromptForSugar(consoleWrapperMock, consoleWrapperMockSequence, sugarConfirmation,
                        beverageName);

                    UserInteractions.PromptForUnitsOfSugar(consoleWrapperMock, consoleWrapperMockSequence,
                        sugarUnitsRequired, beverageName);

                    UserInteractions.ConfirmBeverageNotifications(consoleWrapperMock, consoleWrapperMockSequence);
                }
            }
        }
    }
}