using UnityEngine;

/// <summary>
/// A script that identifies the game object that the player is controlling.
/// </summary>
[RequireComponent(typeof(Character))]
public sealed class Player : PersistableComponent
{
    public int GoldAmount { get; private set; }

    public Transform GroundPoint => m_character.GroundPoint;

    public void AddGold(int amount)
    {
        GoldAmount += amount;
        Debug.Log($"Got more gold! I now have {GoldAmount}");
    }

    protected override void Awake()
    {
        base.Awake();
        m_gameSingletons = GameSingletons.Instance;
        m_character = GetComponent<Character>();
        GoldAmount = 0;
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.WriteInt16((short)GoldAmount);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        GoldAmount = reader.ReadInt16();
    }

    void OnEnable()
    {
        m_gameSingletons.Player = this;
    }

    void OnDisable()
    {
        m_gameSingletons.Player = null;
    }

    static Player()
    {
        PersistableComponent.Register<Player>("Player");
    }

    Character m_character;
    GameSingletons m_gameSingletons;
}
