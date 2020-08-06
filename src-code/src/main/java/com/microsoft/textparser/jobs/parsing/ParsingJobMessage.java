package com.microsoft.textparser.jobs.parsing;

import java.util.List;

import com.microsoft.textparser.models.ParseConfiguration;

public class ParsingJobMessage {

    private String id;

    private String connectionString;

    private String sourceContainerName;

    private String destinationContainerName;

    private List<String> fileNames;

    private ParseConfiguration parseConfiguration;

    public ParsingJobMessage() {
    }

    public ParsingJobMessage(String id, String connectionString, String sourceContainerName,
            String destinationContainerName, List<String> fileNames, ParseConfiguration parseConfiguration) {
        this.id = id;
        this.connectionString = connectionString;
        this.sourceContainerName = sourceContainerName;
        this.destinationContainerName = destinationContainerName;
        this.fileNames = fileNames;
        this.parseConfiguration = parseConfiguration;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
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

    public ParseConfiguration getParseConfiguration() {
        return parseConfiguration;
    }

    public void setParseConfiguration(ParseConfiguration parseConfiguration) {
        this.parseConfiguration = parseConfiguration;
    }

    @Override
    public String toString() {
        return "ParsingOperationMessage{ " + "connectionString: " + connectionString + ", sourceContainerName: "
                + sourceContainerName + ", destinationContainerName: " + destinationContainerName + ", fileNames: "
                + fileNames.toString() + ", parseConfiguration: " + parseConfiguration.toString() + " }";
    }
}
