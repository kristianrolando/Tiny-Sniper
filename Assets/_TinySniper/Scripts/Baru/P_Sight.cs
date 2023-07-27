using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Sight
{
    private Transform _playerBody;

    private float _maxSens;
    private float _minSens;
    private float _sensitivity;
    private float _maxAngleFovX;
    private float _maxAngleFovY;

    private float _initXrotation;
    private float _initYrotation;

    private float xRotation;
    private float yRotation;

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
    private bool _firstInputOnce = false;

    internal void Initial(Transform playerBody, float maxSens, float minSens, float maxAngleFovX, float maxAngleFovY, float initXrotation, float initYrotation)
    {
        _playerBody = playerBody;
        _maxSens = maxSens;
        _minSens = minSens;
        _maxAngleFovX = maxAngleFovX;
        _maxAngleFovY = maxAngleFovY;
        _initXrotation = initXrotation;
        _initYrotation = initYrotation;

        _sensitivity = _minSens;
    }

    
    internal void TouchInput()
    {
        //if (!GameplayScene.Instance.isUiInteract && !jusOnce)
        //{
        //    jusOnce = true;
        //    return;
        //}
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touch.deltaPosition = Vector2.zero;
                _deltaPosTouch = touch.deltaPosition;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                _deltaPosTouch = touch.deltaPosition;
            }
            if (touch.phase == TouchPhase.Stationary)
            {
                touch.deltaPosition = Vector2.zero;
                _deltaPosTouch = touch.deltaPosition;
            }

            yRotation += (_deltaPosTouch.x * _sensitivity);
            if (!_firstInputOnce) yRotation = _initYrotation; _firstInputOnce = true;
            yRotation = Mathf.Clamp(yRotation, _initYrotation - _maxAngleFovX, _initYrotation + _maxAngleFovX);

            xRotation -= (_deltaPosTouch.y * _sensitivity);
            xRotation = Mathf.Clamp(xRotation, _initXrotation - _maxAngleFovY, _initXrotation + _maxAngleFovY);

            _playerBody.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            isInput = true;
            isBobbing = false;
        }
        else
        {
            isInput = false;
        }
    }

    internal void BobbingCam()
    {
        if (isInput)
            return;
        if (!isBobbing)
            SetRandomStability();

        Quaternion _target = Quaternion.Euler(_bobbing.x, _bobbing.y, _bobbing.z);
        _playerBody.rotation = Quaternion.RotateTowards(_playerBody.rotation, _target, Time.deltaTime * _bobbingSpeed);
        if (_playerBody.rotation == _target)
        {
            isBobbing = false;
            SetRandomStability();
        }
    }

    private void SetRandomStability()
    {
        float x = Random.Range(_playerBody.rotation.eulerAngles.x - _bobbingRange, _playerBody.rotation.eulerAngles.x + _bobbingRange);
        float y = Random.Range(_playerBody.rotation.eulerAngles.y - _bobbingRange, _playerBody.rotation.eulerAngles.y + _bobbingRange);
        _bobbing = new Vector3(x, y, 0);
        isBobbing = true;
    }
}
