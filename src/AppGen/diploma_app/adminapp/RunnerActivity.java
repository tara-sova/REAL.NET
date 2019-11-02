package com.example.polina.adminapp;

import android.content.Intent;
import android.os.Build;
import android.support.annotation.RequiresApi;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

public class RunnerActivity extends AppCompatActivity {

    @RequiresApi(api = Build.VERSION_CODES.N)
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_runner);
        startConfiguredApp();
    }

    private void startConfiguredApp() {
        Intent intent = new Intent(RunnerActivity.this, LectureListActivity.class);
        startActivity(intent);
    }
}
