public class CharacterFireAttachment : FireAttachment
{
    protected override void Awake()
    {
        base.Awake();
        GetComponentInParent<CharacterAnimationController>().RegisterFireAttachment(this);
    }
}
