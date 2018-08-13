using _Decal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public float minScale;
    public float maxScale;

    public GameObject[] boxPrefabs;
    public GameObject[] garbagePrefabs;

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

    GameManager gm;

    private void Awake()
    {
        codes = new string[] { "A", "B", "C", "D", "E" };
    }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
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
        WaveInfo waveInfo = new WaveInfo
        {
            minNumberOfBoxes = ammountOfBoxes,
            maxNumberOfBoxes = ammountOfBoxes + 1,
            minNumberOfDecals = ammountOfDecals,
            maxNumberOfDecals = ammountOfDecals + 1
        };

        StartCoroutine(SpawnBoxesDelayed(waveInfo));
    }
#endif    

    public void SpawnBoxes(WaveInfo waveInfo)
    {
        int nBoxes = Random.Range(waveInfo.minNumberOfBoxes, waveInfo.maxNumberOfBoxes);
        StartCoroutine(SpawnBoxesDelayed(waveInfo));
    }

    void GenerateTags(GameObject baseObject, string districtCode, int ammountOfTags, bool canSpawnCrossedLabel = true, bool fragile = false)
    {
        Transform baseTransform = baseObject.transform;
        float depth = 0.0001f;

        for (int i = 0; i < ammountOfTags; i++)
        {
            int idx = Random.Range(0, randomDecals.Length);
            Material decalMaterial = randomDecals[idx];

            GenerateTag(baseObject, baseTransform, decalMaterial, 1.0f, ref depth);
        }

        int districtIndex = GetDistrictIndex(districtCode);

        bool useBigDecal = Random.value >= 0.5f;
        if (!canSpawnCrossedLabel)
        {
            useBigDecal = true;
        }

        Material colorDecal = colorDecals[districtIndex];
        GenerateTag(baseObject, baseTransform, colorDecal, 1.0f, ref depth);

        if (fragile)
        {
            GenerateTag(baseObject, baseTransform, fragileDecal, 2.0f, ref depth);
        }

        if (useBigDecal)
        {
            Material districtDecalBig = districtDecalsBig[districtIndex];
            GenerateTag(baseObject, baseTransform, districtDecalBig, 2.0f, ref depth);
        }
        else
        {
            Material districtDecal = districtDecalsBigCrossed[AnotherRandomDistrict(districtIndex)];
            GenerateTag(baseObject, baseTransform, districtDecal, 2.0f, ref depth);

            Material districtDecalSmall = districtDecals[districtIndex];
            GenerateTag(baseObject, baseTransform, districtDecalSmall, 1.0f, ref depth);
        }
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

    private void GenerateTag(GameObject baseObject, Transform baseTransform, Material decalMaterial, float scale, ref float depth)
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

        decalTransform.localScale = scale * new Vector3(0.1f, 0.1f * aspectRatio, 3.0f);
        // decalTransform.position -= new Vector3(0.0f, 0.0f, 1.5f);
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

    IEnumerator SpawnBoxesDelayed(WaveInfo waveInfo)
    {
        int ammountOfBoxes = Random.Range(waveInfo.minNumberOfBoxes, waveInfo.maxNumberOfBoxes);

        for (int i = 0; i < ammountOfBoxes; i++)
        {
            string code;
            if (!string.IsNullOrEmpty(waveInfo.districtCode))
            {
                code = waveInfo.districtCode;
            }
            else if (waveInfo.canSpawnGarbage)
            {
                code = codes[Random.Range(0, codes.Length)];
            }
            else
            {
                code = codes[Random.Range(0, codes.Length - 1)];
            }

            GameObject box;
            if (code == "E")
            {
                box = Instantiate(garbagePrefabs.GetRandomItem());
            }
            else
            {
                box = Instantiate(boxPrefabs.GetRandomItem());
            }

            Transform boxTransform = box.transform;

            bool fragile = Random.value < 0.1f;

            boxTransform.position = transform.position + new Vector3(spawnArea.x, 0.0f, spawnArea.y);
            float scale = 1.0f;
            if (code != "E")
            {
                scale = Random.Range(minScale, maxScale);
                boxTransform.localScale = scale * Vector3.one;
                GenerateTags(box, code, Random.Range(waveInfo.minNumberOfDecals, waveInfo.maxNumberOfDecals), waveInfo.canSpawnCrossedLabel, fragile);
            }

            box.gameObject.GetComponent<Rigidbody>().mass *= scale;
            Caja bb = box.GetComponent<Caja>();


            bb.fragile = fragile;
            bb.points *= scale;
            bb.code = code;

            gm.OnBoxSpawned();

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(spawnArea.x, 0.0f, spawnArea.y), new Vector3(spawnArea.width, 1.0f, spawnArea.height));
    }
}
