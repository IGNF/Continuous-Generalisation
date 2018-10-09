using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace ContinuousGeneralizer.FrmGISCommand
{
    public partial class FrmAttributeTable : Form
    {
        private ILayer m_pLayer;
        private IMapControl4 m_mapControl;

        public FrmAttributeTable()
        {
            InitializeComponent();
        }

        public FrmAttributeTable(ref IMapControl4 mapControl, ILayer pLayer)
        {
            InitializeComponent();
            m_mapControl = mapControl;
            m_pLayer = pLayer;
        }


        //������DataTable
        //���ȴ���ILayer���ٲ�ѯ��ITable����ITable�е�Fileds�л��ÿ��Field���ٸ���Filed����DataTable��DataColumn���ɴ˴���һ��ֻ��ͼ���ֶεĿ�DataTable��ʵ�ֺ������£�
        /// <summary>
        /// ����ͼ���ֶδ���һ��ֻ���ֶεĿ�DataTable
        /// </summary>
        /// <param name="pLayer"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private static DataTable CreateDataTableByLayer(ILayer pLayer, string tableName)
        {
            //����һ��DataTable��
            DataTable pDataTable = new DataTable(tableName);
            //ȡ��ITable�ӿ�
            ITable pTable = pLayer as ITable;
            IField pField = null;
            DataColumn pDataColumn;
            //����ÿ���ֶε����Խ���DataColumn����
            for (int i = 0; i < pTable.Fields.FieldCount; i++)
            {
                pField = pTable.Fields.get_Field(i);
                //�½�һ��DataColumn������������
                pDataColumn = new DataColumn(pField.Name);
                if (pField.Name == pTable.OIDFieldName)
                {
                    pDataColumn.Unique = true;//�ֶ�ֵ�Ƿ�Ψһ
                }
                //�ֶ�ֵ�Ƿ�����Ϊ��
                pDataColumn.AllowDBNull = pField.IsNullable;
                //�ֶα���
                pDataColumn.Caption = pField.AliasName;
                //�ֶ���������
                pDataColumn.DataType = System.Type.GetType(ParseFieldType(pField.Type));
                //�ֶ�Ĭ��ֵ
                pDataColumn.DefaultValue = pField.DefaultValue;
                //���ֶ�ΪString�����������ֶγ���
                if (pField.VarType == 8)
                {
                    pDataColumn.MaxLength = pField.Length;
                }
                //�ֶ���ӵ�����
                pDataTable.Columns.Add(pDataColumn);
                pField = null;
                pDataColumn = null;
            }
            return pDataTable;
        }


        //��ΪGeoDatabase������������.NET���������Ͳ�ͬ����Ҫ����ת����ת���������£�
        /// <summary>
        /// ��GeoDatabase�ֶ�����ת����.Net��Ӧ����������
        /// </summary>
        /// <param name="fieldType">�ֶ�����</param>
        /// <returns></returns>
        public static string ParseFieldType(esriFieldType fieldType)
        {
            switch (fieldType)
            {
                case esriFieldType.esriFieldTypeBlob:
                    return "System.String";
                case esriFieldType.esriFieldTypeDate:
                    return "System.DateTime";
                case esriFieldType.esriFieldTypeDouble:
                    return "System.Double";
                case esriFieldType.esriFieldTypeGeometry:
                    return "System.String";
                case esriFieldType.esriFieldTypeGlobalID:
                    return "System.String";
                case esriFieldType.esriFieldTypeGUID:
                    return "System.String";
                case esriFieldType.esriFieldTypeInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeOID:
                    return "System.String";
                case esriFieldType.esriFieldTypeRaster:
                    return "System.String";
                case esriFieldType.esriFieldTypeSingle:
                    return "System.Single";
                case esriFieldType.esriFieldTypeSmallInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeString:
                    return "System.String";
                default:
                    return "System.String";
            }
        }

        //װ��DataTable����
        //����һ���õ���DataTable��û�����ݣ�ֻ���ֶ���Ϣ����ˣ�����Ҫͨ��ICursor��ITable����һȡ��ÿһ�����ݣ���IRow��
        //�ٴ���DataTable����Ӧ��DataRow������IRow����DataRow��Ϣ���ٽ����е�DataRow��ӵ�DataTable�У��������DataTable���ݵ�װ�ء�
        //Ϊ��֤Ч�ʣ�һ�����ֻװ��2000�����ݵ�DataGridView�������������£�
        /// <summary>
        /// ���DataTable�е�����
        /// </summary>
        /// <param name="pLayer"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable CreateDataTable(ILayer pLayer, string tableName)
        {
            //������DataTable
            DataTable pDataTable = CreateDataTableByLayer(pLayer, tableName);
            //ȡ��ͼ������
            string shapeType = getShapeType(pLayer);
            //����DataTable���ж���
            DataRow pDataRow = null;
            //��ILayer��ѯ��ITable
            ITable pTable = pLayer as ITable;
            ICursor pCursor = pTable.Search(null, false);
            //ȡ��ITable�е�����Ϣ
            IRow pRow = pCursor.NextRow();
            int n = 0;
            while (pRow != null)
            {
                //�½�DataTable���ж���
                pDataRow = pDataTable.NewRow();
                for (int i = 0; i < pRow.Fields.FieldCount; i++)
                {
                    //����ֶ�����ΪesriFieldTypeGeometry�������ͼ�����������ֶ�ֵ
                    if (pRow.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        pDataRow[i] = shapeType;
                    }
                    //��ͼ������ΪAnotationʱ��Ҫ�����л���esriFieldTypeBlob���͵����ݣ�
                    //��洢���Ǳ�ע���ݣ��������轫��Ӧ���ֶ�ֵ����ΪElement
                    else if (pRow.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeBlob)
                    {
                        pDataRow[i] = "Element";
                    }
                    else
                    {
                        pDataRow[i] = pRow.get_Value(i);
                    }
                }
                //���DataRow��DataTable
                pDataTable.Rows.Add(pDataRow);
                pDataRow = null;
                n++;
                //Ϊ��֤Ч�ʣ�һ��ֻװ���������¼
                if (n == 2000)
                {
                    pRow = null;
                }
                else
                {
                    pRow = pCursor.NextRow();
                }
            }
            return pDataTable;
        }



        //����Ĵ������漰��һ����ȡͼ�����͵ĺ���getShapeTape���˺�����ͨ��ILayer�ж�ͼ�����͵ģ��������£�
        /// <summary>
        /// ���ͼ���Shape����
        /// </summary>
        /// <param name="pLayer">ͼ��</param>
        /// <returns></returns>
        public static string getShapeType(ILayer pLayer)
        {
            IFeatureLayer pFeatLyr = (IFeatureLayer)pLayer;
            switch (pFeatLyr.FeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    return "Point";
                case esriGeometryType.esriGeometryPolyline:
                    return "Polyline";
                case esriGeometryType.esriGeometryPolygon:
                    return "Polygon";
                default:
                    return "";
            }
        }

        //��DataTable��DataGridView
        //ͨ�����ϲ��裬�����Ѿ��õ���һ������ͼ���������ݵ�DataTable���ֶ���һ��AttributeTableFrm��ĳ�Ա������
        public DataTable attributeTable;
        //ͨ�����º��������Ǻ����׽���󶨵�DataGridView�ؼ��С�
        /// <summary>
        /// ��DataTable��DataGridView
        /// </summary>
        /// <param name="player"></param>
        public void CreateAttributeTable()
        {
            ILayer player = m_pLayer;
            string tableName;
            tableName = getVaIDFeatureClassName(player.Name);
            attributeTable = CreateDataTable(player, tableName);
            this.dataGridView.DataSource = attributeTable;
            this.Text = "���Ա�[" + tableName + "] " + "��¼����" + attributeTable.Rows.Count.ToString();
        }


        //��ΪDataTable�ı����������С�.������������á�_���滻���������£�
        /// <summary>
        /// �滻���ݱ����еĵ�
        /// </summary>
        /// <param name="FCname"></param>
        /// <returns></returns>
        public static string getVaIDFeatureClassName(string FCname)
        {
            int dot = FCname.IndexOf(".");
            if (dot != -1)
            {
                return FCname.Replace(".", "_");
            }
            return FCname;
        }


        private void dataGridView_MouseUp(object sender, MouseEventArgs e)
        {
            //MouseUp�¼�ʵ���ǽ����ѡ(��ѡ)���Ա���ʾ��ӦԪ�صľ���ѡ�񣬵��û��������ط����ʱ��dataGridView.SelectedRows.Count==0��
            //����ִ����������
            if (dataGridView.SelectedRows.Count > 0)
            {
                int i, j;
                IMapControl4 mapControl = m_mapControl;
                IMap pMap = mapControl.Map;
                pMap.ClearSelection();

                //"FID"�ֶ���ArcGIS�е��������(�����ϣ�intpFIDIndex == 0)
                ILayer pLayer = m_pLayer;
                ITable pTable = pLayer as ITable;
                int intpFIDIndex = pTable.FindField("FID");


                DataGridViewSelectedRowCollection pDataGridViewSelectedRowCollection = dataGridView.SelectedRows;  //��ȡ�û������Ա���ѡ�е���
                IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                IFeatureCursor pFeatureCursor = pFeatureLayer.FeatureClass.Search(null, true);
                int intFeatureCount = pFeatureLayer.FeatureClass.FeatureCount(null);  //��ȡҪ�ظ������޶�ѭ������
                IFeature pFeature = pFeatureCursor.NextFeature();
                for (i = 0; i < pDataGridViewSelectedRowCollection.Count; i++)
                {
                    DataGridViewRow pDataGridViewRow = pDataGridViewSelectedRowCollection[i];
                    int intFID = Convert.ToInt16(pDataGridViewRow.Cells["FID"].Value);

                    for (j = 0; j < intFeatureCount; j++)
                    {

                        //Ϊʲô��ÿ�ҵ�һ��Ҫ�ؾ�ִ�����"pFeatureCursor = pFeatureLayer.FeatureClass.Search(null, true);"����
                        //    ��ΪContinuousGeneralizer��ArcGIS�����Ա������Ƕ�Ӧ��(������û�з�������ȱ任���Ա�����������)��
                        //�����������У�ƥ��ĳ��Ԫ�سɹ���������ֱ��������һ��Ԫ��(����Ҳ����ƥ���)���������Խ�ʡ��������ʱ��
                        if (pFeature == null)
                        {
                            pFeatureCursor = pFeatureLayer.FeatureClass.Search(null, true);
                            pFeature = pFeatureCursor.NextFeature();
                        }

                        int intpFID = Convert.ToInt16(pFeature.get_Value(intpFIDIndex));
                        if (intFID == intpFID)
                        {
                            pMap.SelectFeature(pLayer, pFeature);
                            break;
                        }
                        pFeature = pFeatureCursor.NextFeature();
                    }
                }
                mapControl.ActiveView.Refresh();   //���������Ǿ����(�����Ա�ѡ���У����ù������ϵ�ѡ�񹤾�ѡ��ʱ)
                //mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

            }
        }

        private void dataGridView_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //˫���б�ͷ����λ����Ҫ��
            int i, j;
            IMapControl4 mapControl = m_mapControl;
            IMap pMap = mapControl.Map;
            pMap.ClearSelection();

            double dblMinValue = m_mapControl.FullExtent.Width / 100;

            //"FID"�ֶ���ArcGIS�е��������(�����ϣ�intpFIDIndex == 0)
            ILayer pLayer = m_pLayer;
            ITable pTable = pLayer as ITable;
            int intpFIDIndex = pTable.FindField("FID");

            DataGridViewSelectedRowCollection pDataGridViewSelectedRowCollection = dataGridView.SelectedRows;  //��ȡ�û������Ա���ѡ�е���
            IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
            IFeatureCursor pFeatureCursor = pFeatureLayer.FeatureClass.Search(null, true);
            int intFeatureCount = pFeatureLayer.FeatureClass.FeatureCount(null);  //��ȡҪ�ظ������޶�ѭ������
            IFeature pFeature = pFeatureCursor.NextFeature();
            for (i = 0; i < pDataGridViewSelectedRowCollection.Count; i++)
            {
                DataGridViewRow pDataGridViewRow = pDataGridViewSelectedRowCollection[i];
                int intFID = Convert.ToInt32(pDataGridViewRow.Cells["FID"].Value);

                for (j = 0; j < intFeatureCount; j++)
                {
                    int intpFID = Convert.ToInt32(pFeature.get_Value(intpFIDIndex));
                    if (intFID == intpFID)
                    {
                        if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
                        {
                            //���ڵ�û��һ��������Σ��ʶ���һ���������(ע�⣺���ڵ��Extent��˵Max=Min)
                            IEnvelope pEnvelope = new EnvelopeClass();
                            pEnvelope.XMax = pFeature.Extent.XMax + dblMinValue;
                            pEnvelope.XMin = pFeature.Extent.XMax - dblMinValue;
                            pEnvelope.YMax = pFeature.Extent.YMax + dblMinValue;
                            pEnvelope.YMin = pFeature.Extent.YMax - dblMinValue;
                            mapControl.Extent = pEnvelope;
                        }
                        else
                        {
                            mapControl.Extent = pFeature.Extent;
                        }
                        break;
                    }
                    pFeature = pFeatureCursor.NextFeature();
                }
            }
            mapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

        }


    }
}