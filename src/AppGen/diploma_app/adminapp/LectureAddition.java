package com.example.polina.adminapp;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import com.google.gson.Gson;

@AnnotationList.OnButtonClickFromArgTO(featureNameFrom = "LectureListActivity")
@AnnotationList.AbstractFeature(abstractFatureName = "EventManipulations")
@AnnotationList.Feature(featureName = "LectureAddition")
@AnnotationList.OrGroup(groupName = "1")

@AnnotationList.XorAbstractGroup(groupName = "1")

public class LectureAddition extends AppCompatActivity {

    @AnnotationList.InComingArg(convertedClass = Integer.class)
    int position;

    @AnnotationList.InComingArg(convertedClass = Lecture.class)
    String lectureAsAString;

    EditText lecturerEdit;
    EditText themeEdit;
    EditText abstractEdit;
    EditText timeStartEdit;
    EditText timeEndEdit;

    Button button;
    Button removeButton;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_lecture_edition);

        Gson gson = new Gson();
        Intent intent = getIntent();
        lectureAsAString = intent.getStringExtra("lectureAsAString");
        Lecture lecture = gson.fromJson(lectureAsAString, Lecture.class);
        position = getIntent().getExtras().getInt("position");

        lecturerEdit = findViewById(R.id.editText9);
        themeEdit = findViewById(R.id.editText10);
        abstractEdit = findViewById(R.id.editText11);
        timeStartEdit = findViewById(R.id.editText);
        timeEndEdit = findViewById(R.id.editText5);


        button = findViewById(R.id.button);
        button.setOnClickListener(buttonListener);

        button.setText("Добавить");

        lecturerEdit.setText("Лектор");
        themeEdit.setText("Тема");
        abstractEdit.setText("Описание");
        timeStartEdit.setText("Начало");
        timeEndEdit.setText("Конец");

    }

    private String normalizeTime(String time) {
        StringBuilder timeBuilder = new StringBuilder(time);
        while (timeBuilder.length() < 5) {
            timeBuilder.insert(0, "0");
        }
        time = timeBuilder.toString();
        return  time;
    }

    private View.OnClickListener buttonListener = new View.OnClickListener() {
        public void onClick(View v) {
            if (v.getId() == R.id.button)
            {
                Gson gson = new Gson();

                String timeStart = normalizeTime(timeStartEdit.getText().toString());
                int intTimeStart = Integer.parseInt(timeStart.replace(":", ""));

                String timeEnd = normalizeTime(timeEndEdit.getText().toString());
                int intTimeEnd = Integer.parseInt(timeEnd.replace(":", ""));

                Lecture editedLecture = new Lecture(lecturerEdit.getText().toString(),
                        themeEdit.getText().toString(),
                        abstractEdit.getText().toString(),
                        timeStart, intTimeStart,
                        timeEnd, intTimeEnd);

                lectureAsAString = gson.toJson(editedLecture);

                Intent intent = new Intent(LectureAddition.this, LectureListActivity.class);
                intent.putExtra("lectureAsAString", lectureAsAString);
                intent.putExtra("position", position);

                setResult(Activity.RESULT_OK, intent);
                finish();
            }
        }
    };
}
