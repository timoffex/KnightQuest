using UnityEngine;
using System.IO;

public sealed class GameDataWriter
{
    readonly BinaryWriter m_writer;

    public GameDataWriter(BinaryWriter writer)
    {
        m_writer = writer;
        m_writer.Write((short)GameDataReader.majorVersion);
    }

    public void WriteString(string str)
    {
        m_writer.Write((short)str.Length);
        m_writer.Write(str.ToCharArray());
    }

    public void WriteInt16(short v)
    {
        m_writer.Write(v);
    }

    public void WriteBool(bool b)
    {
        m_writer.Write(b);
    }

    public void WriteFloat(float f)
    {
        m_writer.Write((double)f);
    }

    public void WriteVector3(Vector3 vector3)
    {
        WriteFloat(vector3.x);
        WriteFloat(vector3.y);
        WriteFloat(vector3.z);
    }

    public void WriteQuaternion(Quaternion quaternion)
    {
        WriteFloat(quaternion.x);
        WriteFloat(quaternion.y);
        WriteFloat(quaternion.z);
        WriteFloat(quaternion.w);
    }

    public void WriteByteArray(byte[] bytes)
    {
        WriteInt16((short)bytes.Length);
        m_writer.Write(bytes);
    }
}