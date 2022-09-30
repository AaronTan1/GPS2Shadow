using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;// For Cast function
using Unity.Collections;

[ExecuteInEditMode]
public class RefreshStaticShadows : MonoBehaviour
{
    //Refresh all static shadows
    //More paramaters can be added, need to pass an object of a class with all the parameters
    [Header("Transform Controls")]
    [SerializeField] Transform initialLightPosition;
    [SerializeField] Transform initialWallPosition;
    Transform[] trList;

    [Header("Shadow Generation")]
    [SerializeField] Camera cam;//Shadow generator camera
    [SerializeField] SpriteRenderer testSR;//Sprite renderer for testing
    [SerializeField] string outputfilename; //Placeholder for texture file name
    [Range(9.0f, 15.0f)]
    [SerializeField] int shadowQuality;
    public int verticalResolution;
    public int horizontalResolution;
    [SerializeField] int captureDepth = 24;
    [SerializeField] GameObject staticShadowPrefab;
    [SerializeField] Color32 shadowColor;

    private Vector3 subcamPosition;
    private int textureFailsafeID = 1;
    public void RefreshAllStaticShadows()
    {
        trList = new Transform[] { initialWallPosition, initialLightPosition };
        subcamPosition = cam.transform.position;

        verticalResolution = (int)Mathf.Pow(2, shadowQuality);
        horizontalResolution = (int)Mathf.Pow(2, shadowQuality);



        // - Checking each static object for shadow, generate one if there is none
        foreach (Transform child in transform)
        {
            Transform tempShadow = child.Find("Shadow");
            if (tempShadow != null)
            {
                // - Set all shadow to have same color
                tempShadow.GetComponent<SpriteRenderer>().color = shadowColor;
            }
            else
            {
                GameObject newShadow = Instantiate(staticShadowPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                newShadow.name = "Shadow";
                newShadow.transform.parent = child;

                foreach (Transform camChild in cam.transform)
                {
                    DestroyImmediate(camChild.gameObject);
                }

                // *** Note: tempObj is the object used to screenshot and converted into 2D sprite
                GameObject tempObj = Instantiate(child.gameObject, subcamPosition, Quaternion.identity);
                Transform tempTR = tempObj.transform;
                tempTR.parent = cam.transform;
                tempTR.position = new Vector3(subcamPosition.x, subcamPosition.y, subcamPosition.z - 5);



                // - Camera Scaling based on model size (Checks Y for now, Z and X later)
                Transform[] modelTransforms = tempTR.Cast<Transform>().ToArray();
                float maxX = 0f;
                float maxY = 0f;
                float maxZ = 0f;
                foreach (Transform ctr in modelTransforms)
                {
                    MeshFilter thisMeshFilter = ctr.GetComponent<MeshFilter>();
                    if (!(thisMeshFilter == null))
                    {

                        Mesh thisMesh = thisMeshFilter.sharedMesh;
                        Vector3 thisBoundSize = thisMesh.bounds.size;
                        if (thisBoundSize.x > maxX)
                        {
                            maxX = thisBoundSize.x;
                        }
                        if (thisBoundSize.y > maxY)
                        {
                            maxY = thisBoundSize.y;
                        }
                        if (thisBoundSize.z > maxZ)
                        {
                            maxZ = thisBoundSize.z;
                        }
                    }
                }
                //Debug.Log(maxX);
                //Debug.Log(maxY);
                //Debug.Log(maxZ);
                if(maxZ * tempTR.localScale.z > 0.5)//Z and Y is swapped for some reason, will look into it later
                {
                    cam.orthographicSize = maxZ;
                }
                else
                {
                    cam.orthographicSize = 1;
                }


                // - Generate Shadow
                GenerateShadow(child.name, newShadow.GetComponent<SpriteRenderer>(), cam.orthographicSize);
            }

            StaticFakeShadow sfs = child.gameObject.GetComponent<StaticFakeShadow>();
            if (sfs == null)
            {
                child.gameObject.AddComponent<StaticFakeShadow>();
            }
        }
        textureFailsafeID = 1;
        BroadcastMessage("CastFakeShadow", trList);
    }


    public void GenerateShadow(string parentName, SpriteRenderer tempSR, float shadowSizeOffset)
    {
        // - Creating the texture and capturing
        RenderTexture renderTexture = new RenderTexture(horizontalResolution, verticalResolution, captureDepth);
        Rect rect = new Rect(0, 0, horizontalResolution, verticalResolution);
        Texture2D texture = new Texture2D(horizontalResolution, verticalResolution, TextureFormat.ARGB32, false);

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



        // - Saving texture as PNG
        byte[] itemBGBytes = sprite.texture.EncodeToPNG();
        outputfilename = $"{parentName}_ShadowSprite_{textureFailsafeID}";
        textureFailsafeID++;
        File.WriteAllBytes($"Assets/Resources/GeneratedShadowTextures/{outputfilename}.png", itemBGBytes);


        // - Setting texture settings
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath($"Assets/Resources/GeneratedShadowTextures/{outputfilename}.png");
        importer.textureType = TextureImporterType.Sprite;
        importer.alphaIsTransparency = true;

        importer.spritePixelsPerUnit = (100 * (shadowQuality - 8)) / (shadowSizeOffset / 2);
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();
#endif

        // - Applying sprite to shadow
        tempSR.sprite = Resources.Load<Sprite>($"GeneratedShadowTextures/{outputfilename}");
        tempSR.color = shadowColor;



        // - Reset Camera Size
        //cam.orthographicSize = 1;
    }




    //-----------------------------------------------Testing use only to be removed later----------------------------------------------------
    public void DebugChild(GameObject cgm)
    {
        foreach (Transform child in cam.transform)
        {
            DestroyImmediate(child.gameObject);
        }
        GameObject tempObj = Instantiate(cgm, subcamPosition, Quaternion.identity);
        Transform tempTR = tempObj.transform;
        tempTR.parent = cam.transform;
        tempTR.position = new Vector3(subcamPosition.x, subcamPosition.y - 0.5f, subcamPosition.z - 1);
    }
}
