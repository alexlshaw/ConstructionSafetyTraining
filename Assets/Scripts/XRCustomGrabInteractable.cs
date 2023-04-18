using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRCustomGrabInteractable : XRGrabInteractable
{
    private Vector3 interactorPosition = Vector3.zero;
    private Quaternion interactorRotation = Quaternion.identity;
    private LayerMask originalLayer;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        MatchAttachPoint(args.interactor);
        originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("GrabInteractables");
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);

        gameObject.layer = originalLayer;

    }

    private void MatchAttachPoint(XRBaseInteractor interactor)
    {
        bool isDirect = interactor is XRDirectInteractor;
        attachTransform.position = isDirect ? interactor.attachTransform.position : transform.position;
        attachTransform.rotation = isDirect ? interactor.attachTransform.rotation : transform.rotation;
    }



}
