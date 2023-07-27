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
    [SerializeField] float maxSen;
    [SerializeField] float minSens;
    public float sensitivity;
    [SerializeField] float maxAngleFovX = 90;
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
    private bool _firsInputOnce = false;

    private void Awake()
    {
        Subcribe();
        PlayerScope.OnScoping += UpdateSens;
    }
    private void OnDestroy()
    {
        UnSubribe();
        PlayerScope.OnScoping -= UpdateSens;
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
            if (!_firsInputOnce) yRotation = _rangeYRotation; _firsInputOnce = true;
            yRotation = Mathf.Clamp(yRotation, _rangeYRotation - maxAngleFovX, _rangeYRotation + maxAngleFovX);

            xRotation -= (_deltaPosTouch.y * sensitivity);
            xRotation = Mathf.Clamp(xRotation, _rangeXRotation - maxAngleFovY, _rangeXRotation + maxAngleFovY);

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
    private void UpdateSens(float maxZoom, float minZoom, float current)
    {
        sensitivity = Mathf.Abs( minSens + (((current - maxZoom) * (maxSen - minSens)) / (maxZoom - minZoom)));
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
