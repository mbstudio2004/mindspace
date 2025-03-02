//using SimpleJSON;

using UnityEngine;

namespace Nocci.Zayebunny.GameDB.Base
{
    [DefaultExecutionOrder(-5)]
    public class ZayebunnyDatabaseHandler : MonoBehaviour
    {
        public GameDBSettings Settings;

        //  [FormerlySerializedAs("SettingsModule")]
        // public GameDBSettingsModule gameDBSettingsModule;

        private void Awake()
        {
            /*    var apiHit = new APIHit("", "GET")
                {
                    url = gameDBSettingsModule.GetDownloadLink,
                    OnSuccess = s =>
                    {
                        var json = JSON.Parse(s);
                        GameDB.Setup();
                        GameDB.UpdateDatabaseFromJson(json.AsObject);
                    }
                };

                apiHit.Hit();*/
        }
    }
}