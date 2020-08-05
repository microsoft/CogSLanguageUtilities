package com.microsoft.textparser.services.parsing;

import java.io.IOException;
import java.util.Map;
import org.apache.tika.exception.TikaException;
import org.xml.sax.SAXException;

public interface IParsingService {

    /**
     * parse given file to plain text string
     * @param file
     * @return
     * @throws IOException
     * @throws SAXException
     * @throws TikaException
     */
    public String parseToPlainText(byte[] file) throws IOException, SAXException, TikaException;

    /**
     * parse given file to xhtml string
     * @param file
     * @return
     * @throws IOException
     * @throws SAXException
     * @throws TikaException
     */
    public String parseToXHTML(byte[] file) throws IOException, SAXException, TikaException;

    public String parseToPlainTextPerformance(byte[] file, String fileName, Map<String, Long> map)
            throws IOException, SAXException, TikaException;

    public String parsePdfWithOcr() throws IOException, SAXException, TikaException;
}