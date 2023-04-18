using UnityEngine;

public class MiniMapIcon : MonoBehaviour
{
    Transform MinimapCam;
    float MinimapSize;
    Vector3 TempV3;
    private void Start()
    {
        MinimapSize = Globals.miniMapSize;
        MinimapCam = GameObject.Find("MinimapCamera").transform;
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    void Update()
    {
        TempV3 = transform.parent.transform.position;
        TempV3.y = 10f;
        transform.position = TempV3;
        transform.rotation = MinimapCam.rotation;

    }

    void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, MinimapCam.position.x - MinimapSize, MinimapSize + MinimapCam.position.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, MinimapCam.position.z - MinimapSize, MinimapSize + MinimapCam.position.z)
        );
    }
}
