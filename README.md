# SpringBootGenerator
Generates SpringBoot files easily

Usage: 
```
SpringBootGeneratorCli [--entity <EntityName> <packageIdentifier>] [--fix-tests] [--docker] [--script] [--config]  
                       [--root='.'] [--name=...] [--port=8080] [--imageName=app21] [--appName=tru_app] [--dbName=truDb] [--dbUser=usr] [--dbPassword=pwd]
  
--entity......Create an Entity with given Name in the given package.
	     The packageIdentifier has to point to the package in which the model,
	     repository, controller and so on is.

--fix-tests...Creates a correct application.properties file for the tests uses the 'h2' db, so make sure to have that installed.

--docker......Creates a dockerfile and a dockercompose file that contains also the db.
--script......Creates a script to build, package and run the whole project.
--config......Creates the application.properties file for normal usage.
```

For further Information see the [Wiki](https://github.com/TruFelix/SpringBootGenerator/wiki)
