using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultDisplay : Singleton<ResultDisplay>
{
    public Text info, timeLast, totalTime, countPracticed, countCompleted, needComplete, averageScore;
    public void OnEnable()
    {
        var inf = ApiFromCms.Instance.info.objectData;
        info.text = inf.fullName;
        timeLast.text = inf.latestDate.ToString();
        totalTime.text = inf.totalTime.ToString();
        countPracticed.text = "" + inf.totalPracticed;
        countCompleted.text = "" + inf.totalCompleted;
        needComplete.text = "" + (inf.totalPracticed - inf.totalCompleted);

        double total = 0;
        foreach (var item in inf.avgScoreByPractices)
            total += item.point;

        double avg = (double)total / inf.avgScoreByPractices.Count;
        averageScore.text = "" + Math.Round(avg,2);
    }
}
