package com.microsoft.textparser.models;

import java.util.List;

/**
 * ParseSingleRequest
 */
public class ParseMultipleRequest {

    private List<String> fileNames;

    private String sourceContainerName;

    private String destinationContainerName;

    public List<String> getFileNames() {
        return fileNames;
    }

    public void setFileName(List<String> fileNames) {
        this.fileNames = fileNames;
    }

    public String getSourceContainerName() {
        return sourceContainerName;
    }

    public void setSourceContainerName(String sourceContainerName) {
        this.sourceContainerName = sourceContainerName;
    }

    public String getDestinationContainerName() {
        return destinationContainerName;
    }

    public void setDestinationContainerName(String destinationContainerName) {
        this.destinationContainerName = destinationContainerName;
    }

    @Override
    public String toString() {
        return "ParseSingleRequest: {fileName: " + fileNames.toString() + ", sourceContainerName: " + sourceContainerName
                + ", destinationContainerName: " + destinationContainerName + "}";
    }
}