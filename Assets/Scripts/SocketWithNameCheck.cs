using UnityEngine.XR.Interaction.Toolkit;


public class SocketWithNameCheck : XRSocketInteractor
{
    public string targetName = string.Empty;

    public override bool CanHover(XRBaseInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.name.Equals(targetName);

    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        //Debug.Log(interactable.tag);
        return base.CanSelect(interactable) && interactable.name.Equals(targetName);
    }
}
