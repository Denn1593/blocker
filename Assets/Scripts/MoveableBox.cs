using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxEvent{
    FINALIZED_TURN,
    FINISHED_ROUND,
};

public enum BoxState{
    UNDECIDED,
    DECIDED,
    ANIMATING,
}

public class MoveableBox : GridObject{
    public BoxState state = BoxState.UNDECIDED;
    public override void onEvent(GridEvent e){
        if(e is MoveEvent){
            if(state == BoxState.DECIDED){
                return;
            }

            MoveEvent me = (MoveEvent) e;
            Debug.Log(me.direction);
            if(me.direction == Direction.LEFT){
                ProcessMoveOrder(x-1, y);
            }
        }

        if(e is AnimateEvent){
            transform.position = new Vector3(
                x * grid.gridSize,
                y * grid.gridSize
            );
        }

        if(e is ResetEvent){
            state = BoxState.UNDECIDED;
        }
    }

    void ProcessMoveOrder(int mx, int my){
        MoveableBox blockingBox = (MoveableBox) grid.getObjectAt(x, y, typeof(MoveableBox));

        Debug.Log(blockingBox);

        if(blockingBox.state == BoxState.UNDECIDED){
            grid.onEvent(BoxEvent.FINISHED_ROUND);
            return;
        }
        if(blockingBox.state == BoxState.DECIDED){
            state = BoxState.DECIDED;
            grid.onEvent(BoxEvent.FINALIZED_TURN);
            return;
        }
        if(blockingBox == null){
            x = mx;
            y = my;
            state = BoxState.DECIDED;
            grid.onEvent(BoxEvent.FINALIZED_TURN);
            return;
        }
        grid.onEvent(BoxEvent.FINISHED_ROUND);
    }

    // Update is called once per frame
    void Update(){
    }

   
}
