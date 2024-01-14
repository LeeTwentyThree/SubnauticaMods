using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheRedPlague.Mono;

public class DeathCountdownUI : MonoBehaviour
{
    private TextMeshProUGUI _timerText;

    public static DeathCountdownUI Create()
    {
        var sunbeamCountdown = uGUI_SunbeamCountdown.main;
        var go = Instantiate(sunbeamCountdown.gameObject);
        go.name = "DeathCountdownUI";
        var rt = go.GetComponent<RectTransform>();
        rt.SetParent(sunbeamCountdown.transform.parent.GetComponent<RectTransform>(), true);
        rt.localPosition = Vector3.zero;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.position = new Vector3(0, 0, 1);
        rt.localScale = Vector3.one;
        go.transform.GetChild(0).gameObject.SetActive(true);
        go.transform.GetChild(0).localPosition = new Vector3(930, -400);
        go.transform.GetChild(0).GetComponent<Image>().color = Color.red;
        DestroyImmediate(go.GetComponent<uGUI_SunbeamCountdown>());
        var ui = go.AddComponent<DeathCountdownUI>();
        go.transform.Find("Background/TimerTitle").GetComponent<TextMeshProUGUI>().text = "Fatal Symptoms In:";
        ui._timerText = go.transform.Find("Background/Timer").GetComponent<TextMeshProUGUI>();
        return ui;
    }

    public void UpdateDisplay(int remainingSeconds)
    {
        var seconds = (remainingSeconds % 60).ToString();
        if (seconds.Length == 1) seconds = 0 + seconds;
        _timerText.text = $"{remainingSeconds / 60}:{seconds}";
    }
}