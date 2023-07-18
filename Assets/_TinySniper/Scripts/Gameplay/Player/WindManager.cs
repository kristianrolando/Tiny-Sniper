using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    public float winDelta = 0.1f;
    public float windUpdateTime = 0.5f;
    public float windMaxMagnitude = 10f;
    public float windStartMaxMagnitude = 3f;

    [SerializeField] Vector2 wind;
    Vector2 windUpdated;
    float time;

    
    private void Start()
    {
        SetWind(Random.insideUnitCircle * windStartMaxMagnitude);
    }
    private void Update()
    {
        UpdateWind();
        time += Time.deltaTime;
        
        if(time > windUpdateTime)
        {
            time = 0f;
            windUpdated = GetUpdatedWind();
        }

    }
    public Vector2 GetWind()
    {
        return wind;
    }

    public void SetWind(Vector2 newWind)
    {
        wind = newWind;
        windUpdated = wind;
    }

    private void UpdateWind()
    {
        wind = Vector2.Lerp(wind, windUpdated, Time.deltaTime);
    }
    private Vector2 GetUpdatedWind()
    {
        Vector2 result = wind + Random.insideUnitCircle * winDelta;
        result = result.normalized * Mathf.Min(result.magnitude, windMaxMagnitude);
        return result;
    }
}
