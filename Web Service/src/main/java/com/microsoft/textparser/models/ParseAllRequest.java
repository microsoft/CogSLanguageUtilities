package com.microsoft.textparser.models;

import com.fasterxml.jackson.annotation.JsonUnwrapped;

public class ParseAllRequest {

    private String sourceContainerName;

    private String destinationContainerName;

    @JsonUnwrapped
    private ParseConfiguration parseConfiguration;

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
        return "ParseSingleRequest { sourceContainerName: " + sourceContainerName + ", destinationContainerName: "
                + destinationContainerName + " , parseConfiguration: " + parseConfiguration.toString() + " }";
    }
}