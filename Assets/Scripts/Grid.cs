using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction{
    LEFT,
    RIGHT,
    UP,
    DOWN,
}

public class GridEvent{
}

public class MoveEvent : GridEvent{
    public Direction direction;

    public MoveEvent(Direction direction){
        this.direction = direction;
    }
}

public class ResetEvent : GridEvent{
}
public class AnimateEvent : GridEvent{
}

public class Grid : MonoBehaviour{
    public float gridSize;
    private List<GridObject> objects;
    private int boxCount;

    private int finalizedCount;
    private int processedInTurnCount;

    private MoveEvent currentMoveEvent;

    // Start is called before the first frame update
    void Start(){
        objects = new List<GridObject>();

        foreach(Transform child in transform){
            GridObject obj = child.gameObject.GetComponent<GridObject>();
            if(obj != null){
                objects.Add(obj);
            }
            if(obj is MoveableBox){
                boxCount++;
            }
        }
    }

    private void distributeEvent(GridEvent e){
        foreach(MoveableBox box in objects){
            box.onEvent(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0){
            finalizedCount = 0;
            currentMoveEvent = new MoveEvent(Direction.LEFT);
            distributeEvent(currentMoveEvent);
        }
    }

    public void onEvent(BoxEvent e){
        if(e == BoxEvent.FINALIZED_TURN){
            finalizedCount++;
        }
        if(processedInTurnCount == boxCount){
            if(finalizedCount == boxCount){
                finalizedCount = 0;
                distributeEvent(new AnimateEvent());
            }
            else {
                processedInTurnCount = 0;
                distributeEvent(currentMoveEvent);
            }
        }
    }

    public GridObject getObjectAt(int x, int y, Type type){
        return objects.Find(obj => obj.x == x && obj.y == y && obj.GetType() == type);
    }
}
