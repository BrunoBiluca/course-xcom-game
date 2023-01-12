using System.Threading.Tasks;

namespace GameAssets
{
    public interface IAIUnit : ICharacterUnit
    {
        Task TakeActions();
    }
}
