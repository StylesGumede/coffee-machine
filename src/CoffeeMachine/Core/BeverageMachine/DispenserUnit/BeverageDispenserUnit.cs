using System;
using System.Collections.Generic;
using CoffeeMachine.Core.BeverageMachine.Beverage;
using CoffeeMachine.Core.BeverageMachine.DispenserUnit.States;

namespace CoffeeMachine.Core.BeverageMachine.DispenserUnit
{
    public class BeverageDispenserUnit : IDispenserUnit
    {
        private readonly List<BeverageBase> _tray = new List<BeverageBase>();

        public BeverageDispenserUnit(int capacity)
        {
            Capacity = capacity;
            this.State = new Available();
        }

        public DispenserUnitStateBase State { get; private set; }
        public int Capacity { get; }

        public IEnumerable<BeverageBase> Tray => _tray;

        public void Add(BeverageBase beverageBase)
        {
            if (beverageBase == null) throw new ArgumentNullException(nameof(beverageBase));

            if (this.State is TrayFull) return;

            if (this._tray.Count == this.Capacity)
            {
                this.State = new TrayFull();
                return;
            }

            this._tray.Add(beverageBase);
            
            if (this._tray.Count == this.Capacity)
                this.State = new TrayFull();
        }

        public void Remove(BeverageBase beverageBase)
        {
            this._tray.Remove(beverageBase);
            
            if (this._tray.Count < this.Capacity)
                this.State = new Available();
        }
    }
}