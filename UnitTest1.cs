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
            //����һ��PDF�ĵ�����
            PdfDocument doc = new PdfDocument();
            //���ñ��⣬�����ĵ������п����˱���
            doc.Info.Title = "���� PDF �ĵ�";
            //����һ����ҳ��
            PdfPage page1 = doc.Pages.Add();
            //һ����ͼ����
            XGraphics gfx = XGraphics.FromPdfPage(page1);
            //��������
            XFont font = new XFont("���Ŀ���", 20, XFontStyle.Regular);
            //��������
            gfx.DrawString("��ã����磡 Hello World!", font, XBrushes.Black, 
                new XRect(0, 0, page1.Width, page1.Height), 
                XStringFormats.Center);
            //�����ĵ�
            doc.Save(DESKTOPPATH + "test1.pdf");
        }

        /// <summary>
        /// �ı�����
        /// </summary>
        [Test]
        public void MultipleBlockText()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages.Add();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("���Ŀ���", 10, XFontStyle.Regular);
            //XTextFormatter ֧���Զ�����
            XTextFormatter tf = new XTextFormatter(gfx);
            

            //����һ��β����ı�
            string s = GetTestString(100);

            //��һ���ı���
            XRect rect = new XRect(40, 100, 250, 250);
            gfx.DrawRectangle(XBrushes.AliceBlue, rect);
            //�ı�����루Ĭ��ֵ��
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(s, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            //�ڶ����ı���
            rect = new XRect(310, 100, 250, 250);
            gfx.DrawRectangle(XBrushes.SeaShell, rect);
            //�Ҷ���
            tf.Alignment = XParagraphAlignment.Right;
            tf.DrawString(s, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            //�������ı���
            rect = new XRect(40, 400, 250, 250);
            gfx.DrawRectangle(XBrushes.LightGray, rect);
            //���ж���
            tf.Alignment = XParagraphAlignment.Center;
            tf.DrawString(s, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            //���ĸ��ı���
            rect = new XRect(310, 400, 250, 250);
            gfx.DrawRectangle(XBrushes.LightGreen, rect);
            //������루���˶��룩
            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(s, font, XBrushes.Black, rect, XStringFormats.TopLeft);

            //�����ļ�
            document.Save(DESKTOPPATH + "test1.pdf");
        }

        private string GetTestString(int count)
        {
            string s = "";
            for(int i = 0; i < count;i++)
            {
                s += string.Format(" �����ı�{0}", i);
            }
            return s;
        }

        /// <summary>
        /// ������ǩ
        /// </summary>
        [Test]
        public void CreateBookmark()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PdfDocument document = new PdfDocument();
            PdfPage page1 = document.Pages.Add();
            XGraphics gfx = XGraphics.FromPdfPage(page1);
            XFont font = new XFont("���Ŀ���", 15, XFontStyle.Regular);
            //XTextFormatter ֧���Զ�����
            XTextFormatter tf = new XTextFormatter(gfx);
            gfx.DrawString("ҳ�� 1 Page 1", font, XBrushes.Blue, 20, 30,XStringFormats.Default);

            //��������ǩ�������ֱ�Ϊ������, ���ӵ���ҳ��, �Ƿ�չ��, ������ʽ, ������ɫ
            PdfOutline outline = document.Outlines.Add("��һ�� ����", page1, true, PdfOutlineStyle.Bold, XColors.Red);

            //��������ҳ��
            for(int i = 0; i < 10; i++)
            {
                page1 = document.AddPage();
                gfx = XGraphics.FromPdfPage(page1);

                string str = string.Format("ҳ�� {0} Page {0}", i + 2);
                gfx.DrawString(str, font, XBrushes.Black, new XRect(20,50,page1.Width,page1.Height), XStringFormats.Center);

                //��������ǩ
                outline.Outlines.Add("ҳ��" + (i + 2).ToString(), page1, true);
            }

            //����
            document.Save(DESKTOPPATH + "test2.pdf");
        }

        /// <summary>
        /// ҳ��ߴ�
        /// </summary>
        [Test]
        public void PageSize()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PdfDocument document = new PdfDocument();
            XFont font = new XFont("���Ŀ���", 35, XFontStyle.Regular);
            //PdfSharp.PageSize ö���ж��������г��õ�ֽ�ųߴ�
            PageSize[] pageSizes = (PageSize[])Enum.GetValues(typeof(PageSize));
            
            foreach(PageSize pageSize in pageSizes)
            {
                if (pageSize == PdfSharp.PageSize.Undefined)
                    continue;

                //����ҳ��
                PdfPage page = document.AddPage();
                page.Size = pageSize;
                //ҳ�淽��Ĭ��Ϊ����
                //page.Orientation = PageOrientation.Portrait;
                XGraphics gfx = XGraphics.FromPdfPage(page);
                //���ｫ����ҳ����Ϊһ�����������ƣ��ı����ж���
                gfx.DrawString(pageSize.ToString(), font, XBrushes.Black, 
                    new XRect(0,0,page.Width,page.Height), XStringFormats.Center);

                //����ҳ��
                page = document.AddPage();
                page.Size = pageSize;
                page.Orientation = PageOrientation.Landscape;
                gfx = XGraphics.FromPdfPage(page);
                gfx.DrawString(pageSize + "(����)", font, XBrushes.Black,
                    new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
            }

            //����
            document.Save(DESKTOPPATH + "PageSize.pdf");
        }

        [Test]
        public void TestCreate()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            PdfDocument document = new PdfDocument();
            var page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont fontMainTitle = new XFont("����", 20, XFontStyle.Bold);
            //����
            gfx.DrawString("MOD ֵ���¼", fontMainTitle, XBrushes.Black, new XRect(0, 0, page.Width, 80), XStringFormats.Center);

            XTextFormatter tf = new XTextFormatter(gfx);
            XRect rect = new XRect(0, 100, page.Width, 20);
            gfx.DrawRectangle(XBrushes.LightPink, rect);
            XFont fontSubtitle = new XFont("����", 18, XFontStyle.Bold);
            gfx.DrawString("ÿ���¹ڷ��׵ǼǱ�", fontSubtitle, XBrushes.Black, rect, XStringFormats.Center);
            
            document.Save(DESKTOPPATH + "MODֵ���¼.pdf");
        }

    }
}