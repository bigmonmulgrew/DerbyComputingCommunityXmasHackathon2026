using UnityEngine;
using UnityEngine.Splines;

public class LightChain : MonoBehaviour
{
    [SerializeField] GameObject lightPrefab;
    [Range(0f, 0.2f)]
    [SerializeField] float minProgress = 0.05f;
    [Range(0.01f, 1f)]
    [SerializeField] float maxProgress = 0.3f;
    [SerializeField] bool spawnAtStart = true;
    [SerializeField] bool spawnAtEnd = true;

    SplineContainer spline;
    float progress;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spline = GetComponent<SplineContainer>();
        SpawnLights();
    }

    void SpawnLights()
    {
        if (spawnAtStart) SpawnLight();

        while (progress < 1)
        {
            SpawnLight();
        }
        progress = 1;

        if (spawnAtEnd) SpawnLight();
    }

    private void SpawnLight()
    {
        float i = Random.Range(minProgress, maxProgress);

        progress += i;

        Vector3 pos = spline.EvaluatePosition(progress);

        Instantiate(lightPrefab, pos, Quaternion.identity);
    }

}
