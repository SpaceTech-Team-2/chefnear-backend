namespace ChefNear.Domain.Enums;

public enum CancellationReasonType
{
    // Client reasons
    ClientChangedMind = 1,
    ClientOrderDelayed = 2,
    ClientIncorrectDetails = 3,
    ClientOther = 4,

    // Chef reasons
    ChefOutofIngredients = 5,
    ChefKitchenBusy = 6,
    ChefPersonalEmergency = 7,
    ChefOther = 8
}
