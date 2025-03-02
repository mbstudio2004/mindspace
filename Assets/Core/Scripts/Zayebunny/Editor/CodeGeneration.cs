using System.Collections.Generic;
using System.IO;
using System.Text;
using Nocci.Zayebunny.GameDB.Base;
using UnityEditor;

//using MonadGames.Toolkit.Extensions;

namespace Nocci.Zayebunny.Editor
{
    public static class CodeGeneration
    {
        [MenuItem("MonadGames/GameDB Manager/Generate Constants")]
        public static void GenerateConstants()
        {
            // var settings = GameDBSettings.Instance;
            // var modules = settings.Modules;
            var builder = new StringBuilder();
            builder.AppendLine("namespace MonadGames.ZB.GameDB.Constants");
            builder.AppendLine("{");
            builder.AppendLine("\tpublic static class GameDBConstants");
            builder.AppendLine("\t{");
            // Recursive(modules, builder);
            builder.AppendLine("\t}");
            builder.AppendLine("}");

            var code = builder.ToString();


            if (!Directory.Exists("Assets/Zayebunny"))
                Directory.CreateDirectory("Assets/Zayebunny");

            var path = "Assets/Zayebunny/GameDBConstants.cs";


            File.WriteAllText(path, code);

            AssetDatabase.Refresh();
        }


        private static void Recursive(List<BaseModule> modules, StringBuilder builder)
        {
            /* foreach (var module in modules)
             {
                 builder.AppendLine($"\t\tpublic class {module.DisplayName.ToCamelCase()}");
                 builder.AppendLine("\t\t{");
                 foreach (var moduleObject in module.Objects)
                 {
                     var key = !char.IsLetter(moduleObject.key[0])
                         ? "_" + moduleObject.key.Substring(0, 1).ToLower() + moduleObject.key.Substring(1)
                         : moduleObject.key;
                     var constantName = key.ToCamelCase();
                     builder.AppendLine($"\t\tpublic class {constantName}");
                     builder.AppendLine("\t\t{");
                     builder.AppendLine($"\t\t\tpublic const string key = \"{moduleObject.key}\";");
                     moduleObject.CustomCodeGen(ref builder);
                     builder.AppendLine("\t\t}");
                 }

                 Recursive(module.SubModules, builder);

                 builder.AppendLine("\t\t}");
             }*/
        }
    }
}