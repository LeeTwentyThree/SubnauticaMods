using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ModStructureHelperPlugin.UI.Utility;

public class Vector3InputField : MonoBehaviour
{
    [SerializeField] private TMP_InputField xField;
    [SerializeField] private TMP_InputField yField;
    [SerializeField] private TMP_InputField zField;
    [SerializeField] private bool eulerAngles;

    public Vector3 Value { get; private set; }
    
    public UnityEvent onValueChanged;

    public void OnUpdateAny()
    {
        Value = GetValueFromFields();
        onValueChanged.Invoke();
    }

    public void SetValue(Vector3 value)
    {
        xField.text = value.x.ToString("0.#");
        yField.text = value.y.ToString("0.#");
        zField.text = value.z.ToString("0.#");
        onValueChanged.Invoke();
    }

    private Vector3 GetValueFromFields()
    {
        Vector3 newValue;
        if (float.TryParse(xField.text, out var x))
            newValue.x = x;
        else
            newValue.x = 0;
        if (float.TryParse(yField.text, out var y))
            newValue.y = y;
        else
            newValue.y = 0;
        if (float.TryParse(zField.text, out var z))
            newValue.z = z;
        else
            newValue.z = 0;
        
        if (!eulerAngles)
        {
            return newValue;
        }
        
        return new Vector3(newValue.x % 360, newValue.y % 360, newValue.z % 360);
    }
}