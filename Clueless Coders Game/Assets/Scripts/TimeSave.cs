using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TimeSave : MonoBehaviour
{
    public Text leaderboardText;

    // Method to save the elapsed time
    public void SaveTime(float elapsedTime)
    {
        string path = "Assets/Leaderboard.txt";
        using (StreamWriter writer = new StreamWriter(path, true))
        {
            writer.WriteLine(elapsedTime);
        }
        DisplayLeaderboard();
    }

    // Method to read and sort times from the file
    List<float> ReadTimesFromFile()
    {
        string path = "Assets/Leaderboard.txt";
        List<float> times = new List<float>();

        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                if (float.TryParse(line, out float time))
                {
                    times.Add(time);
                }
            }
        }

        return times.OrderBy(t => t).ToList();
    }

    // Method to display the leaderboard
    void DisplayLeaderboard()
    {
        List<float> times = ReadTimesFromFile();
        if (leaderboardText != null)
        {
            leaderboardText.text = "Leaderboard:\n";
            for (int i = 0; i < times.Count; i++)
            {
                leaderboardText.text += $"{i + 1}. {times[i]:F2} seconds\n";
            }
        }
    }
}
