public sealed class ArrowFireAttachment : FireAttachment
{
    protected override void Awake()
    {
        base.Awake();
        GetComponentInParent<Arrow>().FireAttachment = this;
    }
}
