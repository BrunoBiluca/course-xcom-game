using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class SpinActionIntent : IAPActionIntent
    {
        public bool ExecuteImmediatly => true;

        public int ActionPointsCost { get; set; } = 2;

        private readonly UnitSelectionMono unitSelection;

        public SpinActionIntent(
            UnitSelectionMono unitSelection
        )
        {
            this.unitSelection = unitSelection;
        }

        public IAction Create()
        {
            return new SpinUnitAction(
                AsyncProcessor.I,
                unitSelection.CurrentUnit.Transform
            ) {
                Logger = UnityDebug.I
            };
        }
    }
}