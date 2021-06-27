package com.microsoft.textparser.models;

import java.util.List;

import com.fasterxml.jackson.annotation.JsonUnwrapped;

public class ParseRequest {

    private List<String> fileNames;

    private String sourceContainerName;

    private String destinationContainerName;

    @JsonUnwrapped
    private ParseConfiguration parseConfiguration; 

    public List<String> getFileNames() {
        return fileNames;
    }

    public void setFileNames(List<String> fileNames) {
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

    public ParseConfiguration getParseConfiguration() {
        return parseConfiguration;
    }

    public void setParseConfiguration(ParseConfiguration parseConfiguration) {
        this.parseConfiguration = parseConfiguration;
    }

    @Override
    public String toString() {
        return "ParseSingleRequest: {fileName: " + fileNames.toString() + ", sourceContainerName: " + sourceContainerName
                + ", destinationContainerName: " + destinationContainerName + "}";
    }
}