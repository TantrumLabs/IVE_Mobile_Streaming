using UnityEngine;
using System.Collections;

public class UIManager : Singleton<UIManager>
{
    new void Awake()
    {
        base.Awake();
    }
    /// <summary>
    /// Toggles the GUI menu's vivibility
    /// </summary>
    public void ToggleMenuShow()
    {
        StopCoroutine(ToggleMenu());
        StartCoroutine(ToggleMenu());
    }

    IEnumerator ToggleMenu()
    {
        if (!MenuBar)
            StopCoroutine(ToggleMenu());

        // Menu is showing, so we need to hide it
        if(menuShow)
        {
            while(MenuBar.anchorMin.y <= 1)
            {
                float speed = AnimationSpeed * Time.deltaTime;

                Vector2 newMin = new Vector2(0, speed);
                Vector2 newMax = new Vector2(0, speed);

                MenuBar.anchorMin += newMin;
                MenuBar.anchorMax += newMax;
                
                yield return null;
            }

            MenuBar.anchorMin = new Vector2(0.0f, 1.0f);
            MenuBar.anchorMax = new Vector2(1.0f, 1.1f);

        }

        // Menu is hidden, so we need to show it
        else if(!menuShow)
        {
            while(MenuBar.anchorMax.y >= 1)
            {
                float speed = AnimationSpeed * Time.deltaTime;

                Vector2 newMin = new Vector2(0, speed);
                Vector2 newMax = new Vector2(0, speed);

                MenuBar.anchorMin -= newMin;
                MenuBar.anchorMax -= newMax;

                yield return null;
            }

        MenuBar.anchorMin = new Vector2(0.0f, 0.9f);
        MenuBar.anchorMax = new Vector2(1.0f, 1.0f);
    }

        menuShow = !menuShow;
    }

    public RectTransform MenuBar;

    public float AnimationSpeed = 1;

    private bool menuShow = false;
}
