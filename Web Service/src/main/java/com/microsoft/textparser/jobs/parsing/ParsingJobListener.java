package com.microsoft.textparser.jobs.parsing;

import java.io.IOException;

import com.microsoft.textparser.config.Constants;
import com.microsoft.textparser.services.parsing.ParsingService;
import com.microsoft.textparser.services.storage.AzureStorageService;
import com.microsoft.textparser.services.storage.IStorageService;

import org.apache.tika.exception.TikaException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jms.annotation.JmsListener;
import org.springframework.stereotype.Component;
import org.xml.sax.SAXException;

@Component
public class ParsingJobListener {

    @Autowired
    ParsingService parsingService;

    @JmsListener(destination = Constants.QUEUE_NAME)
    public void receiveMessage(ParsingJobMessage message) {
        System.out.println(message.toString());

        IStorageService storageService = new AzureStorageService(message.getConnectionString());

        for (String fileName : message.getFileNames()) {
            // get file from blob storage
            byte[] file = storageService.getFileAsByteArray(message.getSourceContainerName(), fileName);
            // parse file into text
            String text;
            try {
                text = parsingService.parseToPlainText(file, message.getParseConfiguration());
                // store file in blob storage
                storageService.storeFile(message.getDestinationContainerName(), fileName + ".txt", text);
            } catch (IOException | SAXException | TikaException e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
            }
        }
    }
}