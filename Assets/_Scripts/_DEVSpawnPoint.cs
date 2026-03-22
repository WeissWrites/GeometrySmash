using UnityEngine;

public class _DEVSpawnPoint : MonoBehaviour
{
    public int spawnIndex;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
        Gizmos.DrawIcon(transform.position + Vector3.up, "MoveTool", true);
    }
}