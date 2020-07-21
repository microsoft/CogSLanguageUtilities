package com.example.demo.controller;

import org.apache.tika.exception.TikaException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.xml.sax.SAXException;

import java.io.IOException;

import com.example.demo.service.ParsingService;
import com.example.demo.service.StorageService;

@RestController
public class Parser {

    @Autowired
    ParsingService parsingService;

    @Autowired
    StorageService StorageService;

    @GetMapping("/test")
    public String test()
    {
        return "test";
    }

    @GetMapping("/parse")
	public String parseDocument(@RequestParam String fileName) throws IOException, SAXException, TikaException {
        byte[] file = StorageService.getFileAsByteArray(fileName);
		return parsingService.parseToPlainText(file);
    }
}