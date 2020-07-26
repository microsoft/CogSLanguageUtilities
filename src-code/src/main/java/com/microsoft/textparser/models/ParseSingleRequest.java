package com.microsoft.textparser.models;

/**
 * ParseSingleRequest
 */
public class ParseSingleRequest {

    private String fileName;

    private String sourceContainerName;

    private String destinationContainerName;

    public String getFileName() {
        return fileName;
    }

    public void setFileName(String fileName) {
        this.fileName = fileName;
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
        return "ParseSingleRequest: {fileName: " + fileName + ", sourceContainerName: " + sourceContainerName
                + ", destinationContainerName: " + destinationContainerName + "}";
    }
}