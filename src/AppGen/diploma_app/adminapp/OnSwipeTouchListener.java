package com.example.polina.adminapp;

import android.content.Context;
import android.view.GestureDetector;
import android.view.MotionEvent;
import android.view.View;
import android.widget.ListView;

@AnnotationList.NeededAnywayFeatureFile(featureName = "LectureListActivity")
public class OnSwipeTouchListener implements View.OnTouchListener {

    ListView list;
    private GestureDetector gestureDetector;
    private Context context;

    public OnSwipeTouchListener(Context ctx, ListView list) {
        gestureDetector = new GestureDetector(ctx, new GestureListener());
        context = ctx;
        this.list = list;
    }

    public OnSwipeTouchListener() {
        super();
    }

    @Override
    public boolean onTouch(View v, MotionEvent event) {
        gestureDetector.onTouchEvent(event);
        return true;
    }

    private void onSwipeRight(int pos) {
        ((LectureListActivity) this.context).onSwipeRight(pos);
    }

    private void onSwipeLeft(int pos) {
        // Not implemented, possible usage: see lections details
    }

    private boolean onLongItemClickAction(int position) {
        LectureListActivity cont = ((LectureListActivity) this.context);
        cont.onItemLongClick(cont.getListView(), cont.getListView(), position, cont.getTaskId());
        return true;
    }

    private boolean onItemClickAction(int position) {
        LectureListActivity cont = ((LectureListActivity) this.context);
        cont.onListItemClick(cont.getListView(), cont.getListView(), position, cont.getTaskId());
        return true;
    }

    private final class GestureListener extends GestureDetector.SimpleOnGestureListener {

        private static final int SWIPE_THRESHOLD = 100;
        private static final int SWIPE_VELOCITY_THRESHOLD = 100;

        @Override
        public boolean onDown(MotionEvent e) {
            return false;
        }

        private int getPostion(MotionEvent e1) {
            return list.pointToPosition((int) e1.getX(), (int) e1.getY());
        }

        @Override
        public void onLongPress(MotionEvent e) {
            onLongItemClickAction(getPostion(e));
        }

        @Override
        public boolean onSingleTapUp(MotionEvent e) {
            onItemClickAction(getPostion(e));
            return true;
        }

        @Override
        public boolean onFling(MotionEvent e1, MotionEvent e2,
                               float velocityX, float velocityY) {
            float distanceX = e2.getX() - e1.getX();
            float distanceY = e2.getY() - e1.getY();
            if (Math.abs(distanceX) > Math.abs(distanceY)
                    && Math.abs(distanceX) > SWIPE_THRESHOLD
                    && Math.abs(velocityX) > SWIPE_VELOCITY_THRESHOLD) {
                if (distanceX > 0)
                    onSwipeRight(getPostion(e1));
                else
                    onSwipeLeft(getPostion(e1));
                return true;
            }
            return false;
        }

    }
}