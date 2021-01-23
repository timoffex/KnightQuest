/// <summary>
/// An attachment to a <see cref="Pickup"/> that activates it when a player walks into a trigger
/// collider.
/// </summary>
public sealed class PickupTrigger : PlayerTriggerZone
{
    Pickup m_pickup;

    protected override void Start()
    {
        base.Start();
        m_pickup = GetComponentInParent<Pickup>();
    }

    protected override void OnPlayerEntered(Player player)
    {
        m_pickup.GetPickedUpBy(player);
    }
}
