using NUnit.Framework;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Text;
using PdfSharp.Drawing.Layout;
using System;
using PdfSharp;

namespace PDFSharpTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }
        const string DESKTOPPATH = @"C:\Users\SwordOfJustice\Desktop\";
        [Test]
        public void CreateSimplePDF()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //创建一个PDF文档对象
            PdfDocument doc = new PdfDocument();
            //设置标题，可在文档属性中看到此标题
            doc.Info.Title = "测试 PDF 文档";
            //创建一个空页面
            PdfPage page1 = doc.Pages.Add();
            //一个绘图对象
            XGraphics gfx = XGraphics.FromPdfPage(page1);
            //创建字体
            XFont font = new XFont("华文楷体", 20, XFontStyle.Regular);
            //绘制字体
            gfx.DrawString("你好，世界！ Hello World!", font, XBrushes.Black, 
                new XRect(0, 0, page1.Width, page1.Height), 
                XStringFormats.Center);
            //保存文档
            doc.Save(DESKTOPPATH + "test1.pdf");
        }

        /// <summary>
        /// 文本布局
        /// </summary>
        [Test]
        public void MultipleBlockText()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages.Add();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("华文楷体", 10, XFontStyle.Regular);
            //XTextFormatter 支持自动换行
            XTextFormatter tf = new XTextFormatter(gfx);
            

            //生成一大段测试文本
            string s = GetTestString(100);

            //第一个文本块
            XRect rect = new XRect(40, 100, 250, 250);
            gfx.DrawRectangle(XBrushes.AliceBlue, rect);
            //文本左对齐（默认值）
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(s, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            //第二个文本块
            rect = new XRect(310, 100, 250, 250);
            gfx.DrawRectangle(XBrushes.SeaShell, rect);
            //右对齐
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(s, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            //第三个文本块
            rect = new XRect(40, 400, 250, 250);
            gfx.DrawRectangle(XBrushes.LightGray, rect);
            //居中对齐
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(s, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            //第四个文本块
            rect = new XRect(310, 400, 250, 250);
            gfx.DrawRectangle(XBrushes.LightGreen, rect);
            //拉伸对齐（两端对齐）
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(s, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            //保存文件
            document.Save(DESKTOPPATH + "test1.pdf");
        }

        private string GetTestString(int count)
        {
            string s = "";
            for(int i = 0; i < count;i++)
            {
                s += string.Format(" 测试文本{0}", i);
            }
            return s;
        }

        /// <summary>
        /// 创建书签
        /// </summary>
        [Test]
        public void CreateBookmark()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PdfDocument document = new PdfDocument();
            PdfPage page1 = document.Pages.Add();
            XGraphics gfx = XGraphics.FromPdfPage(page1);
            XFont font = new XFont("华文楷体", 15, XFontStyle.Regular);
            //XTextFormatter 支持自动换行
            XTextFormatter tf = new XTextFormatter(gfx);
            gfx.DrawString("页面 1 Page 1", font, XBrushes.Blue, 20, 30,XStringFormats.Default);

            //创建根书签，参数分别为：标题, 链接到的页面, 是否展开, 字体样式, 字体颜色
            PdfOutline outline = document.Outlines.Add("第一章 测试", page1, true, PdfOutlineStyle.Bold, XColors.Red);

            //创建更多页面
            for(int i = 0; i < 10; i++)
            {
                page1 = document.AddPage();
                gfx = XGraphics.FromPdfPage(page1);

                string str = string.Format("页面 {0} Page {0}", i + 2);
                gfx.DrawString(str, font, XBrushes.Black, new XRect(20,50,page1.Width,page1.Height), XStringFormats.Center);

                //创建子书签
                outline.Outlines.Add("页面" + (i + 2).ToString(), page1, true);
            }

            //保存
            document.Save(DESKTOPPATH + "test2.pdf");
        }

        /// <summary>
        /// 页面尺寸
        /// </summary>
        [Test]
        public void PageSize()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PdfDocument document = new PdfDocument();
            XFont font = new XFont("华文楷体", 35, XFontStyle.Regular);
            //PdfSharp.PageSize 枚举中定义了所有常用的纸张尺寸
            PageSize[] pageSizes = (PageSize[])Enum.GetValues(typeof(PageSize));
            
            foreach(PageSize pageSize in pageSizes)
            {
                if (pageSize == PdfSharp.PageSize.Undefined)
                    continue;

                //纵向页面
                PdfPage page = document.AddPage();
                page.Size = pageSize;
                //页面方向，默认为纵向
                //page.Orientation = PageOrientation.Portrait;
                XGraphics gfx = XGraphics.FromPdfPage(page);
                //这里将整个页面作为一个矩形来绘制，文本居中对齐
                gfx.DrawString(pageSize.ToString(), font, XBrushes.Black, 
                    new XRect(0,0,page.Width,page.Height), XStringFormats.Center);

                //横向页面
                page = document.AddPage();
                page.Size = pageSize;
                page.Orientation = PageOrientation.Landscape;
                gfx = XGraphics.FromPdfPage(page);
                gfx.DrawString(pageSize + "(横向)", font, XBrushes.Black,
                    new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            }

            //保存
            document.Save(DESKTOPPATH + "PageSize.pdf");
        }

        [Test]
        public void TestCreate()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PdfDocument document = new PdfDocument();
            var page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont fontMainTitle = new XFont("黑体", 20, XFontStyle.Bold);
            //标题
            gfx.DrawString("MOD 值班记录", fontMainTitle, XBrushes.Black, new XRect(0, 0, page.Width, 80), XStringFormats.Center);

            XTextFormatter tf = new XTextFormatter(gfx);
            XRect rect = new XRect(0, 100, page.Width, 20);
            gfx.DrawRectangle(XBrushes.LightPink, rect);
            XFont fontSubtitle = new XFont("黑体", 18, XFontStyle.Bold);
            gfx.DrawString("每日新冠肺炎登记表", fontSubtitle, XBrushes.Black, rect, XStringFormats.Center);
            
            document.Save(DESKTOPPATH + "MOD值班记录.pdf");
        }

    }
}