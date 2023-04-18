using UnityEngine;

public class DiggableBlock : MonoBehaviour
{
    DiggableBlocks diggableBlocks;
    private void Start()
    {
        diggableBlocks = GetComponentInParent<DiggableBlocks>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Bucket"))
        {
            this.gameObject.SetActive(false);
            diggableBlocks.addDestroyed();
        }
    }
}
