public class ArrowRemainsFireAttachment : FireAttachment
{
    protected override void Awake()
    {
        base.Awake();
        GetComponentInParent<ArrowRemains>().FireAttachment = this;
    }
}
