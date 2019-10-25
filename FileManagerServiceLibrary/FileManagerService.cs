using SharedLibrary;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;

namespace FileManagerServiceLibrary
{
    /// <summary>
    /// FileManagerService
    /// Implements the File Manager Service Interface
    /// Service Behaviors: 
    /// Allows for concurrency and multi-threading
    /// Creates a new service instance for each method call 
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class FileManagerService : IFileManagerService
    {
        // constant value of chunk size
        const int CHUNK_SIZE = 10000;

        // Private path string to local directory
        // In MyDocuments (Personal) Folder + FileManagerService
        private static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FileManagerService");

        /// <summary>
        /// Default Constructor
        /// </summary>
        public FileManagerService()
        {

        }

        /// <summary>
        /// FilePath Property
        /// Returns a fixed directory path to a tempoary folder on a user's computer
        /// This path will be the main directory where the server hosts the files
        /// and receives files from the client
        /// In MyDocuments (Personal) Folder + FileManagerLocal
        /// </summary>
        private string FilePath
        {
            get
            {
                try
                {                   
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    return path;
                }
                catch (Exception ex)
                {
                    throw new FaultException(ex.Message);
                }
            }
        } // end of Property

        /// <summary>
        /// GetFiles()
        /// Gets the list of all files in the file manager directory
        /// </summary>
        /// <returns>List of files (FileInfo object) on server in the directory</returns>
        public List<FileInformation> GetFiles()
        {
            try
            {
                List<FileInformation> files = new List<FileInformation>();
                string[] allFiles = Directory.GetFiles(FilePath);
                foreach (string file in allFiles)
                {
                    FileInfo fi = new FileInfo(file);
                    files.Add(new FileInformation(fi.Name, fi.Length));
                }
                return files;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        } // end of method

        /// <summary>
        /// AddFile
        /// Creates a staging directory and returns the default chunk size
        /// </summary>
        /// <param name="fileName">Name of file (string) to upload</param>
        /// <param name="fileSize">Size of file (bytes) to upload</param>
        /// <returns>Configured size of chunk (int) representing bytes </returns>
        public int AddFile(string fileName)
        {
            try
            {
                // Create temp directory for file chunks
                Directory.CreateDirectory(Path.Combine(FilePath, fileName + "_TEMP"));

                return CHUNK_SIZE;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        } // end of method

        /// <summary>
        /// AddFileChunk
        /// Adds a file chunk to the stage directory for the file chunks
        /// </summary>
        /// <param name="fileName">Name of file (string) being uploaded</param>
        /// <param name="chunk">Chunk number (int) </param>
        /// <param name="data">Data for the given chunk (byte[])</param>
        public void AddFileChunk(string fileName, int chunk, byte[] chunkData)
        {
            try
            {
                string path = Path.Combine(FilePath, fileName + "_TEMP");

                // creates the staging directory if it does not already exist
                if (!Directory.Exists(path))
                {
                    throw new Exception("Directory not found!");
                }

                // Adds the file chunk to the staging directory
                using (FileStream fs = File.Open(Path.Combine(path, string.Format("{0:00000000}.chunk", chunk)), FileMode.Create, FileAccess.Write))
                {
                    fs.Write(chunkData, 0, chunkData.Length);
                    fs.Flush();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                System.Reflection.MethodBase mb = System.Reflection.MethodBase.GetCurrentMethod();
                System.Diagnostics.EventLog.WriteEntry("FileManagerService", string.Format("{0}.{1}.{2}: {3}", mb.DeclaringType.Namespace, mb.DeclaringType.Name, mb.Name, ex.Message));
                throw new FaultException(ex.Message);
            }

        } // end of method

        /// <summary>
        /// CompleteAddFile
        /// Call from client to server to indicate all chunks are complete
        /// Turns the chunks into a single file
        /// Deletes the chunk files and the staging directory
        /// </summary>
        /// <param name="fileName">Name of file (string) being uploaded</param>
        /// <param name="chunks">Total number of chunks (int)</param>
        public void CompleteAddFile(string fileName, int chunks)
        {
            try
            {
                string path = Path.Combine(FilePath, fileName + "_TEMP");

                // make sure the staging directory is available
                if (!Directory.Exists(path))
                {
                    throw new Exception("Directory not found!");
                }

                // Wait for all files to be written
                // Because this could be done in a multithreaded process
                // Multiple threads creating different chunks (to speed up the upload)
                // Verify all the chunk files are there before proceeding further...
                while (true)
                {
                    System.Threading.Thread.Sleep(500);
                    string[] files = Directory.GetFiles(path);
                    if (files.Length == chunks)
                    {
                        break;
                    }
                } // end of while loop

                // File stream the chunks to one file destination
                using (FileStream fsDest = new FileStream(Path.Combine(FilePath, fileName), FileMode.Create, FileAccess.Write))
                {
                    for (int i = 0; i < chunks; i++)
                    {
                        using (FileStream fs = File.Open(Path.Combine(path, string.Format("{0:00000000}.chunk", i)), FileMode.Open, FileAccess.Read))
                        {
                            byte[] bytes = new byte[CHUNK_SIZE];
                            int read = fs.Read(bytes, 0, CHUNK_SIZE);
                            fsDest.Write(bytes, 0, read);
                            fs.Close();
                        } // end of inner using
                    } // end of for
              
                    fsDest.Flush();
                    fsDest.Close();
                } // end of using

                // Clean up tempory files and delete the staging directory
                Directory.Delete(path, true);
            }
            catch (Exception ex)
            {
                System.Reflection.MethodBase mb = System.Reflection.MethodBase.GetCurrentMethod();
                System.Diagnostics.EventLog.WriteEntry("FileManagerService", string.Format("{0}.{1}.{2}: {3}", mb.DeclaringType.Namespace, mb.DeclaringType.Name, mb.Name, ex.Message));
                throw new FaultException(ex.Message);
            }
        } // end of method

        /// <summary>
        /// GetFile
        /// Downloads a file (stream) from server to client
        /// </summary>
        /// <param name="fileName">Name of file (string) to stream</param>
        /// <returns>Stream containing the file data</returns>
        public Stream GetFile(string fileName)
        {
            try
            {
                string path = Path.Combine(FilePath, fileName);

                // make sure the file path and file exists
                if (!File.Exists(path))
                {
                    throw new Exception("File not found!");
                }

                // return a stream access to the file
                return File.Open(path, FileMode.Open, FileAccess.Read);
            }
            catch (Exception ex)
            {
                System.Reflection.MethodBase mb = System.Reflection.MethodBase.GetCurrentMethod();
                System.Diagnostics.EventLog.WriteEntry("FileManagerService", string.Format("{0}.{1}.{2}: {3}", mb.DeclaringType.Namespace, mb.DeclaringType.Name, mb.Name, ex.Message));
                throw new FaultException(ex.Message);
            }
        } // end of method

    } // end of class
} // end of namespace
