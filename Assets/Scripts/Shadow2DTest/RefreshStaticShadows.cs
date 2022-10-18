using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;// For Cast function
using Unity.Collections;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class RefreshStaticShadows : MonoBehaviour
{
    //Refresh all static shadows
    //More paramaters can be added, need to pass an object of a class with all the parameters
    [Header("Transform Controls")]
    [SerializeField] Transform initialLightPosition;
    [SerializeField] Transform initialWallPosition;
    Transform[] trList;

#region shadowStuff
    [Header("Shadow Generation")]
    [SerializeField] Camera generationCam;//Shadow generator camera
    [SerializeField] SpriteRenderer testSR;//Sprite renderer for testing
    [SerializeField] string outputfilename; //Placeholder for texture file name
    [Range(9.0f, 15.0f)]
    [SerializeField] int shadowQuality;
    public int verticalResolution;
    public int horizontalResolution;
    [SerializeField] int captureDepth = 24;
    [SerializeField] GameObject staticShadowPrefab;
    [SerializeField] Color32 shadowColor;
    [SerializeField] Material shadowMaterial;//Somewhat solves additive alpha blending

    [Header("Shadow Generation for specified object (Maybe buggy)")]
    [SerializeField] GameObject specifiedObj;

    [SerializeField] List<Transform> partsList;
    [SerializeField] bool generateForPartsSeperately;

    [Header("Shadow Regen Safety Lock")]
    [Tooltip("Check if you want to actually regenerate all shadows and delete old ones, keep it unchecked if you're afraid of accidentally regenerating every shadow")]
    [SerializeField] bool allowDeleteOldShadow = false;

    private Vector3 subcamPosition;
    private int textureFailsafeID = 1;
    private string spritePath;

#region BasicFunctions
    public void RefreshAllStaticShadows(bool deleteOldShadows)
    {
        deleteOldShadows = allowDeleteOldShadow ? (deleteOldShadows ? true : false) : false;       
        spritePath = $"Assets/Resources/GeneratedShadowTextures/{SceneManager.GetActiveScene().name}";
        trList = new Transform[] { initialWallPosition, initialLightPosition };
        subcamPosition = generationCam.transform.position;

        verticalResolution = (int)Mathf.Pow(2, shadowQuality);
        horizontalResolution = (int)Mathf.Pow(2, shadowQuality);

#if UNITY_EDITOR
        // - Checking Directory for scene specific sprites, create if it doesnt exist
        if(deleteOldShadows)
        {
            if (AssetDatabase.IsValidFolder(spritePath))
            {
                Directory.Delete(spritePath, true);
            }
            Directory.CreateDirectory(spritePath);
        }
        
#endif

        // - Checking each static object for shadow, generate one if there is none
        foreach (Transform child in transform)
        {
            PreshadowGenerationCheck(child, deleteOldShadows);
        }
        textureFailsafeID = 1;
        BroadcastMessage("CastFakeShadow", trList);
    }

    // - Checking if components and scripts exists before generating
    public void PreshadowGenerationCheck(Transform thisChild, bool delShadows, bool isSpecified = false)
    {
        if(isSpecified)
        {
            if(specifiedObj != null)
            {
                thisChild = specifiedObj.transform;
            }
            else
            {
                Debug.LogWarning("No Specified Object Given");
                return;
            }
        }

        // - Delete old shadows
        Transform tempShadow = thisChild.Find("Shadow");
        if (tempShadow != null)
        {
            // - Set all shadow to have same color [Disabled]
            //tempShadow.GetComponent<SpriteRenderer>().color = shadowColor;
            if (delShadows)
            {
                DestroyImmediate(tempShadow.gameObject);
            }
            else
            {
                return;
            }

        }

        GameObject newShadow = Instantiate(staticShadowPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        newShadow.name = "Shadow";
        newShadow.transform.parent = thisChild;
        newShadow.transform.SetAsFirstSibling();
        foreach (Transform camChild in generationCam.transform)
        {
            DestroyImmediate(camChild.gameObject);
        }


        // *** Note to self: tempObj is the object used to screenshot and converted into 2D sprite
        GameObject tempObj = Instantiate(thisChild.gameObject, subcamPosition, thisChild.rotation);
        Transform tempTR = tempObj.transform;

        tempTR.parent = generationCam.transform;
        tempTR.localScale = new Vector3(1, 1, 1);
        tempTR.position = new Vector3(subcamPosition.x, subcamPosition.y, subcamPosition.z - 5);
        if (thisChild.rotation.eulerAngles.x == 270)
        {
            tempTR.Rotate(0, 0, 180);
        }
        else
        {
            tempTR.Rotate(0, 180, 0);
        }
        //Debug.Log($"{child.rotation.eulerAngles.x} , {child.rotation.eulerAngles.y} , {child.rotation.eulerAngles.z}");


        // - Camera Scaling based on model size (Checks Y for now, Z and X later)

        float maxX = 0f;
        float maxY = 0f;
        float maxZ = 0f;

        // - - Parent Check if it's an independent object
        MeshFilter parentMeshFilter = tempTR.GetComponent<MeshFilter>();
        if (parentMeshFilter != null)
        {
            maxX = parentMeshFilter.sharedMesh.bounds.size.x;
            maxY = parentMeshFilter.sharedMesh.bounds.size.y;
            maxZ = parentMeshFilter.sharedMesh.bounds.size.z;
        }
        else
        {
            // - - Check all child transform to get the largest size
            Transform[] modelTransforms = tempTR.Cast<Transform>().ToArray();

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
        }
        Debug.Log($"X: {maxX} Y:{maxY} Z:{maxZ}  [{tempObj.name}]");

        //Because of rotation, Y and Z needs to swap as the generationCam only checks the height to resize the camera the Y axis, if X is rotated then Z would be the new Y axis
        // !!! Need to check if Horizontal/width is larger than the screen size as well !!! [WIP]
        if (thisChild.rotation.x == 0)
        {
            generationCam.orthographicSize = maxY * 2;
        }
        else
        {
            generationCam.orthographicSize = maxZ * 2;
        }

        Debug.Log(thisChild.rotation.x);
        // - Check for static shadow script [Can remove the script later if not needed]
        StaticFakeShadow sfs = thisChild.gameObject.GetComponent<StaticFakeShadow>();
        if (sfs == null)
        {
            thisChild.gameObject.AddComponent<StaticFakeShadow>();
        }

        // - Generate Shadow

        if (isSpecified)
        {
            string permSpritePath = $"Assets/Resources/GeneratedShadowTextures/PermanentSprites";
            GenerateShadow(thisChild.name, newShadow.GetComponent<SpriteRenderer>(), generationCam.orthographicSize, permSpritePath);
            thisChild.GetComponent<StaticFakeShadow>().CastFakeShadow(new Transform[] { initialWallPosition, initialLightPosition });
        }
        else
        {
            GenerateShadow(thisChild.name, newShadow.GetComponent<SpriteRenderer>(), generationCam.orthographicSize);
        }
        
        // ++++++++++++++++++++++ END of environment setup for the camera


        
    }

    public void GeneratePartByPart() // Special shadow generation part by part, hence was done in a seperate function
    {
        if (partsList.Count <= 0)
        {
            Debug.LogWarning("No Specified Object Given");
            return;
        }

        float maxX = 0f;
        float maxY = 0f;
        float maxZ = 0f;

        foreach (Transform ctr in partsList)
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

        generationCam.orthographicSize = maxY * 2;

        foreach (Transform part in partsList)
        {
            // *** Note to self: tempObj is the object used to screenshot and converted into 2D sprite
            GameObject tempObj = Instantiate(part.gameObject, subcamPosition, part.rotation);
            Transform tempTR = tempObj.transform;

            tempTR.parent = generationCam.transform;
            tempTR.localScale = new Vector3(1, 1, 1);
            tempTR.position = new Vector3(subcamPosition.x, subcamPosition.y, subcamPosition.z - 5);
            if (part.rotation.eulerAngles.x == 270)
            {
                tempTR.Rotate(0, 0, 180);
            }
            else
            {
                tempTR.Rotate(0, 180, 0);
            }


            //Disable children if there are parts that are also children
            if (tempTR.childCount > 0)
            {
                foreach(Transform childOfthis in tempTR.Cast<Transform>().ToArray())
                {
                    childOfthis.gameObject.SetActive(false);
                }
            }

            //Shadow prefab generation
            GameObject newShadow = Instantiate(staticShadowPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            newShadow.name = "Shadow";
            newShadow.transform.parent = part;
            newShadow.transform.SetAsFirstSibling();
            foreach (Transform camChild in generationCam.transform)
            {
                DestroyImmediate(camChild.gameObject);
            }

            StaticFakeShadow sfs = part.gameObject.GetComponent<StaticFakeShadow>();
            if (sfs == null)
            {
                part.gameObject.AddComponent<StaticFakeShadow>();
            }


            

            // - Generate Shadow
            string permSpritePath = $"Assets/Resources/GeneratedShadowTextures/PermanentSprites";
            GenerateShadow(part.name, newShadow.GetComponent<SpriteRenderer>(), generationCam.orthographicSize, permSpritePath);
            part.GetComponent<StaticFakeShadow>().CastFakeShadow(new Transform[] { initialWallPosition, initialLightPosition });
        }
        
    }



#endregion



    // - Generate Shadow function
    public void GenerateShadow(string parentName, SpriteRenderer tempSR, float shadowSizeOffset, string specifiedSpritePath = null)
    {
        // - Creating the texture and capturing
        RenderTexture renderTexture = new RenderTexture(horizontalResolution, verticalResolution, captureDepth);
        Rect rect = new Rect(0, 0, horizontalResolution, verticalResolution);
        Texture2D texture = new Texture2D(horizontalResolution, verticalResolution, TextureFormat.ARGB32, false);

        generationCam.targetTexture = renderTexture;
        generationCam.Render();

        RenderTexture currentRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();

        generationCam.targetTexture = null;
        RenderTexture.active = currentRenderTexture;
        DestroyImmediate(renderTexture);

        Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);


        if(specifiedSpritePath!= null)
        {
            spritePath = specifiedSpritePath;
        }

        // - Saving texture as PNG
        byte[] itemBGBytes = sprite.texture.EncodeToPNG();
        outputfilename = $"{parentName}_ShadowSprite";
        textureFailsafeID++;
        File.WriteAllBytes($"{spritePath}/{outputfilename}.png", itemBGBytes);

#if UNITY_EDITOR

        // - Setting texture settings
        UnityEditor.AssetDatabase.Refresh();
        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath($"{spritePath}/{outputfilename}.png");
        importer.textureType = TextureImporterType.Sprite;
        importer.alphaIsTransparency = true;
        importer.filterMode = FilterMode.Point;
        importer.spritePixelsPerUnit = (100 * (shadowQuality - 8)) / (shadowSizeOffset / 2);
        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();
#endif

        // - Applying sprite to shadow
        if(specifiedSpritePath != null)
        {
            tempSR.sprite = Resources.Load<Sprite>($"GeneratedShadowTextures/PermanentSprites/{outputfilename}");
        }
        else
        {
            tempSR.sprite = Resources.Load<Sprite>($"GeneratedShadowTextures/{SceneManager.GetActiveScene().name}/{outputfilename}");
        }
        
        tempSR.color = shadowColor;
        tempSR.material = shadowMaterial;
        tempSR.gameObject.AddComponent<PolygonCollider2D>();



        // - Reset Camera Size
        //generationCam.orthographicSize = 1;
    }


    // *** Refresh all shadow based on light!!, This can be called to refresh shadows when the light source moves.
    public void RefreshAllShadowsProperties()
    {
        trList = new Transform[] { initialWallPosition, initialLightPosition };
        BroadcastMessage("CastFakeShadow", trList);
    }
    #endregion

    public void GenerateAllCollider()
    {
        foreach (Transform child in transform)
        {
            // - Delete old shadows
            Transform tempShadow = child.Find("Shadow");
            if (tempShadow != null)
            {
                if (tempShadow.GetComponent<PolygonCollider2D>() == null)
                {
                    tempShadow.gameObject.AddComponent<PolygonCollider2D>();
                }
                else
                {
                    continue;
                }
               
            }
            else
            {
                continue;
            }
        }
    }

    
    //-----------------------------------------------Testing use only to be removed later----------------------------------------------------
    public void DebugChild(GameObject cgm)
    {
        foreach (Transform child in generationCam.transform)
        {
            DestroyImmediate(child.gameObject);
        }
        GameObject tempObj = Instantiate(cgm, subcamPosition, Quaternion.identity);
        Transform tempTR = tempObj.transform;
        tempTR.parent = generationCam.transform;
        tempTR.position = new Vector3(subcamPosition.x, subcamPosition.y - 0.5f, subcamPosition.z - 1);
    }
}
