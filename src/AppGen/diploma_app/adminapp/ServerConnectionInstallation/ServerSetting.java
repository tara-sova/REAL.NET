package com.example.polina.adminapp.ServerConnectionInstallation;
import com.example.polina.adminapp.AnnotationList;
import com.example.polina.adminapp.Lecture;

import org.springframework.http.ResponseEntity;

@AnnotationList.AbstractFeature(abstractFatureName = "UseServer")
@AnnotationList.Feature(featureName = "Server")
@AnnotationList.NeededAnywayFeatureFile(featureName = "Server")
public class ServerSetting {

    private static IServerConnection server = null;

    public static void init(boolean serverFeatureActivated) {
        server = serverFeatureActivated
                 ? new ServerConnection()
                 : new NoServerConnection();
    }

    public static ResponseEntity<String> postLecture(Lecture lecture) {
        ResponseEntity<String> postResponse = server.postLecture(lecture);
        return postResponse;
    }

    public static Lecture postThanGetLecture(Lecture lecture) {
        Lecture lect = server.postThanGetLecture(lecture);
        return lect;
    }

    public static void putLecture(Lecture lecture, long position) {
        server.putLecture(lecture, position);
    }

    public static Lecture getLectureByPosition(long position) {
        Lecture lecture = server.getLectureByPosition(position);
        return lecture;
    }

    public static void deleteLecture(long position) {
        server.deleteLecture(position);
    }
}