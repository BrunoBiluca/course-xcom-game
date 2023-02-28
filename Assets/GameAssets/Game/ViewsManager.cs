using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.TurnSystem;

namespace GameAssets
{
    public class ViewsManager 
        : Singleton<ViewsManager>
        , IDependencySetup<ICharacterSelector>
        , IContainerProvide
    {
        public IDependencyContainer Container { get; set; }

        private ViewsGroup allViews;
        private ViewsGroup playerViewGroup;
        private ViewsGroup unitsViewGroup;

        public void Init()
        {
            allViews = new ViewsGroup();
            allViews.Register(Container.Resolve<PlayerInputsView>());
            allViews.Register(
                new ViewDecorator(Container.Resolve<TurnSystemView>().gameObject.Decorate())
            );
            allViews.Register(Container.Resolve<UnitsView>());
            allViews.Register(Container.Resolve<UnitIntentsView>());
            allViews.Register(Container.Resolve<ActionPointsView>());
            allViews.Register(Container.Resolve<WorldGridView>());

            playerViewGroup = new ViewsGroup();
            playerViewGroup.Register(Container.Resolve<PlayerInputsView>());
            playerViewGroup.Register(
                new ViewDecorator(Container.Resolve<TurnSystemView>().gameObject.Decorate())
            );
            playerViewGroup.Register(Container.Resolve<UnitsView>());
            playerViewGroup.Register(Container.Resolve<WorldGridView>());
            playerViewGroup.Show();

            unitsViewGroup = new ViewsGroup();
            unitsViewGroup.Register(Container.Resolve<UnitIntentsView>());
            unitsViewGroup.Register(Container.Resolve<ActionPointsView>());
            unitsViewGroup.Hide();
        }

        public void AllViewsHide()
        {
            Debug.Log("hide all views");
            allViews.Hide();
        }

        public void AllViewsShow()
        {
            Debug.Log("show all views");
            allViews.Show();
        }

        public void Setup(ICharacterSelector characterSelector)
        {
            characterSelector.OnUnitSelected += () => unitsViewGroup.Show();
            characterSelector.OnUnitUnselected += () => unitsViewGroup.Hide();
        }
    }
}
