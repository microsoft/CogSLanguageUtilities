package com.microsoft.textparser.services.parsing;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.Map;

import com.microsoft.textparser.models.ParseConfiguration;

import java.nio.charset.Charset;

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

    public String parseToPlainText(byte[] file, ParseConfiguration parseConfig)
            throws IOException, SAXException, TikaException {

        Parser parser = new AutoDetectParser();
        ParseContext parsecontext = prepareParseContext(parser, parseConfig);

        InputStream inputFile = new ByteArrayInputStream(file);
        ByteArrayOutputStream out = new ByteArrayOutputStream();

        BodyContentHandler handler = new BodyContentHandler(out);
        Metadata meta = new Metadata();

        parser.parse(inputFile, handler, meta, parsecontext);
        return new String(out.toByteArray(), Charset.defaultCharset());
    }

    private ParseContext prepareParseContext(Parser parser, ParseConfiguration parseConfig) {
        // pdf configs
        PDFParserConfig pdfConfig = new PDFParserConfig();
        pdfConfig.setOcrDPI(100); // scalastyle:ignore magic.number
        pdfConfig.setExtractInlineImages(parseConfig.getExtractInlineImages());
        pdfConfig.setExtractUniqueInlineImagesOnly(false);
        pdfConfig.setOcrStrategy(PDFParserConfig.OCR_STRATEGY.OCR_AND_TEXT_EXTRACTION);
        pdfConfig.setDetectAngles(parseConfig.getDetectAngles());
        pdfConfig.setSortByPosition(parseConfig.getSortByPosition());

        // tesseract
        TesseractOCRConfig tessConf = new TesseractOCRConfig();
        tessConf.setEnableImageProcessing(1);

        // tika parse context
        ParseContext parsecontext = new ParseContext();
        parsecontext.set(Parser.class, parser);
        parsecontext.set(PDFParserConfig.class, pdfConfig);
        parsecontext.set(TesseractOCRConfig.class, tessConf);
        return parsecontext;
    }
}