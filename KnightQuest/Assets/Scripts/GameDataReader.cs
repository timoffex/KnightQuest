using UnityEngine;
using System.IO;

public sealed class GameDataReader
{
    /// <summary>
    /// The current save/load version.
    /// 
    /// Bump this whenever some saving/loading code changed in a non-backwards-compatible way.
    /// </summary>
    public const short majorVersion = 1;

    readonly BinaryReader m_reader;

    public GameDataReader(BinaryReader reader)
    {
        m_reader = reader;

        var version = m_reader.ReadInt16();
        if (version != majorVersion)
        {
            throw new System.ArgumentException(
                $"Cannot read file with major version {version} (current version: {majorVersion})");
        }
    }

    public string ReadString()
    {
        var length = m_reader.ReadInt16();
        var chars = m_reader.ReadChars(length);

        return new string(chars);
    }

    public short ReadInt16()
    {
        return m_reader.ReadInt16();
    }

    public bool ReadBool()
    {
        return m_reader.ReadBoolean();
    }

    public float ReadFloat()
    {
        return (float)m_reader.ReadDouble();
    }

    public Vector3 ReadVector3()
    {
        return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
    }

    public Quaternion ReadQuaternion()
    {
        return new Quaternion(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
    }

    public byte[] ReadByteArray()
    {
        var count = m_reader.ReadInt16();
        return m_reader.ReadBytes(count);
    }
}