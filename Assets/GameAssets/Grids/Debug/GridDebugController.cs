using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public class GridDebugController : WorldGridDebug<UnitValue>
    {
        public void Start()
        {
            GameInstaller.I.OnInstallerFinish 
                += () => Setup(GameInstaller.I.GridManager.Grid);
        }
    }
}
