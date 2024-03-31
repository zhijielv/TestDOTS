using System;
using System.Diagnostics;
using Behaviour;
using UnityEngine;
using UnityEngine.UI;


public class TestButton : MonoBehaviour
{
    public TestStart TestStart;
    public InputField InputField;
    public Button ButtonStart;
    public Button ButtonRelease;
    public Text Text;
    public Text TimeText;

    private void Start()
    {
        var sw = new Stopwatch();
        ButtonStart.onClick.AddListener(() =>
        {
            TestStart.ReleaseCube();
            sw.Start();
            if (InputField.text.Equals(""))
                InputField.text = "1";
            int value = Convert.ToInt32(InputField.text);
            TestStart.count = value;
            TestStart.StartCube();
            Text.text = (value * value).ToString();
            sw.Stop();
            TimeText.text = sw.Elapsed.ToString();
        });

        ButtonRelease.onClick.AddListener((() =>
        {
            TestStart.ReleaseCube();
            Text.text = "0";
        }));
    }
}