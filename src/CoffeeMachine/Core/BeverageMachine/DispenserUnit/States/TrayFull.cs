namespace CoffeeMachine.Core.BeverageMachine.DispenserUnit.States
{
    public class TrayFull : DispenserUnitStateBase
    {
        public TrayFull()
        {
            this.Message = "Dispensing unit is full, please remove one or more of your beverages";
        }
    }
}