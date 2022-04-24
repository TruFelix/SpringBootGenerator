using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SpringBootGenerator {
	public static class Generator {
		private static string projectRoot = ".";
		private static string projectName;
		private static string port = "8080";
		private static string appImageName;
		private static string appName = "tru_app";
		private static string dbName = "truDb";
		private static string dbUser = "usr";
		private static string dbPassword = "pwd";


		public static string ProjectRoot {
			get { return projectRoot; }
			set {
				if (value != null) projectRoot = value;
			}
		}
		public static string ProjectName {
			get { return projectName; }
			set {
				if (value != null) projectName = value;
			}
		}

		public static string Port {
			get { return port; }
			set {
				if (value != null) port = value;
			}
		}
		public static string AppImageName {
			get {
				if (appImageName == null) {
					return appName;
				} else {
					return appImageName;
				}
			}
			set {
				if (value != null) appImageName = value;
			}
		}
		public static string AppName {
			get { return appName; }
			set {
				if (value != null) appName = value;
			}
		}
		public static string DbName {
			get { return dbName; }
			set {
				if (value != null) dbName = value;
			}
		}
		public static string DbUser {
			get { return dbUser; }
			set {
				if (value != null) dbUser = value;
			}
		}
		public static string DbPassword {
			get { return dbPassword; }
			set {
				if (value != null) dbPassword = value;
			}
		}

		public static int Setup(string packageName) {
			string templateDir = AppDomain.CurrentDomain.BaseDirectory + "/Templates";
			string packageDirs = packageName.Replace('.', '/');
			string sourceDir = ProjectRoot + "/src/main/java/" + packageDirs;

			Templater.FromFile(templateDir + "/ObjectId.template")
				.Set("package", packageName)
				.ToFile(sourceDir + "/models/ObjectId.java")
				;

			Templater.FromFile(templateDir + "/EntityService.template")
				.Set("package", packageName)
				.ToFile(sourceDir + "/services/EntityService.java")
				;

			Templater.FromFile(templateDir + "/GetSingleService.template")
				.Set("package", packageName)
				.ToFile(sourceDir + "/services/GetSingleService.java")
				;

			Templater.FromFile(templateDir + "/test.Util.template")
				.Set("package", packageName)
				.ToFile(ProjectRoot + "/src/test/java/" + packageDirs + "/controllers/Util.java");

			return 0;
		}

		public static int Entity(string entityName, string packageName) {
			string templateDir = AppDomain.CurrentDomain.BaseDirectory + "/Templates";
			string packageDirs = packageName.Replace('.', '/');
			string sourceDir = ProjectRoot + "/src/main/java/" + packageDirs;

			Setup(packageName);

			Templater.FromFile(templateDir + "/Entity.template")
				.Set("entity", entityName)
				.Set("package", packageName)
				.ToFile(sourceDir + "/models/" + entityName + "/" + entityName + ".java")
				;
			Templater.FromFile(templateDir + "/CreateEntity.template")
				.Set("entity", entityName)
				.Set("package", packageName)
				.ToFile(sourceDir + "/models/" + entityName + "/Create" + entityName + ".java")
				;

			Templater.FromFile(templateDir + "/Repository.template")
				.Set("entity", entityName)
				.Set("package", packageName)
				.ToFile(sourceDir + "/repositories/" + entityName + "Repository.java")
				;

			Templater.FromFile(templateDir + "/Service.template")
				.Set("entity", entityName)
				.Set("package", packageName)
				.ToFile(sourceDir + "/services/" + entityName + "Service.java")
				;

			Templater.FromFile(templateDir + "/Controller.template")
				.Set("lowerEntity", entityName.ToLower())
				.Set("package", packageName)
				.Set("entity", entityName)
				.ToFile(sourceDir + "/controllers/" + entityName + "Controller.java");

			Templater.FromFile(templateDir + "/test.Controller.template")
				.Set("package", packageName)
				.Set("entity", entityName)
				.Set("lowerEntity", entityName.ToLower())
				.ToFile(ProjectRoot + "/src/test/java/" + packageDirs + "/controllers/" + entityName + "ControllerTest.java");

			return 4;
		}

		public static int FixDbForTests() {
			Templater.FromFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/application.properties.test.template")
				.Set("dbUser", dbUser)
				.Set("dbPassword", dbPassword)
				.Set("port", port)
				.ToFile(ProjectRoot + "/src/test/resources/application.properties");

			return 1;
		}

		public static void Config() {
			Templater.FromFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/application.properties.template")
				.Set("port", port)
				.Set("dbUser", dbUser)
				.Set("dbPassword", dbPassword)
				.ToFile(ProjectRoot + "/src/main/resources/application.properties");
		}

		public static void Runscript() {
			Templater.FromFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/build.cmd.template")
				.Set("appImageName", AppImageName)
				.Set("appName", AppName)
				.Set("dbName", DbName)
				.ToFile(ProjectRoot + "/build.cmd");
		}

		public static void Dockerization() {
			Templater.FromFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/DockerFile.template")
				.Set("port", Port)
				.ToFile(ProjectRoot + "/dockerfile");

			Templater.FromFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/DockerCompose.template")
				.Set("port", Port)
				.Set("appName", AppName)
				.Set("appImageName", AppImageName)
				.Set("dbName", DbName)
				.Set("dbUser", DbUser)
				.Set("dbPassword", DbPassword)
				.ToFile(ProjectRoot + "/docker-compose.yml");
		}

		internal class Templater {
			public string TemplateString { get; protected set; }

			public Templater(string templateString) {
				this.TemplateString = templateString;
			}

			public static Templater FromFile(string filePath) {
				return new Templater(File.ReadAllText(filePath));
			}

			public string Replace(string a, string b) {
				return TemplateString.Replace(a, b);
			}

			public Templater Set(string tag, string value) {
				TemplateString = Replace("[{" + tag + "}]", value);
				return this;
			}

			private static string OnlyDirectory(string path) {
				var split = path.Split('/');
				var splitRemoved = new List<string>();
				for (int i = 0; i < split.Length - 1; i++) {
					splitRemoved.Add(split[i]);
				}

				var joined = "";
				foreach (string val in splitRemoved) {
					joined += "/" + val;
				}
				joined = joined.Remove(0, 1);

				return joined;
			}

			public virtual void ToFile(string filePath) {
				var dirs = OnlyDirectory(filePath);
				Directory.CreateDirectory(dirs);
				try {
					File.WriteAllText(filePath, TemplateString);
					Console.WriteLine(filePath.ToUpper() + "...CREATED");
					Console.WriteLine("  Filename: " + filePath);
				} catch (PathTooLongException ptle) {
					Console.WriteLine(filePath.ToUpper() + "...ERROR");
					Console.WriteLine("  Error: PATH TOO LONG");
					Console.WriteLine(ptle.Message);
				} catch (IOException ioe) {
					Console.WriteLine(filePath.ToUpper() + "...ERROR");
					Console.WriteLine("  Error: IO-ERROR");
					Console.WriteLine(ioe.Message);
				}
			}
		}
	}

}
