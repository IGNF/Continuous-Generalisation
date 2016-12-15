using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;

namespace ContinuousGeneralizer.FrmGISCommand
{
    public partial class FrmSymbolSelector : Form
    {
        private IStyleGalleryItem pStyleGalleryItem;
        private ILegendClass pLegendClass;
        private ILayer pLayer;
        public ISymbol pSymbol;
        public Image pSymbolImage;
        //�˵��Ƿ��Ѿ���ʼ����־
        bool contextMenuMoreSymbolInitiated = false;

        //�޸�frmSymbolSelector�Ĺ��캯��������ͼ���ͼ���ӿڡ��������£�
        /// <summary>
        /// ���캯��,��ʼ��ȫ�ֱ���
        /// </summary>
        /// <param name="tempLegendClass">TOCͼ��</param>
        /// <param name="tempLayer">ͼ��</param>
        public FrmSymbolSelector(ILegendClass tempLegendClass, ILayer tempLayer)
        {
            InitializeComponent();
            this.pLegendClass = tempLegendClass;
            this.pLayer = tempLayer;
        }

        //���SymbolSelectorFrm��Load�¼�������ͼ������ΪSymbologyControl������Ӧ�ķ�����ʽ�ļ�����㡢�ߡ��棬�����ÿؼ��Ŀ�����
        private void FrmSymbolSelector_Load(object sender, EventArgs e)
        {
            //ȡ��ArcGIS��װ·��
            string sInstall = ReadRegistry("SOFTWARE\\ESRI\\CoreRuntime");
            //����ESRI.ServerStyle�ļ���SymbologyControl
            this.axSymbologyControl.LoadStyleFile(sInstall + "\\Styles\\ESRI.ServerStyle");
            //ȷ��ͼ�������(������),���ú�SymbologyControl��StyleClass,���úø��ؼ��Ŀɼ���(visible)
            IGeoFeatureLayer pGeoFeatureLayer = (IGeoFeatureLayer)pLayer;
            switch (((IFeatureLayer)pLayer).FeatureClass.ShapeType)
            {
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
                    this.SetFeatureClassStyle(esriSymbologyStyleClass.esriStyleClassMarkerSymbols);
                    this.lblAngle.Visible = true;
                    this.nudAngle.Visible = true;
                    this.lblSize.Visible = true;
                    this.nudSize.Visible = true;
                    this.lblWidth.Visible = false;
                    this.nudWidth.Visible = false;
                    this.lblOutlineColor.Visible = false;
                    this.btnOutlineColor.Visible = false;
                    break;
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline:
                    this.SetFeatureClassStyle(esriSymbologyStyleClass.esriStyleClassLineSymbols);
                    this.lblAngle.Visible = false;
                    this.nudAngle.Visible = false;
                    this.lblSize.Visible = false;
                    this.nudSize.Visible = false;
                    this.lblWidth.Visible = true;
                    this.nudWidth.Visible = true;
                    this.lblOutlineColor.Visible = false;
                    this.btnOutlineColor.Visible = false;
                    break;
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
                    this.SetFeatureClassStyle(esriSymbologyStyleClass.esriStyleClassFillSymbols);
                    this.lblAngle.Visible = false;
                    this.nudAngle.Visible = false;
                    this.lblSize.Visible = false;
                    this.nudSize.Visible = false;
                    this.lblWidth.Visible = true;
                    this.nudWidth.Visible = true;
                    this.lblOutlineColor.Visible = true;
                    this.btnOutlineColor.Visible = true;
                    break;
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryMultiPatch:
                    this.SetFeatureClassStyle(esriSymbologyStyleClass.esriStyleClassFillSymbols);
                    this.lblAngle.Visible = false;
                    this.nudAngle.Visible = false;
                    this.lblSize.Visible = false;
                    this.nudSize.Visible = false;
                    this.lblWidth.Visible = true;
                    this.nudWidth.Visible = true;
                    this.lblOutlineColor.Visible = true;
                    this.btnOutlineColor.Visible = true;
                    break;
                default:
                    this.Close();
                    break;
            }
        }

        //���SymbolControl��SymbologyStyleClass���ú���SetFeatureClassStyle()���������£�
        /// <summary>
        /// ��ʼ��SymbologyControl��StyleClass,ͼ��������з���,��ѷ�����ӵ�SymbologyControl�еĵ�һ������,��ѡ��
        /// </summary>
        /// <param name="symbologyStyleClass"></param>
        private void SetFeatureClassStyle(esriSymbologyStyleClass symbologyStyleClass)
        {
            this.axSymbologyControl.StyleClass = symbologyStyleClass;
            ISymbologyStyleClass pSymbologyStyleClass = this.axSymbologyControl.GetStyleClass(symbologyStyleClass);
            if (this.pLegendClass != null)
            {
                IStyleGalleryItem currentStyleGalleryItem = new ServerStyleGalleryItem();
                currentStyleGalleryItem.Name = "��ǰ����";
                currentStyleGalleryItem.Item = pLegendClass.Symbol;
                pSymbologyStyleClass.AddItem(currentStyleGalleryItem, 0);
                this.pStyleGalleryItem = currentStyleGalleryItem;
            }
            pSymbologyStyleClass.SelectItem(0);
        }

        //��4�����ע����ȡ����ReadRegistry()���˺�����ע����ж�ȡArcGIS�İ�װ·�����������£�
        /// <summary>
        /// ��ע�����ȡ��ָ�������·��
        /// </summary>
        /// <param name="sKey"></param>
        /// <returns></returns>
        private string ReadRegistry(string sKey)
        {
            //Open the subkey for reading
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(sKey, true);
            if (rk == null) return "";
            // Get the data from a specified item in the key.
            return (string)rk.GetValue("InstallDir");
        }

        /// <summary>
        /// ȷ����ť
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            //ȡ��ѡ���ķ���
            this.pSymbol = (ISymbol)pStyleGalleryItem.Item;
            //����Ԥ��ͼ��
            this.pSymbolImage = this.ptbPreview.Image;
            //�رմ���
            this.Close();

        }

        /// <summary>
        /// ȡ����ť
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        //Ϊ�˲����ϵķ��㣬�������SymbologyControl��DoubleClick�¼�����˫������ʱͬ����ȷ����ťһ����ѡ�����Ų��رշ���ѡ�������塣�������£�
        /// <summary>
        /// ˫������ͬ����ȷ����ť���رշ���ѡ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axSymbologyControl_OnDoubleClick(object sender, ISymbologyControlEvents_OnDoubleClickEvent e)
        {
            this.btnOK.PerformClick();
        }

        //����ӷ���Ԥ�����������û�ѡ��ĳһ����ʱ�����ſ�����ʾ��PictureBox�ؼ��У�����Ԥ���������������£�
        /// <summary>
        /// ��ѡ�в����úõķ�����picturebox�ؼ���Ԥ��
        /// </summary>
        private void PreviewImage()
        {
            //stdole.IPictureDisp picture = this.axSymbologyControl.GetStyleClass(this.axSymbologyControl.StyleClass).PreviewItem(pStyleGalleryItem, this.ptbPreview.Width, this.ptbPreview.Height);
            //System.Drawing.Image image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
            //this.ptbPreview.Image = image;
        }

        //��SymbologyControl����ʽ�ı�ʱ����Ҫ�������÷��Ų��������ؼ��Ŀ����ԣ���Ҫ���SymbologyControl��OnStyleClassChanged�¼����¼�������Load�¼����ƣ����£�
        /// <summary>
        /// ����ʽ��Style���ı�ʱ���������÷������ͺͿؼ��Ŀ�����/// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axSymbologyControl_OnStyleClassChanged(object sender, ESRI.ArcGIS.Controls.ISymbologyControlEvents_OnStyleClassChangedEvent e)
        {
            switch ((esriSymbologyStyleClass)(e.symbologyStyleClass))
            {
                case esriSymbologyStyleClass.esriStyleClassMarkerSymbols:
                    this.lblAngle.Visible = true;
                    this.nudAngle.Visible = true;
                    this.lblSize.Visible = true;
                    this.nudSize.Visible = true;
                    this.lblWidth.Visible = false;
                    this.nudWidth.Visible = false;
                    this.lblOutlineColor.Visible = false;
                    this.btnOutlineColor.Visible = false;
                    break;

                case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                    this.lblAngle.Visible = false;
                    this.nudAngle.Visible = false;
                    this.lblSize.Visible = false;
                    this.nudSize.Visible = false;
                    this.lblWidth.Visible = true;
                    this.nudWidth.Visible = true;
                    this.lblOutlineColor.Visible = false;
                    this.btnOutlineColor.Visible = false;
                    break;

                case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                    this.lblAngle.Visible = false;
                    this.nudAngle.Visible = false;
                    this.lblSize.Visible = false;
                    this.nudSize.Visible = false;
                    this.lblWidth.Visible = true;
                    this.nudWidth.Visible = true;
                    this.lblOutlineColor.Visible = true;
                    this.btnOutlineColor.Visible = true;
                    break;
            }
        }

        //���SymbologyControl��OnItemSelected�¼������¼������ѡ�з���ʱ��������ʱ��ʾ��ѡ�����ŵĳ�ʼ�������¼���Ӧ������������
        /// <summary>
        /// ѡ�з���ʱ�������¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void axSymbologyControl_OnItemSelected(object sender, ISymbologyControlEvents_OnItemSelectedEvent e)
        {
            pStyleGalleryItem = (IStyleGalleryItem)e.styleGalleryItem;
            Color color;
            switch (this.axSymbologyControl.StyleClass)
            {
                //�����
                case esriSymbologyStyleClass.esriStyleClassMarkerSymbols:
                    color = this.ConvertIRgbColorToColor(((IMarkerSymbol)pStyleGalleryItem.Item).Color as IRgbColor);
                    //���õ���ŽǶȺʹ�С��ʼֵ
                    this.nudAngle.Value = (decimal)((IMarkerSymbol)this.pStyleGalleryItem.Item).Angle;
                    this.nudSize.Value = (decimal)((IMarkerSymbol)this.pStyleGalleryItem.Item).Size;
                    break;
                //�߷���
                case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                    color = this.ConvertIRgbColorToColor(((ILineSymbol)pStyleGalleryItem.Item).Color as IRgbColor);
                    //�����߿��ʼֵ
                    this.nudWidth.Value = (decimal)((ILineSymbol)this.pStyleGalleryItem.Item).Width;
                    break;
                //�����
                case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                    color = this.ConvertIRgbColorToColor(((IFillSymbol)pStyleGalleryItem.Item).Color as IRgbColor);
                    this.btnOutlineColor.BackColor = this.ConvertIRgbColorToColor(((IFillSymbol)pStyleGalleryItem.Item).Outline.Color as IRgbColor);
                    //��������߿�ȳ�ʼֵ
                    this.nudWidth.Value = (decimal)((IFillSymbol)this.pStyleGalleryItem.Item).Outline.Width;
                    break;
                default:
                    color = Color.Black;
                    break;
            }
            //���ð�ť����ɫ
            this.btnColor.BackColor = color;
            //Ԥ������
            this.PreviewImage();
        }

        //���nudSize�ؼ���ValueChanged�¼������ڿؼ���ֵ�ı�ʱ��Ӧ���¼���Ȼ���������õ���ŵĴ�С
        /// <summary>
        /// �������Ŵ�С-�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudSize_ValueChanged(object sender, EventArgs e)
        {
            ((IMarkerSymbol)this.pStyleGalleryItem.Item).Size = (double)this.nudSize.Value;
            this.PreviewImage();
        }


        //���nudAngle�ؼ���ValueChanged�¼������������õ���ŵĽǶȡ��������£�
        /// <summary>
        /// �������ŽǶ�-�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudAngle_ValueChanged(object sender, EventArgs e)
        {
            ((IMarkerSymbol)this.pStyleGalleryItem.Item).Angle = (double)this.nudAngle.Value;
            this.PreviewImage();
        }


        //���nudWidth�ؼ���ValueChanged�¼��������������߷��ŵ��߿������ŵ�����ߵ��߿�
        /// <summary>
        /// �������ſ��-�����߷��ź������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudWidth_ValueChanged(object sender, EventArgs e)
        {
            switch (this.axSymbologyControl.StyleClass)
            {
                case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                    ((ILineSymbol)this.pStyleGalleryItem.Item).Width = Convert.ToDouble(this.nudWidth.Value);
                    break;
                case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                    //ȡ������ŵ������߷���
                    ILineSymbol pLineSymbol = ((IFillSymbol)this.pStyleGalleryItem.Item).Outline;
                    pLineSymbol.Width = Convert.ToDouble(this.nudWidth.Value);
                    ((IFillSymbol)this.pStyleGalleryItem.Item).Outline = pLineSymbol;
                    break;
            }
            this.PreviewImage();
        }

        //��ɫת��
        //��ArcGIS Engine�У���ɫ��IRgbColor�ӿ�ʵ�֣�����.NET����У���ɫ����Color�ṹ��ʾ�����ڵ�����ɫ����֮ǰ�����Ǳ�������������ֲ�ͬ��ɫ��ʾ��ʽ��ת����
        //������������ɫ�ṹ�ľ�����Ϣ���������в���������ϡ��������������ɫת��������
        //ArcGIS Engine�е�IRgbColor�ӿ�ת����.NET�е�Color�ṹ�ĺ�����
        /// <summary>
        /// ��ArcGIS Engine�е�IRgbColor�ӿ�ת����.NET�е�Color�ṹ
        /// </summary>
        /// <param name="pRgbColor">IRgbColor</param>
        /// <returns>.NET�е�System.Drawing.Color�ṹ��ʾARGB��ɫ</returns>
        public Color ConvertIRgbColorToColor(IRgbColor pRgbColor)
        {
            return ColorTranslator.FromOle(pRgbColor.RGB);
        }
        //.NET�е�Color�ṹת������ArcGIS Engine�е�IColor�ӿڵĺ�����
        /// <summary>
        /// ��.NET�е�Color�ṹת������ArcGIS Engine�е�IColor�ӿ�
        /// </summary>
        /// <param name="color">.NET�е�System.Drawing.Color�ṹ��ʾARGB��ɫ</param>
        /// <returns>IColor</returns>
        public IColor ConvertColorToIColor(Color color)
        {
            IColor pColor = new RgbColorClass();
            pColor.RGB = color.B * 65536 + color.G * 256 + color.R;
            return pColor;
        }

        //�������з��ŵ���ɫ
        //ѡ����ɫʱ�����ǵ���.NET����ɫ�Ի���ColorDialog��ѡ����ɫ���޸���ɫ��ť�ı���ɫΪѡ������ɫ���Է���Ԥ����˫��btnColor��ť��������´���
        /// <summary>
        /// ��ɫ��ť
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnColor_Click(object sender, EventArgs e)
        {
            //����ϵͳ��ɫ�Ի���
            if (this.colorDialog.ShowDialog() == DialogResult.OK)
            {
                //����ɫ��ť�ı�����ɫ����Ϊ�û�ѡ������ɫ
                this.btnColor.BackColor = this.colorDialog.Color;
                //���÷�����ɫΪ�û�ѡ������ɫ
                switch (this.axSymbologyControl.StyleClass)
                {
                    //�����
                    case esriSymbologyStyleClass.esriStyleClassMarkerSymbols:
                        ((IMarkerSymbol)this.pStyleGalleryItem.Item).Color = this.ConvertColorToIColor(this.colorDialog.Color);
                        break;
                    //�߷���
                    case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                        ((ILineSymbol)this.pStyleGalleryItem.Item).Color = this.ConvertColorToIColor(this.colorDialog.Color);
                        break;
                    //�����
                    case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                        ((IFillSymbol)this.pStyleGalleryItem.Item).Color = this.ConvertColorToIColor(this.colorDialog.Color);
                        break;
                }
                //���·���Ԥ��
                this.PreviewImage();
            }
        }


        //��������ŵ��������ɫ
        /// <summary>
        /// �����ɫ��ť
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutlineColor_Click(object sender, EventArgs e)
        {
            if (this.colorDialog.ShowDialog() == DialogResult.OK)
            {
                //ȡ��������е�����߷���
                ILineSymbol pLineSymbol = ((IFillSymbol)this.pStyleGalleryItem.Item).Outline;
                //�����������ɫ
                pLineSymbol.Color = this.ConvertColorToIColor(this.colorDialog.Color);
                //��������������е�����߷���
                ((IFillSymbol)this.pStyleGalleryItem.Item).Outline = pLineSymbol;
                //���ð�ť������ɫ
                this.btnOutlineColor.BackColor = this.colorDialog.Color;
                //���·���Ԥ��
                this.PreviewImage();
            }
        }

        //�ڴ��¼���Ӧ�����У�����Ҫ���ServerStyle�ļ��Ķ�ȡ�������ļ�����Ϊ�˵����������ɲ˵�����ʾ�˵�
        /// <summary>
        /// ��������š�����ʱ�������¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMoreSymbols_Click(object sender, EventArgs e)
        {
            if (this.contextMenuMoreSymbolInitiated == false)
            {
                string sInstall = ReadRegistry("SOFTWARE\\ESRI\\CoreRuntime");
                string path = System.IO.Path.Combine(sInstall, "Styles");
                //ȡ�ò˵�������
                string[] styleNames = System.IO.Directory.GetFiles(path, "*.ServerStyle");
                ToolStripMenuItem[] symbolContextMenuItem = new ToolStripMenuItem[styleNames.Length + 1];
                //ѭ������������Ų˵���˵�
                for (int i = 0; i < styleNames.Length; i++)
                {
                    symbolContextMenuItem[i] = new ToolStripMenuItem();
                    symbolContextMenuItem[i].CheckOnClick = true;
                    symbolContextMenuItem[i].Text = System.IO.Path.GetFileNameWithoutExtension(styleNames[i]);
                    if (symbolContextMenuItem[i].Text == "ESRI")
                    {
                        symbolContextMenuItem[i].Checked = true;
                    }
                    symbolContextMenuItem[i].Name = styleNames[i];
                }
                //��ӡ�������š��˵���˵����һ��
                symbolContextMenuItem[styleNames.Length] = new ToolStripMenuItem();
                symbolContextMenuItem[styleNames.Length].Text = "��ӷ���";
                symbolContextMenuItem[styleNames.Length].Name = "AddMoreSymbol";
                //������еĲ˵���˵�
                this.contextMenuStripMoreSymbol.Items.AddRange(symbolContextMenuItem);
                this.contextMenuMoreSymbolInitiated = true;
            }
            //��ʾ�˵�
            this.contextMenuStripMoreSymbol.Show(this.btnMoreSymbols.Location);
        }

        //������ĳһ�˵���ʱ��ӦItemClicked�¼�����ѡ�е�ServerStyle�ļ����뵽SymbologyControl�в�ˢ�¡����û���������ӷ��š��˵���ʱ���������ļ��Ի��򣬹��û�ѡ��������ServerStyle�ļ�
        /// <summary>
        /// ��������š���ť�����Ĳ˵�����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStripMoreSymbol_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem pToolStripMenuItem = (ToolStripMenuItem)e.ClickedItem;
            //����������ǡ���ӷ��š�
            if (pToolStripMenuItem.Name == "AddMoreSymbol")
            {
                //�������ļ��Ի���
                if (this.openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //����style file��SymbologyControl
                    this.axSymbologyControl.LoadStyleFile(this.openFileDialog.FileName);
                    //ˢ��axSymbologyControl�ؼ�
                    this.axSymbologyControl.Refresh();
                }
            }
            else//���������ѡ��
            {
                if (pToolStripMenuItem.Checked == false)
                {
                    this.axSymbologyControl.LoadStyleFile(pToolStripMenuItem.Name);
                    this.axSymbologyControl.Refresh();
                }
                else
                {
                    this.axSymbologyControl.RemoveFile(pToolStripMenuItem.Name);
                    this.axSymbologyControl.Refresh();
                }
            }
        }
    }
}