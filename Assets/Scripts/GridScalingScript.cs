using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GridScalingScript : MonoBehaviour
{
    [SerializeField]
    private GridLayoutGroup grid;
    [SerializeField]
    private int VerticalPadding;
    [SerializeField]
    private int HorizontalPadding;

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        grid.padding.left = HorizontalPadding;
        grid.padding.right = HorizontalPadding;
        grid.padding.top = VerticalPadding;
        grid.padding.bottom = VerticalPadding;
        grid.cellSize= new Vector2((Screen.width - HorizontalPadding * 2 - grid.spacing.x*2) / 3,(Screen.height-VerticalPadding*2-grid.spacing.y)/2);
    }
}
