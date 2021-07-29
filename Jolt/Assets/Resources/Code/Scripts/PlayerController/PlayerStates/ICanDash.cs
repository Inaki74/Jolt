namespace Jolt.PlayerController.PlayerStates
{
    public interface ICanDash
    {
        bool CanDash();
        void DecreaseAmountOfDashes();
        void ResetAmountOfDashes();
    }
}