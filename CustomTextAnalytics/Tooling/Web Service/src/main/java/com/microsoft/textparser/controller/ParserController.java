package com.microsoft.textparser.controller;

import java.io.IOException;
import java.util.UUID;

import com.microsoft.textparser.config.Constants;
import com.microsoft.textparser.jobs.parsing.ParsingJobMessage;
import com.microsoft.textparser.models.ParseAllRequest;
import com.microsoft.textparser.models.ParseConfiguration;
import com.microsoft.textparser.models.ParseRequest;
import com.microsoft.textparser.services.parsing.IParsingService;
import com.microsoft.textparser.services.storage.AzureStorageService;
import com.microsoft.textparser.services.storage.IStorageService;

import org.apache.tika.exception.TikaException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.jms.core.JmsTemplate;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
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
    public String parseAll(@RequestHeader String connectionString, @RequestBody ParseAllRequest parseAllRequest)
            throws IOException, SAXException, TikaException {
        // get files in from container
        IStorageService storageService = new AzureStorageService(connectionString);
        String id = UUID.randomUUID().toString();
        ParsingJobMessage message = new ParsingJobMessage(id, connectionString,
                parseAllRequest.getSourceContainerName(), parseAllRequest.getDestinationContainerName(),
                storageService.listFiles(parseAllRequest.getSourceContainerName()), new ParseConfiguration());
        jmsTemplate.convertAndSend(Constants.QUEUE_NAME, message);
        return id;
    }

    @PostMapping("/parser/parse")
    @ResponseStatus(HttpStatus.ACCEPTED)
    public String parse(@RequestHeader String connectionString, @RequestBody ParseRequest parseRequest)
            throws IOException, SAXException, TikaException {
        String id = UUID.randomUUID().toString();
        ParsingJobMessage message = new ParsingJobMessage(id, connectionString,
                parseRequest.getSourceContainerName(), parseRequest.getDestinationContainerName(),
                parseRequest.getFileNames(), parseRequest.getParseConfiguration());
        jmsTemplate.convertAndSend(Constants.QUEUE_NAME, message);
        return id;
    }
}