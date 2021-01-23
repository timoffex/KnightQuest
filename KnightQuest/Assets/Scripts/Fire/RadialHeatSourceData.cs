using UnityEngine;

sealed class RadialHeatSourceData : PersistableComponent
{
    public float centerHeat;

    [Range(0, 1)]
    public float constantHeatRadiusRatio;

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.WriteFloat(centerHeat);
        writer.WriteFloat(constantHeatRadiusRatio);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        centerHeat = reader.ReadFloat();
        constantHeatRadiusRatio = reader.ReadFloat();
    }

    static RadialHeatSourceData()
    {
        PersistableComponent.Register<RadialHeatSourceData>("RadialHeatSourceData");
    }
}
