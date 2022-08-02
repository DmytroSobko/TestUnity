using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

[RequireComponent(typeof(TMP_Dropdown))]
public class PoolingSystemDropdown : MonoBehaviour
{
    public Type SelectedValue { get; private set; }

    private TMP_Dropdown dropdown;
    private List<Type> dropdownTypeOptions = new List<Type>();

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

    public void Init(List<PoolData> poolsData)
    {
        if (poolsData.Count > 0)
        {
            SelectedValue = poolsData.First().PoolableObject.GetType();

            foreach (PoolData poolData in poolsData)
            {
                Type poolableObjectType = poolData.PoolableObject.GetType();
                var optionData = new TMP_Dropdown.OptionData(poolableObjectType.ToString());
                dropdown.options.Add(optionData);
                dropdownTypeOptions.Add(poolableObjectType);
            }

            dropdown.RefreshShownValue();
        }
    }

    private void OnValueChanged(int value)
    {
        SelectedValue = dropdownTypeOptions[value];
    }
}