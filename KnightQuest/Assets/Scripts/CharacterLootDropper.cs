using UnityEngine;

[RequireComponent(typeof(Character))]
public sealed class CharacterLootDropper : PersistableComponent
{
    [SerializeField] PersistablePrefab lootPrefab;

    GameSingletons m_gameSingletons;
    Character m_character;

    public void Initialize(PersistablePrefab lootPrefab)
    {
        this.lootPrefab = lootPrefab;
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.WriteString(lootPrefab.PrefabId);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        lootPrefab = m_gameSingletons.PrefabCollection.GetPrefabById(reader.ReadString());
    }

    protected override void Awake()
    {
        base.Awake();
        m_gameSingletons = GameSingletons.Instance;
    }

    void Start()
    {
        m_character = GetComponent<Character>();
        m_character.OnDied += DropLoot;
    }

    void DropLoot()
    {
        var loot = Instantiate(lootPrefab, m_character.GroundPoint.position, Quaternion.identity);
        loot.GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle;
    }

    static CharacterLootDropper()
    {
        PersistableComponent.Register<CharacterLootDropper>("CharacterLootDropper");
    }
}
