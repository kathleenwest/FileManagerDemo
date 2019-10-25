using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;
using System.Diagnostics;
using System.Timers;
using System.Threading;
using SharedLibrary;
using System.IO;

namespace Client
{
    public partial class Client : Form
    {
        #region fields

        // Monitoring for Cancellation
        private CancellationTokenSource m_TokenSource = null;

        // Selected Remote File
        private static FileInformation selectedRemoteFile;

        // Selected Local File
        private static FileInformation selectedLocalFile;

        // Constant for Stream Buffer Size
        private const int BUFFER = 5000;

        // List of Current Local Files
        private static List<FileInformation> myLocalFiles = new List<FileInformation>();

        // List of Current Remote Files
        private static List<FileInformation> remoteFiles = new List<FileInformation>();

        // Private path string to local directory
        // In MyDocuments (Personal) Folder + FileManagerLocal
        private static string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FileManagerLocal");

        #endregion fields

        #region properties

        // Local Directory Property
        // Example: C:\Users\kathl\OneDrive\Documents\FileManagerLocal
        public static string LocalDirectory
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
                    throw new FaultException($"Cannot Create Directory {ex.Message}");
                }
            }
            set
            {
                // Validate the Path
                if (Directory.Exists(value))
                {
                    path = value;
                }
                else
                {
                    throw new FaultException("Invalid Directory Set");
                }
            }

        } // end of Property

        #endregion properties

        #region constructor

        public Client()
        {
            InitializeComponent();

            // Initialize my GUI Buttons to Enabled
            UpdateGUIButtons(ServiceMenu.Load, true);

            // Update the Local Files
            UpdateLocalFiles();

            // Clear the Remote Files
            lstRemoteFiles.Items.Clear();
        
        } // end of constructor

        #endregion constructor

        #region events

        /// <summary>
        /// ListFiles
        /// Processes the button click event
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void btnListFiles_Click(object sender, EventArgs e)
        {
            // Update the GUI Buttons for this menu action enum
            UpdateGUIButtons(ServiceMenu.ListFiles, false);

            // Create Action Delegate
            // Target is the Method Name
            Action method = new Action(ListFiles);

            // Start Task, Passing Action Delegate and
            // Menu Action Item
            StartNewTask(method, ServiceMenu.ListFiles);

        } // end of method

        /// <summary>
        /// GetFile
        /// Processes the button click event
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void btnGetFile_Click(object sender, EventArgs e)
        {

            // Validate there is a file Selected
            if (lstRemoteFiles.SelectedIndex < 0)
                return;

            // Record which file was selected
            selectedRemoteFile = (FileInformation)lstRemoteFiles.SelectedItem;

            // Update the GUI Buttons for this menu action enum
            UpdateGUIButtons(ServiceMenu.GetFile, false);

            // Create Action Delegate
            // Target is the Method Name
            Action method = new Action(GetFile);

            // Start Task, Passing Action Delegate and
            // Menu Action Item
            StartNewTask(method, ServiceMenu.GetFile);

        } // end of method

        /// <summary>
        /// AddFile
        /// Processes the button click event
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void btnAddFile_Click(object sender, EventArgs e)
        {
            // Validate there is a file Selected
            if (lstLocalFiles.SelectedIndex < 0)
                return;

            // Record which file was selected
            selectedLocalFile = (FileInformation)lstLocalFiles.SelectedItem;

            // Update the GUI Buttons for this menu action enum
            UpdateGUIButtons(ServiceMenu.AddFile, false);

            // Create Action Delegate
            // Target is the Method Name
            Action method = new Action(AddFile);

            // Start Task, Passing Action Delegate and
            // Menu Action Item
            StartNewTask(method, ServiceMenu.AddFile);

        } // end of method

        /// <summary>
        /// Cancel
        /// Processes the button click event
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // if a valid token source is available then signal 
            // to any listeners that the Task has been cancelled
            if (m_TokenSource != null)
            {
                // Send Cancel to Token
                m_TokenSource.Cancel();

            } // end of if
        } // end of Cancel event

        /// <summary>
        /// Local Refresh Button Click
        /// Processes the button click
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void btnLocalRefresh_Click(object sender, EventArgs e)
        {
            UpdateLocalFiles();
        }

        #endregion events

        #region methods

        /// <summary>
        /// StartNewTask
        /// Starts a new thread task for the service request 
        /// </summary>
        /// <param name="method">the method target (Action delegate)</param>
        /// <param name="menu">the type of request enum (ServiceMenu)</param>
        private void StartNewTask(Action method, ServiceMenu menu)
        {
            // set the CancelationTokenSource object to a new instance
            m_TokenSource = new CancellationTokenSource();

            // When Tasks complete, TPL provides a mechanism to automatically 
            // continue execution on a WinForm or WPF UI thread.To do this 
            // we need a handle to the UI thread which we’ll use later
            TaskScheduler ui = TaskScheduler.FromCurrentSynchronizationContext();

            // start the Task, passing it the name of the method that 
            // will be the entry point for the Task and the token used for cancellation:
            Task myServiceCallTask = Task.Run(method, m_TokenSource.Token);

            // first continuation call will execute only when the Task completes successfully
            // notifies the user by showing a message box then resetting the buttons to their 
            // default configuration. Notice that the last parameter to ContinueWith is “ui”. 
            // This tells ContinueWith to execute the lambda statements to execute within 
            // the context of the UI thread.No Invoke / BeginInvoke needed here.
            Task resultOK = myServiceCallTask.ContinueWith(resultTask =>
            {
                // Update GUI After Task is Complete
                // Refresh the Remote Files
                UpdateRemoteFiles();

                // Refresh the Local Files
                UpdateLocalFiles();

                // Reset this menu action button
                UpdateGUIButtons(menu, true);

                // Refresh the Remote Directory
                if(menu == ServiceMenu.AddFile)
                {
                    btnListFiles_Click(this, null);
                }

            }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, ui);

            // second continuation call only executes if the task is  cancelled
            Task resultCancel = myServiceCallTask.ContinueWith(resultTask =>
            {
                // Update GUI After Cancellation
                // Refresh the Remote Files
                UpdateRemoteFiles();

                // Refresh the Local Files
                UpdateLocalFiles();

                // Reset this menu action button
                UpdateGUIButtons(menu, true);

            }, CancellationToken.None, TaskContinuationOptions.OnlyOnCanceled, ui);
        } // end of method

        /// <summary>
        /// CallService
        /// Creates a ChannelFactory proxy to the service
        /// Listens for Cancellation Event from user clicking cancel
        /// Called from the Async Task from the UI Thread
        /// Get resources from the service (when connected)
        /// </summary>
        private void CallService(ServiceMenu menuSelection)
        {
            try
            {
                // Make a ChannelFactory Proxy to the Service
                using (ChannelFactory<IFileManagerService> cf = new ChannelFactory<IFileManagerService>("NetTcpBinding_IFileManagerService"))
                {
                    cf.Open();
                    IFileManagerService proxy = cf.CreateChannel();

                    if (proxy != null)
                    {
                        // check to see if cancellation was requested
                        // if (m_TokenSource.Token.IsCancellationRequested) throw new OperationCanceledException();
                        m_TokenSource.Token.ThrowIfCancellationRequested();

                        try
                        {
                            // Call the Service 
                            switch (menuSelection)
                            {
                                case ServiceMenu.ListFiles:
                                    remoteFiles = proxy.GetFiles();
                                    break;
                                case ServiceMenu.GetFile:
                                    #region GetFile

                                    string fileName = selectedRemoteFile.FileName;
                                    string outputPath = LocalDirectory;
                                    string outputFileName = Path.Combine(outputPath, fileName);

                                    using (Stream s = proxy.GetFile(fileName))
                                    {
                                        if (s == null)
                                        {
                                            throw new Exception("Stream is empty.");
                                        }

                                        int byteCount = 0;
                                        int offset = 0;
                                        using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
                                        {
                                            do
                                            {
                                                byte[] bytes = new byte[BUFFER];
                                                byteCount = s.Read(bytes, 0, BUFFER);
                                                offset += byteCount;
                                                fs.Write(bytes, 0, byteCount);
                                            }
                                            while (byteCount > 0);

                                        } // end of using
                                    } // end of using

                                    #endregion GetFile
                                    break;
                                case ServiceMenu.AddFile:
                                    #region AddFile

                                    string inputPath = LocalDirectory;
                                    fileName = selectedLocalFile.FileName;
                                    string inputFileName = Path.Combine(inputPath, fileName);

                                    if (!File.Exists(inputFileName))
                                    {
                                        return;
                                    }
                                    FileInfo fi = new FileInfo(inputFileName);

                                    int chunkSize = proxy.AddFile(fi.Name);
                                    int chunks = (int)(fi.Length / chunkSize);
                                    int rem = (int)(fi.Length % chunkSize);

                                    using (FileStream fs = fi.Open(FileMode.Open, FileAccess.Read))
                                    {
                                        byte[] bytes = new byte[chunkSize];
                                        for (int i = 0; i < chunks; i++)
                                        {
                                            fs.Seek((long)i * (long)chunkSize, SeekOrigin.Begin);
                                            fs.Read(bytes, 0, chunkSize);
                                            proxy.AddFileChunk(fi.Name, i, bytes);
                                        }
                                        if (rem > 0)
                                        {
                                            byte[] remBytes = new byte[rem];
                                            fs.Seek((long)chunks * (long)chunkSize, SeekOrigin.Begin);
                                            fs.Read(remBytes, 0, rem);
                                            proxy.AddFileChunk(fi.Name, chunks, remBytes);
                                        }
                                    } // end of using

                                    // Signal all file chunks uploaded
                                    proxy.CompleteAddFile(fi.Name, chunks + (rem > 0 ? 1 : 0));

                                    break;

                                #endregion AddFile
                                default:
                                    break;
                            } // end of switch

                        } // end of try

                        // when the user cancels the Task
                        catch (OperationCanceledException)
                        {
                            throw;
                        } // end of catch

                        catch (Exception)
                        {
                            // Cancel the Task
                            btnCancel_Click(this, null);
                        } // end of catch

                    } // end of if

                    else
                    {
                        // Cannot Connect to Server 
                        MessageBox.Show("Cannot Create a Channel to a Proxy. Check Your Configuration Settings.", "Proxy", MessageBoxButtons.OK);
                        return;
                    } // end of else

                } // end of using
            } // end of main try

            // when the user cancels the Task
            catch (OperationCanceledException)
            {
                throw;
            } // end of catch

        } // end of method

        /// <summary>
        /// AddFile
        /// Calls the Service to Add a File
        /// </summary>
        private void AddFile()
        {
            CallService(ServiceMenu.AddFile);
        }

        /// <summary>
        /// GetFile
        /// Calls the Service to Get a File
        /// </summary>
        private void GetFile()
        {
            CallService(ServiceMenu.GetFile);
        }

        /// <summary>
        /// ListFiles
        /// Calls the Service
        /// </summary>
        private void ListFiles()
        {
            // The Service calls will update the in-memory static resources
            // The file listings of the local and remote

            // List Remote Files
            CallService(ServiceMenu.ListFiles);

        } // end of method

        /// <summary>
        /// UpdateGUIButtons
        /// </summary>
        /// <param name="menu">A selection list enum (ServiceMenu)</param>
        /// <param name="enabled">if the button should be enabled (true or false) </param>
        private void UpdateGUIButtons(ServiceMenu menu, bool enabled)
        {
            switch (menu)
            {
                case ServiceMenu.ListFiles:
                    if (enabled)
                    {
                        btnListFiles.Enabled = true;
                        btnListFiles.Focus();
                        btnCancel.Enabled = false;
                    }
                    else
                    {
                        btnListFiles.Enabled = false;
                        btnCancel.Enabled = true;
                    }
                    break;
                case ServiceMenu.GetFile:
                    if (enabled)
                    {
                        btnGetFile.Enabled = true;
                        btnGetFile.Focus();
                        btnCancel.Enabled = false;
                    }
                    else
                    {
                        btnGetFile.Enabled = false;
                        btnCancel.Enabled = true;
                    }
                    break;
                case ServiceMenu.AddFile:
                    if (enabled)
                    {
                        btnAddFile.Enabled = true;
                        btnAddFile.Focus();
                        btnCancel.Enabled = false;
                    }
                    else
                    {
                        btnAddFile.Enabled = false;
                        btnCancel.Enabled = true;
                    }
                    break;
                case ServiceMenu.Load:
                default:                    
                    if (enabled)
                    {
                        lblTitle.Focus();
                        btnListFiles.Enabled = true;
                        btnAddFile.Enabled = true;
                        btnGetFile.Enabled = true;
                        btnCancel.Enabled = false;
                    }
                    else
                    {
                        btnListFiles.Enabled = false;
                        btnAddFile.Enabled = false;
                        btnGetFile.Enabled = false;
                        btnCancel.Enabled = true;
                    }
                    break;
            } // end of switch

        } // end of menu

        /// <summary>
        /// UpdateLocalFiles
        /// Updates the List Box Area of Local Files
        /// In a Set Directory Path
        /// </summary>
        private void UpdateLocalFiles()
        {
            // First Clear the List Box
            lstLocalFiles.Items.Clear();

            // Clear the List
            myLocalFiles.Clear();

            // Get Files From Directory
            string[] allFiles = Directory.GetFiles(LocalDirectory);
            foreach (string file in allFiles)
            {
                FileInfo fi = new FileInfo(file);
                FileInformation fm = new FileInformation(fi.Name, fi.Length);
                // Add Item to List
                myLocalFiles.Add(fm);

                // Add Item to List Text Box
                lstLocalFiles.Items.Add(fm);
            } // end of foreach
        } // end if method

        /// <summary>
        /// UpdateRemoteFiles
        /// Updates the List Box Area of Remote Files
        /// In a Set Directory Path
        /// </summary>
        private void UpdateRemoteFiles()
        {
            // First Clear the List Box
            lstRemoteFiles.Items.Clear();
            
            // Update the Box
            foreach (FileInformation file in remoteFiles)
            {
                // Add Item to List Text Box
                lstRemoteFiles.Items.Add(file);

            } // end of foreach
        } // end if method

        #endregion methods

    } // end of class
} // end of namespace
