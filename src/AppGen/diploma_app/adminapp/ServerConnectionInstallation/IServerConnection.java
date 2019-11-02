package com.example.polina.adminapp.ServerConnectionInstallation;

import com.example.polina.adminapp.AnnotationList;
import com.example.polina.adminapp.Lecture;

import org.springframework.http.ResponseEntity;

@AnnotationList.ConnectedToFeature(featureName = "Server")
public interface IServerConnection {
    ResponseEntity<String> postLecture(Lecture lecture);
    Lecture postThanGetLecture(Lecture lecture);
    Lecture getLectureByPosition(long position);
    void putLecture(Lecture lecture, long position);
    void deleteLecture(long position);
}
