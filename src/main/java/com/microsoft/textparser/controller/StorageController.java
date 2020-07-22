package com.microsoft.textparser.controller;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestHeader;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

import com.microsoft.textparser.services.storage.AzureStorageService;
import com.microsoft.textparser.services.storage.IStorageService;

@RestController
@RequestMapping("api")
public class StorageController {

    @GetMapping("/container/{containerName}/blobs")
    public List<String> getBlobs(@RequestHeader("Connection-String") String connectionString, @PathVariable String containerName)
    {
        IStorageService storageService = new AzureStorageService(connectionString);
        return storageService.listFiles(containerName);
    }
    
}