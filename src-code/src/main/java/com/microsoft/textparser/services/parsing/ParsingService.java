package com.microsoft.textparser.services.parsing;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Map;

import org.apache.tika.exception.TikaException;
import org.apache.tika.metadata.Metadata;
import org.apache.tika.parser.AutoDetectParser;
import org.apache.tika.sax.BodyContentHandler;
import org.apache.tika.sax.ToXMLContentHandler;
import org.springframework.stereotype.Component;
import org.xml.sax.SAXException;
import org.xml.sax.ContentHandler;

@Component
public class ParsingService implements IParsingService {

    public String parseToPlainText(byte[] file) throws IOException, SAXException, TikaException {
        BodyContentHandler handler = new BodyContentHandler(10000000);
        AutoDetectParser parser = new AutoDetectParser();
        Metadata metadata = new Metadata();
        try (InputStream stream = new ByteArrayInputStream(file)) {
            parser.parse(stream, handler, metadata);
            return handler.toString();
        }
    }

    public String parseToXHTML(byte[] file) throws IOException, SAXException, TikaException {
        long start = System.currentTimeMillis();

        ContentHandler handler = new ToXMLContentHandler();

        AutoDetectParser parser = new AutoDetectParser();
        Metadata metadata = new Metadata();
        try (InputStream stream = new ByteArrayInputStream(file)) {
            parser.parse(stream, handler, metadata);
            long end = System.currentTimeMillis();
            System.out.println("time taken: " + (end - start));
            return handler.toString();
        }
    }

    public String parseToPlainTextPerformance(byte[] file, String fileName, Map<String, Long> map)
            throws IOException, SAXException, TikaException {
        long start = System.currentTimeMillis();

        BodyContentHandler handler = new BodyContentHandler(10000000);

        AutoDetectParser parser = new AutoDetectParser();
        Metadata metadata = new Metadata();
        try (InputStream stream = new ByteArrayInputStream(file)) {
            parser.parse(stream, handler, metadata);
            long end = System.currentTimeMillis();
            System.out.println("time taken: " + (end - start));
            map.put(fileName, end - start);
            return handler.toString();
        }
    }
}