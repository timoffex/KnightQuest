using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

public class CharacterAnimationGenerator : MonoBehaviour
{
    public Sprite[] frames;

    public int numColumns = 3;
    public int numRows = 5;

    [ContextMenu("Generate Clips")]
    public void Generate()
    {
        AssetPath = AssetDatabase.GetAssetPath(frames[0]);
        FolderPath = Path.GetDirectoryName(AssetPath);
        NamePrefix = Path.GetFileNameWithoutExtension(AssetPath);

        AssetDatabase.StartAssetEditing();

        CreateClipAsset("down", ClipFromSprites(GetRow(0), flipped: false));

        CreateClipAsset("right", ClipFromSprites(GetRow(1), flipped: false));
        CreateClipAsset("left", ClipFromSprites(GetRow(1), flipped: true));

        CreateClipAsset("left_down", ClipFromSprites(GetRow(2), flipped: false));
        CreateClipAsset("right_down", ClipFromSprites(GetRow(2), flipped: true));

        CreateClipAsset("right_up", ClipFromSprites(GetRow(3), flipped: false));
        CreateClipAsset("left_up", ClipFromSprites(GetRow(3), flipped: true));

        CreateClipAsset("up", ClipFromSprites(GetRow(4), flipped: false));

        AssetDatabase.StopAssetEditing();
        AssetDatabase.SaveAssets();
    }

    void CreateClipAsset(string direction, AnimationClip clip)
    {
        var name = $"{NamePrefix}_{direction}.anim";
        var path = Path.Combine(FolderPath, name);

        var existingAsset = AssetDatabase.LoadMainAssetAtPath(path);
        if (existingAsset != null)
        {
            EditorUtility.CopySerialized(clip, existingAsset);
        }
        else
        {
            AssetDatabase.CreateAsset(clip, path);
        }
    }

    string AssetPath { get; set; }
    string FolderPath { get; set; }
    string NamePrefix { get; set; }

    Sprite[] GetRow(int r) =>
        Enumerable
            .Range(0, numColumns)
            .Select(c => frames[r * numColumns + c])
            .ToArray();

    AnimationClip ClipFromSprites(Sprite[] sprites, bool flipped)
    {
        var clip = new AnimationClip();
        float deltaTime = 0.5f / sprites.Length;
        Sprite[] frames = sprites.Concat(sprites.Reverse().Take(1)).ToArray();

        ObjectReferenceKeyframe newFrame(Sprite sprite, int idx) => new ObjectReferenceKeyframe
        {
            time = deltaTime * idx,
            value = sprite
        };

        AnimationUtility.SetObjectReferenceCurve(
            clip,
            EditorCurveBinding.PPtrCurve("", typeof(SpriteRenderer), "m_Sprite"),
            frames.Select(newFrame).ToArray());

        AnimationUtility.SetEditorCurve(
            clip,
            EditorCurveBinding.DiscreteCurve("", typeof(SpriteRenderer), "m_FlipX"),
            new AnimationCurve(new Keyframe { time = 0, value = flipped ? 1 : 0 }));

        return clip;
    }
}
