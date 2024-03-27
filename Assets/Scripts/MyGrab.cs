using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrab : MonoBehaviour
{
    public OVRInput.Controller controller;
    private float triggerValue;
    private bool isInCollider;
    private bool isSelected;
    private GameObject selectedObj;
    public SelectionTaskMeasure selectionTaskMeasure;
    public LocomotionTechnique locomotionTech;
    public bool isInPortal = false;
    private GameObject portal;
    private Vector3 portalOffset;
    private Quaternion relativeRot;

    void Update()
    {
        triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);

        if (isInCollider)
        {
            if (!isSelected && triggerValue > 0.95f)
            {
                isSelected = true;
                selectedObj.transform.parent.transform.parent = this.transform;
            }
            else if (isSelected && triggerValue < 0.95f)
            {
                isSelected = false;
                selectedObj.transform.parent.transform.parent = null;
            }
        }
    }

    // make sure to use global coordinates for this function
    public bool IsPositionInsideBoxCollider(Vector3 position, BoxCollider boxCollider)
    {
        return boxCollider.bounds.Contains(position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("portalEnter") && !isInPortal)
        {
            // TODO: FIXME: When both controllers where in the portal, one gets pulled out, the movement is currently unblocked
            // using transform.parent instead of transform does not work unfortunately
            locomotionTech.preventMovement = true;
            isInPortal = true;
            portal = other.gameObject;

            selectionTaskMeasure.scoreText.text = "OnTriggerEnter";

            portalOffset = other.GetComponent<TaskPortal>().linkedPortal.transform.position - other.transform.position;
            selectionTaskMeasure.parkourCounter.Log("transform.position: " + transform.position.ToString());
            selectionTaskMeasure.parkourCounter.Log("transform.parent.position: " + transform.parent.position.ToString());
            transform.position = transform.position + portalOffset;
            selectionTaskMeasure.parkourCounter.Log("transform.parent.position new: " + transform.parent.position.ToString());

            // // Calculate relative rotation from child.transform.rotation to other.transform.rotation
            // // This math was suggeted by GitHub Copilot
            relativeRot = Quaternion.Inverse(other.transform.rotation) * transform.rotation;

            // // Set rotation to the same relative rotation in relation to the linkedPortal
            transform.rotation = other.GetComponent<TaskPortal>().linkedPortal.transform.rotation * relativeRot;
        }

        if (other.gameObject.CompareTag("objectT"))
        {
            isInCollider = true;
            selectedObj = other.gameObject;
        }
        else if (other.gameObject.CompareTag("selectionTaskStart"))
        {
            if (!selectionTaskMeasure.isCountdown)
            {
                selectionTaskMeasure.isTaskStart = true;
                selectionTaskMeasure.StartOneTask();
            }
        }
        else if (other.gameObject.CompareTag("done"))
        {
            selectionTaskMeasure.isTaskStart = false;
            selectionTaskMeasure.EndOneTask();
        }
        else if (other.CompareTag("startIntro"))
        {
            locomotionTech.IntroductionScene();
            other.gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("objectT"))
        {
            isInCollider = false;
            selectedObj = null;
        }
    }

    public void ExitPortal()
    {
        isInPortal = false;
        portal = null;

        selectionTaskMeasure.scoreText.text = "OnTriggerExit";

        selectionTaskMeasure.parkourCounter.Log("OnTriggerExit before local pos: " + transform.localPosition.ToString());
        selectionTaskMeasure.parkourCounter.Log("OnTriggerExit before : local rot " + transform.localRotation.ToString());
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.identity;
        selectionTaskMeasure.parkourCounter.Log("OnTriggerExit local pos: " + transform.localPosition.ToString());
        selectionTaskMeasure.parkourCounter.Log("OnTriggerExit: local rot " + transform.localRotation.ToString());

        locomotionTech.preventMovement = false;
    }
}
