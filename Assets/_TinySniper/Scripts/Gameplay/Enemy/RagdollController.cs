using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController
{
    [SerializeField] GameObject mainBody;
    [SerializeField] GameObject rigBodyParent;

    private Collider _mainCollider;
    private Rigidbody _mainRb;
    private Collider[] _rigCollider;
    private Rigidbody[] _rigRb;

    internal void Initial(GameObject mainBody, GameObject rigBodyParent)
    {
        this.mainBody = mainBody;
        this.rigBodyParent = rigBodyParent;

        GetAllComponent();
    }

    private void GetAllComponent()
    {
        _mainCollider = mainBody.GetComponent<Collider>();
        _mainRb = mainBody.GetComponent<Rigidbody>();

        _rigCollider = rigBodyParent.GetComponentsInChildren<Collider>();
        _rigRb = rigBodyParent.GetComponentsInChildren<Rigidbody>();
    }

    internal void EnableRagdollMode()
    {
        foreach(Collider col in _rigCollider)
        {
            col.enabled = true;
        }
        foreach(Rigidbody rb in _rigRb)
        {
            rb.isKinematic = false;
        }

        _mainCollider.enabled = false;
        _mainRb.isKinematic = true;
    }
    internal void DisableRagdollMode()
    {
        foreach (Collider col in _rigCollider)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rb in _rigRb)
        {
            rb.isKinematic = true;
        }

        _mainCollider.enabled = true;
        _mainRb.isKinematic = false;
    }
}
