using System;

namespace SpringBootGeneratorCli {
	class Program {
		static int Main(string[] args) {
			string entityName = null;
			if (args.Length > 0) entityName = args[0];

			string packageIdentifier = null;
			if (args.Length > 1) packageIdentifier = args[1];

			string path = ".";
			if (args.Length > 2) path = args[2];

			if (entityName == null || packageIdentifier == null) {
				Console.WriteLine("EntityName or packageIdentifier not given!");
				Console.WriteLine("Usage: sbg <EntityName> <packageIdentifier> [<path='.'>]");
				return -1;
			}

			bool dockerization = false;
			bool createRunscript = false;
			foreach (string arg in args) {
				if (arg == "--docker" || arg == "--dockerization") dockerization = true;
				if (arg == "--script") createRunscript = true;
			}

			return SpringBootGenerator.Generator.Entity(entityName, path, packageIdentifier);

			//if (dockerization) {
			//	SpringBootGenerator.Generator.Dockerization(path);
			//}
			//if (createRunscript) {
			//	SpringBootGenerator.Generator.Runscript(dockerization, path);
			//}
		}
	}
}
