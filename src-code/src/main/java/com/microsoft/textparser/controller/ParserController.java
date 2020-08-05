package com.microsoft.textparser.controller;

import java.io.IOException;

import com.microsoft.textparser.config.Constants;
import com.microsoft.textparser.jobs.parsing.ParsingJobMessage;
import com.microsoft.textparser.models.ParseMultipleRequest;
import com.microsoft.textparser.services.parsing.IParsingService;
import com.microsoft.textparser.services.storage.AzureStorageService;
import com.microsoft.textparser.services.storage.IStorageService;

import org.apache.tika.exception.TikaException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.jms.core.JmsTemplate;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestHeader;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseStatus;
import org.springframework.web.bind.annotation.RestController;
import org.xml.sax.SAXException;

@RestController
@RequestMapping("api")
public class ParserController {

    @Autowired
    IParsingService parsingService;

    @Autowired
    JmsTemplate jmsTemplate;

    @GetMapping("/parser/parseAll")
    @ResponseStatus(HttpStatus.ACCEPTED)
    public boolean parseAll(@RequestHeader("Connection-String") String connectionString,
            @RequestParam String sourceContainerName, @RequestParam String destinationContainerName)
            throws IOException, SAXException, TikaException {
        // get files in from container
        IStorageService storageService = new AzureStorageService(connectionString);
        ParsingJobMessage message = new ParsingJobMessage(connectionString, sourceContainerName,
                destinationContainerName, storageService.listFiles(sourceContainerName));
        jmsTemplate.convertAndSend(Constants.QUEUE_NAME, message);
        return true;
    }

    @GetMapping("/parser/parseMultiple")
    @ResponseStatus(HttpStatus.ACCEPTED)
    public boolean parseSingle(@RequestHeader("Connection-String") String connectionString,
            ParseMultipleRequest parseSingleRequest) throws IOException, SAXException, TikaException {
        ParsingJobMessage message = new ParsingJobMessage(connectionString, parseSingleRequest.getSourceContainerName(),
                parseSingleRequest.getDestinationContainerName(), parseSingleRequest.getFileNames());
        jmsTemplate.convertAndSend(Constants.QUEUE_NAME, message);
        return true;
    }

    @GetMapping("/parser/parseOCR")
    public String parseORC() throws IOException, SAXException, TikaException {

        return parsingService.parsePdfWithOcr();
    }
}