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
public class TestController {

    @GetMapping("/test")
    public String test() {
        return "test";
    }
    
}