using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketWithTagCheck : XRSocketInteractor
{
    public string targetTag = string.Empty;

    public override bool CanHover(XRBaseInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.CompareTag(targetTag);

    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        //Debug.Log(interactable.tag);
        return base.CanSelect(interactable) && interactable.CompareTag(targetTag);
    }

}
