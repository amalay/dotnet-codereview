# Automation of Coding Standards for .Net using StyleCop

While writing c# code, it is very difficult to ensure that every developer is following coding standards guideline. Forget the advanced level of coding standards even we have experienced that basic level of coding standards are not followed properly in the project. And the major reason behind this is the manual process of forcing team members to follow the coding standards some time developer minds it and taking negatively. It is better idea to force developer by machine rather than human to follow coding standards in their code. For this there are couple of tools which are used to automate the coding standards process such as 

1.	StyleCop
2.	FxCop
3.	Code Analysis etc.

All these tools have some good and bad thing with own.

Here this document is going to contain all about StyleCop.

<b>StyleCop</b> is a tool from Microsoft (an open Source Tool btw.) to analyse the Source Code. It works on C# source files directly rather than .DLLs.

<b>StyleCop</b> defined a set of rules and on the basis of these rules every c# file is evaluated to ensure that the code written in the file must follow the coding standards.

### Installation of StyleCop
1.	Download ReSharper (ReSharperSetup.8.2.0.2160) and install it before installing StyleCop.
2.	Download StyleCop (StyleCop-4.7.49.0) and install it.

### How to execute StyleCop from Visual Studio after installing it:

After installed StyleCop to your machine, you can see it inside tool menu of the visual studio or right click window of the project as shown below:

![image](https://user-images.githubusercontent.com/84455469/132862270-5ac5ef80-7444-4fe6-b37a-8d7571155046.png)

To run StyleCop, you have to click on Run StyleCop or Run StyleCop (Rescan All) button. Alternatively you can run StyleCop by right clicking on the c# file and Run StyleCop button.

#### Example:

1.	Open Visual Studion ----> Create any project ---> Create any file ---> Right click mouse button over file ---> Click on Run StyleCop

![image](https://user-images.githubusercontent.com/84455469/132862401-290b1251-eb17-4aac-9d32-27f89d3bc88f.png)

Here we have just created new file and run it for StyleCop without writing any line of code in this file and we get 0 Error, 11 Warnings and 0 Message as below in table/screenshot.

![image](https://user-images.githubusercontent.com/84455469/132862510-7bc40d59-7423-4209-a1f5-4db3a861314d.png)

![image](https://user-images.githubusercontent.com/84455469/132862547-c581f553-454c-4979-aa64-fb74190ba194.png)


#### Importent: 
Here we are seeing that developer need to execute StyleCop separately to validate StyleCop violations. And I believe no one developer will do this action explicitly that’s why we have to integrate StyleCop with MSBuild so that StyleCop automatically run whenever developer builds the application.

#### Integration of StyleCop with MSBuild

#### 1.	Installing MSBuild Files

To enable build integration, first be sure to select the MSBuild option when installing the tool, as shown in the image below:

![image](https://user-images.githubusercontent.com/84455469/132862750-245b107b-7d71-4b9c-80fa-50db0fe49415.png)

This will cause the StyleCop binaries and supporting MSBuild targets files to be installed under the Program Files (x86)\MSBuild\StyleCop folder OR C:\Program Files (x86)\StyleCop 4.7 folder

#### 2.	Adding the Import Tag

Once the StyleCop MSBuild files are installed, the next step is to import the StyleCop targets file into your C# projects. This is done by adding an Import tag to each C# project file.

For example, to integrate StyleCop to the project SampleProject, open the project file SampleProject.csproj within your favorite text editor. Scroll down to the bottom of the file and add a new tag to import the <b>StyleCop.targets</b> file. This import tag should be added just below the import of Microsoft.CSharp.targets:

```
<Project ToolsVersion="3.5" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">  	
		.
		.
	<Import Project="$(MSBuildBinPath)\Microsoft.CSharp.targets" />
	<Import Project="$(MSBuildExtensionsPath)\Microsoft\VisualStudio\v9.0\WebApplications\Microsoft.WebApplication.targets" />
	<Import Project="StyleCop\StyleCop.Targets" />
		.
		.
</Project>
```

Now save the modified .csproj file. 

Now when you build this project either within Visual Studio or on the command line; StyleCop will run automatically against the entire C #source files within the project and StyleCop violations will appear as build warnings into the warning windows.

Comments: By default StyleCop violations will be displayed as build warnings and again I am very confident that no one developer will take care these warnings seriously. So we have to force developer to resolve these warnings at any cost before checked in the code. To achieve this we have to show StyleCop Violations as build errors instead of build warnings in to error window.

Showing StyleCop violations as build errors instead of warnings

To turn StyleCop violations into build errors, the flag StyleCopTreatErrorsAsWarnings must be set to false.  This flag can be set as an environment variable on the machine, or within the build environment command window. Setting the flag this way will cause StyleCop violations to appear as build errors automatically for all projects where StyleCop build integration is enabled.

Alternately, this flag can be set within the project file for a particular project. Open the .csproj file for your project again, and find the first PropertyGroup section within the file. Add a new tag to set the <b>StyleCopTreatErrorsAsWarnings</b> flag to false. For example:

```
<Project ToolsVersion="3.5" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">  	
	<PropertyGroup>
		<Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    		.
		.
		<RootNamespace>Sample_02</RootNamespace>
		<AssemblyName>Sample_02</AssemblyName>
		<TargetFrameworkVersion>v3.5</TargetFrameworkVersion>
		<StyleCopTreatErrorsAsWarnings>false</StyleCopTreatErrorsAsWarnings>
	</PropertyGroup>	
</Project>
```

Now if we build the application, all the StyleCop violations will be displayed as build errors in error window as shown below:

![image](https://user-images.githubusercontent.com/84455469/132863778-0e93f9c2-0eff-4a56-8fcb-8d5f8a0a1c29.png)

Now we will perform certain action/steps to remove all these errors/warnings.

#### Action to remove all above warnings:

1. To remove 1st warning add below text at top of the class

![image](https://user-images.githubusercontent.com/84455469/132864038-3e6613c5-55db-4717-895d-3830f9283eaa.png)
   
2. To remove 2nd warning add below text at top of the method:

![image](https://user-images.githubusercontent.com/84455469/132864161-7f392988-e6a5-4401-9772-f02e829f90ea.png)

3. To remove 3rd warning, add below text at top of the file:

![image](https://user-images.githubusercontent.com/84455469/132864224-da9c29df-c7e8-41e1-898d-0153ec7ea1e8.png)

4. To remove 4th and 5th warning, add below text/code inside method:

![image](https://user-images.githubusercontent.com/84455469/132864302-da32a6cb-ab70-441f-9cc5-6d34c489c465.png)

5. To remove warning from 6th to 11th, cut all using directive from top and paste it inside namespace as below:

![image](https://user-images.githubusercontent.com/84455469/132864361-a20f43e5-deca-4316-b1ee-b3bb2d80ced5.png)

6. Now we have removed all errors, warnings and messages:

![image](https://user-images.githubusercontent.com/84455469/132864434-0ee65d3f-7499-4c40-978b-12ee20c4d411.png)

