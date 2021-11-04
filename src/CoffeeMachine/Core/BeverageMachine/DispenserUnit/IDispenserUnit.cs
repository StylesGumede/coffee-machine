using System.Collections.Generic;
using CoffeeMachine.Core.BeverageMachine.Beverage;
using CoffeeMachine.Core.BeverageMachine.DispenserUnit.States;

namespace CoffeeMachine.Core.BeverageMachine.DispenserUnit
{
    public interface IDispenserUnit
    {
        DispenserUnitStateBase State { get; }
        int Capacity { get; }
        IEnumerable<BeverageBase> Tray { get; }

        void Add(BeverageBase beverageBase);
        void Remove(BeverageBase beverageBase);
    }
}