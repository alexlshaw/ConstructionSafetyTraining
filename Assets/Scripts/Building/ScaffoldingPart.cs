using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaffoldingPart : MonoBehaviour
{
    public float emptyChance = 0.1f;
    public bool isFilled;
    public GameObject socketObject;
    SocketWithNameCheck socket;

    private void Awake()
    {
        Initialise();
        socketObject = transform.Find("SocketInteractable").gameObject;
        socket = socketObject.GetComponent<SocketWithNameCheck>();
        if (!isFilled)
        {
            socket.startingSelectedInteractable = null;
        }
    }

    private void Initialise()
    {
        float random = Random.Range(0f, 1f);
        if (random < emptyChance)
        {
            isFilled = false;
        }
        else
        {
            isFilled = true;
        }

    }

}
