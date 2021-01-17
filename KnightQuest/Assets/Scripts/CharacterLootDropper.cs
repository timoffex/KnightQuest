using UnityEngine;

[RequireComponent(typeof(Character))]
public sealed class CharacterLootDropper : PersistableComponent
{
    [SerializeField] Rigidbody2D lootPrefab;

    Character m_character;

    public void Initialize(Rigidbody2D lootPrefab)
    {
        this.lootPrefab = lootPrefab;
    }

    void Start()
    {
        m_character = GetComponent<Character>();
        m_character.OnDied += DropLoot;
    }

    void DropLoot()
    {
        var loot = Instantiate(lootPrefab, m_character.GroundPoint.position, Quaternion.identity);
        loot.velocity = Random.insideUnitCircle;
    }
}
