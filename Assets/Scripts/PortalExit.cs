using UnityEngine;

public class PortalExit : MonoBehaviour
{
    public MyGrab controllerInPortal;

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("portalEnter"))
        {
            controllerInPortal.ExitPortal();
        }
    }
}
