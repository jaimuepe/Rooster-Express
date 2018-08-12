using _Decal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public float minScale;
    public float maxScale;

    public GameObject[] boxPrefabs;
    private string[] codes;

    public Material[] randomDecals;

    public Material[] districtDecals;
    public Material[] districtDecalsBig;
    public Material[] districtDecalsBigCrossed;

    public Material[] colorDecals;

    public Material urgentDecal;
    public Material fragileDecal;

    public Rect spawnArea;

    public int ammountOfBoxes;
    public int ammountOfDecals;

    private void Awake()
    {
        codes = new string[] { "A", "B", "C", "D", "E" };
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SpawnBoxes(ammountOfBoxes);
        }
    }

    public void SpawnBoxes(int ammountOfBoxes)
    {
        StartCoroutine(SpawnBoxesDelayed(ammountOfBoxes, minScale, maxScale, ammountOfDecals, ammountOfDecals));
    }
#endif    

    public void SpawnBoxes(WaveInfo waveInfo)
    {
        int nBoxes = Random.Range(waveInfo.minNumberOfBoxes, waveInfo.maxNumberOfBoxes);
        StartCoroutine(SpawnBoxesDelayed(nBoxes, minScale, maxScale, waveInfo.minNumberOfDecals, waveInfo.maxNumberOfDecals));
    }

    void GenerateTags(GameObject baseObject, string districtCode, int ammountOfTags)
    {
        Transform baseTransform = baseObject.transform;
        float depth = 0.0001f;

        for (int i = 0; i < ammountOfTags; i++)
        {
            int idx = Random.Range(0, randomDecals.Length);
            Material decalMaterial = randomDecals[idx];

            GenerateTag(baseObject, baseTransform, decalMaterial, ref depth);
        }

        int districtIndex = GetDistrictIndex(districtCode);

        bool useBigDecal = Random.Range(0.0f, 1.0f) > 0.5f;
        if (useBigDecal)
        {
            Material districtDecalBig = districtDecalsBig[districtIndex];
            GenerateTag(baseObject, baseTransform, districtDecalBig, ref depth);
        }
        else
        {
            Material districtDecal = districtDecalsBigCrossed[AnotherRandomDistrict(districtIndex)];
            GenerateTag(baseObject, baseTransform, districtDecal, ref depth);

            Material districtDecalSmall = districtDecals[districtIndex];
            GenerateTag(baseObject, baseTransform, districtDecalSmall, ref depth);
        }

        Material colorDecal = colorDecals[districtIndex];
        GenerateTag(baseObject, baseTransform, colorDecal, ref depth);
    }

    int AnotherRandomDistrict(int distIndex)
    {
        List<int> indices = new List<int>()
        {
            0, 1, 2, 3
        };
        indices.Remove(distIndex);
        return indices.GetRandomItem();
    }

    private void GenerateTag(GameObject baseObject, Transform baseTransform, Material decalMaterial, ref float depth)
    {
        GameObject decalContainer = new GameObject("DecalContainer")
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


        float aspectRatio = (float)decalMaterial.mainTexture.height / decalMaterial.mainTexture.width;

        decalTransform.localScale = new Vector3(0.1f, 0.1f * aspectRatio, 3.0f);
        decalTransform.position -= new Vector3(0.0f, 0.0f, 1.5f);
        decalTransform.SetParent(decalContainerTransform, false);

        Decal decal = decalObj.AddComponent<Decal>();
        decal.maxAngle = 60;
        decal.pushDistance = depth;
        decal.material = decalMaterial;

        DecalBuilder.BuildAndSetDirty(decal, baseObject);
        depth += 0.0001f;
    }

    int GetDistrictIndex(string districtCode)
    {
        if (districtCode == "A")
        {
            return 0;
        }
        else if (districtCode == "B")
        {
            return 1;
        }
        else if (districtCode == "C")
        {
            return 2;
        }
        else if (districtCode == "D")
        {
            return 3;
        }
        return 4;
    }

    IEnumerator SpawnBoxesDelayed(int ammoutOfBoxes, float minScale, float maxScale, int minDecals, int maxDecals)
    {
        for (int i = 0; i < ammoutOfBoxes; i++)
        {
            GameObject box = Instantiate(boxPrefabs.GetRandomItem());
            Transform boxTransform = box.transform;

            boxTransform.position = transform.position + new Vector3(spawnArea.x, 0.0f, spawnArea.y);
            float scale = Random.Range(minScale, maxScale);

            boxTransform.localScale = scale * Vector3.one;

            string code = codes[Random.Range(0, codes.Length - 1)];

            GenerateTags(box, code, Random.Range(minDecals, maxDecals));
            box.gameObject.GetComponent<Rigidbody>().mass *= scale;
            box.GetComponent<Caja>().points *= scale;
            box.GetComponent<Caja>().code = code;

            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(spawnArea.x, 0.0f, spawnArea.y), new Vector3(spawnArea.width, 1.0f, spawnArea.height));
    }
}
