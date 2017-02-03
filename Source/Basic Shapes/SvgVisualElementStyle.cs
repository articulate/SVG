using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Svg.DataTypes;
using System.Text.RegularExpressions;
using System.Linq;

namespace Svg
{
    public abstract partial class SvgVisualElement
    {
        private static float FixOpacityValue(float value)
        {
            const float max = 1.0f;
            const float min = 0.0f;
            return Math.Min(Math.Max(value, min), max);
        }

        /// <summary>
        /// Gets or sets a value to determine whether the element will be rendered.
        /// </summary>
        [TypeConverter(typeof(SvgBoolConverter))]
        [SvgAttribute("visibility")]
        public virtual bool Visible
        {
            get { return (this.Attributes["visibility"] == null) ? true : (bool)this.Attributes["visibility"]; }
            set { this.Attributes["visibility"] = value; }
        }

        /// <summary>
        /// Gets or sets a value to determine whether the element will be rendered.
        /// Needed to support SVG attribute display="none"
        /// </summary>
        [SvgAttribute("display")]
        public virtual string Display
        {
            get { return this.Attributes["display"] as string; }
            set { this.Attributes["display"] = value; }
        }

        // Displayable - false if attribute display="none", true otherwise
        protected virtual bool Displayable
        {
            get
            {
                string checkForDisplayNone = this.Attributes["display"] as string;
                if ((!string.IsNullOrEmpty(checkForDisplayNone)) && (checkForDisplayNone == "none"))
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Gets or sets the fill <see cref="SvgPaintServer"/> of this element.
        /// </summary>
        [SvgAttribute("fill")]
        public virtual SvgPaintServer Fill
        {
            get { return (this.Attributes["fill"] == null) ? SvgColourServer.NotSet : (SvgPaintServer)this.Attributes["fill"]; }
            set { this.Attributes["fill"] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="SvgPaintServer"/> to be used when rendering a stroke around this element.
        /// </summary>
        [SvgAttribute("stroke")]
        public virtual SvgPaintServer Stroke
        {
            get { return (this.Attributes["stroke"] == null) ? null : (SvgPaintServer)this.Attributes["stroke"]; }
            set { this.Attributes["stroke"] = value; }
        }

        [SvgAttribute("fill-rule")]
        public virtual SvgFillRule FillRule
        {
            get { return (this.Attributes["fill-rule"] == null) ? SvgFillRule.NonZero : (SvgFillRule)this.Attributes["fill-rule"]; }
            set { this.Attributes["fill-rule"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of this element's <see cref="Fill"/>.
        /// </summary>
        [SvgAttribute("fill-opacity")]
        public virtual float FillOpacity
        {
            get { return (this.Attributes["fill-opacity"] == null) ? this.Opacity : (float)this.Attributes["fill-opacity"]; }
            set { this.Attributes["fill-opacity"] = FixOpacityValue(value); }
        }

        /// <summary>
        /// Gets or sets the width of the stroke (if the <see cref="Stroke"/> property has a valid value specified.
        /// </summary>
        [SvgAttribute("stroke-width")]
        public virtual SvgUnit StrokeWidth
        {
            get { return (this.Attributes["stroke-width"] == null) ? new SvgUnit(1.0f) : (SvgUnit)this.Attributes["stroke-width"]; }
            set { this.Attributes["stroke-width"] = value; }
        }

        [SvgAttribute("stroke-linecap")]
        public virtual SvgStrokeLineCap StrokeLineCap
        {
            get { return (this.Attributes["stroke-linecap"] == null) ? SvgStrokeLineCap.Butt : (SvgStrokeLineCap)this.Attributes["stroke-linecap"]; }
            set { this.Attributes["stroke-linecap"] = value; }
        }

        [SvgAttribute("stroke-linejoin")]
        public virtual SvgStrokeLineJoin StrokeLineJoin
        {
            get { return (this.Attributes["stroke-linejoin"] == null) ? SvgStrokeLineJoin.Miter : (SvgStrokeLineJoin)this.Attributes["stroke-linejoin"]; }
            set { this.Attributes["stroke-linejoin"] = value; }
        }

        [SvgAttribute("stroke-miterlimit")]
        public virtual float StrokeMiterLimit
        {
            get { return (this.Attributes["stroke-miterlimit"] == null) ? 4.0f : (float)this.Attributes["stroke-miterlimit"]; }
            set { this.Attributes["stroke-miterlimit"] = value; }
        }

        [SvgAttribute("stroke-dasharray")]
        public virtual SvgUnitCollection StrokeDashArray
        {
            get { return this.Attributes["stroke-dasharray"] as SvgUnitCollection; }
            set { this.Attributes["stroke-dasharray"] = value; }
        }

        [SvgAttribute("stroke-dashoffset")]
        public virtual SvgUnit StrokeDashOffset
        {
            get { return (this.Attributes["stroke-dashoffset"] == null) ? SvgUnit.Empty : (SvgUnit)this.Attributes["stroke-dashoffset"]; }
            set { this.Attributes["stroke-dashoffset"] = value; }
        }

        /// <summary>
        /// Gets or sets the opacity of the stroke, if the <see cref="Stroke"/> property has been specified. 1.0 is fully opaque; 0.0 is transparent.
        /// </summary>
        [SvgAttribute("stroke-opacity")]
        public virtual float StrokeOpacity
        {
            get { return (this.Attributes["stroke-opacity"] == null) ? this.Opacity : (float)this.Attributes["stroke-opacity"]; }
            set { this.Attributes["stroke-opacity"] = FixOpacityValue(value); }
        }

        /// <summary>
        /// Gets or sets the opacity of the element. 1.0 is fully opaque; 0.0 is transparent.
        /// </summary>
        [SvgAttribute("opacity")]
        public virtual float Opacity
        {
            get { return (this.Attributes["opacity"] == null) ? 1.0f : (float)this.Attributes["opacity"]; }
            set { this.Attributes["opacity"] = FixOpacityValue(value); }
        }

        /// <summary>
        /// Indicates which font family is to be used to render the text.
        /// </summary>
        [SvgAttribute("font-family")]
        public virtual string FontFamily
        {
            get { return this.Attributes["font-family"] as string; }
            set { this.Attributes["font-family"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Refers to the size of the font from baseline to baseline when multiple lines of text are set solid in a multiline layout environment.
        /// </summary>
        [SvgAttribute("font-size")]
        public virtual SvgUnit FontSize
        {
            get { return (this.Attributes["font-size"] == null) ? SvgUnit.Empty : (SvgUnit)this.Attributes["font-size"]; }
            set { this.Attributes["font-size"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Refers to the boldness of the font.
        /// </summary>
        [SvgAttribute("font-style")]
        public virtual SvgFontStyle FontStyle
        {
            get { return (this.Attributes["font-style"] == null) ? SvgFontStyle.inherit : (SvgFontStyle)this.Attributes["font-style"]; }
            set { this.Attributes["font-style"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Refers to the boldness of the font.
        /// </summary>
        [SvgAttribute("font-variant")]
        public virtual SvgFontVariant FontVariant
        {
            get { return (this.Attributes["font-variant"] == null) ? SvgFontVariant.inherit : (SvgFontVariant)this.Attributes["font-variant"]; }
            set { this.Attributes["font-variant"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Refers to the boldness of the font.
        /// </summary>
        [SvgAttribute("text-decoration")]
        public virtual SvgTextDecoration TextDecoration
        {
            get { return (this.Attributes["text-decoration"] == null) ? SvgTextDecoration.inherit : (SvgTextDecoration)this.Attributes["text-decoration"]; }
            set { this.Attributes["text-decoration"] = value; this.IsPathDirty = true; }
        }

        /// <summary>
        /// Refers to the boldness of the font.
        /// </summary>
        [SvgAttribute("font-weight")]
        public virtual SvgFontWeight FontWeight
        {
            get { return (this.Attributes["font-weight"] == null) ? SvgFontWeight.inherit : (SvgFontWeight)this.Attributes["font-weight"]; }
            set { this.Attributes["font-weight"] = value; this.IsPathDirty = true; }
        }

        private enum FontParseState
        {
            fontStyle,
            fontVariant,
            fontWeight,
            fontSize,
            fontFamilyNext,
            fontFamilyCurr
        }

        /// <summary>
        /// Set all font information.
        /// </summary>
        [SvgAttribute("font")]
        public virtual string Font
        {
            get { return (this.Attributes["font"] == null ? "" : this.Attributes["font"] as string); }
            set
            {
                var state = FontParseState.fontStyle;
                var parts = value.Split(' ');

                SvgFontStyle fontStyle;
                SvgFontVariant fontVariant;
                SvgFontWeight fontWeight;
                SvgUnit fontSize;

                bool success;
                string[] sizes;
                string part;

                for (int i = 0; i < parts.Length; i++)
                {
                    part = parts[i];
                    success = false;
                    while (!success)
                    {
                        switch (state)
                        {
                            case FontParseState.fontStyle:
                                success = Enums.TryParse<SvgFontStyle>(part, out fontStyle);
                                if (success) this.FontStyle = fontStyle;
                                state++;
                                break;
                            case FontParseState.fontVariant:
                                success = Enums.TryParse<SvgFontVariant>(part, out fontVariant);
                                if (success) this.FontVariant = fontVariant;
                                state++;
                                break;
                            case FontParseState.fontWeight:
                                success = Enums.TryParse<SvgFontWeight>(part, out fontWeight);
                                if (success) this.FontWeight = fontWeight;
                                state++;
                                break;
                            case FontParseState.fontSize:
                                sizes = part.Split('/');
                                try
                                {
                                    fontSize = (SvgUnit)(new SvgUnitConverter().ConvertFromInvariantString(sizes[0]));
                                    success = true;
                                    this.FontSize = fontSize;
                                }
                                catch { }
                                state++;
                                break;
                            case FontParseState.fontFamilyNext:
                                state++;
                                success = true;
                                break;
                        }
                    }

                    switch (state)
                    {
                        case FontParseState.fontFamilyNext:
                            this.FontFamily = string.Join(" ", parts, i + 1, parts.Length - (i + 1));
                            i = int.MaxValue - 2;
                            break;
                        case FontParseState.fontFamilyCurr:
                            this.FontFamily = string.Join(" ", parts, i, parts.Length - (i));
                            i = int.MaxValue - 2;
                            break;
                    }

                }

                this.Attributes["font"] = value;
                this.IsPathDirty = true;
            }
        }

        private const string DefaultFontFamily = "Times New Roman";

        /// <summary>
        /// Get the font information based on data stored with the text object or inherited from the parent.
        /// </summary>
        /// <returns></returns>
        internal System.Drawing.Font GetFont(SvgRenderer renderer)
        {
            // Get the font-size
            float fontSize;
            var fontSizeUnit = this.FontSize;
            if (fontSizeUnit == SvgUnit.None || fontSizeUnit == SvgUnit.Empty)
            {
                fontSize = 1.0f;
            }
            else
            {
                fontSize = fontSizeUnit.ToDeviceValue(renderer, UnitRenderingType.Vertical, this);
            }

            var fontStyle = System.Drawing.FontStyle.Regular;

            // Get the font-weight
            switch (this.FontWeight)
            {
                case SvgFontWeight.bold:
                case SvgFontWeight.bolder:
                case SvgFontWeight.w600:
                case SvgFontWeight.w700:
                case SvgFontWeight.w800:
                case SvgFontWeight.w900:
                    fontStyle |= System.Drawing.FontStyle.Bold;
                    break;
            }

            // Get the font-style
            switch (this.FontStyle)
            {
                case SvgFontStyle.italic:
                case SvgFontStyle.oblique:
                    fontStyle |= System.Drawing.FontStyle.Italic;
                    break;
            }

            // Get the text-decoration
            switch (this.TextDecoration)
            {
                case SvgTextDecoration.lineThrough:
                    fontStyle |= System.Drawing.FontStyle.Strikeout;
                    break;
                case SvgTextDecoration.underline:
                    fontStyle |= System.Drawing.FontStyle.Underline;
                    break;
            }

            // Get the font-family
            string family = ValidateFontFamily(this.FontFamily) ?? DefaultFontFamily;
            return new System.Drawing.Font(family, fontSize, fontStyle, System.Drawing.GraphicsUnit.Pixel);
        }

        private static string ValidateFontFamily(string fontFamilyList)
        {
            // Split font family list on "," and then trim start and end spaces and quotes.
            var fontParts = (fontFamilyList ?? "").Split(new[] { ',' }).Select(fontName => fontName.Trim(new[] { '"', ' ', '\'' }));

            var families = System.Drawing.FontFamily.Families;

            // Find a the first font that exists in the list of installed font families.
            //styles from IE get sent through as lowercase.
            foreach (var f in fontParts.Where(f => families.Any(family => family.Name.ToLower() == f.ToLower())))
            {
                return f;
            }
            // No valid font family found from the list requested.
            return null;
        }

    }
}