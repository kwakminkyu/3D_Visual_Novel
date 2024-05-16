using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] private Transform tf_Crosshair;

    [SerializeField] private Transform tf_Cam;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float applySpeed;

    [SerializeField] private float fieldSensitivity;
    [SerializeField] private float fieldLookLimitX;

    [SerializeField] private Vector2 camBoundary;
    [SerializeField] private float sightMoveSpeed;
    [SerializeField] private float sightSensitivity;
    [SerializeField] private float lookLimitX;
    [SerializeField] private float lookLimitY;
    private float currentAngleX;
    private float currentAngleY;

    [SerializeField] private GameObject notCamUp;
    [SerializeField] private GameObject notCamDown;
    [SerializeField] private GameObject notCamLeft; 
    [SerializeField] private GameObject notCamRight;

    private float originPosY;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        originPosY = transform.localPosition.y;
    }

    private void Update()
    {
        if (!InteractionController.isInteract)
        {
            if (CameraController.onlyView)
            {
                CrosshairMoving();
                ViewMoving();
                KeyViewMoving();
                CameraLimit();
                NotCamUI();
            }
            else
            {
                FieldMoving();
                FieldLooking();
            }
        }
    }

    private void FieldMoving()
    {
        if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

            if (Input.GetKey(KeyCode.LeftShift))
                applySpeed = runSpeed;
            else
                applySpeed = walkSpeed;
            transform.Translate(direction * applySpeed * Time.deltaTime, Space.Self);
        }
    }

    private void FieldLooking()
    {
        if (Input.GetAxisRaw("Mouse X") != 0)
        {
            float angleY = Input.GetAxisRaw("Mouse X");
            Vector3 rot = new Vector3(0, angleY * fieldSensitivity, 0);
            transform.rotation = Quaternion.Euler(transform.localEulerAngles + rot);
        }
        if (Input.GetAxisRaw("Mouse Y") != 0)
        {
            float angleX = Input.GetAxisRaw("Mouse Y");
            currentAngleX -= angleX;
            currentAngleX = Mathf.Clamp(currentAngleX, -fieldLookLimitX, fieldLookLimitX);
            tf_Cam.localEulerAngles = new Vector3(currentAngleX, 0, 0);
        }
    }

    public void ResetCam()
    {
        tf_Crosshair.localPosition = Vector3.zero;
        currentAngleX = 0;
        currentAngleY = 0;
    }

    private void NotCamUI()
    {
        notCamUp.SetActive(false);
        notCamDown.SetActive(false);
        notCamLeft.SetActive(false);
        notCamRight.SetActive(false);

        if (currentAngleY >= lookLimitX)
            notCamRight.SetActive(true);
        else if (currentAngleY <= -lookLimitX)
            notCamLeft.SetActive(true);

        if (currentAngleX <= -lookLimitY && tf_Cam.localPosition.y == originPosY + camBoundary.y)
            notCamUp.SetActive(true);
        else if (currentAngleX >= lookLimitY && tf_Cam.localPosition.y == originPosY - camBoundary.y)
            notCamDown.SetActive(true);
    }

    private void CameraLimit()
    {
        if (tf_Cam.localPosition.x >= camBoundary.x)
            tf_Cam.localPosition = new Vector3(camBoundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        else if (tf_Cam.localPosition.x <= camBoundary.x)
            tf_Cam.localPosition = new Vector3(-camBoundary.x, tf_Cam.localPosition.y, tf_Cam.localPosition.z);

        if (tf_Cam.localPosition.y >= originPosY + camBoundary.y)
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, originPosY + camBoundary.y, tf_Cam.localPosition.z);
        else if (tf_Cam.localPosition.y <= originPosY - camBoundary.y)
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, originPosY - camBoundary.y, tf_Cam.localPosition.z);
    }

    private void KeyViewMoving()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            currentAngleY += sightSensitivity * Input.GetAxisRaw("Horizontal");
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX);
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x + sightMoveSpeed * Input.GetAxisRaw("Horizontal"), tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            currentAngleX += sightSensitivity * -Input.GetAxisRaw("Vertical");
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, tf_Cam.localPosition.y + sightMoveSpeed * Input.GetAxisRaw("Vertical"), tf_Cam.localPosition.z);
        }

        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z);
    }

    private void ViewMoving()
    {
        if (tf_Crosshair.localPosition.x > (Screen.width / 2) - 100 || tf_Crosshair.localPosition.x < (-Screen.width / 2) + 100)
        {
            currentAngleY += (tf_Crosshair.localPosition.x > 0) ? sightSensitivity : -sightSensitivity;
            currentAngleY = Mathf.Clamp(currentAngleY, -lookLimitX, lookLimitX);

            float t_applySpeed = (tf_Crosshair.localPosition.x > 0) ? sightMoveSpeed : -sightMoveSpeed;
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x + t_applySpeed, tf_Cam.localPosition.y, tf_Cam.localPosition.z);
        }

        if (tf_Crosshair.localPosition.y > (Screen.height / 2) - 100 || tf_Crosshair.localPosition.y < (-Screen.height / 2) + 100)
        {
            currentAngleX += (tf_Crosshair.localPosition.y > 0) ? -sightSensitivity : sightSensitivity;
            currentAngleX = Mathf.Clamp(currentAngleX, -lookLimitY, lookLimitY);

            float t_applySpeed = (tf_Crosshair.localPosition.y > 0) ? sightMoveSpeed : -sightMoveSpeed;
            tf_Cam.localPosition = new Vector3(tf_Cam.localPosition.x, tf_Cam.localPosition.y + t_applySpeed, tf_Cam.localPosition.z);
        }

        tf_Cam.localEulerAngles = new Vector3(currentAngleX, currentAngleY, tf_Cam.localEulerAngles.z);
    }

    private void CrosshairMoving()
    {
        tf_Crosshair.localPosition = new Vector2(Input.mousePosition.x - (Screen.width / 2),
                                                 Input.mousePosition.y - (Screen.height / 2));

        float cursorPosX = tf_Crosshair.localPosition.x;
        float cursorPosY = tf_Crosshair.localPosition.y;

        cursorPosX = Mathf.Clamp(cursorPosX, (-Screen.width / 2) + 25, (Screen.width / 2) - 25);
        cursorPosY = Mathf.Clamp(cursorPosY, (-Screen.height / 2) + 25, (Screen.height / 2) - 25);

        tf_Crosshair.localPosition = new Vector2(cursorPosX, cursorPosY);
    }
}
