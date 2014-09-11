using System.Drawing;
using System.Drawing.Drawing2D;

namespace Svg
{
    /// <summary>
    /// An element used to group SVG shapes.
    /// </summary>
    [SvgElement("g")]
    public class SvgGroup : SvgVisualElement
    {
        public SvgGroup()
        {
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        /// <value></value>
        public override System.Drawing.Drawing2D.GraphicsPath Path(SvgRenderer renderer)
        {
            return GetPaths(this, renderer);
        }

        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <returns>The bounds.</returns>
        public override RectangleF CalculateBounds()
        {
            var r = new RectangleF();
            foreach (var c in this.Children)
            {
                if (c is SvgVisualElement)
                {
                    // First it should check if rectangle is empty or it will return the wrong Bounds.
                    // This is because when the Rectangle is Empty, the Union method adds as if the first values where X=0, Y=0
                    if (r.IsEmpty)
                    {
                        r = ((SvgVisualElement) c).CalculateBounds();
                    }
                    else
                    {
                        var childBounds = ((SvgVisualElement) c).CalculateBounds();
                        if (!childBounds.IsEmpty)
                        {
                            r = RectangleF.Union(r, childBounds);
                        }
                    }
                }
            }

            return r;
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> object to render to.</param>
        protected override void Render(SvgRenderer renderer)
        {
            if (!Visible || !Displayable)
                return;

            if (this.PushTransforms(renderer))
            {
                this.SetClip(renderer);
                base.RenderChildren(renderer);
                this.ResetClip(renderer);
                this.PopTransforms(renderer);
            }
        }

        
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgGroup>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgGroup;
            if (this.Fill != null)
                newObj.Fill = this.Fill.DeepCopy() as SvgPaintServer;
            return newObj;
        }
    }
}
