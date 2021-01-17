using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class CoinCounter : MonoBehaviour
{
    [SerializeField] Text coinText;

    GameSingletons m_gameSingletons;

    void Start()
    {
        m_gameSingletons = GameSingletons.Instance;
    }

    void Update()
    {
        var playerGold = m_gameSingletons.Player?.GoldAmount;
        coinText.text = playerGold?.ToString() ?? "...";
    }
}
