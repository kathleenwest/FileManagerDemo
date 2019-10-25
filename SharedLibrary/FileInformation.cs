using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace SharedLibrary
{
    /// <summary>
    /// FileInfo
    /// Represents a file object with the 
    /// name and size with the intent of common
    /// OOP class between server and client
    /// </summary>
    [DataContract]
    public class FileInformation
    {
        #region Properties

        // Name of File
        [DataMember]
        public string FileName { get; set; }

        // Size of File (Bytes)
        [DataMember]
        public long FileSize { get; set; }

        #endregion Properties

        #region constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public FileInformation()
        {
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="fileName">name of file (string)</param>
        /// <param name="fileSize">size of file (bytes)</param>
        public FileInformation(string fileName, long fileSize)
        {
            FileName = fileName;
            FileSize = fileSize;
        }

        #endregion constructors

        #region methods

        /// <summary>
        /// String Override of Object
        /// </summary>
        /// <returns>Overrides the Object ToString Representation</returns>
        public override string ToString()
        {           
            return string.Format("{0} {1}", FileName.PadRight(20, ' '), FormatSize(FileSize).PadLeft(10, ' '));
        }

        /// <summary>
        /// FormatSize
        /// Returns the formatted string representation of the data
        /// input (bytes) to a more concise value (ex: GB)
        /// </summary>
        /// <param name="value">value of filesize (bytes)</param>
        /// <returns>formatted string of the filesize by category</returns>
        public static string FormatSize(decimal value)
        {
            int groups = 0;
            string suffix = string.Empty;

            while (value > 1024M)
            {
                value /= 1024M;
                groups++;
            }

            switch (groups)
            {
                case 0:
                    suffix = "Bytes";
                    break;
                case 1:
                    suffix = "KB";
                    break;
                case 2:
                    suffix = "MB";
                    break;
                case 3:
                    suffix = "GB";
                    break;
                case 4:
                    suffix = "TB";
                    break;
                default:
                    suffix = "NOPE";
                    break;
            }

            if (groups == 0)
            {
                return string.Format("{0} {1}", value, suffix);
            }
            else
            {
                return string.Format("{0:0.0} {1}", value, suffix);
            }
        } // end of method

        #endregion methods

    } // end of class
} // end of namespace
