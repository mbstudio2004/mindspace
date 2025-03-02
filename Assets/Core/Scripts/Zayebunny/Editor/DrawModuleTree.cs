using Nocci.Zayebunny.GameDB.Base;

namespace Nocci.Zayebunny.Editor
{
    public class DrawModuleTree
    {
        public BaseModule module;

        public BaseObject selected;

        public void SetSelected(object item)
        {
            if (item is BaseObject attempt) selected = attempt;
        }

        public void SetModule(BaseModule module)
        {
            if (module != null)
                this.module = module;
        }

        public int GetSelectedIndex()
        {
            return module.Objects.IndexOf(selected);
        }
    }
}