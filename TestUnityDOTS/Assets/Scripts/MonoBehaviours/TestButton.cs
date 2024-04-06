using System;
using System.Collections;
using System.Diagnostics;
using Components;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    public InputField InputField;
    public Button ButtonStart;
    public Button ButtonRelease;
    public Text Text;
    public Text TimeText;
    private InstantiatePrefabSystem _instantiatePrefabSystem;
    private DestroyPrefabSystem _destroyPrefabSystem;
    public static int Count;
    public static Stopwatch Stopwatch;

    private void Start()
    {
        Stopwatch = new Stopwatch();
        _instantiatePrefabSystem =
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<InstantiatePrefabSystem>();
        _destroyPrefabSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<DestroyPrefabSystem>();

        ButtonStart.onClick.AddListener(OnStartBtnClick);

        ButtonRelease.onClick.AddListener(OnReleaseBtnClick);
    }

    private void OnStartBtnClick()
    {
        ReleaseAllPrefab();
        StartCoroutine(CreatePrefab());
    }

    private IEnumerator CreatePrefab()
    {
        yield return new WaitForNextFrameUnit();
        _destroyPrefabSystem.Enabled = false;
        _instantiatePrefabSystem.Enabled = true;

        if (InputField.text.Equals("")) InputField.text = "1";
        var value = Convert.ToInt32(InputField.text);
        Count = value;
        Text.text = (value * value).ToString();
    }

    private void OnReleaseBtnClick()
    {
        ReleaseAllPrefab();
    }

    private void ReleaseAllPrefab()
    {
        _instantiatePrefabSystem.Enabled = false;
        _destroyPrefabSystem.Enabled = true;
        Text.text = "0";
    }

    private void Update()
    {
        TimeText.text = Stopwatch.Elapsed.ToString();
    }
}