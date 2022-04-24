using Newtonsoft.Json.Linq;
using SpringBootGenerator;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpringBootGeneratorCli {
	class Program {
		static string SetValue(string value) {
			if (value.StartsWith("--")) return null;
			return value;
		}

		static int Main(string[] args) {
			bool arg(string arg) {
				foreach (string argument in args) {
					if (argument.ToLower() == arg.ToLower()) return true;
				}
				return false;
			}
			int argIndex(string arg) {
				for (int i = 0; i < args.Length; i++) {
					if (args[i].ToLower() == arg.ToLower()) return i;
				}
				return -1;
			}
			string val(string argName) {
				foreach (string argument in args) {
					if (argument.ToLower().StartsWith(argName.ToLower()+"=")) {
						return argument.Replace(argName + "=", "");
					}
				}
				return null;
			}
			string SetValue(int index) {
				if (args.Length <= index) return null;
				return Program.SetValue(args[index]);
			}

			if (args.Length == 0 || arg("--help")) {
				showHelp();
				return 0;
			}

			Generator.ProjectRoot = val("--root");
			Generator.ProjectName = val("--name");
			Generator.Port = val("--port");
			Generator.AppImageName = val("--imageName");
			Generator.AppName = val("--appName");
			Generator.DbName = val("--dbName");
			Generator.DbUser = val("--dbUser");
			Generator.DbPassword = val("--dbPassword");
			Generator.ProjectName = val("--name");

			if (arg("--entity")) {
				int index = argIndex("--entity");

				string entityName = SetValue(++index);
				string packageIdentifier = SetValue(++index);

				if (entityName == null || packageIdentifier == null) {
					Console.WriteLine("EntityName or packageIdentifier not given!");
					showHelp();
					return -1;
				}

				Generator.Entity(entityName, packageIdentifier);
			}

			if (arg("--fix-tests")) Generator.FixDbForTests();
			if (arg("--config")) Generator.Config();
			if (arg("--docker") || arg("--dockerization")) Generator.Dockerization();
			if (arg("--script")) Generator.Runscript();

			return 0;
		}

		static void showHelp() {
			Console.WriteLine("SpringBootGenerator");
			Console.WriteLine("Usage: sbg [--entity <EntityName> <packageIdentifier>] [--fix-tests] [--docker] [--script] [--config]\n" +
							  "       [--root='.'] [--name=...] [--port=8080] [--imageName=app21] [--appName=tru_app] [--dbName=truDb]\n" +
							  "       [--dbUser=usr] [--dbPassword=pwd]");
			Console.WriteLine();
			Console.WriteLine("--entity......Create an Entity with given Name in the given package.\n" +
							 "               The packageIdentifier has to point to the package in which\n" +
							 "               the model, repository, controller and so on is.");
			Console.WriteLine("--fix-tests...Creates a correct application.properties file for the tests\n" +
							 "               uses the 'h2' db, so make sure to have that installed.");
			Console.WriteLine("--docker......Creates a dockerfile and a dockercompose file that contains\n" +
							 "               also the db.");
			Console.WriteLine("--script......Creates a script to build, package and run the whole project.");
			Console.WriteLine("--config......Creates the application.properties file for normal usage.");
		}
	}
}
