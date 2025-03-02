//using MonadGames.Toolkit.SaveSystem;

namespace Nocci.Zayebunny.GameDB.GameParameters
{
    public static class MTK_GameParameters
    {
        public static void SetParameter(GameParametersObject parameterGroup, string id, object data)
        {
            /*     if (!Data.Store.Parameters.ContainsKey(parameterGroup.key))
                 {
                     Data.Store.Parameters.Add(parameterGroup.key, new Dictionary<string, object>());
                 }

                 if (!Data.Store.Parameters[parameterGroup.key].ContainsKey(id))
                 {
                     Data.Store.Parameters[parameterGroup.key].Add(id, data);
                 }
                 else
                 {
                     Data.Store.Parameters[parameterGroup.key][id] = data;
                 }

                 MTK_SaveController.Instance.Save();*/
        }

        public static T GetParameter<T>(GameParametersObject parameterGroup, string id)
        {
            return (T)GetParameter(parameterGroup, id);
        }

        public static object GetParameter(GameParametersObject parameterGroup, string id)
        {
            /*if (!Data.Store.Parameters.ContainsKey(parameterGroup.key) || !Data.Store.Parameters[parameterGroup.key].ContainsKey(id))
            {
                SetParameter(parameterGroup, id, parameterGroup.GetData(id, out _).Value);
            }

            return Data.Store.Parameters[parameterGroup.key][id];*/
            return default;
        }

        #region Save

        /*  private class ParametersSaveData : SaveStoreBase
          {
              public Dictionary<string, Dictionary<string, object>> Parameters = new();
              public override string ID { get; set; } = "GameParameters";
          }

          private class ParametersSaveInterpreter : SaveInterpreter<ParametersSaveData>
          {
          }

          private static ParametersSaveInterpreter Data = new();

          [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
          private static void Setup()
          {
              MTK_UnityEvents.EnableEvent += () => { Data.Initialize(); };
          }*/

        #endregion
    }
}