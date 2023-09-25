using System.Collections.Generic;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public class EOGUIList : EOGUIList<EOGUIList, IEOGUIElementChild>
    {
        public EOGUIList(bool isHorizontal) : base(isHorizontal)
        {
        }
    }

    public abstract class EOGUIList<TSelf, IElement> : EOGUIElementParent<TSelf, List<IElement>, IElement>
        where TSelf : EOGUIList<TSelf, IElement>
        where IElement : IEOGUIElementChild
    {
        public EOGUIList(bool isHorizontal) : base(new List<IElement>(), isHorizontal)
        {
        }

        public override EOGUIHeightType HeightType => EOGUIHeightType.Animation;

        public new IList<IElement> Elements => this;
    }
}