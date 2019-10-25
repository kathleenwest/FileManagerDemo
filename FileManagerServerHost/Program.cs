using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using FileManagerServiceLibrary;


namespace FileManagerServerHost
{
    /// <summary>
    /// This program launches the file manager
    /// service by creating a host and starting the service.
    /// After it is started it will be available to clients until
    /// the user terminates the service. 
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main entry for Service Host
        /// </summary>
        /// <param name="args">no input arguments used</param>
        static void Main(string[] args)
        {
            ServiceHost myServiceHost = null;
            try
            {
                myServiceHost = new ServiceHost(typeof(FileManagerService));

                Console.WriteLine("Starting file management service...");
                myServiceHost.Open();
                Console.WriteLine("File management service started.");

                Console.Write("Press <ENTER> to quit...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                System.Reflection.MethodBase mb = System.Reflection.MethodBase.GetCurrentMethod();
                Console.WriteLine("{0}.{1}.{2}: {3}", mb.DeclaringType.Namespace, mb.DeclaringType.Name, mb.Name, ex.Message);
                Console.Write("Press <ENTER> to quit...");
                Console.ReadLine();
            }
            finally
            {
                if (myServiceHost != null)
                {
                    try
                    {
                        // Dispose of the resource
                        myServiceHost.Close();
                    }
                    catch
                    {
                        myServiceHost.Abort();
                    }
                }
            }
        } // end of main method

    } // end of class
} // end of namespace
