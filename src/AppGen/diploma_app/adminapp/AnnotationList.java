package com.example.polina.adminapp;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

public class AnnotationList {

    // Link from featureNameFrom TO this activity
    @Target(ElementType.FIELD)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface OnItemClickTO {
        String featureNameFrom() default "LectureListActivity";
    }

    // Link FROM this activity to featureNameFrom
    @Target(ElementType.FIELD)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface OnItemClickFROM {
        String featureNameTo() default "LectureListActivity";
    }

    // Link from featureNameFrom TO this activity
    @Target(ElementType.FIELD)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface OnLongItemClickTO {
        String featureNameFrom() default "LectureListActivity";
    }

    // Link FROM this activity to featureNameFrom
    @Target(ElementType.FIELD)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface OnLongItemClickFROM {
        String featureNameTo() default "LectureListActivity";
    }

    @Target(ElementType.FIELD)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface InComingArg {
        Class convertedClass() default String.class;
    }

    @Target(ElementType.FIELD)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface OutComingArg {
        Class convertedClass() default String.class;
    }

    @Target(ElementType.TYPE)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface Feature {
        String featureName() default "SimpleFeature";
    }

    @Target(ElementType.TYPE)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface ConnectedToFeature {
        String featureName() default "SimpleFeature";
    }

    @Target(ElementType.TYPE)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface AbstractFeature {
        String abstractFatureName() default "AbstractFeature";
    }

    @Target(ElementType.TYPE)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface NeededAnywayFeatureFile {
        String featureName() default "NeededAnywayFeatureFile";
    }

    @Target(ElementType.TYPE)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface RequiredFeature {
        String featureName() default "RequiredFeature";
    }

    @Target(ElementType.TYPE)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface XorGroup {
        String groupName() default "-1";
    }

    @Target(ElementType.TYPE)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface XorAbstractGroup {
        String groupName() default "-1";
    }

    @Target(ElementType.TYPE)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface OrGroup {
        String groupName() default "-1";
    }

    @Target(ElementType.TYPE)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface OrAbstractGroup {
        String groupName() default "-1";
    }

    @Target(ElementType.TYPE)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface AndGroup {
        String groupName() default "-1";
    }

    @Target(ElementType.TYPE)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface AndAbstractGroup {
        String groupName() default "-1";
    }

    // Link from featureNameFrom TO this activity
    @Target(ElementType.TYPE)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface OnSwipeRightTO {
        String featureNameFrom() default "LectureListActivity";
    }

    // Link from featureNameFrom TO this activity
    @Target(ElementType.TYPE)
    @Retention(RetentionPolicy.RUNTIME)
    public @interface OnButtonClickFromArgTO {
        String featureNameFrom() default "LectureListActivity";
    }
}
