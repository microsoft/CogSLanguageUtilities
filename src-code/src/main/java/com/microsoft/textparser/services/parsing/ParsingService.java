package com.microsoft.textparser.services.parsing;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Map;
import java.nio.charset.Charset;
import java.nio.file.Files;
import java.nio.file.Paths;

import org.apache.tika.config.TikaConfig;
import org.apache.tika.exception.TikaException;
import org.apache.tika.metadata.Metadata;
import org.apache.tika.parser.AutoDetectParser;
import org.apache.tika.parser.ParseContext;
import org.apache.tika.parser.Parser;
import org.apache.tika.parser.ocr.TesseractOCRConfig;
import org.apache.tika.parser.pdf.PDFParserConfig;
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

    public String parsePdfWithOcr() throws IOException, SAXException, TikaException {


        // pdf configs
        PDFParserConfig pdfConfig = new PDFParserConfig();
        pdfConfig.setOcrDPI(100); //scalastyle:ignore magic.number
        //pdfConfig.setDetectAngles(true);
        pdfConfig.setExtractInlineImages(true);
        pdfConfig.setExtractUniqueInlineImagesOnly(false);
        pdfConfig.setOcrStrategy(PDFParserConfig.OCR_STRATEGY.OCR_AND_TEXT_EXTRACTION);

        // tesserect
        TesseractOCRConfig tessConf = new TesseractOCRConfig();
        tessConf.setLanguage("eng");
        tessConf.setEnableImageProcessing(1);

        // tika configs
        TikaConfig config = TikaConfig.getDefaultConfig();

        Parser parser = new AutoDetectParser(config);
        ParseContext parsecontext = new ParseContext();
        parsecontext.set(Parser.class, parser);
        parsecontext.set(PDFParserConfig.class, pdfConfig);
        parsecontext.set(TesseractOCRConfig.class, tessConf);

        // asd
        InputStream pdf = Files.newInputStream(Paths.get("C:\\Users\\a-moshab\\Desktop\\LUIS-D Tool\\src-code\\src\\test\\resources\\sample.png"));
        ByteArrayOutputStream out = new ByteArrayOutputStream();
  
        // adsd
        BodyContentHandler handler = new BodyContentHandler(out);
        Metadata meta = new Metadata();
        
        // dsa
        parser.parse(pdf, handler, meta, parsecontext);
        return new String(out.toByteArray(), Charset.defaultCharset());
    }
}