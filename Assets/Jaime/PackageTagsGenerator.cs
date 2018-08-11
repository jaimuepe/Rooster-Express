using _Decal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageTagsGenerator : MonoBehaviour
{
    public GameObject testObject;
    public Material[] decalMaterials;
    public Sprite[] decalSprites;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GenerateTags(testObject, 5);
        }
    }
    public void GenerateTags(GameObject baseObject, int ammountOfTags)
    {
        Transform baseTransform = baseObject.transform;
        float depth = 0.0001f;

        for (int i = 0; i < ammountOfTags; i++)
        {
            GameObject decalContainer = new GameObject("DecalContainer_" + i)
            {
                layer = baseObject.layer
            };
            Transform decalContainerTransform = decalContainer.transform;

            decalContainerTransform.rotation = Random.rotation;
            decalContainerTransform.SetParent(baseTransform, false);

            GameObject decalObj = new GameObject("Decal")
            {
                layer = baseObject.layer
            };
            Transform decalTransform = decalObj.transform;

            int idx = Random.Range(0, decalMaterials.Length);
            Material mat = decalMaterials[idx];

            float aspectRatio = (float) mat.mainTexture.height / mat.mainTexture.width;
            Debug.Log(mat.mainTexture.name + ", " + aspectRatio);

            decalTransform.localScale = new Vector3(0.5f, 0.5f * aspectRatio, 3.0f);
            decalTransform.position -= new Vector3(0.0f, 0.0f, 1.5f);
            decalTransform.SetParent(decalContainerTransform, false);

            Decal decal = decalObj.AddComponent<Decal>();
            decal.maxAngle = 180;
            decal.pushDistance = depth;
            decal.material = mat;

            DecalBuilder.BuildAndSetDirty(decal, baseObject);

            depth += 0.0001f;
        }
    }
}
