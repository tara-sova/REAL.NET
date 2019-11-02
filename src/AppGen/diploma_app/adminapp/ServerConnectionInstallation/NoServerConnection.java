package com.example.polina.adminapp.ServerConnectionInstallation;

import com.example.polina.adminapp.AnnotationList;
import com.example.polina.adminapp.Lecture;

import org.springframework.http.ResponseEntity;

import java.util.ArrayList;

@AnnotationList.ConnectedToFeature(featureName = "Server")
public class NoServerConnection implements IServerConnection {

    private static ArrayList<Lecture> lectureList = null;
    private static int counter = 0;

    public NoServerConnection() {
        lectureList = new ArrayList<>();
    }

    public ResponseEntity<String> postLecture(Lecture lecture) {
        addLecture(lecture);
        return null;
    }

    public Lecture postThanGetLecture(Lecture lecture) {
        return addLecture(lecture);
    }

    private Lecture addLecture(Lecture lecture) {
        lecture.setId(counter);
        lectureList.add(lecture);
        ++counter;
        return lecture;
    }

    public void putLecture(Lecture lecture, long id) {
        Lecture lectureForRemove = this.getLectureByPosition(id);
        lectureList.remove(lectureForRemove);
        lectureList.add(lecture);
    }

    public Lecture getLectureByPosition(long id) {
        Lecture lecture = null;
        for (Lecture lect : lectureList) {
            if (lect.getId() == id) {
                lecture = lect;
                break;
            }
        }
        return lecture;
    }

    public void deleteLecture(long id) {
        Lecture lectureForRemove = this.getLectureByPosition(id);
        lectureList.remove(lectureForRemove);
    }
}
