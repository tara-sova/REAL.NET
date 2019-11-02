package com.example.polina.adminapp;

import android.app.Activity;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;



import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import com.google.gson.Gson;


@AnnotationList.OnSwipeRightTO(featureNameFrom = "LectureListActivity")
@AnnotationList.AbstractFeature(abstractFatureName = "EventManipulations")
@AnnotationList.Feature(featureName = "LectureDeletion")
@AnnotationList.OrGroup(groupName = "1")

@AnnotationList.XorAbstractGroup(groupName = "1")

public class LectureDeletion extends AppCompatActivity {

    @AnnotationList.InComingArg(convertedClass = String.class)
    int position;

    @AnnotationList.InComingArg(convertedClass = Lecture.class)
    String selectedItemAsAString;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Gson gson = new Gson();
        Intent incomingIntent = getIntent();
        position = incomingIntent.getExtras().getInt("position");
        selectedItemAsAString = incomingIntent.getStringExtra("selectedItemAsAString");
        Intent outComingIntent = new Intent(LectureDeletion.this, LectureListActivity.class);
        outComingIntent.putExtra("position", position);
        outComingIntent.putExtra("selectedItemAsAString", selectedItemAsAString);
        setResult(Activity.RESULT_OK, outComingIntent);
        finish();
    }
}
