using System;
using System.Collections.Generic;

/// <summary>
/// Base class for (immutable!) plain C# classes that can be persisted.
/// 
/// All concrete subclasses of this (that get instantiated) must be registered in this class.
/// </summary>
/// <remarks>
/// Note that as of now, there is no support for saving "object graphs", so subclasses should be
/// immutable (because shared references to a single object will be replaced by multiple copies
/// of that object, changing semantics if the object was mutable).
/// </remarks>
public abstract class PersistableObject
{
    readonly string m_id;

    public PersistableObject()
    {
        if (!IsTypeKnown(GetType()))
        {
            throw new InvalidOperationException(
                $"Type {GetType()} was not registered in {nameof(PersistableObject)}");
        }

        m_id = m_typeToId[GetType()];
    }

    public virtual void Save(GameDataWriter writer)
    {
        writer.WriteString(m_id);
    }

    public static T Load<T>(GameDataReader reader) where T : PersistableObject
    {
        var id = reader.ReadString();
        return (T)m_idToLoader[id](reader);
    }

    public static void Register<T>(string id, Func<GameDataReader, T> loader)
        where T : PersistableObject
    {
        m_typeToId.Add(typeof(T), id);
        m_idToLoader.Add(id, loader);
    }

    static bool IsTypeKnown(Type type) => m_typeToId.ContainsKey(type);

    readonly static Dictionary<string, Func<GameDataReader, object>> m_idToLoader =
        new Dictionary<string, Func<GameDataReader, object>>();
    readonly static Dictionary<Type, string> m_typeToId = new Dictionary<Type, string>();
}