using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndProgressUI : MonoBehaviour
{
    public Text txtTime, txtScore, txtKetQua;

    public Transform root;
    public ResultCard resultCardPrefab;
    public void Init(float _time, float _score)
    {
        gameObject.SetActive(true);
        txtTime.text = $"Time: {(int)_time}s";
        txtScore.text = $"Score: {_score}";
        txtKetQua.text = $"Result: {(_score >= 5 ? "PASS" : "FAIL")} ";

        foreach (var t in ScoreMechanic.Instance.listError)
        {
            if(t.Value > 0)
            {
                ResultCard card = Instantiate(resultCardPrefab, root);
                card.Init(t.Key, t.Value.ToString());
                card.gameObject.SetActive(true);
            }
            
        }
    }
}
