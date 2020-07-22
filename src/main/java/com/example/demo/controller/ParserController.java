package com.example.demo.controller;

import org.apache.tika.exception.TikaException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestHeader;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.xml.sax.SAXException;

import java.io.IOException;
import java.util.List;

import com.example.demo.services.ParsingService.IParsingService;
import com.example.demo.services.StorageService.StorageService;

@RestController
@RequestMapping("api")
public class ParserController {

    @Autowired
    IParsingService parsingService;
    
    @GetMapping("/parser/parseAll")
    public boolean parseAll(@RequestHeader("Connection-String") String connectionString, @RequestParam String fromContainerName, @RequestParam String toContainerName) 
        throws IOException, SAXException, TikaException {
        // get files in from container
        StorageService storageService = new StorageService(connectionString);
        for (String fileName : storageService.listBlobs(fromContainerName)) {
            // get file from blob storage
            byte[] file = storageService.getFileAsByteArray(fromContainerName, fileName);
            // parse file into text
            String text = parsingService.parseToPlainText(file);
            // store file in blob storage
            storageService.storeFile(toContainerName, fileName + ".txt", text);
        }
        return true;
    }

    @GetMapping("/parser/parseSingle")
    public boolean parseSingle(@RequestHeader("Connection-String") String connectionString, @RequestParam String fileName, @RequestParam String fromContainerName, @RequestParam String toContainerName) 
        throws IOException, SAXException, TikaException {
        StorageService storageService = new StorageService(connectionString);
        // get file from blob storage
        byte[] file = storageService.getFileAsByteArray(fromContainerName, fileName);
        // parse file into text
        String text = parsingService.parseToPlainText(file);
        // store file in blob storage
        storageService.storeFile(toContainerName, fileName + ".txt", text);
        return true;
    }
    
}