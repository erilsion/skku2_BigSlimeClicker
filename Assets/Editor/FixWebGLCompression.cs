using UnityEditor;
using UnityEngine;

public class FixWebGLCompression
{
    [MenuItem("Tools/Fix WebGL Compression Settings")]
    public static void FixCompression()
    {
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        PlayerSettings.WebGL.decompressionFallback = false;
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
