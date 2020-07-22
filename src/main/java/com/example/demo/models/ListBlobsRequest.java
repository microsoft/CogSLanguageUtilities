package com.example.demo.models;

public class ListBlobsRequest {

    @RequestHeader("Connection-String") String connectionString
    String connectionString;

    @PathVariable String containerName
    String containerName;
    
}