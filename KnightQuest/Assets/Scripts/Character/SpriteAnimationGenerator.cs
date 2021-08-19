using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

public sealed class SpriteAnimationGenerator : MonoBehaviour
{
    [Tooltip("A spritesheet for a single motion. This should have a name like Knight_Walk.png or " +
             "Sword_Attack_Front.png / Sword_Attack_Back.png")]
    public Texture2D spritesheet;

    [Tooltip("The directions corresponding to each column in the spritesheet.")]
    public CharacterDirection[] directions = new CharacterDirection[] {
        CharacterDirection.Down,
        CharacterDirection.Up,
        CharacterDirection.Right,
        CharacterDirection.Left,
    };

    [Tooltip("Allow overwriting existing file?")]
    public bool allowOverwrite = false;

    [ContextMenu("Generate SpriteAnimation")]
    public void Generate()
    {
        if (spritesheet == null)
        {
            throw new System.ApplicationException("No spritesheet set");
        }

        if (directions.Length == 0)
        {
            throw new System.ApplicationException("Directions not set");
        }

        ParseAssetPath();
        if (System.IO.File.Exists(OutputPath) && !allowOverwrite)
        {
            Debug.LogWarning("Output file already exists " + OutputPath);
            return;
        }

        if (FrontSprites != null && BackSprites != null)
        {
            if (FrontSprites.Length != BackSprites.Length)
            {
                throw new System.ApplicationException(
                    "Front and back spritesheets should have the same number of sprites but " +
                    $"don't: front has {FrontSprites.Length}, back has {BackSprites.Length}.");
            }
        }

        if (FrontSprites != null && FrontSprites.Length % directions.Length != 0)
        {
            throw new System.ApplicationException(
                $"The number of sprites in the sheet ({FrontSprites.Length}) is not a multiple " +
                $"of the number of specified number of directions ({directions.Length})");
        }

        if (BackSprites != null && BackSprites.Length % directions.Length != 0)
        {
            throw new System.ApplicationException(
                $"The number of sprites in the sheet ({BackSprites.Length}) is not a multiple " +
                $"of the number of specified number of directions ({directions.Length})");
        }

        NumFrames = (FrontSprites?.Length ?? BackSprites.Length) / directions.Length;

        var spriteAnimation = SpriteAnimation.CreateInstance(
                name: AnimationName,
                frontDownFrames: GetDirection(FrontSprites, CharacterDirection.Down),
                frontUpFrames: GetDirection(FrontSprites, CharacterDirection.Up),
                frontRightFrames: GetDirection(FrontSprites, CharacterDirection.RightUp),
                frontLeftFrames: GetDirection(FrontSprites, CharacterDirection.LeftUp),
                backDownFrames: GetDirection(BackSprites, CharacterDirection.Down),
                backUpFrames: GetDirection(BackSprites, CharacterDirection.Up),
                backRightFrames: GetDirection(BackSprites, CharacterDirection.RightUp),
                backLeftFrames: GetDirection(BackSprites, CharacterDirection.LeftUp));

        AssetDatabase.CreateAsset(spriteAnimation, OutputPath);
        AssetDatabase.SaveAssets();
    }

    void ParseAssetPath()
    {
        var assetPath = AssetDatabase.GetAssetPath(spritesheet);
        var name = Path.GetFileNameWithoutExtension(assetPath);
        FolderPath = Path.GetDirectoryName(assetPath);
        var nameParts = name.Split('_');
        if (nameParts.Length == 2)
        {
            SpriteName = nameParts[0];
            AnimationName = nameParts[1];
            Debug.Log($"Sprite {SpriteName}, animation {AnimationName}; front only");
            FrontSprites =
                AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath).Cast<Sprite>().ToArray();
            BackSprites = null;
        }
        else if (nameParts.Length == 3)
        {
            SpriteName = nameParts[0];
            AnimationName = nameParts[1];
            Debug.Log($"Sprite {SpriteName}, animation {AnimationName}");
            var ext = Path.GetExtension(assetPath);

            var frontAssetPath =
                Path.Combine(FolderPath, $"{SpriteName}_{AnimationName}_Front.{ext}");
            var backAssetPath =
                Path.Combine(FolderPath, $"{SpriteName}_{AnimationName}_Back.{ext}");

            if (System.IO.File.Exists(frontAssetPath))
            {
                FrontSprites =
                    AssetDatabase
                        .LoadAllAssetRepresentationsAtPath(frontAssetPath)
                        .Cast<Sprite>()
                        .ToArray();
            }

            if (System.IO.File.Exists(backAssetPath))
            {
                BackSprites =
                    AssetDatabase
                        .LoadAllAssetRepresentationsAtPath(backAssetPath)
                        .Cast<Sprite>()
                        .ToArray();
            }
        }
    }

    string FolderPath { get; set; }
    string SpriteName { get; set; }
    string AnimationName { get; set; }
    Sprite[] FrontSprites { get; set; }
    Sprite[] BackSprites { get; set; }
    int NumFrames { get; set; }
    string OutputPath => Path.Combine(FolderPath, $"{SpriteName}_{AnimationName}.asset");

    Sprite[] GetDirection(Sprite[] sheet, CharacterDirection direction)
    {
        if (sheet == null) return new Sprite[] { };

        var columnIndex = ArrayUtility.IndexOf(directions, direction);
        if (columnIndex == -1)
        {
            return new Sprite[] { };
        }

        return GetColumn(sheet, columnIndex);
    }

    Sprite[] GetColumn(Sprite[] sheet, int c) =>
        Enumerable
            .Range(0, NumFrames)
            .Select(r => sheet[r * directions.Length + c])
            .ToArray();
}
