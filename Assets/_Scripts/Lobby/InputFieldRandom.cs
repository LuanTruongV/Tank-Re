using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InputFieldRandom : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private string _text;
    private void OnEnable()
    {
        _inputField.placeholder.GetComponent<TMP_Text>().text = RandomName();
    }

    private string RandomName()
    {
        return _text+$" {Random.Range(1000, 10000)}";
    }
}
