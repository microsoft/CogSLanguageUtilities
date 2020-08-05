package com.microsoft.textparser.jobs.parsing;

import java.util.List;

public class ParsingJobMessage {

    private String connectionString;

    private String sourceContainerName;

    private String destinationContainerName;

    private List<String> fileNames;

    public ParsingJobMessage() {
    }

    public ParsingJobMessage(String connectionString, String sourceContainerName,
            String destinationContainerName, List<String> fileNames) {
        this.connectionString = connectionString;
        this.sourceContainerName = sourceContainerName;
        this.destinationContainerName = destinationContainerName;
        this.fileNames = fileNames;
    }

    public String getConnectionString() {
        return connectionString;
    }

    public void setConnectionString(String connectionString) {
        this.connectionString = connectionString;
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

    public List<String> getFileNames() {
        return fileNames;
    }

    public void setFileNames(List<String> fileNames) {
        this.fileNames = fileNames;
    }

    @Override
    public String toString() {
        return "ParsingOperationMessage{ " + "connectionString: " + connectionString + ", sourceContainerName: "
                + sourceContainerName + ", destinationContainerName: " + destinationContainerName + " }";
    }
}
