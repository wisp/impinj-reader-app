
* This file contains general information of LLRP Tool Kit for .Net
* Last updated 21 Jan 2008


Note: 

All the projects will be loaded through LTKNet.sln.

1. To build the LLRP project, you need copy the llrp-1x0-def.xml or latest verion to LLRP directory
2. To build the LLRPVendorExt project, you need to copy a LLRP schema compatible vendor extesion 
   definition file to LLRPVendorExt directory. The namespace of the vendor extension is LLRP.xxxxxx. The dll file
   name should be LLRP.xxxxxx.dll accordingly.
3. When load the solution file in Visual Studio, there might be an alert. You need to choose "Load Normally".
4. The IDE shows several missed files. They will be generated once you build the project.
5. LLRPTest project XML parser parses single message
6. Projects only work on LTK schema 1.0
7. If the code generator generates wrong code, it is possibly that the LLRP definition file is wrong. Please delete
   generated files: LLRPClient.cs, LLRPEndPoint.cs, LLRPEnum.cs, LLRPMsg.cs, LLRPParam.cs, and LLRPXmlParser.cs from
   the project folder. Note: not from IDE.


INTRODUCTION
============

This is a high level overview of and orientation to
the LLRP Tool Kit for .Net (LTKNet) on Windows.

The most recent versions of and news about the
LLRP Tool Kit is available on SourceForge.net

    http://sourceforge.net/projects/llrp-toolkit/

This document is organized:
    OVERVIEW 			-- a summary for an apps programmer
    COMPATIBILITY		-- compatibility information
    LTKNet KIT STRUCTURE 	-- LTKNet structure
    MESSAGE STATES 		-- message state transition diagram
    USE SOURCE CODE 		-- how to use this source code
    TODO LIST			--	


OVERVIEW
==========

    - There is a .Net class definition for each LLRP
      message and parameter type
    - Each class contains methods including FromBitArray, ToBitArray, 
      FromString(XmlNode), ToString. Thus it can be decoded/encoded by itself
    - LLRPEndPoint supports end-to-end programming mode
    - LLRPClient supports client programming mode. It uses event and 
      synchronized message/response
    - Messages from the reader are decoded into object trees. It can then be
      encoded into XML string
    - An application constructs messages as "object trees" or from a XML string
    - Applications can traverse the object trees to retrieve results and tag data
    - Message factory is used to generate message from xml string
    - Type check in IDE is enforced


PREREQUESTS
===========

The source codes are compiled and tested in Microsoft Visual Studio 2005 standard 
version.

COMPATIBILITY
=============

It requires .Net framework 2.0 or up. 

 



LTKNet KIT STRUCTURE
====================

LTKNet kit contains five major modules. 

      - LLRP is the LLRP base library. It contains definitions of all LLRP messages and parameters. 
	Interfaces, base class of custom parameters and message are defined. Vendors are encoraged 
        to derive their extension from these interfaces and base classes.
      - LLRPTest is a GUI based LLRP example and test utility. It contains ROSpec operation commands,
        ACCESSSpec operation commands, reader configuration, capability, and XML encoding/decoding.
      - TestData contains four test files.
      - LLRPVendorExt is optional vendor extension implementation. 


* represents code generated code

LTKNet-
      |- LTKNet.sln           	: Solution file
      |- README.TXT		: This file
      |- RELEASE NOTES.TXT	: Release notes
      |- LLRP 			: LLRP base libary	
      |- LLRPTest		: LLRP .Net example & test utility
      |- LLRPEndPointServerTest : LLRP xml test data
      |- LLRPVendorExt		: LLRP Vendor extension



LLRP--
      |- LLRPParam.xslt 	  : XSL transform stylesheet for parameters
      |- LLRPMsg.xslt	  	  : XSL transform stylesheet for messages
      |- LLRPEndPoint.xslt	  : XSL transform stylesheet for LLRPEndPoint
      |- LLRPClient.xslt	  : XSL transform stylesheet for parameters
      |- LLRPXmlParser.xslt       : XSL transform stylesheet for general xml parser
      |- LLRPEnum.xslt      	  : XSL transform stylesheet for enumerations
      |- templates.xslt		  : XSL transform general templates
      |- Transaction.cs		  : Simple TCPIP send and receive class
      |- TCPIPConnection.cs       : LLRP TCPIP client and Server implementation
      |- LLRPUtil.cs		  : Data type conversion
      |- LLRPDataType.cs	  : LLRP supporting data types
      |- Customparameters.cs	  : interfaces and base classes for LLRP custom parameters
      |- CommunicationInterface.cs: Communication interface base class
      |- LLRP.csproj		  : project file
      |- LLRPHelper.cs		  : Helper classes
      |- CodeGenerator.dll	  : Code generator



LLRPVendorExt-
      |- VendorExt.xml     	  : Vendor LLRP extension definitions
      |- templates.xslt		  : XSL transform general templates
      |- VendorExt.xslt	  	  : Vendor LLRP extension XSL tranform stylesheet
      |- CodeGenerator.dll	  : Code generator
      |- LLRPImpinjExt.csproj     : project file


LLRPTest
      |- MainFrm.cs		  : GUI 
      |- MainFrm.resx		  : Resource
      |- MainFrm.Designer.cs	  : GUI layout
      |- Program.cs		  : Program entry
      |- LLRPTest.csproj 	  : Project file   

LLRPEndPointServerTest
      |- MainFrm.cs		  : GUI 
      |- MainFrm.resx		  : Resource
      |- MainFrm.Designer.cs	  : GUI layout
      |- Program.cs		  : Program entry
      |- LLRPEndPointServerTest.csproj : Project file
   



LLRP MESSAGE STATES
===================


LLRP .Net library supports both xml-based or object(binary)-based programming
approaches. messages that flows from reader to application or from application 
to reader have following states.  XML state is optional in an application.

message states: from application to reader

----------      ----------      ----------
|        |      |        |      |        |
|   XML  | ---> |   Obj  | ---->|  Bin   |
|        |      |        |      |        |
----------      ----------      ----------

message states: from reader to application

----------      ----------      ----------
|        |      |        |      |        |
|  Bin   | ---> |  Obj   | ---->|  XML   |
|        |      |        |      |        |
----------      ----------      ----------



USE SOURCE CODE
===============

All projects can be loaded from LTKNet.sln solution file.

	- For end application programmer, follow the source code of LLRPTest project. (package)
	- For LLRP driver developer, follow LLRP and LLRPNetCodeGenerator.


TODO LIST
=========
