using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionTechnique : MonoBehaviour
{
    // Please implement your locomotion technique in this script. 
    public OVRInput.Controller leftController;
    public OVRInput.Controller rightController;
    [Range(0, 10)] public float translationGain = 0.5f;
    public GameObject hmd;
    [SerializeField] private float leftTriggerValue;    
    [SerializeField] private float rightTriggerValue;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isIndexTriggerDown;
    [SerializeField] private bool isLeftTriggerDown;
    [SerializeField] private bool isRightTriggerDown;

    public GameObject player;

    public GameObject leftEye;

    public GameObject rightEye;


    private float forceBuildUp;

    private float forceBuildUpFly;

    private float maxForce;

    private float maxForceFly;

    private Vector3 flatVector;

    private Vector3 tmp;

    private Vector3 upVector;



    /////////////////////////////////////////////////////////
    // These are for the game mechanism.
    public ParkourCounter parkourCounter;
    public string stage;
    public SelectionTaskMeasure selectionTaskMeasure;

    public GameObject startingArea;
    public GameObject startingAreaExplosion;
    public GameObject startingAreaRadio;
    
    void Start()
    {
        maxForce = 10;
        maxForceFly = 5;
        forceBuildUp = 0.02f;
        forceBuildUpFly = 0.02f;
        flatVector = new Vector3(1,0,1);
        upVector = new Vector3(0,1,0);
    }

    void Update()
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // Please implement your LOCOMOTION TECHNIQUE in this script :D.
        leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, leftController); 
        rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, rightController);

        /*if (leftTriggerValue > 0.95f && rightTriggerValue > 0.95f)
        {
            if (!isIndexTriggerDown)
            {
                isIndexTriggerDown = true;
                startPos = (OVRInput.GetLocalControllerPosition(leftController) + OVRInput.GetLocalControllerPosition(rightController)) / 2;
            }
            offset = hmd.transform.forward.normalized *
                    ((OVRInput.GetLocalControllerPosition(leftController) - startPos) +
                     (OVRInput.GetLocalControllerPosition(rightController) - startPos)).magnitude;
            Debug.DrawRay(startPos, offset, Color.red, 0.2f);
        }
        else if (leftTriggerValue > 0.95f && rightTriggerValue < 0.95f)
        {
            if (!isIndexTriggerDown)
            {
                isIndexTriggerDown = true;
                startPos = OVRInput.GetLocalControllerPosition(leftController);
            }
            offset = hmd.transform.forward.normalized *
                     (OVRInput.GetLocalControllerPosition(leftController) - startPos).magnitude;
            Debug.DrawRay(startPos, offset, Color.red, 0.2f);
        }
        else if (leftTriggerValue < 0.95f && rightTriggerValue > 0.95f)
        {
            if (!isIndexTriggerDown)
            {
                isIndexTriggerDown = true;
                startPos = OVRInput.GetLocalControllerPosition(rightController);
            }
           offset = hmd.transform.forward.normalized *
                    (OVRInput.GetLocalControllerPosition(rightController) - startPos).magnitude;
            Debug.DrawRay(startPos, offset, Color.red, 0.2f);
        }

        player.transform.position = player.transform.position + (offset) * translationGain;*/

        offset = Vector3.zero;

        if (leftTriggerValue > 0.95f)
        {
            /*parkourCounter.Log("Vignette: " +leftEye.GetComponent<OVRVignette>().VignetteFieldOfView.ToString());
            leftEye.GetComponent<OVRVignette>().VignetteFieldOfView = 80;
            rightEye.GetComponent<OVRVignette>().VignetteFieldOfView = 80;
            leftEye.GetComponent<OVRVignette>().enabled = !leftEye.GetComponent<OVRVignette>().enabled;
            rightEye.GetComponent<OVRVignette>().enabled = !rightEye.GetComponent<OVRVignette>().enabled;*/

            if (!isLeftTriggerDown)
            {
                isLeftTriggerDown = true;
                forceBuildUp = 0.14f;
            }
            else
            {
                // forceBuildUp += 0.01f * Time.deltaTime;
                if(forceBuildUp > maxForce)
                {
                    forceBuildUp = maxForce;
                }
            }
            parkourCounter.Log("hmd.transform.forward.normalized: " + hmd.transform.forward.normalized.ToString());
            tmp = Vector3.Scale(hmd.transform.forward.normalized,flatVector);
            // parkourCounter.Log(tmp.ToString());
            // tmp = Vector.Multiply(hmd.transform.forward.normalized * flatVector);
            offset += tmp * forceBuildUp;

        }
        else
        {
            isLeftTriggerDown = false;
        }

        // parkourCounter.Log("rightTriggerValue:" + rightTriggerValue);
        // TODO: is my right controller damaged? often times it seems to only reach a trigger value of up to ~80 while pressing it fully
        if (rightTriggerValue > 0.75f)
        {
            /*parkourCounter.Log("Vignette: " + leftEye.GetComponent<OVRVignette>().VignetteFieldOfView.ToString());
            leftEye.GetComponent<OVRVignette>().VignetteFieldOfView = 40;
            rightEye.GetComponent<OVRVignette>().VignetteFieldOfView = 40;
            leftEye.GetComponent<OVRVignette>().enabled = !leftEye.GetComponent<OVRVignette>().enabled;
            rightEye.GetComponent<OVRVignette>().enabled = !rightEye.GetComponent<OVRVignette>().enabled;*/

            if (!isRightTriggerDown)
            {
                isRightTriggerDown = true;
                forceBuildUpFly = 0.2f;
            }
            else
            {
                // forceBuildUpFly += 0.01f * Time.deltaTime;
                if (forceBuildUpFly > maxForceFly)
                {
                    forceBuildUpFly = maxForceFly;
                }
            }


            // parkourCounter.Log((upVector * forceBuildUpFly).ToString());
            parkourCounter.Log("up vector: " + (player.transform.up.normalized * forceBuildUpFly).ToString());
            // offset += upVector * forceBuildUpFly;
            /*parkourCounter.Log("hmd.tansform.up:");
            parkourCounter.Log(hmd.transform.up.normalized.ToString());
            parkourCounter.Log("player.transform.up.normalized:");
            parkourCounter.Log(player.transform.up.normalized.ToString());*/
            offset += player.transform.up.normalized * forceBuildUpFly;
            // offset += hmd.transform.up.normalized * forceBuildUpFly;
        }
        else
        {
            isRightTriggerDown = false;
        }

        /*if (leftTriggerValue > 0.95f && rightTriggerValue < 0.95f)
        {
            if (isIndexTriggerDown)
            {
                isIndexTriggerDown = false;
            }
        }*/

        // parkourCounter.Log("hmd forward:");
        // parkourCounter.Log(hmd.transform.forward.normalized.ToString());
        if(offset != Vector3.zero)
        {
            parkourCounter.Log("offset not zero: " + offset.ToString());
           
        }

        // Really hard to change directions
        player.GetComponent<Rigidbody>().AddForce(offset, ForceMode.Impulse);

        // Would need time to figure out meanigfull values
        // player.GetComponent<Rigidbody>().AddForce(offset, ForceMode.Acceleration);


        ////////////////////////////////////////////////////////////////////////////////
        // These are for the game mechanism.
        if (OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four))
        {
            if (parkourCounter.parkourStart)
            {
                player.transform.position = parkourCounter.currentRespawnPos;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {

        // These are for the game mechanism.
        if (other.CompareTag("banner"))
        {
            stage = other.gameObject.name;
            parkourCounter.isStageChange = true;
        }
        else if (other.CompareTag("objectInteractionTask"))
        {
            selectionTaskMeasure.isTaskStart = true;
            selectionTaskMeasure.scoreText.text = "";
            selectionTaskMeasure.partSumErr = 0f;
            selectionTaskMeasure.partSumTime = 0f;
            // rotation: facing the user's entering direction

            // float tempValueY = other.transform.position.y > 0 ? 12 : 0;
            float tempValueY = player.transform.position.y;
            // Vector3 tmpTarget = new Vector3(hmd.transform.position.x, tempValueY, hmd.transform.position.z);
            Vector3 tmpTarget = new Vector3(player.transform.position.x, tempValueY, player.transform.position.z);
            selectionTaskMeasure.taskUI.transform.position = new Vector3(selectionTaskMeasure.taskUI.transform.position.x, tempValueY, selectionTaskMeasure.taskUI.transform.position.z);
            selectionTaskMeasure.taskUI.transform.LookAt(tmpTarget);
            selectionTaskMeasure.taskUI.transform.Rotate(new Vector3(0, 180f, 0));
            parkourCounter.Log("transform.position: " + other.transform.position.ToString());
            parkourCounter.Log("hmd.transform.position: " + hmd.transform.position.ToString());
            parkourCounter.Log("selectionTaskMeasure.taskUI.transform.position: " + selectionTaskMeasure.taskUI.transform.position.ToString());
            selectionTaskMeasure.taskStartPanel.SetActive(true);
        }
        else if (other.CompareTag("coin"))
        {
            parkourCounter.coinCount += 1;
            this.GetComponent<AudioSource>().Play();
            other.gameObject.SetActive(false);
        }
        // These are for the game mechanism.
    }

    public void IntroductionScene()
    {
        startingAreaRadio.GetComponent<AudioSource>().Play();
        StartCoroutine(IntroductionScenePart2());
    }

    IEnumerator IntroductionScenePart2()
    {
        yield return new WaitForSeconds(15);
        startingAreaExplosion.GetComponent<ParticleSystem>().Play();
        StartCoroutine(IntroductionSceneEnd());
    }

    IEnumerator IntroductionSceneEnd()
    {
        yield return new WaitForSeconds(2);
        startingArea.SetActive(false);
    }
}
