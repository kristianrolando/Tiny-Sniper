using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aldo.PubSub;

/// <summary>
/// handle movement and sight player
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Sight")]
    [SerializeField] float sensitivity = 20;
    [SerializeField] float maxAngelFovX = 90;
    [SerializeField] float maxAngleFovY = 60;

    [SerializeField] float _rangeXRotation;
    [SerializeField] float _rangeYRotation;
    float xRotation;
    float yRotation;

    Vector2 _deltaPosTouch;
    bool isInput;

    [Header("Bobbing Cam")]
    [SerializeField] float _bobbingRange = 2f;
    [SerializeField] float _bobbingSpeed = 5f;

    Vector3 _bobbing;
    bool isBobbing;
    bool isHoldBreath;

    bool isGameplayStart = false;
    public bool jusOnce = false;

    private void Awake()
    {
        Subcribe();
    }
    private void OnDestroy()
    {
        UnSubribe();
    }

    private void Start()
    {
        xRotation = transform.rotation.x;
        yRotation = transform.rotation.y;

        _rangeXRotation = transform.rotation.eulerAngles.x;
        _rangeYRotation = transform.rotation.eulerAngles.y;
        isHoldBreath = false;
    }
    private void Update()
    {
        if (!isGameplayStart)
            return;
        if(!isHoldBreath)
            BobbingCam();
        TouchInput();
        //StandAloneInput();
    }

    private void StandAloneInput()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

        if (!GameplayScene.Instance.isUiInteract)
        {
            if (mouseX != 0 || mouseY != 0)
            {
                yRotation += mouseX;
                yRotation = Mathf.Clamp(yRotation, _rangeYRotation - maxAngleFovY, _rangeYRotation + maxAngelFovX);

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, _rangeXRotation - maxAngelFovX, _rangeXRotation + maxAngleFovY);

                transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            }
        }
    }

    private void TouchInput()
    {
        if(!GameplayScene.Instance.isUiInteract && !jusOnce)
        {
            jusOnce = true;
            return;
        }
        if(Input.touchCount > 0 && !GameplayScene.Instance.isUiInteract)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                touch.deltaPosition = Vector2.zero;
                _deltaPosTouch = touch.deltaPosition;
            }
            if(touch.phase == TouchPhase.Moved)
            {
                _deltaPosTouch = touch.deltaPosition;
            }
            if(touch.phase == TouchPhase.Stationary)
            {
                touch.deltaPosition = Vector2.zero;
                _deltaPosTouch = touch.deltaPosition;
            }

            yRotation += (_deltaPosTouch.x * sensitivity);
            yRotation = Mathf.Clamp(yRotation, _rangeYRotation - maxAngleFovY, _rangeYRotation + maxAngelFovX);

            xRotation -= (_deltaPosTouch.y * sensitivity);
            xRotation = Mathf.Clamp(xRotation, _rangeXRotation - maxAngelFovX, _rangeXRotation + maxAngleFovY);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

            isInput = true;
            isBobbing = false;
        }
        else
        {
            isInput = false;
        }
    }

    private void BobbingCam()
    {
        if (isInput)
            return;
        if(!isBobbing)
         SetRandomStability();

        Quaternion _target = Quaternion.Euler(_bobbing.x, _bobbing.y, _bobbing.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _target, Time.deltaTime * _bobbingSpeed);
        if (transform.rotation == _target)
        {
            isBobbing = false;
            SetRandomStability();
        }
    }

    private void SetRandomStability()
    {
        float x = Random.Range(transform.rotation.eulerAngles.x - _bobbingRange, transform.rotation.eulerAngles.x + _bobbingRange);
        float y = Random.Range(transform.rotation.eulerAngles.y - _bobbingRange, transform.rotation.eulerAngles.y + _bobbingRange);
        _bobbing = new Vector3(x, y, 0);
        isBobbing = true;
    }

    #region PubSub

    private void ReceiveMessageGameplayStart(MessageGameplayStart message)
    {
        isGameplayStart = true;
    }
    private void ReceiveMessageHoldBreath(MessageHoldBreath message)
    {
        isHoldBreath = message.holdBreath;
        isBobbing = false;
    }
    private void Subcribe()
    {
        PublishSubscribe.Instance.Subscribe<MessageGameplayStart>(ReceiveMessageGameplayStart);
        PublishSubscribe.Instance.Subscribe<MessageHoldBreath>(ReceiveMessageHoldBreath);
    }
    private void UnSubribe()
    {
        PublishSubscribe.Instance.Unsubscribe<MessageGameplayStart>(ReceiveMessageGameplayStart);
        PublishSubscribe.Instance.Unsubscribe<MessageHoldBreath>(ReceiveMessageHoldBreath);
    }
    #endregion
}
