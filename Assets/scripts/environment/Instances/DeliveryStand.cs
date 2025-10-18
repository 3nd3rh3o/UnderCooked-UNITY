public class DeliveryStand : StandInstance
{
    public Environment environment;
    public override void Start()
    {
        // Register this delivery stand in the environment
        env.deliveryStands.Add(this);
    }

    public override void OnDestroy()
    {
        env.deliveryStands.Remove(this);
    }
}