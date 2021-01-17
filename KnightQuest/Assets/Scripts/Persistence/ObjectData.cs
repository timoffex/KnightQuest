using System.Linq;
using System.IO;

public sealed class ObjectData
{
    public static ObjectData Serialize(PersistablePrefab obj)
    {
        var memoryStream = new MemoryStream();
        obj.Save(new GameDataWriter(new BinaryWriter(memoryStream)));
        return new ObjectData(
            obj.PrefabId,
            memoryStream.GetBuffer(),
            obj.Subobjects.Select(Serialize).ToArray());
    }

    public void Instantiate(PersistablePrefab parent = null)
    {
        var self =
            PersistablePrefab.InstantiatePrefab(
                m_prefabId,
                new GameDataReader(
                    new BinaryReader(new MemoryStream(m_data))),
                parent);

        foreach (var subobject in m_subobjects)
        {
            subobject.Instantiate(parent: self);
        }
    }

    public void SaveTo(GameDataWriter writer)
    {
        writer.WriteString(m_prefabId);
        writer.WriteByteArray(m_data);
        writer.WriteInt16((short)m_subobjects.Length);
        foreach (var subobject in m_subobjects)
            subobject.SaveTo(writer);
    }

    public static ObjectData LoadFrom(GameDataReader reader)
    {
        var prefabId = reader.ReadString();
        var data = reader.ReadByteArray();
        var numSubobjects = reader.ReadInt16();
        var subobjects = Enumerable.Range(0, numSubobjects)
            .Select((_) => ObjectData.LoadFrom(reader))
            .ToArray();

        return new ObjectData(prefabId, data, subobjects);
    }

    ObjectData(string prefabId, byte[] data, ObjectData[] subobjects)
    {
        m_prefabId = prefabId;
        m_data = data;
        m_subobjects = subobjects;
    }

    readonly string m_prefabId;
    readonly byte[] m_data;

    readonly ObjectData[] m_subobjects;
}