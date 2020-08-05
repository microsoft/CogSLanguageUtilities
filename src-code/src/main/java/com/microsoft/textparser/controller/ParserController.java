package com.microsoft.textparser.controller;

import java.io.IOException;

import com.microsoft.textparser.models.ParseSingleRequest;
import com.microsoft.textparser.services.parsing.IParsingService;
import com.microsoft.textparser.services.storage.AzureStorageService;
import com.microsoft.textparser.services.storage.IStorageService;

import org.apache.tika.exception.TikaException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestHeader;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.xml.sax.SAXException;

@RestController
@RequestMapping("api")
public class ParserController {

    @Autowired
    IParsingService parsingService;

    @GetMapping("/parser/parseAll")
    public boolean parseAll(@RequestHeader("Connection-String") String connectionString,
            @RequestParam String sourceContainerName, @RequestParam String destinationContainerName)
            throws IOException, SAXException, TikaException {
        // get files in from container
        IStorageService storageService = new AzureStorageService(connectionString);
        for (String fileName : storageService.listFiles(sourceContainerName)) {
            // get file from blob storage
            byte[] file = storageService.getFileAsByteArray(sourceContainerName, fileName);
            // parse file into text
            String text = parsingService.parseToPlainText(file);
            // store file in blob storage
            storageService.storeFile(destinationContainerName, fileName + ".txt", text);
        }
        return true;
    }

    @GetMapping("/parser/parseSingle")
    public boolean parseSingle(@RequestHeader("Connection-String") String connectionString,
            ParseSingleRequest parseSingleRequest) throws IOException, SAXException, TikaException {
        IStorageService storageService = new AzureStorageService(connectionString);
        // get file from blob storage
        byte[] file = storageService.getFileAsByteArray(parseSingleRequest.getSourceContainerName(),
                parseSingleRequest.getFileName());
        // parse file into text
        String text = parsingService.parseToPlainText(file);
        // store file in blob storage
        storageService.storeFile(parseSingleRequest.getDestinationContainerName(),
                parseSingleRequest.getFileName() + ".txt", text);
        return true;
    }

    @GetMapping("/parser/parseOCR")
    public String parseORC() throws IOException, SAXException, TikaException {

        return parsingService.parsePdfWithOcr();
    }
    

}