package com.example.demo.service;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;

import org.apache.tika.exception.TikaException;
import org.apache.tika.metadata.Metadata;
import org.apache.tika.parser.AutoDetectParser;
import org.apache.tika.sax.BodyContentHandler;
import org.springframework.stereotype.Component;
import org.xml.sax.SAXException;

@Component
public class ParsingService {
    
    public String parseToPlainText(byte[] file) throws IOException, SAXException, TikaException {
        long start =  System.currentTimeMillis();
        
        BodyContentHandler handler = new BodyContentHandler();
     
        AutoDetectParser parser = new AutoDetectParser();
        Metadata metadata = new Metadata();
        try (InputStream stream = new ByteArrayInputStream(file)) {
            parser.parse(stream, handler, metadata);
            long end =  System.currentTimeMillis();
            System.out.println("time taken: " + (end - start));
            return handler.toString();
        }
    }
}