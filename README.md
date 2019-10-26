# FileManagerDemo
File Manager – A Demo of a WCF Self-Hosted Service & Client "Tester" Windows Form Application Exchanging Files


Project Article:

https://portfolio.katiegirl.net/2019/10/26/file-manager-a-demo-of-a-wcf-self-hosted-service-client-tester-windows-form-application-exchanging-files/


About


This project presents a simple File Manager Service and Client Application demonstration. The File Manager is a self-hosted (service host) WCF application launched and managed with a simple console interface. The client “tester” has a simplified GUI user interface to quickly demo and test the service (Windows Form Application).



Architecture 

 
The demo project consists of these component topics:


•	Shared Class Library Project “SharedLibrary”
o	IFileManagerService (Interface for Service)
o	FileInformation (Class Describing a File Object)


•	Service Class Library Project “FileManagerServiceLibrary”
o	FileManagerService (Code that Implements the Service Interface)
o	App.config (Configuration Reference for Service Host)
o	Reference to the Shared Class Library


•	WCF Service (Host) Application Project “FileManagerServerHost”
o	Program (Starts, Manages, Stops the Service)
o	App.config (Configuration for Service Host)
o	Reference to the FileManagerServiceLibrary


•	Client “Tester to Service” Windows Form Application Project “Client”
o	Reference to the Shared Class Library
o	Main Form GUI User Interface
o	Form Code – Processes GUI User Interface

The service interface is defined not in the service application but in a Shared Library. This library defines the interface contracts for the file manager services (ex: Get File) and is referenced by both the client and service host projects.  The SharedLibrary also has a class definition that defines a file object with a name and size property.  


The FileManagerServiceLibrary implements the File Manager service and contracts as defined in the SharedLibrary. The FileManagerServerHost is a simple console application that is responsible for starting the File Manager service, hosting, and managing the service (self-hosted). 
The service behaviors were designed to allow multiple threads (concurrency) while each client call to the service spins up a new service instance (PerCall). Although this project is not meant to demo concurrency behaviors, I would recommend referencing “The Money Pot Problem” [link] to read my discussion on service behaviors and concurrency. 


A client “tester” windows form application tests the service and provides output to the user in a simple GUI.   
In addition, there will be a short discussion of streaming and chunking techniques used by the project to download and upload files. Also, the client application implements asynchronous programming techniques in creating separate threads while allowing for cancellation.

