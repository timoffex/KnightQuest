using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Helper class to replace texture assets so that I can avoid uploading paid-for asset packs to
/// GitHub.
/// </summary>
/// <remarks>
/// To use this:
/// 
/// 1. Create a new scene temporarily to host this script.
/// 2. Put this script on an empty gameobject.
/// 3. Select all texture assets that you want to convert (easiest way is to have them all in the
///    same folder). I recommend adding "t:Texture" to the project hierarchy search bar to filter
///    out non-texture assets.
/// 4. In the Inspector for the textures, make sure "Read/Write Enabled" is checked under
///    Advanced.
/// 5. Select the game object with this component and lock the inspector.
/// 6. Select all of the textures again and drag them into this component.
/// 7. In the context menu for the component (three vertical dots) select "Replace Texture Assets".
/// 8. Uncheck "Read/Write Enabled" on your textures again.
/// 
/// This script could be made nicer / more automatic, but I feel the effort isn't worth it right
/// now.
/// </remarks>
public sealed class ReplaceTextures : MonoBehaviour
{
    public Texture2D[] textureAssets;

    [ContextMenu("Replace Texture Assets")]
    public void ReplaceTextureAssets()
    {
        foreach (var texture in textureAssets)
        {
            var texturePath = AssetDatabase.GetAssetPath(texture);

            var newTexture = new Texture2D(texture.width, texture.height);
            var colors = Enumerable.Range(0, texture.height)
                .SelectMany((y) => Enumerable.Range(0, texture.width)
                    .Select((x) => (x, y)))
                .Zip(texture.GetPixels(), (xy, c) => (xy.x, xy.y, color: c))
                .Select((val) =>
                    val.color.a == 0
                        ? Color.clear
                        : (val.x + val.y) % 2 == 0
                            ? Color.magenta
                            : Color.blue)
                .ToArray();
            newTexture.SetPixels(colors);
            newTexture.Apply(true);

            // https://answers.unity.com/questions/731557/texture2d-setpixelsapply-changes-not-permanent.html
            File.WriteAllBytes(
                Path.Combine(Application.dataPath, texturePath.Substring("Assets/".Length)),
                newTexture.EncodeToPNG());

            AssetDatabase.ImportAsset(texturePath);
        }
    }
}
