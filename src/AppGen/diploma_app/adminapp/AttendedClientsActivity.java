package com.example.polina.adminapp;

import android.app.ListActivity;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;

import android.os.Bundle;
import android.widget.ArrayAdapter;

import com.google.gson.Gson;

@AnnotationList.AbstractFeature(abstractFatureName = "EventInfo")
@AnnotationList.Feature(featureName = "AttendedClients")
public class AttendedClientsActivity extends ListActivity {

    @AnnotationList.OnItemClickTO(featureNameFrom = "LectureListActivity")
    public String link;

    public ArrayAdapter<String> mAdapter;

    @AnnotationList.InComingArg(convertedClass = Lecture.class)
    private String lectureAsAString;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        Gson gson = new Gson();
        Intent intent = getIntent();
        lectureAsAString = intent.getStringExtra("lectureAsAString");
        Lecture lecture = gson.fromJson(lectureAsAString, Lecture.class);

        mAdapter = new ArrayAdapter<>(this,
                android.R.layout.simple_list_item_1, lecture.attentedClients);

        setListAdapter(mAdapter);
    }
}
