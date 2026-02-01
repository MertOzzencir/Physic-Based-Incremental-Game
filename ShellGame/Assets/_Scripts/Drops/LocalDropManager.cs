using UnityEngine;

public class LocalDropManager : MonoBehaviour
{
    [SerializeField] private DropSO childDropData;


    public void SpawnDrop(Vector3 spawnPositon)
    {
        Instantiate(childDropData.Prefab, spawnPositon, Quaternion.identity);
        Destroy(gameObject);
    }
}
