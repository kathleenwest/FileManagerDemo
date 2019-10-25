using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.IO;

namespace SharedLibrary
{
    /// <summary>
    /// IFileManagerService
    /// Service interface for the File Manager
    /// </summary>
    [ServiceContract]
    public interface IFileManagerService
    {
        /// <summary>
        /// GetFiles()
        /// Returns a list of files in the server directory
        /// </summary>
        /// <returns>Returns a list of files (List<FileInfo>) in the server directory
        /// with each list item being a FileInfo object</returns>
        [OperationContract]
        List<FileInformation> GetFiles();

        /// <summary>
        /// GetFile(string fileName)
        /// Stream downloads a specified file from the server to the client
        /// </summary>
        /// <param name="fileName">the file name (string) to download</param>
        /// <returns>the file (Stream)</returns>
        [OperationContract]
        Stream GetFile(string fileName);

        /// <summary>
        /// AddFile
        /// Uses the chunk process and style of uploading a file from a client
        /// to a server.
        /// </summary>
        /// <param name="fileName">file name (string) to add from client to server</param>
        /// <returns>configured size (int) of chunk</returns>
        [OperationContract]
        int AddFile(string fileName);

        /// <summary>
        /// AddFileChunk
        /// This method takes care of adding file chunks on the server for
        /// the client to upload/add their file
        /// </summary>
        /// <param name="fileName">the file name (string) of the chunk</param>
        /// <param name="chunk">the chunk identifier (int)</param>
        /// <param name="data">the chunk data byte array (byte[])</param>
        [OperationContract]
        void AddFileChunk(string fileName, int chunk, byte[] data);

        /// <summary>
        /// CompleteAddFile
        /// Call from the client to the server to indicate that all
        /// chunks are completed
        /// </summary>
        /// <param name="fileName">file name (string)</param>
        /// <param name="chunks">number of chunk segments (int)</param>
        [OperationContract]
        void CompleteAddFile(string fileName, int chunks);

    } // end of interface
} // end of namespace
