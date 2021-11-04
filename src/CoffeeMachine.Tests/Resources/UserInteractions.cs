using CoffeeMachine.Core.Notifications;
using CoffeeMachine.Core.Presentation.Console;
using Moq;
using NUnit.Framework;

namespace CoffeeMachine.Tests.Resources
{
    public class UserInteractions
    {
        public static void ConfirmBeverageNotifications(Mock<IConsoleWrapper> consoleWrapperMock,
            MockSequence consoleWrapperMockSequence)
        {
            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual(new CreatingBeverageNotification().Message, x); });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual(new EnjoyYourBeverageNotification().Message, x); });
        }

        public static void PromptForUnitsOfSugar(Mock<IConsoleWrapper> consoleWrapperMock,
            MockSequence consoleWrapperMockSequence, string beverageOption, string beverageName)
        {
            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x =>
                {
                    Assert.AreEqual($"How many units of sugar would you like with your {beverageName}?", x);
                });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual("[0] 1", x); });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual("[1] 2", x); });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual("[2] 3", x); });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.ReadLine())
                .Returns(beverageOption);
        }

        public static void PromptForSugar(Mock<IConsoleWrapper> consoleWrapperMock,
            MockSequence consoleWrapperMockSequence, string selectedOption, string beverageName)
        {
            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual($"Would you like sugar with your {beverageName}?", x); });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual("[0] Yes", x); });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual("[1] No", x); });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.ReadLine())
                .Returns(selectedOption);
        }

        public static void PromptForMilk(Mock<IConsoleWrapper> consoleWrapperMock,
            MockSequence consoleWrapperMockSequence, string beverageOption, string beverageName)
        {
            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual($"Would you like milk with your {beverageName}?", x); });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual("[0] Yes", x); });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual("[1] No", x); });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.ReadLine())
                .Returns(beverageOption);
        }

        public static void ShowBeverageOptions(Mock<IConsoleWrapper> consoleWrapperMock,
            MockSequence consoleWrapperMockSequence, string option, bool ignoreDispenseUnitAssert = false)

        {
            if (!ignoreDispenseUnitAssert)
            {
                consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                    .Setup(x => x.WriteLine(It.IsAny<string>()))
                    .Callback<string>(x => { Assert.AreEqual("Checking dispense unit...", x); }); 
            }
            
            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual("Hello there, let's make you a hot beverage!!", x); });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual("What type of Coffee would you like to have?", x); });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual("[0] Coffee", x); });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual("[1] Latte", x); });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.WriteLine(It.IsAny<string>()))
                .Callback<string>(x => { Assert.AreEqual("[2] Cappuccino", x); });

            consoleWrapperMock.InSequence(consoleWrapperMockSequence)
                .Setup(x => x.ReadLine())
                .Returns(option);
        }
    }
}