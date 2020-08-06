package com.microsoft.textparser.models;

public class ParseConfiguration {

    private Boolean detectAngles = false;

    private Boolean extractInlineImages = false;

    private Boolean sortByPosition = false;

    public ParseConfiguration() {
    }

    public ParseConfiguration(Boolean detectAngles, Boolean extractInlineImages, Boolean sortByPosition) {
        this.detectAngles = detectAngles;
        this.extractInlineImages = extractInlineImages;
        this.sortByPosition = sortByPosition;
    }

    public Boolean getDetectAngles() {
        return detectAngles;
    }

    public void setDetectAngles(Boolean detectAngles) {
        this.detectAngles = detectAngles;
    }

    public Boolean getExtractInlineImages() {
        return extractInlineImages;
    }

    public void setExtractInlineImages(Boolean extractInlineImages) {
        this.extractInlineImages = extractInlineImages;
    }

    public Boolean getSortByPosition() {
        return sortByPosition;
    }

    public void setSortByPosition(Boolean sortByPosition) {
        this.sortByPosition = sortByPosition;
    }

    @Override
    public String toString() {
        return "ParseConfiguration { detectAngles: " + detectAngles + ", extractInlineImages: " + extractInlineImages
                + ", sortByPosition: " + sortByPosition + " }";
    }
}