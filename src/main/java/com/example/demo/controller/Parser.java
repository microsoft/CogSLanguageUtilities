package com.example.demo.controller;

import org.apache.tika.exception.TikaException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.xml.sax.SAXException;

import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

import com.example.demo.service.ParsingService;
import com.example.demo.service.StorageService;

@RestController
@RequestMapping("parse")
public class Parser {

    @Autowired
    ParsingService parsingService;

    @Autowired
    StorageService storageService;

    @GetMapping("/test")
    public String test() {
        return "test";
    }

    @GetMapping("/text")
    public ResponseEntity<?> parseDocumentToPlainText(@RequestParam String fileName)
            throws IOException, SAXException, TikaException {
        byte[] file = storageService.getFileAsByteArray(fileName);
        String text = parsingService.parseToPlainText(file);
        storageService.writeFile(fileName + ".txt", text);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @GetMapping("/xhtml")
    public ResponseEntity<?> parseDocumentToXHTML(@RequestParam String fileName)
            throws IOException, SAXException, TikaException {
        byte[] file = storageService.getFileAsByteArray(fileName);
        String text = parsingService.parseToXHTML(file);
        storageService.writeFile(fileName + ".xhtml", text);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @GetMapping("/performanceTest")
    public Map<String, Long> runTest() throws IOException, SAXException, TikaException {
        Map<String, Long> map = new HashMap<>();
        for (String fileName : storageService.getBlobNames()) {
            byte[] file = storageService.getFileAsByteArray(fileName);
            System.out.println("fileName: " + fileName);
            String text = parsingService.parseToPlainTextPerformance(file, fileName, map);
            storageService.writeFile(fileName + ".txt", text);
        }
        return map;
    }
}