using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarRating : MonoBehaviour
{
    public Image[] stars;

    public Sprite emptyStar;

    public Sprite filledStar;

    private int currentRating = 0;

    void Start()
    {
        UpdateStars();
    }

    public void SetRating(int rating)
    {
        currentRating = rating;
        UpdateStars();
    }

    public int GetRating()
    {
        return currentRating;
    }
    
    private void UpdateStars()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < currentRating)
            {
                stars[i].sprite = filledStar;
            }
            else
            {
                stars[i].sprite = emptyStar;
            }
        }
    }
    
    public void OnStarClick(int starIndex)
    {
        SetRating(starIndex + 1);
    }
}
