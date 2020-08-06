package com.microsoft.textparser.controller;

import java.io.IOException;
import java.util.UUID;

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
    public String parseAll(@RequestHeader("Connection-String") String connectionString,
            @RequestParam String sourceContainerName, @RequestParam String destinationContainerName)
            throws IOException, SAXException, TikaException {
        // get files in from container
        IStorageService storageService = new AzureStorageService(connectionString);
        String id = UUID.randomUUID().toString();
        ParsingJobMessage message = new ParsingJobMessage(id, connectionString, sourceContainerName,
                destinationContainerName, storageService.listFiles(sourceContainerName));
        jmsTemplate.convertAndSend(Constants.QUEUE_NAME, message);
        return id;
    }

    @GetMapping("/parser/parseMultiple")
    @ResponseStatus(HttpStatus.ACCEPTED)
    public String parseSingle(@RequestHeader("Connection-String") String connectionString,
            ParseMultipleRequest parseSingleRequest) throws IOException, SAXException, TikaException {
        String id = UUID.randomUUID().toString();
        ParsingJobMessage message = new ParsingJobMessage(id, connectionString, parseSingleRequest.getSourceContainerName(),
                parseSingleRequest.getDestinationContainerName(), parseSingleRequest.getFileNames());
        jmsTemplate.convertAndSend(Constants.QUEUE_NAME, message);
        return id;
    }

    @GetMapping("/parser/parseOCR")
    public String parseORC() throws IOException, SAXException, TikaException {

        return parsingService.parsePdfWithOcr();
    }
}