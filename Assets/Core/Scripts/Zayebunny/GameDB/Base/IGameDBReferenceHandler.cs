using UnityEngine;

namespace Nocci.Zayebunny.GameDB.Base
{
    public interface IDataReferenceHandler
    {
        BaseObject GetDataAtIndex(int index);
        int GetDataCount();
        int GetIdAtIndex(int index);
        int GetIndexOfId(int id);
        GUIContent[] GetAllGUIContents(bool name, bool tooltip, bool icon, GUIContent including = null);
    }
}