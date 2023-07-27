using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Aldo.PubSub;

public class GameplayScene : MonoBehaviour
{
    public static GameplayScene Instance;

    [Header("PlayerShoot")]
    public Button _shootButton;
    public Button _reloadButton;
    public Slider _scopeSlider;
    public TextMeshProUGUI _currentBulletText;
    public TextMeshProUGUI _maxBulletText;
    public GameObject _reloadNotifObj;
    public GameObject _arlertNotifObj;

    [Header("HoldBreath")]
    public Button _holdBreathButton;
    public Slider _breatheBar;

    [Header("GameOver")]
    public TextMeshProUGUI _winText;

    [Header("Refrences")]
    [SerializeField] private PlayerScope scope;
    [SerializeField] private PlayerShoot shoot;
    [SerializeField] private HoldBreath breath;
    [SerializeField] private PlayerMovement sight;

    [HideInInspector]
    public bool isUiInteract;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        _reloadNotifObj.SetActive(false);
        _arlertNotifObj.SetActive(false);
    }

    private void Start()
    {
        _scopeSlider.value = 0;
        _breatheBar.value = 0;
    }

    #region PlayerShoot
    public void UpdateBulletText(int maxBullet, int currentBullet)
    {
        _currentBulletText.text = currentBullet.ToString();
        _maxBulletText.text = maxBullet.ToString();
    }
    public void ShootButton()
    {
        shoot.Shoot();
    }
    #endregion

    #region PlayerScope
    public void UpdateScopeZoom()
    {
        float percent = _scopeSlider.value / _scopeSlider.maxValue;
        float FOV = scope.minZoom - ((scope.minZoom - scope.maxZoom) * percent);
        if (FOV < scope.minZoom)
            scope.OnScoped(FOV);
        else
            scope.OnUnscoped();
    }

    #endregion

    #region HoldBreath
    public void UpdateBreatheBar(float value)
    {
        _breatheBar.value = value;
    }
    public void HoldBreathButton()
    {
        breath.HoldButton();
    }

    #endregion

    public void StartButton()
    {
        PublishSubscribe.Instance.Publish<MessageStartButtonPressed>(new MessageStartButtonPressed());
    }

    public void UiInteract(bool value)
    {
        isUiInteract = value;
        if (!value)
            sight.jusOnce = false;
    }
}
