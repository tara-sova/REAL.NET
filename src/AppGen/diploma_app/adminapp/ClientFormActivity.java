package com.example.polina.adminapp;

import android.app.Activity;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;



import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.EditText;

import com.google.gson.Gson;

import java.util.ArrayList;

@AnnotationList.AbstractFeature(abstractFatureName = "RegisterOnAnEvent")
@AnnotationList.Feature(featureName = "BeAttended")

@AnnotationList.XorAbstractGroup(groupName = "1")

public class ClientFormActivity extends AppCompatActivity {

    @AnnotationList.OnLongItemClickTO(featureNameFrom = "LectureListActivity")
    public String link;

    @AnnotationList.InComingArg(convertedClass = Lecture.class)
    @AnnotationList.OutComingArg(convertedClass = Lecture.class)
    private String lectureAsAString;

    @AnnotationList.InComingArg(convertedClass = Integer.class)
    @AnnotationList.OutComingArg(convertedClass = Integer.class)
    private int position;

    @AnnotationList.InComingArg(convertedClass = String.class)
    @AnnotationList.OutComingArg(convertedClass = String.class)
    private String currentClient;

    private Lecture lecture = null;

    private EditText nameEdit;
    private CheckBox checkBox;
    private Boolean checkBoxStateOnCreateState;
    private Button button;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_attented_client_form);

        Gson gson = new Gson();
        Intent intent = getIntent();
        lectureAsAString = intent.getStringExtra("lectureAsAString");
        position = getIntent().getExtras().getInt("position");
        currentClient = getIntent().getStringExtra("currentClient");

        lecture = gson.fromJson(lectureAsAString, Lecture.class);

        nameEdit = findViewById(R.id.clientName);
        checkBox = findViewById(R.id.checkBox);
        button = findViewById(R.id.confirmButton);
        button.setOnClickListener(buttonListener);

        if (currentClient != null) {
            nameEdit.setText(currentClient);
            if (lecture.attentedClients.contains(currentClient)) {
                checkBox.setChecked(true);
                checkBoxStateOnCreateState = checkBox.isChecked();
            }
            return;
        }
        nameEdit.setText("Ваше имя");
        checkBox.setChecked(false);
        checkBoxStateOnCreateState = checkBox.isChecked();
    }

    private View.OnClickListener buttonListener = new View.OnClickListener() {
        public void onClick(View v) {
            if (v.getId() == R.id.confirmButton)
            {
                currentClient = nameEdit.getText().toString();

                Gson gson = new Gson();
                ArrayList<String> attentedClientsList = lecture.attentedClients;
                Boolean checkBoxStateNow = checkBox.isChecked();
                if (!checkBoxStateNow.equals(checkBoxStateOnCreateState)) {
                    if (checkBoxStateNow) {
                        attentedClientsList.add(currentClient);
                        lecture.updateAttendedClientsList(attentedClientsList);
                    }
                    else {
                        attentedClientsList.remove(currentClient);
                        lecture.updateAttendedClientsList(attentedClientsList);
                    }
                }

                lectureAsAString = gson.toJson(lecture);

                Intent intent = new Intent(ClientFormActivity.this, LectureListActivity.class);
                intent.putExtra("currentClient", currentClient);
                intent.putExtra("lectureAsAString", lectureAsAString);
                intent.putExtra("position", position);

                setResult(Activity.RESULT_OK, intent);
                finish();
            }
        }
    };

}
