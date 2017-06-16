using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.GeoAnalyst;
using ESRI.ArcGIS.Display;

using ContinuousGeneralizer;
using MorphingClass.CUtility;
using MorphingClass.CMorphingMethods;
using MorphingClass.CGeometry;

namespace ContinuousGeneralizer.FrmAid
{
    public partial class FrmSettleRailwayNetwork : Form
    {
        private CFrmOperation _FrmOperation;
        
        CDataRecords _DataRecords;


        public FrmSettleRailwayNetwork()
        {
            InitializeComponent();
        }

        public FrmSettleRailwayNetwork(CDataRecords pDataRecords)
        {
            InitializeComponent();
            _DataRecords = pDataRecords;
            //m_mapControl = m_MapControl;
            //_tsslTime = tsslTime;
        }

        private void FrmSettleRailwayNetwork_Load(object sender, EventArgs e)
        {
            CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            
            
            
            ParameterInitialize.cboLayer = this.cboLayer;

            //����Load��������ʼ������
            _FrmOperation = new CFrmOperation(ref ParameterInitialize);
            throw new ArgumentException("improve loading layesr!");
        }

        public void btnRun_Click(object sender, EventArgs e)
        {
            string strSelectedLayer = this.cboLayer.Text;
            IFeatureLayer pFeatureLayer = null;

            CParameterInitialize ParameterInitialize = _DataRecords.ParameterInitialize;
            try
            {
                for (int i = 0; i < ParameterInitialize.m_mapFeature.LayerCount; i++)
                {
                    if (strSelectedLayer == ParameterInitialize.m_mapFeature.get_Layer(i).Name)
                    {
                        pFeatureLayer = (IFeatureLayer)ParameterInitialize.m_mapFeature.get_Layer(i);
                    }
                }

            }
            catch (Exception)
            {
                MessageBox.Show("��ѡ��Ҫ��ͼ�㣡");
                return;
            }

            //��������Ի���
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.ShowDialog();
            string strPath = SFD.FileName;
            ParameterInitialize.pWorkspace = CHelpFunc.OpenWorkspace(strPath);


            long lngStartTime = System.Environment.TickCount; //��¼��ʼʱ��
            double dblError = Convert.ToDouble(ParameterInitialize.txtError.Text);
            //��ȡ������
            List<CPolyline> CPolylineLt = CHelpFunc.GetCPlLtByFeatureLayer(pFeatureLayer);
            List<CPolyline> crtpllt = new List<CPolyline>();
            for (int i = 0; i < CPolylineLt.Count; i++)
            {
                List<CPoint> cptlt = CPolylineLt[i].CptLt;
                CPolyline crtpl = GetBezierDetectedCpl(cptlt, dblError);
                crtpllt.Add(crtpl);
            }

            CHelpFunc.SaveCPlLt(crtpllt, "BezierDetectedPl", ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);


            long lngEndTime = System.Environment.TickCount;//��¼����ʱ��
            _DataRecords.ParameterInitialize.tsslTime.Text = "Running Time: " + Convert.ToString(lngEndTime - lngStartTime) + "ms";  //��ʾ����ʱ
        }

        private CPolyline GetBezierDetectedCpl(List<CPoint> cptlt, double dblErrorDis)
        {
            double dblMinDis = FindMinLength(cptlt);
            double dblSmallDis = dblMinDis / 2;


            List<CPoint> crtptlt = SettleRailwayNetwork(cptlt, dblSmallDis, dblErrorDis);
            CPolyline crtpl = new CPolyline(0, crtptlt);
            return crtpl;







        }


        private double FindMinLength(List<CPoint> cptlt)
        {
            double dblMinDis = double.MaxValue;
            for (int i = 0; i < cptlt.Count - 1; i++)
            {
                double dblDis = CGeoFunc.CalDis(cptlt[i], cptlt[i + 1]);
                if (dblDis < dblMinDis)
                {
                    dblMinDis = dblDis;
                }
            }
            return dblMinDis;

        }


        private List<CPoint> SettleRailwayNetwork(List<CPoint> cptlt, double dblSmallDis, double dblErrorDis)
        {
            List<CPoint> crtptlt = new List<CPoint>();  //crtptlt:characteristicptlt
            crtptlt.Add(cptlt[0]);



            //List<CPoint> beziercptlt = new List<CPoint>();
            IPointCollection4 pOriginalCol = new PolylineClass();
            object Missing = Type.Missing;
            bool blnNew = true;
            for (int i = 1; i < cptlt.Count - 1; i++)
            {
                if (blnNew == true)
                {
                    pOriginalCol = new PolylineClass();
                    blnNew = false;
                    pOriginalCol.AddPoint((IPoint)cptlt[i - 1], ref Missing, ref Missing);
                    pOriginalCol.AddPoint((IPoint)cptlt[i], ref Missing, ref Missing);
                    pOriginalCol.AddPoint((IPoint)cptlt[i + 1], ref Missing, ref Missing);

                    //beziercptlt.Add(cptlt[i - 1]);
                    //beziercptlt.Add(cptlt[i ]);
                    //beziercptlt.Add(cptlt[i + 1]);
                    i = i + 1;
                }
                else
                {
                    pOriginalCol.AddPoint((IPoint)cptlt[i], ref Missing, ref Missing);
                    //beziercptlt.Add(cptlt[i]);  
                }

                IPolyline5 pOriginalpl = pOriginalCol as IPolyline5;
                double dblBuBeLength = pOriginalpl.Length / 3;  //dblBuBeLength: dblBuildBezierLength
                int intPointCount = pOriginalCol.PointCount;

                IPointCollection4 pfrCol = new PolylineClass();
                pfrCol.AddPoint(pOriginalCol.get_Point(0), ref Missing, ref Missing);
                pfrCol.AddPoint(pOriginalCol.get_Point(1), ref Missing, ref Missing);
                IPolyline5 pfrpl = pfrCol as IPolyline5;
                IPoint froutpt = new PointClass();
                pfrpl.QueryPoint(esriSegmentExtension.esriExtendAtTo, dblBuBeLength, false, froutpt);

                IPointCollection4 ptoCol = new PolylineClass();
                ptoCol.AddPoint(pOriginalCol.get_Point(intPointCount - 1), ref Missing, ref Missing);
                ptoCol.AddPoint(pOriginalCol.get_Point(intPointCount - 2), ref Missing, ref Missing);
                IPolyline5 ptopl = ptoCol as IPolyline5;
                IPoint tooutpt = new PointClass();
                ptopl.QueryPoint(esriSegmentExtension.esriExtendAtTo, dblBuBeLength, false, tooutpt);


                //����BezierCurve
                IBezierCurve pBezierCurve = new BezierCurveClass();
                pBezierCurve.PutCoord(0, pOriginalCol.get_Point(0));
                pBezierCurve.PutCoord(1, froutpt);
                pBezierCurve.PutCoord(2, tooutpt);
                pBezierCurve.PutCoord(3, pOriginalCol.get_Point(intPointCount - 1));


                double dblQueryNum = pOriginalpl.Length / dblSmallDis;
                double dblMaxDis = 0;
                for (int j = 0; j < dblQueryNum - 1; j++)
                {
                    IPoint originalpt = new PointClass();
                    pOriginalpl.QueryPoint(esriSegmentExtension.esriNoExtension, j / dblQueryNum, true, originalpt);

                    IPoint bezierpt = new PointClass();
                    pBezierCurve.QueryPoint(esriSegmentExtension.esriNoExtension, j / dblQueryNum, true, bezierpt);

                    double dblDis = CGeoFunc.CalDis(originalpt, bezierpt);
                    if (dblDis > dblMaxDis)
                    {
                        dblMaxDis = dblDis;
                    }
                }

                if (dblMaxDis > dblErrorDis)
                {
                    blnNew = true;
                    crtptlt.Add(cptlt[i]);
                }
            }



            crtptlt.Add(cptlt[cptlt.Count - 1]);
            return crtptlt;

        }




    }
}