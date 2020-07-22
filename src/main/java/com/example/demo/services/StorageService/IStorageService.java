package com.example.demo.services.StorageService;

import java.util.List;


public interface IStorageService {

    /**
     * list blobs in the specified container
     * @param containerName
     * @return
     */
    public List<String> listBlobs(String containerName);

    /**
     * downloads a file form specified container as Byte Array
     * @param containerName
     * @param fileName
     * @return
     */
    public byte[] getFileAsByteArray(String containerName, String fileName);

    /**
     * stores given textData as text file in given container
     * @param toContainerName
     * @param fileName
     * @param textData
     */
    public void storeFile(String toContainerName, String fileName, String textData);

    

}