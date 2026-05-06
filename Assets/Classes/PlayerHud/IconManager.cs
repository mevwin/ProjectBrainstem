using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    public List<Image> icons = new();
    public int currentIndex = 0;

    public void SetIcon(JobManager.Job job)
    {
        icons[currentIndex].gameObject.SetActive(false);
        currentIndex = (int) job;
        icons[currentIndex].gameObject.SetActive(true);
    }
}
