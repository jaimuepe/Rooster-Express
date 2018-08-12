using _Decal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public float minScale;
    public float maxScale;

    public GameObject[] boxPrefabs;
    public Material[] decalMaterials;
    private string[] codes;

    public Rect spawnArea;

    public int ammountOfBoxes;
    public int ammountOfDecals;

    private void Start()
    {
        codes = new string[] { "A", "B", "C", "D", "E" };
        SpawnBoxes(ammountOfBoxes);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SpawnBoxes(ammountOfBoxes);
        }
#endif    
    }

    public void SpawnBoxes(int ammountOfBoxes)
    {
        StartCoroutine(SpawnBoxDelayed(ammountOfBoxes));
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

            float aspectRatio = (float)mat.mainTexture.height / mat.mainTexture.width;
            Debug.Log(mat.mainTexture.name + ", " + aspectRatio);

            decalTransform.localScale = new Vector3(0.1f, 0.1f * aspectRatio, 3.0f);
            decalTransform.position -= new Vector3(0.0f, 0.0f, 1.5f);
            decalTransform.SetParent(decalContainerTransform, false);

            Decal decal = decalObj.AddComponent<Decal>();
            decal.maxAngle = 60;
            decal.pushDistance = depth;
            decal.material = mat;

            DecalBuilder.BuildAndSetDirty(decal, baseObject);

            depth += 0.0001f;
        }
    }

    IEnumerator SpawnBoxDelayed(int ammoutOfBoxes)
    {
        for (int i = 0; i < ammoutOfBoxes; i++)
        {
            GameObject box = Instantiate(boxPrefabs.GetRandomItem());
            Transform boxTransform = box.transform;

            boxTransform.position = transform.position + new Vector3(spawnArea.x, 0.0f, spawnArea.y);
            float scale = Random.Range(minScale, maxScale);

            boxTransform.localScale = scale * Vector3.one;

            GenerateTags(box, ammountOfDecals);
            box.gameObject.GetComponent<Rigidbody>().mass *= scale;
            box.GetComponent<Caja>().points *= scale;
            box.GetComponent<Caja>().code = codes[Random.Range(0, 5)];

            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(spawnArea.x, 0.0f, spawnArea.y), new Vector3(spawnArea.width, 1.0f, spawnArea.height));
    }
}
