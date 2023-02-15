using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace GameAssets
{
    public class ViewsGroup
    {
        private readonly List<IView> views = new();

        public void Hide()
        {
            foreach(var view in views)
            {
                view.Hide();
            }
        }

        public void Register(IView view)
        {
            views.Add(view);
        }

        public void Show()
        {
            foreach(var view in views)
            {
                view.Show();
            }
        }
    }
}
