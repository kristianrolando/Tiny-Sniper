using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _playerBody;

    [Header("Sight")]
    [SerializeField] private float maxSens;
    [SerializeField] private float minSens;
    [SerializeField] private float maxAngleFovX;
    [SerializeField] private float maxAngleFovY;
    [SerializeField] private float initXrotation;
    [SerializeField] private float initYrotation;

    P_Sight Sight = new P_Sight();

    private void Awake()
    {
        Initial();
    }
    private void Initial()
    {
        Sight.Initial(_playerBody, maxSens, minSens, maxAngleFovX, maxAngleFovY, initXrotation, initYrotation);
    }


    private void Update()
    {
        Sight.TouchInput();
    }
}
