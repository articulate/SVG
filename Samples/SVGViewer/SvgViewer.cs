using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Svg;
using Svg.Transforms;
using System.Xml;
using System.IO;

namespace SVGViewer
{
    public partial class SVGViewer : Form
    {
        public SVGViewer()
        {
            InitializeComponent();
        }

        private void open_Click(object sender, EventArgs e)
        {
            if (openSvgFile.ShowDialog() == DialogResult.OK)
            {
            	SvgDocument svgDoc = SvgDocument.Open(openSvgFile.FileName);
            	
            	RenderSvg(svgDoc);
            }
        }

        private string FXML = "";

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        	using(var s = new MemoryStream(UTF8Encoding.Default.GetBytes(textBox1.Text)))
        	{
        		SvgDocument svgDoc = SvgDocument.Open<SvgDocument>(s, null);
        		RenderSvg(svgDoc);
        	}
        }
        
        private void RenderSvg(SvgDocument svgDoc)
        {
            var size = new Size(0, 0);
            using (var fileStream = File.OpenWrite(@"C:\Users\dbackes\Desktop\test.png"))
            {
                var image = svgDoc.Draw();
                image.Save(fileStream, ImageFormat.Png);
                size = image.Size;
            }

            var bitmap = DrawBitmap(svgDoc, new SizeF(1024, 768));
            var bitmap2 = DrawBitmap(svgDoc, new SizeF(size.Width*.9f, size.Height*.9f));

            svgImage.Image = bitmap;
        }

        private static Bitmap DrawBitmap(SvgDocument svgDoc, SizeF size)
        {
            var bitmap = new Bitmap((int) Math.Round(size.Width), (int) Math.Round(size.Height));

            using (var graphics = Graphics.FromImage(bitmap))
            {
                var graphicsUnit = GraphicsUnit.Pixel;
                graphics.FillRectangle(Brushes.Magenta, bitmap.GetBounds(ref graphicsUnit));
            }

            svgDoc.Draw(bitmap);
            return bitmap;
        }
    }
}
