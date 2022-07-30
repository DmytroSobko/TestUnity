using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

[RequireComponent(typeof(TMP_Dropdown))]
public class PoolingSystemDropdown : MonoBehaviour
{
    public Type SelectedValue { get; private set; }

    private List<Type> dropdownTypeOptions = new List<Type>();

    private TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    private void OnEnable()
    {
        dropdown.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnDisable()
    {
        dropdown.onValueChanged.RemoveListener(OnValueChanged);
    }

    public void Init(List<ObjectPool> objectPools)
    {
        if (objectPools.Count > 0)
        {
            SelectedValue = objectPools.First().PoolableObjectType;

            foreach (ObjectPool objectPool in objectPools)
            {
                var optionData = new TMP_Dropdown.OptionData(objectPool.PoolableObjectType.ToString());
                dropdown.options.Add(optionData);
                dropdownTypeOptions.Add(objectPool.PoolableObjectType);
            }

            dropdown.RefreshShownValue();
        }
    }

    private void OnValueChanged(int value)
    {
        SelectedValue = dropdownTypeOptions[value];
    }
}