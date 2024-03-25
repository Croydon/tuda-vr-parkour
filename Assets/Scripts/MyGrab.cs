using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;

public class MyGrab : MonoBehaviour
{
    public OVRInput.Controller controller;
    /*public OVRInput.Controller controllerRight = OVRInput.Controller.RTouch;
    public OVRInput.Controller controllerLeft = OVRInput.Controller.RTouch;*/
    private float triggerValue;
    private bool isInCollider;
    private bool isSelected;
    private GameObject selectedObj;
    public SelectionTaskMeasure selectionTaskMeasure;
    public LocomotionTechnique locomotionTech;
    public bool isHand = false;
    public GameObject handTracking;
    public bool isInPortal = false;
    public GameObject portal;
    private Vector3 portalOffset;
    private Vector3 portalEnterPos;
    private TaskPortal activePortalComponent;

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

    public bool IsPositionInsideBoxCollider(Vector3 position, BoxCollider boxCollider)
    {
        // Convert the position to the boxCollider's local space
        // Vector3 localPosition = boxCollider.transform.InverseTransformPoint(position);

        // Bounds localBounds = new Bounds(boxCollider.center, boxCollider.size);

        return boxCollider.bounds.Contains(position);
    }

    void FixedUpdate()
    {
        if (isInPortal)
        {
            // Get the controller's local position
            // Vector3 localPosition = OVRInput.GetLocalControllerPosition(controller);
            // Vector3 localPosition = OVRInput.GetLocalControllerPosition(controller);
            // Convert the controller's local position to world coordinates
            // Vector3 worldPosition = transform.TransformPoint(localPosition);

            // Get physical controler position mapped to virtual
            // Basically virtual plus the portal offset
            // Vector3 physicalPosition = transform.position - portalOffset;

            // calculate relative position from controller to linkedPortal position
            Vector3 relativePos = transform.position - activePortalComponent.linkedPortal.transform.position;

            // set controller position to the same relative position from portalEnterPos
            Vector3 physicalPosition = portalEnterPos + relativePos;


            selectionTaskMeasure.parkourCounter.Log("physicalPosition: " + physicalPosition.ToString());
            selectionTaskMeasure.parkourCounter.Log("portalColliderPos: " + portal.GetComponent<BoxCollider>().transform.position.ToString());
            selectionTaskMeasure.parkourCounter.timeText.text = physicalPosition.ToString() + "\n -- " + portal.GetComponent<BoxCollider>().transform.position.ToString();


            // Check if the controler is within the collider of the portal
            BoxCollider targetBox = portal.GetComponent<BoxCollider>();
            bool isInside = IsPositionInsideBoxCollider(physicalPosition, targetBox);
            selectionTaskMeasure.parkourCounter.Log("isInside: " + isInside.ToString());
            // bool isInside = Physics.CheckBox(physicalPosition, targetBox.size / 2, targetBox.transform.rotation, -1, QueryTriggerInteraction.Collide);

            if(!isInside)
            {
                isInPortal = false;
                portal = null;
                ExitPortal();
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("portalEnter") && !isInPortal)
        {
            locomotionTech.preventMovement = true;
            isInPortal = true;
            portal = other.gameObject;

            selectionTaskMeasure.scoreText.text = "OnTriggerEnter";
            // selectionTaskMeasure.startPanelText.text = "OnTriggerEnter";

            // transform.position = other.GetComponent<TaskPortal>().linkedPortal.transform.position;
            // calculate relative position from transform.position to other.transform.position
            // Vector3 relativePos = transform.position - other.transform.position;

            /*for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;

                selectionTaskMeasure.parkourCounter.Log("OnTriggerEnter pos: " + i + " " + child.transform.position.ToString());
                selectionTaskMeasure.parkourCounter.Log("OnTriggerEnter: rot " + i + " " + child.transform.rotation.ToString());
                selectionTaskMeasure.parkourCounter.Log("OnTriggerEnter local pos: " + i + " " + child.transform.localPosition.ToString());
                selectionTaskMeasure.parkourCounter.Log("OnTriggerEnter: local rot " + i + " " + child.transform.localRotation.ToString());

                // // set position to the same relativ position from linkedPortal.transform.position
                // child.transform.localPosition = other.GetComponent<TaskPortal>().linkedPortal.transform.position + relativePos;
                // // adjust rotation to the same as linkedPortal
                // child.transform.localRotation = other.GetComponent<TaskPortal>().linkedPortal.transform.rotation;

                // calculate relative position from child.transform.position to other.transform.position
                Vector3 relativePos = child.transform.position - other.transform.position;

                // set position to the same relative position from linkedPortal.transform.position
                child.transform.position = other.GetComponent<TaskPortal>().linkedPortal.transform.position + relativePos;

                // Calculate relative rotation from child.transform.rotation to other.transform.rotation
                // This math was suggeted by GitHub Copilot
                Quaternion relativeRot = Quaternion.Inverse(other.transform.rotation) * child.transform.rotation;

                // Set rotation to the same relative rotation from linkedPortal.transform.rotation
                child.transform.rotation = other.GetComponent<TaskPortal>().linkedPortal.transform.rotation * relativeRot;

                // child.GetComponent<MyGrab>().enabled = true;
            }*/

            // calculate relative position from controller to portal position
            Vector3 relativePos = transform.position - other.transform.position;
            portalEnterPos = other.transform.position;
            portalOffset = relativePos;
            activePortalComponent = other.GetComponent<TaskPortal>();

            // set controller position to the same relative position from linkedPortal.transform.position
            transform.position = other.GetComponent<TaskPortal>().linkedPortal.transform.position + relativePos;

            // Calculate relative rotation from child.transform.rotation to other.transform.rotation
            // This math was suggeted by GitHub Copilot
            Quaternion relativeRot = Quaternion.Inverse(other.transform.rotation) * transform.rotation;

            // Set rotation to the same relative rotation in relation to the linkedPortal
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
        if (other.CompareTag("portalEnter"))
        {
            // ExitPortal();
        }
        else if (other.gameObject.CompareTag("objectT"))
        {
            isInCollider = false;
            selectedObj = null;
        }
    }

    void ExitPortal()
    {
        selectionTaskMeasure.scoreText.text = "OnTriggerExit";
        // selectionTaskMeasure.startPanelText.text = "OnTriggerExit";

        /*
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.transform.localPosition = new Vector3(0, 0, 0);
            child.transform.localRotation = Quaternion.identity;
            selectionTaskMeasure.parkourCounter.Log("OnTriggerExit local pos: " + i + " " + child.transform.localPosition.ToString());
            selectionTaskMeasure.parkourCounter.Log("OnTriggerExit: local rot " + i + " " + child.transform.localRotation.ToString());

            // child.GetComponent<MyGrab>().enabled = false;
        }*/

        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.identity;
        selectionTaskMeasure.parkourCounter.Log("OnTriggerExit local pos: " + transform.localPosition.ToString());
        selectionTaskMeasure.parkourCounter.Log("OnTriggerExit: local rot " + transform.localRotation.ToString());


        locomotionTech.preventMovement = false;
    }
}
