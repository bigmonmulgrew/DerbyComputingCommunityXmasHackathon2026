using UnityEngine;


public class RandomBauble : MonoBehaviour
{
    const string BAUBLE_HOLDER_NAME = "Baubles";

    [SerializeField] GameObject[] baubles;

    static GameObject baubleHolder;     // Stored static so it only needs to be found once.

    private void Start()
    {
        FindBaubleHolder();

        SpawnNewBauble();
    }

    private void SpawnNewBauble()
    {
        // Spawn random bauble at this items location

        GameObject temp = baubles[Random.Range(0, baubles.Length)];
        GameObject newBauble = Instantiate(temp, transform.position, Quaternion.identity);
        
        if (baubleHolder) newBauble.transform.SetParent(transform, true);

    }

    void FindBaubleHolder()
    {
        
        if (baubleHolder != null) return;               // If the holder exists skip this step. 

        baubleHolder = GameObject.Find(BAUBLE_HOLDER_NAME);      // Find "Baubles" Game object in Hierarchy

        if (baubleHolder != null) return;

        baubleHolder = new GameObject(BAUBLE_HOLDER_NAME);

        if (baubleHolder == null) Debug.LogError($"Failed to find bauble holder: {BAUBLE_HOLDER_NAME}, and failed to create it.");


    }
}
