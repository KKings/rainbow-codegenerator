# Code Generator for the Rainbow serialization format (Unicorn)

Unlike other projects that are tightly coupled to t4 templates, this project aims to separate out deserialization of the models from code generation so you can use
any templating language. This project produces a executable that can be called from powershell/gulp or grunt scripts/node/anything really 
that returns back a JSON string of the models for a given directory. 

This project is not a complete solution for generating code from rainbow files but should be used with a gulp talk or similar solution. See below a full solution example
using the executable, handlebars templating engine, and a simple gulp task. 

#### Executable Arguments

* *f* or *folder* is the source directory for all of the unicorn source files
* *p* or *include-sitecore-paths* an array that will only include the sitecore paths from the generated models
  * Example: -p "/sitecore/templates"
* *d* or *debug* will not output JSON but will output messages to the console application

### Example Solution

In the example directory you will find a simple solution using gulp, handlebars, and the executable to generate models. The process is simple, when the gulp 
task executes, it looks for 'unicorn.json' files within your directory, this file contains all the configuration the gulp task needs to generate the model. Foreach
unicorn.json file found, the executable is called passing in the parameters found within the unicorn.json. With the result from the executable, the gulp task then passes
the model to handlebars.

#### Files
* codegen.handlebars - Handlebars template for generating the models
* package.json - Example package.json file for getting the node packages for gulp
* unicorn.json - Configuration file for specifying where the source and destination folders are. Also includes the name of the filename that should be generated, where the template for handlebars can be found
* unicorn.helpers.js - A helpers file for translating Sitecore Types to your Code Generation classes. If you are familiar with the T4 templates, this will give you the ability to do what 'type' and 'generic' parameters allowed you to do. You can define per field mappings in the 'mapping' object (id -> type)
* rp.exe - The Executable
* gulpfile.js - Example gulp script containing the task to generate models

#### Setting up the example
1.  Download the files from the example folder
2.  Move the unicorn.json and unicorn.helpers.js file into the 'code' folder of one of the helix style modules
    * By having multiple unicorn.json files within your projects, you can have per project model generation
3.  Move 'codegen.handlebars' and the 'rp.exe' into a common area
4.  Copy or merge the task within the 'gulpfile.js' into your own gulp script
5.  Modify the gulp task and the unicorn.json file to match your environment and project needs.
6.  Run the gulp task
7.  If it works, a file should be generated in your destination path with the 'unicorn.json' file.
8.  Repeat adding the unicorn.json file for every module that needs code generation
9.  Sit back and relax, you have simple code generation similar to TDS -> T4 templates

### Contributing

Pull requests are welcome!

### Building from Source

This project uses Fody, a nuget package that stuffs all the resources (dll references, etc) for a console application into the executable itself. Once you download the source and build, you only need to 
distribute the executable.