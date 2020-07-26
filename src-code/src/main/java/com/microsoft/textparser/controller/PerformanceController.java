package com.microsoft.textparser.controller;

import org.apache.tika.exception.TikaException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestHeader;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.xml.sax.SAXException;

import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

import com.microsoft.textparser.services.parsing.IParsingService;
import com.microsoft.textparser.services.storage.AzureStorageService;
import com.microsoft.textparser.services.storage.IStorageService;

@RestController
@RequestMapping("api")
public class PerformanceController {
    
    @Autowired
    IParsingService parsingService;
    
    @GetMapping("/performance/parser/parseAll")
    public Map<String, Long> parseAll(@RequestHeader("Connection-String") String connectionString, @RequestParam String sourceContainerName, @RequestParam String destinationContainerName) 
        throws IOException, SAXException, TikaException {
        Map<String, Long> monitor = new HashMap<String, Long>();
        IStorageService storageService = new AzureStorageService(connectionString);
        for (String fileName : storageService.listFiles(sourceContainerName)) {
            // get file from blob storage
            byte[] file = storageService.getFileAsByteArray(sourceContainerName, fileName);
            // parse file into text
            long start = System.currentTimeMillis();
            String text = parsingService.parseToPlainText(file);
            long totalTime = System.currentTimeMillis() - start;
            monitor.put(fileName, totalTime);
            // store file in blob storage
            storageService.storeFile(destinationContainerName, fileName + ".txt", text);
        }
        return monitor;
    }

}