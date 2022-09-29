using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class RefreshStaticShadows : MonoBehaviour
{
    //Refresh all static shadows
    //More paramaters can be added, need to pass an object of a class with all the parameters
    [Header("Transform Controls")]
    [SerializeField] Transform initialLightPosition;
    [SerializeField] Transform initialWallPosition;
    Transform[] trList;

    [Header("Snap And Convert")]
    [SerializeField] Camera cam;
    [SerializeField] SpriteRenderer ssSR;
    [SerializeField] string outputfilename;
    [SerializeField] int height = 2048;
    [SerializeField] int width = 2048;
    [SerializeField] int depth = 24;
    public void RefreshAllStaticShadows()
    {
        trList = new Transform[] { initialWallPosition, initialLightPosition };

        BroadcastMessage("CastFakeShadow", trList);
    }


    //Capture screen
    public void CaptureScreen()
    {
        RenderTexture renderTexture = new RenderTexture(width, height, depth);
        Rect rect = new Rect(0, 0, width, height);
        Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        cam.targetTexture = renderTexture;
        cam.Render();

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();
        
        cam.targetTexture = null;
        RenderTexture.active = currentRenderTexture;
        DestroyImmediate(renderTexture);

        Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

        byte[] itemBGBytes = sprite.texture.EncodeToPNG();
        File.WriteAllBytes($"Assets/Resources/GeneratedShadowTextures/{outputfilename}.png", itemBGBytes);       

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif

        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath($"Assets/Resources/GeneratedShadowTextures/{outputfilename}.png");
        importer.textureType = TextureImporterType.Sprite;
        importer.alphaIsTransparency = true;
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();

        ssSR.sprite = Resources.Load<Sprite>($"GeneratedShadowTextures/{outputfilename}");
        ssSR.color = new Color32(0,0,0, 255);
        Debug.Log("Reached end and loaded");
    }

}
