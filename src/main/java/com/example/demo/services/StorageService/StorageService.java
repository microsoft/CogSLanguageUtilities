package com.example.demo.services.StorageService;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.InputStream;
import java.util.List;
import java.util.stream.Collectors;

import com.azure.storage.blob.BlobClient;
import com.azure.storage.blob.BlobContainerClient;
import com.azure.storage.blob.BlobServiceClient;
import com.azure.storage.blob.BlobServiceClientBuilder;
import com.azure.storage.blob.specialized.BlockBlobClient;

import org.springframework.stereotype.Component;

@Component
public class StorageService {

    // attributes
    private BlobServiceClient storageClient;

    // constructor
    public StorageService(String connectionString)
    {
        // establish connection
        this.storageClient = new BlobServiceClientBuilder().connectionString(connectionString).buildClient();
    }

    // interface methods
    public List<String> listBlobs(String containerName)
    {
        BlobContainerClient blobContainerClient = storageClient.getBlobContainerClient(containerName);
        return blobContainerClient.listBlobs().stream().map(a -> a.getName()).collect(Collectors.toList());
    }

    public byte[] getFileAsByteArray(String containerName, String fileName)
    {
        BlobContainerClient blobContainerClient = storageClient.getBlobContainerClient(containerName);
        BlobClient blobClient = blobContainerClient.getBlobClient(fileName);
        ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
        blobClient.download(outputStream);
        return outputStream.toByteArray();
    }

    public void storeFile(String toContainerName, String fileName, String textData)
    {
        BlobContainerClient blobContainerClient = storageClient.getBlobContainerClient(toContainerName);
        BlockBlobClient blobClient = blobContainerClient.getBlobClient(fileName).getBlockBlobClient();
        byte[] textByteArray = textData.getBytes();
        InputStream textStream = new ByteArrayInputStream(textByteArray);
        blobClient.upload(textStream, textByteArray.length, true);
    }
    

}