using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EquipmentItem : MonoBehaviour
{
    public XRSocketInteractor socket;
    public XRBaseInteractable _startingTransform;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onResetLevel += reset;
        _startingTransform = GetComponent<XRSocketInteractor>().startingSelectedInteractable;
    }

    private void reset()
    {
        StartCoroutine(resetStartingTransform());
    }

    private IEnumerator resetStartingTransform()

    {
        var storeInteractionLayerMask = _startingTransform.interactionLayerMask;

        _startingTransform.interactionLayerMask = 0;
        yield return null;

        _startingTransform.gameObject.transform.position = transform.position;

        _startingTransform.interactionLayerMask = storeInteractionLayerMask;
        GetComponent<XRSocketInteractor>().StartManualInteraction(_startingTransform);

        yield break;

    }


}
