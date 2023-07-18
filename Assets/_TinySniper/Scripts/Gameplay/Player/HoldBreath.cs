using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aldo.PubSub;

/// <summary>
/// Handle hold breath for stability weapon when aim target
/// </summary>
public class HoldBreath : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] GameplayScene UI;

    public float timerHold = 5f;
    public float timerBreath = 10f;

    [HideInInspector]
    public bool isBreath;
    [HideInInspector]
    public bool isHoldBreath;
    float time;
    bool isGameplayStart = false;

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
        time = 0;
        isBreath = true;
        isHoldBreath = false;
        UI._holdBreathButton.interactable = false;
    }

    private void Update()
    {
        if (!isGameplayStart)
            return;

        if(isBreath)
        {
            IncreaseBar();
        }
        else if(isHoldBreath)
        {
            DecreaseBar();
        }

    }
    private void IncreaseBar()
    {
        time += Time.deltaTime;
        float value = (time / timerBreath) * UI._breatheBar.maxValue;
        UpdateBreatheBar(value);
        if (UI._breatheBar.value >= UI._breatheBar.maxValue)
        {
            isBreath = false;
            time = timerHold;
            UI._holdBreathButton.interactable = true;
        }
    }
    private void DecreaseBar()
    {
        time -= Time.deltaTime;
        float value = (time / timerHold) * UI._breatheBar.maxValue;
        UpdateBreatheBar(value);
        if(UI._breatheBar.value <= 0)
        {
            isHoldBreath = false;
            time = 0;
            isBreath = true;
            PublishSubscribe.Instance.Publish<MessageHoldBreath>(new MessageHoldBreath(false));
        }
    }

    private void UpdateBreatheBar(float value)
    {
        UI.UpdateBreatheBar(value);
        if (UI._breatheBar.value > UI._breatheBar.maxValue)
            UI._breatheBar.value = UI._breatheBar.maxValue;
        if (UI._breatheBar.value < 0)
            UI._breatheBar.value = 0;
    }

    public void HoldButton()
    {
        isHoldBreath = true;
        isBreath = false;
        UI._breatheBar.value = 0;
        UI._holdBreathButton.interactable = false;
        PublishSubscribe.Instance.Publish<MessageHoldBreath>(new MessageHoldBreath(true));
    }

    #region PubSub

    private void ReceiveMessageGameplayStart(MessageGameplayStart message)
    {
        isGameplayStart = true;
    }
    private void Subcribe()
    {
        PublishSubscribe.Instance.Subscribe<MessageGameplayStart>(ReceiveMessageGameplayStart);
    }
    private void UnSubribe()
    {
        PublishSubscribe.Instance.Unsubscribe<MessageGameplayStart>(ReceiveMessageGameplayStart);
    }
    #endregion
}
