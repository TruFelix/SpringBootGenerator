using System;
using System.IO;
using System.Threading.Tasks;

namespace SpringBootGenerator {
	public static class Generator {
		public static int Entity(string entityName, string packagePath, string packageName) {
			JavaTemplater.FromFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/Class.template").WithName(entityName).InPackage(packageName).In("models").WriteTo(packagePath);
			JavaTemplater.FromFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/Repository.template").WithName(entityName).InPackage(packageName).In("repositories").WriteTo(packagePath, "Repository");
			JavaTemplater.FromFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/Service.template").WithName(entityName).InPackage(packageName).In("services").WriteTo(packagePath, "Service");
			JavaTemplater.FromFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/Controller.template").WithName(entityName).InPackage(packageName).In("controllers").WriteTo(packagePath, "Controller");

			return 4;
		}

		//public static void Dockerization(path) {
		//	DockerfileTemplater.FromFile("Templates/dockerfile.template");
		//}

		//private class DockerfileTemplater {
		//	private readonly string templateString;

		//	public DockerfileTemplater(string templateString) {
		//		this.templateString = templateString;
		//	}

		//	public static DockerfileTemplater FromFile(string filePath) {
		//		return new DockerfileTemplater(File.ReadAllText(filePath));
		//	}

		//	public string Applied() {
		//		templateString.Replace("~", );
		//	}
		//}

		private class JavaTemplater {
			private readonly string templateString;

			private string entityName;
			private string packageIdentifier;
			private string finalPackage;

			public JavaTemplater(string templateString) {
				this.templateString = templateString;
			}

			public static JavaTemplater FromFile(string filePath) {
				return new JavaTemplater(File.ReadAllText(filePath));
			}

			public JavaTemplater WithName(string entityName) {
				this.entityName = entityName;
				return this;
			}

			/// <param name="packageIdentifier">
			///		Example: <code>at.company.project</code>
			/// </param>
			public JavaTemplater InPackage(string packageIdentifier) {
				this.packageIdentifier = packageIdentifier;
				return this;
			}

			public JavaTemplater In(string finalPackage) {
				this.finalPackage = finalPackage;
				return this;
			}

			private string Applied() {
				string tmp = "package " + packageIdentifier + "." + finalPackage + ";\n";
				tmp += templateString.Replace("~", entityName).Replace("[{package}]", packageIdentifier);

				return tmp;
			}

			public void WriteTo(string pathToProjectRoot, string fileNameAddon="") {
				var fileName = entityName + fileNameAddon + ".java";

				var path = pathToProjectRoot + "/src/main/java/" + packageIdentifier.Replace('.', '/') + "/" + finalPackage;
				Directory.CreateDirectory(path);
				try {
					File.WriteAllText(path + "/" + fileName, this.Applied());
					Console.WriteLine(finalPackage.ToUpper() + "...CREATED");
					Console.WriteLine("  Filename: " + fileName);
				} catch (PathTooLongException ptle) {
					Console.WriteLine(finalPackage.ToUpper() + "...ERROR");
					Console.WriteLine("  Error: PATH TOO LONG");
					Console.WriteLine(ptle.Message);
				} catch (IOException ioe) {
					Console.WriteLine(finalPackage.ToUpper() + "...ERROR");
					Console.WriteLine("  Error: IO-ERROR");
					Console.WriteLine(ioe.Message);
				}
			}
		}

	}

}
