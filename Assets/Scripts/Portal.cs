using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject portalEnter;
    public GameObject portalExit;
    public SelectionTaskMeasure selectionTaskMeasure;
    public GameObject linkedPortal;

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("leftHand") || other.CompareTag("rightHand"))
        {
            other.transform.position = portalExit.transform.position;
            selectionTaskMeasure.scoreText.text = "OnTriggerEnter";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("leftHand") || other.CompareTag("rightHand"))
        {
            other.transform.position = new Vector3(0,0,0);
            selectionTaskMeasure.scoreText.text = "OnTriggerExit";
        }
    }*/
}
