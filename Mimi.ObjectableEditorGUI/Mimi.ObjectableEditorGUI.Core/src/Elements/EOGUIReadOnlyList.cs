using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Mimi.ObjectableEditorGUI.Elements
{
    public sealed class EOGUIReadOnlyList : EOGUIReadOnlyList<EOGUIReadOnlyList, IEOGUIElementChild>
    {
        public EOGUIReadOnlyList(IEnumerable<IEOGUIElementChild> elements, bool isHorizontal) : base(elements.ToArray(), isHorizontal)
        {
        }

        public new EOGUIHeightType HeightType { get => HeightType_Protected; set => HeightType_Protected = value; }

        protected override EOGUIHeightType HeightType_Protected { get; set; }
    }

    public abstract class EOGUIReadOnlyList<TSelf, IElement> : EOGUIElementParent<TSelf, ReadOnlyCollection<IElement>, IElement>
        where TSelf : EOGUIReadOnlyList<TSelf, IElement>
        where IElement : IEOGUIElementChild
    {
        public EOGUIReadOnlyList(IList<IElement> sourceList, bool isHorizontal) :
            base(new ReadOnlyCollection<IElement>(sourceList), isHorizontal)
        {
            SourceList = sourceList;
        }

        public sealed override EOGUIHeightType HeightType => HeightType_Protected;

        protected abstract EOGUIHeightType HeightType_Protected { get; set; }

        public ReadOnlyCollection<IElement> ReadOnlyList => Elements;

        protected IList<IElement> SourceList { get; }
    }
}