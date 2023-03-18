using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class concreteMixerWheelRotation : MonoBehaviour
{
    Rigidbody rbody;
    public bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            rbody.constraints = RigidbodyConstraints.None;
        }
        else
        {
            rbody.constraints = RigidbodyConstraints.FreezeRotationX;
        }
    }
    public void setCanMove()
    {
        canMove = true;
    }

    public void setCannotMove()
    {
        canMove = false;
    }
}
