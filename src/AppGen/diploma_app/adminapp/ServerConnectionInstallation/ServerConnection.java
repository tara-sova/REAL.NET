package com.example.polina.adminapp.ServerConnectionInstallation;

import android.os.StrictMode;

import com.example.polina.adminapp.AnnotationList;
import com.example.polina.adminapp.Lecture;
import com.fasterxml.jackson.databind.ObjectMapper;

import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpMethod;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.client.RestTemplate;

import java.io.IOException;

@AnnotationList.ConnectedToFeature(featureName = "Server")
public class ServerConnection implements IServerConnection {

    private static String URL = "http://10.0.2.2:8080";
    private static String lecturesURL = URL + "/lectures";
    private static long offset = -1;

    public ServerConnection() {
        setThreadPolicy();
        setOffset();
    }

    private void setThreadPolicy() {
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);
    }

    private void setOffset() {
        ResponseEntity<String> getResponse = getAllLectures();
        String idAtribute = "{\"id\":";
        Integer startSymbol = getResponse.getBody().lastIndexOf(idAtribute) + idAtribute.length();
        offset = Long.valueOf(getResponse.getBody().substring(startSymbol, getResponse.getBody().length() - 1).split(",")[0]);
    }

    public void setOffset(long offsetForSet) {
        offset = offsetForSet;
    }

    public long getOffset() {
        return offset;
    }

    private static void checkStatusCode(HttpStatus code) {
        if (code != HttpStatus.OK) {
            try {
                throw new Exception("Problem");
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    private ResponseEntity<String> getAllLectures() {
        ResponseEntity<String> getResponse = (new RestTemplate()).getForEntity(lecturesURL, String.class);
        return getResponse;
    }

    private ResponseEntity<String> getLecture(long id) {
        ResponseEntity<String> getResponse = (new RestTemplate()).getForEntity(lecturesURL + "/" + Long.toString(id), String.class);
        return getResponse;
    }

    public ResponseEntity<String> postLecture(Lecture lectForPost) {
        RestTemplate rest = new RestTemplate();
        HttpEntity<Lecture> entity = new HttpEntity<Lecture>(lectForPost);
        return rest.postForEntity(lecturesURL, entity, String.class);
    }

    public Lecture postThanGetLecture(Lecture lecture) {
        ResponseEntity<String> postResponse = postLecture(lecture);
        checkStatusCode((postResponse.getStatusCode()));
        Lecture lect = getLecture(postResponse);
        return lect;
    }

    public static Lecture getLecture(ResponseEntity<String> response) {
        ObjectMapper mapper = new ObjectMapper();
        Lecture obj = null;
        try {
            String res = response.getBody();
            if (res.contains(",\"_links")) {
                res = res.split(",\"_links")[0] + "}";
            }
            obj = mapper.readValue(res, Lecture.class);
        } catch (IOException e) {
            e.printStackTrace();
        }
        return obj;
    }

    public Lecture getLectureByPosition(long position) {
        ResponseEntity<String> getResponse = getLecture(position + offset + 1);
        return getLecture(getResponse);
    }

    public void putLecture(Lecture lect, long position) {
        HttpEntity<Lecture> requestEntity = new HttpEntity<Lecture>(lect);
        HttpEntity<Lecture> response = (new RestTemplate()).exchange(lecturesURL + "/" + Long.toString(offset + position + 1), HttpMethod.PUT, requestEntity, Lecture.class);
    }

    public void deleteLecture(long position) {
        RestTemplate restTemplate = new RestTemplate();
        restTemplate.delete(lecturesURL + "/" + Long.toString(offset + position + 1));
    }
}
