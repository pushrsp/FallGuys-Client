using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{

    private int _order = 10;
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Helper.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        
        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }
}
