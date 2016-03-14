﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            svgImage.Image = svgDoc.Draw();

            string outputDir = Path.GetDirectoryName(svgDoc.BaseUri == null ? Application.ExecutablePath : svgDoc.BaseUri.LocalPath);
            svgImage.Image.Save(Path.Combine(outputDir, "output.png"));
        }
    }
}
