package com.microsoft.textparser.services.storage;

import java.util.List;

public interface IStorageService {

    /**
     * list files in the specified container
     * 
     * @param containerName
     * @return
     */
    public List<String> listFiles(String containerName);

    /**
     * downloads a file form specified container as Byte Array
     * 
     * @param containerName
     * @param fileName
     * @return
     */
    public byte[] getFileAsByteArray(String containerName, String fileName);

    /**
     * stores given textData as text file in given container
     * 
     * @param destinationContainerName
     * @param fileName
     * @param textData
     */
    public void storeFile(String destinationContainerName, String fileName, String textData);

}