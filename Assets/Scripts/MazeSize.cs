using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MazeSize : MonoBehaviour
{

    [SerializeField]
    private Text _sliderText;

    private Slider _slider;

    void Start()
    {
        _slider = GetComponent<Slider>();
        _sliderText.text = _slider.minValue.ToString(CultureInfo.CurrentCulture);
    }

    public void SetValue(float value)
    {
        _sliderText.text = value.ToString(CultureInfo.CurrentCulture);
    }
}
