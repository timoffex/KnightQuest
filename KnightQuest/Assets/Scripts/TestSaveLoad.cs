using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TestSaveLoad : MonoBehaviour
{
    GameSingletons m_gameSingletons;

    // Start is called before the first frame update
    void Start()
    {
        m_gameSingletons = GameSingletons.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Saving");
            var memoryStream = new MemoryStream();
            GameDataWriter writer = new GameDataWriter(new BinaryWriter(memoryStream));
            m_gameSingletons.SaveTo(writer);
            m_saveData = memoryStream.GetBuffer();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Loading");
            var memoryStream = new MemoryStream(m_saveData);
            GameDataReader reader = new GameDataReader(new BinaryReader(memoryStream));
            m_gameSingletons.LoadFrom(reader);
        }
    }

    byte[] m_saveData;
}
