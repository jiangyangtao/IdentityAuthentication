using Microsoft.Extensions.DependencyModel;

namespace IdentityAuthentication.Extensions
{
    public static class CompilationLibraryExtensions
    {
        public static IReadOnlyList<CompilationLibrary> GetProjectCompileLibraries(this DependencyContext dependency)
        {
            return dependency.CompileLibraries.Where(a => a.Serviceable == false && a.Type == "project").ToArray();
        }

        public static IReadOnlyList<CompilationLibrary> GetCompileLibraries(this DependencyContext dependency, string projectName1)
        {
            var projects = GetProjectCompileLibraries(dependency);
            if (projects.IsNullOrEmpty()) return Array.Empty<CompilationLibrary>();

            return projects.Where(a =>
            {
                var names = a.Name.Split(".");
                return names[0] == projectName1;
            }).ToArray();
        }

        public static IReadOnlyList<CompilationLibrary> GetCompileLibraries(this DependencyContext dependency, string projectName1, string projectName2)
        {
            var projects = GetCompileLibraries(dependency, projectName1);
            if (projects.IsNullOrEmpty()) return Array.Empty<CompilationLibrary>();

            return projects.Where(a =>
            {
                var names = a.Name.Split(".");
                if (names.Length < 2) return false;

                return names[1] == projectName2;
            }).ToArray();
        }

        public static IReadOnlyList<CompilationLibrary> GetCompileLibraries(this DependencyContext dependency, string projectName1, string projectName2, string projectName3)
        {
            var projects = GetCompileLibraries(dependency, projectName1, projectName2);
            if (projects.IsNullOrEmpty()) return Array.Empty<CompilationLibrary>();

            return projects.Where(a =>
            {
                var names = a.Name.Split(".");
                if (names.Length < 3) return false;

                return names[2] == projectName3;
            }).ToArray();
        }

        public static IReadOnlyList<CompilationLibrary> GetCompileLibraries(this DependencyContext dependency, string projectName1, string projectName2, string projectName3, string projectName4)
        {
            var projects = GetCompileLibraries(dependency, projectName1, projectName2, projectName3);
            if (projects.IsNullOrEmpty()) return Array.Empty<CompilationLibrary>();

            return projects.Where(a =>
            {
                var names = a.Name.Split(".");
                if (names.Length < 4) return false;

                return names[3] == projectName4;
            }).ToArray();
        }

        public static IReadOnlyList<CompilationLibrary> GetCompileLibraries(this DependencyContext dependency, string projectName1, string projectName2, string projectName3, string projectName4, string projectName5)
        {
            var projects = GetCompileLibraries(dependency, projectName1, projectName2, projectName3, projectName4);
            if (projects.IsNullOrEmpty()) return Array.Empty<CompilationLibrary>();

            return projects.Where(a =>
            {
                var names = a.Name.Split(".");
                if (names.Length < 5) return false;

                return names[4] == projectName5;
            }).ToArray();
        }
    }
}