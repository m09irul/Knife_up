using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System.Text;

namespace OnefallGames
{
    public class ScriptingSymbolsHandler
    {

        /// <summary>
        /// Determine whether the given namespace is already exist in the project
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static bool NamespaceExists(string nameSpace, string assemblyName = null)
        {
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly asm in assemblies)
            {
                // The assembly must match the given one if any.
                if (!string.IsNullOrEmpty(assemblyName) && !asm.GetName().Name.Equals(assemblyName))
                {
                    continue;
                }

                System.Type[] types = asm.GetTypes();
                foreach (System.Type t in types)
                {
                    // The namespace must match the given one if any. Note that the type may not have a namespace at all.
                    // Must be a class and of course class name must match the given one.
                    if (!string.IsNullOrEmpty(t.Namespace) && t.Namespace.Equals(nameSpace))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// Add the given scripting symbol array to the current build platform.
        /// </summary>
        /// <param name="symbols"></param>
        /// <param name="platform"></param>
        public static void AddDefinedScriptingSymbol(string[] symbols, BuildTargetGroup platform)
        {
            List<string> currentSymbols = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(platform).Split(';'));
            bool isAdded = false;
            foreach (string symbol in symbols)
            {
                if (!currentSymbols.Contains(symbol))
                {
                    currentSymbols.Add(symbol);
                    isAdded = true;
                }
            }

            if (isAdded)
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < currentSymbols.Count; i++)
                {
                    sb.Append(currentSymbols[i]);
                    if (i < currentSymbols.Count - 1)
                        sb.Append(";");
                }

                PlayerSettings.SetScriptingDefineSymbolsForGroup(platform, sb.ToString());
            }
        }



        /// <summary>
        /// Remove the given scripting symbol array to the current build platform.
        /// </summary>
        /// <param name="symbols"></param>
        /// <param name="platform"></param>
        public static void RemoveDefinedScriptingSymbol(string[] symbols, BuildTargetGroup platform)
        {
            List<string> currentSymbols = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(platform).Split(';'));
            List<string> newSymbols = new List<string>();
            bool isRemoved = false; ;

            foreach (string symbol in symbols)
            {
                if (currentSymbols.Contains(symbol))
                {
                    isRemoved = true;
                }
                else
                {
                    newSymbols.Add(symbol);
                }
            }

            if (isRemoved)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newSymbols.Count; i++)
                {
                    sb.Append(currentSymbols[i]);
                    if (i < currentSymbols.Count - 1)
                        sb.Append(";");
                }

                PlayerSettings.SetScriptingDefineSymbolsForGroup(platform, sb.ToString());
            }
        }



        /// <summary>
        /// Define whether the platform symbols contains a given symbol.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        public static bool IsContainedSymbol(string symbol, BuildTargetGroup platform)
        {
            List<string> currentSymbols = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(platform).Split(';'));
            return currentSymbols.Contains(symbol);
        }
    }
}
