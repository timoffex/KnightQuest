using UnityEngine;

public sealed class TestWeaponSwapper : PersistableComponent
{
    [SerializeField] PersistablePrefab[] weaponPrefabs;
    int m_activeIndex = -1;
    GameObject m_activeWeapon;
    GameSingletons m_gameSingletons;

    protected override void Awake()
    {
        base.Awake();
        m_gameSingletons = GameSingletons.Instance;
    }

    void Start()
    {
        m_activeIndex = 0;
        InstantiateOrFindActiveWeapon();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (m_activeWeapon != null)
                Destroy(m_activeWeapon);

            m_activeIndex = (m_activeIndex + 1) % weaponPrefabs.Length;
            InstantiateActiveWeapon();
        }
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.WriteInt16((short)weaponPrefabs.Length);
        foreach (var prefab in weaponPrefabs)
            writer.WriteString(prefab.PrefabId);
        writer.WriteInt16((short)m_activeIndex);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        weaponPrefabs = new PersistablePrefab[reader.ReadInt16()];
        for (int i = 0; i < weaponPrefabs.Length; ++i)
            weaponPrefabs[i] = m_gameSingletons.PrefabCollection.GetPrefabById(reader.ReadString());
        m_activeIndex = reader.ReadInt16();
    }

    void InstantiateActiveWeapon()
    {
        if (m_activeWeapon != null)
            return;

        var prefab = weaponPrefabs[m_activeIndex];
        if (prefab != null)
            m_activeWeapon = Instantiate(prefab, transform).gameObject;
    }

    void InstantiateOrFindActiveWeapon()
    {
        m_activeWeapon = GetComponentInChildren<Weapon>()?.gameObject;
        InstantiateActiveWeapon();
    }
}
