using UnityEngine;

public sealed class TestWeaponSwapper : PersistableComponent
{
    [SerializeField] Weapon[] weaponPrefabs;
    int m_activeIndex = -1;
    Weapon m_activeWeapon;

    void Start()
    {
        m_activeIndex = 0;
        m_activeWeapon = Instantiate(weaponPrefabs[m_activeIndex], transform);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (m_activeWeapon != null)
                Destroy(m_activeWeapon.gameObject);

            m_activeIndex = (m_activeIndex + 1) % weaponPrefabs.Length;
            m_activeWeapon = Instantiate(weaponPrefabs[m_activeIndex], transform);
        }
    }
}
