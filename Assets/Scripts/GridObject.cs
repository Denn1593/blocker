using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class GridObject : MonoBehaviour{
    protected Grid grid;
    public int x;
    public int y;

    // Start is called before the first frame update
    void Start(){
        grid = transform.parent.gameObject.GetComponent<Grid>();

        x = (int) Math.Floor((transform.position.x - grid.gridSize / 2) / grid.gridSize);
        y = (int) Math.Floor((transform.position.y - grid.gridSize / 2) / grid.gridSize);

        transform.position = new Vector3(
            x * grid.gridSize,
            y * grid.gridSize
        );
    }

    public abstract void onEvent(GridEvent e);
}