using System.Collections.Generic;
using System.Text;
using Nocci.Zayebunny.GameDB.Base;
using Sirenix.OdinInspector;

namespace Nocci.Zayebunny.GameDB.GameParameters
{
    public class GameParametersObject : BaseObject
    {
        [BoxGroup(STATS_GROUP, LabelText = "Properties")]
        [ListDrawerSettings(ShowFoldout = true, DraggableItems = false, ShowItemCount = false,
            AlwaysAddDefaultValue = true, ShowIndexLabels = false)]
        public List<GameParameter> MutableProperties = new();

        [BoxGroup(STATS_GROUP)]
        [ListDrawerSettings(ShowFoldout = true, DraggableItems = false, ShowItemCount = false,
            AlwaysAddDefaultValue = true, ShowIndexLabels = false)]
        public List<GameParameter> StaticProperties = new();


        // TODO: IMPLEMENT CAMELCASE TO KEYS
        public override void CustomCodeGen(ref StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("public static class MutableProperties {");
            foreach (var prop in MutableProperties)
            {
                var key = !char.IsLetter(prop.key[0])
                    ? "_" + prop.key.Substring(0, 1).ToLower() + prop.key.Substring(1)
                    : prop.key;
                var constantName = key; //SSS

                stringBuilder.AppendLine($"public const string {constantName} = \"{prop.key}\";");
            }

            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("public static class StaticProperties {");
            foreach (var prop in StaticProperties)
            {
                var key = !char.IsLetter(prop.key[0])
                    ? "_" + prop.key.Substring(0, 1).ToLower() + prop.key.Substring(1)
                    : prop.key;
                var constantName = key; //SSS

                stringBuilder.AppendLine($"public const string {constantName} = \"{prop.key}\";");
            }

            stringBuilder.AppendLine("}");
        }

        public GameParameter GetData(string key, out bool isStatic)
        {
            var prop = MutableProperties.Find(x => x.key == key);
            if (prop != null)
            {
                isStatic = false;
                return prop;
            }

            prop = StaticProperties.Find(x => x.key == key);
            if (prop != null)
            {
                isStatic = true;
                return prop;
            }

            isStatic = false;
            return null;
        }
    }
}