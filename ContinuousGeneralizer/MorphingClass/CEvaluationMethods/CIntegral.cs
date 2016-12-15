using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
//using System.Windows.Forms .

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Output;

using MorphingClass.CGeometry;
using MorphingClass.CUtility;


namespace MorphingClass.CEvaluationMethods
{
    public class CIntegral
    {
        private CDataRecords _pDataRecords;
        
        private CParameterResult _pParameterResultToExcel = new CParameterResult();
        



        //private double _dbInterLSmallDis;
        //private double _dblVerySmall;
        //private double _intIntersectNum = 0;
        private object _Missing = Type.Missing;



        public CIntegral()
        {

        }



        public CIntegral(CDataRecords pDataRecords)
        {
            _pDataRecords = pDataRecords;
            _pDataRecords.ParameterResultToExcel = _pParameterResultToExcel;
            _pParameterResultToExcel.strEvaluationMethod = "Integral";

            //_pParameterResultToExcel.dblIntegralLt = new List<double>();
            //_pParameterResultToExcel.dbInterLSumIntegralLt = new List<double>();
            //_pParameterResultToExcel.FromPtLt = new List<CPoint>();
            //_pParameterResultToExcel.ToPtLt = new List<CPoint>();


        }


        public double CalIntegral()
        {
            MessageBox.Show("need to be improved!");


            //CParameterResult ParameterResult = _pDataRecords.ParameterResult;
            //CParameterInitialize ParameterInitialize=_pDataRecords.ParameterInitialize;
            //StatusStrip ststMain = ParameterInitialize.ststMain;
            //TooInterLStripStatusLabel tsslTime = ParameterInitialize.tsslTime;
            //TooInterLStripStatusLabel tsslMessage = ParameterInitialize.tsslMessage;
            //TooInterLStripProgressBar tspbMain = ParameterInitialize.tspbMain;
            //tsslMessage.Text = "���ڼ���Integral...";
            //tspbMain.Value = 0;
            //ststMain.Refresh();
            //long lngStartTime = System.Environment.TickCount;

            //List<CPoint> frptlt = ParameterResult.FromCpl .CptLt ;
            //List<CPoint> resultptlt = ParameterResult.CResultPtLt;
            //double dblMinDis = CGeometricMethods.CalDis(frptlt[0], frptlt[1]);
            //for (int i = 1; i < frptlt.Count - 1; i++)
            //{
            //    double dblDis = CGeometricMethods.CalDis(frptlt[i], frptlt[i + 1]);
            //    if (dblDis < dblMinDis) dblMinDis = dblDis;
            //}
            //double dbInterLSmallDis = dblMinDis / 10;
            //_dbInterLSmallDis = dbInterLSmallDis;


            //double dbInterLSumArea = 0;
            
            //List<CPoint> CFrPtLtToExcel = new List<CPoint>();
            //List<CPoint> CToPtLtToExcel = new List<CPoint>();
            //List<double> dblIntegralLt = new List<double>();
            //List<double> dbInterLSumIntegralLt = new List<double>();

            ////��ӵ�һ��Ԫ��
            //CFrPtLtToExcel.Add(resultptlt[0]);
            //CToPtLtToExcel.Add(resultptlt[0].CorrespondingPtLt [0]);
            //dblIntegralLt.Add(0);
            //dbInterLSumIntegralLt.Add(0);
            
            //for (int i = 0; i < resultptlt.Count - 1; i++)
            //{
                
            //    IPointCollection4 pCol0 = new PolygonClass();
            //    IPoint ipt0 = new PointClass();
            //    IPoint ipt2 = new PointClass();
            //    if (resultptlt[i].CorrespondingPtLt.Count > 1)
            //    {
            //        //fromcpl�е�һ�����Ӧtocpl�ж��������Σ�ֱ�Ӽ�������������
            //        int j = 0;
            //        for (j = 0; j < resultptlt[i].CorrespondingPtLt.Count - 1; j++)
            //        {

            //            IPointCollection4 pCol1 = new PolygonClass();
            //            //˳ʱ����Ӷ���
            //            pCol1.AddPoint(resultptlt[i]);
            //            pCol1.AddPoint(resultptlt[i].CorrespondingPtLt[j + 1]);
            //            pCol1.AddPoint(resultptlt[i].CorrespondingPtLt[j]);
            //            IArea pArea1 = (IArea)pCol1;
            //            //if (pArea1.Area < 0)
            //            //    throw new ArithmeticException("�����������������Σ�ʱ���ָ�ֵ��");
            //            dbInterLSumArea = dbInterLSumArea + Math.Abs(pArea1.Area);
            //            CFrPtLtToExcel.Add(resultptlt[i]);
            //            CToPtLtToExcel.Add(resultptlt[i].CorrespondingPtLt[j + 1]);
            //            dblIntegralLt.Add(pArea1.Area);
            //            dbInterLSumIntegralLt.Add(dbInterLSumArea);

            //        }
            //        //��resultptlt[i]�����Ӧ�������е����һ����Ӧ����Ϊ��һ���ı��ε���������
            //        ipt0 = resultptlt[i];
            //        ipt2 = resultptlt[i].CorrespondingPtLt[resultptlt[i].CorrespondingPtLt.Count - 1];
            //    }
            //    eInterLSe if (resultptlt[i].CorrespondingPtLt.Count == 1)
            //    {
            //        //��resultptlt[i]�����Ӧ�������еĵ�һ����Ӧ�㣨Ҳ����һ������Ϊ��һ���ı��ε���������
            //        //fromcpl�еĶ�����Ӧtocpl��һ���������Ҳ����������
            //        ipt0 = resultptlt[i];
            //        ipt2 = resultptlt[i].CorrespondingPtLt[0];
            //    }
            //    //��resultptlt[i + 1]�����һ����Ӧ����Ϊ��һ���ı��ε�������������
            //    double dblQuadrangleArea = CalIntegralAreaofAnyQuadrangle(ipt0, (IPoint)resultptlt[i + 1],
            //                                                              ipt2, (IPoint)resultptlt[i + 1].CorrespondingPtLt[0], dbInterLSmallDis);
            //    dbInterLSumArea = dbInterLSumArea + dblQuadrangleArea;

            //    CFrPtLtToExcel.Add(resultptlt[i + 1]);
            //    CToPtLtToExcel.Add(resultptlt[i + 1].CorrespondingPtLt[0]);
            //    dblIntegralLt.Add(dblQuadrangleArea);
            //    dbInterLSumIntegralLt.Add(dbInterLSumArea);

            //    tspbMain.Value = (i + 1) * 100 / (resultptlt.Count - 1);
            //}
            //_pParameterResultToExcel.FromPtLt = CFrPtLtToExcel;
            //_pParameterResultToExcel.ToPtLt = CToPtLtToExcel;
            //_pParameterResultToExcel.dblIntegralLt=dblIntegralLt;
            //_pParameterResultToExcel.dbInterLSumIntegralLt = dbInterLSumIntegralLt;

            //long lngEndTime = System.Environment.TickCount;
            //long lngTime = lngEndTime - lngStartTime;
            //tsslTime.Text = "IntegralRunning Time: " + Convert.ToString(lngTime) + "ms";  //��ʾ����ʱ��
            //tsslMessage.Text = "Integral������ɣ�";
            //return dbInterLSumArea;
            return 0;
        }

        /// <summary>����Integralָ��ֵ</summary>
        /// <param name="resultptlt">��Ӧ�������</param>
        /// <returns>Integralָ��ֵ</returns>
        /// <remarks>���ͷ��ڴ�</remarks>
        public double CalIntegral(List<CPoint> resultptlt)
        {
            ////���ɼ���Integralָ��ֵ���߶�
            //double dblIntegral = 0;
            //IPoint ifrpt = resultptlt[0];
            //IPoint itopt = resultptlt[0].CorrespondingPtLt[0];

            //for (int i = 0; i < resultptlt.Count; i++)
            //{
            //    //һ��һ��һ�Զࡢ���һ�������ڴ�
            //    for (int j = 0; j < resultptlt[i].CorrespondingPtLt.Count; j++)
            //    {
            //        dblIntegral += CaInterLSubIntegral(ifrpt, resultptlt[i], itopt, resultptlt[i].CorrespondingPtLt[j], _dbInterLSmallDis, _dblVerySmall);                    
            //        itopt = resultptlt[i].CorrespondingPtLt[j];
            //    }
            //    ifrpt = resultptlt[i];
            //}

            //return dblIntegral;
            return 0;
        }


        private double CaInterLSubIntegral(CPoint frfrcpt, CPoint frtocpt, CPoint tofrcpt, CPoint totocpt, CPoint StandardVectorCpt, double dbInterLSmallDis, double dblVerySamll)
        {
            CPoint newfrfrcpt = new CPoint(0, frfrcpt.X + StandardVectorCpt.X, frfrcpt.Y + StandardVectorCpt.Y);
            CPoint newfrtocpt = new CPoint(0, frtocpt.X + StandardVectorCpt.X, frtocpt.Y + StandardVectorCpt.Y);

            CEdge frcedge = new CEdge(newfrfrcpt, newfrtocpt);
            CEdge tocedge = new CEdge(tofrcpt, totocpt);
            frcedge.SetLength();
            tocedge.SetLength();


            if (CCompareMethods.CompareCEdgeCoordinates(frcedge, tocedge)==0 ||
               (frcedge.dblLength == 0 && tocedge.dblLength == 0))   //Ϊ��Ӧ���տ�ʼʱ���غϵĶ�Ӧ��
            {
                return 0;
            }


            double dblLength = frcedge.dblLength;
            if (frcedge.dblLength < tocedge.dblLength)
            {
                dblLength = tocedge.dblLength;
            }

            int intSegmentNum = Convert.ToInt32(dblLength / dbInterLSmallDis) + 1;
            double frlength = frcedge.dblLength / intSegmentNum;
            double tolength = tocedge.dblLength / intSegmentNum;

            //�����������Ϊ���е��ϵ׺��µ׶���ͬ����˿����Ƚ��������εĸ���ӣ�����ѭ������Ըߡ�����2��
            double dbledgelength = 0;
            double dblRatio = 1 / Convert.ToDouble(intSegmentNum);
            double dblCurrentRatio = 0;
            for (int k = 0; k < intSegmentNum; k++)
            {
                double dblfrx = (1 - dblCurrentRatio) * newfrfrcpt.X + dblCurrentRatio * newfrtocpt.X;
                double dblfry = (1 - dblCurrentRatio) * newfrfrcpt.Y + dblCurrentRatio * newfrtocpt.Y;
                double dbltox = (1 - dblCurrentRatio) * tofrcpt.X + dblCurrentRatio * totocpt.X;
                double dbltoy = (1 - dblCurrentRatio) * tofrcpt.Y + dblCurrentRatio * totocpt.Y;

                dbledgelength += CGeometricMethods.CalDis(dblfrx, dblfry, dbltox, dbltoy);
                dblCurrentRatio += dblRatio;
            }

            double dbInterLSubIntegral = dbledgelength * (frlength + tolength) / 2;
            return dbInterLSubIntegral;
        }

        /// <summary>����RatioIntegralָ��ֵ</summary>
        /// <param name="resultptlt">��Ӧ�������</param>
        /// <returns>RatioIntegralָ��ֵ</returns>
        /// <remarks></remarks>
        public double CalRatioIntegral(List<CPoint> resultptlt,CPolyline frcpl,CPolyline tocpl)
        {
            //CGeometricMethods.CalDistanceParameters(frcpl);
            //

            ////���ɼ���Integralָ��ֵ���߶�
            //double dbInterLSumRatioIntegral = 0;
            //CPoint frlastcpt = resultptlt[0];
            //CPoint tolastcpt = resultptlt[0].CorrespondingPtLt[0];

            ////StandardVectorCpt
            //double dblX = tocpl.CptLt[0].X - frcpl.CptLt[0].X;
            //double dblY = tocpl.CptLt[0].Y - frcpl.CptLt[0].Y;
            //CPoint StandardVectorCpt = new CPoint(0, dblX, dblY);

            //for (int i = 0; i < resultptlt.Count; i++)
            //{
            //    double dblfrlength = CGeometricMethods.CalDis(resultptlt[i], frlastcpt);//����Ȩ��ֵ����
            //    //һ��һ��һ�Զࡢ���һ�������ڴ�
            //    for (int j = 0; j < resultptlt[i].CorrespondingPtLt.Count; j++)
            //    {
            //        double dbltolength = CGeometricMethods.CalDis(resultptlt[i].CorrespondingPtLt[j], tolastcpt);//����Ȩ��ֵ����
            //        double dbInterLSubintegral = CaInterLSubIntegral(frlastcpt, resultptlt[i], tolastcpt, resultptlt[i].CorrespondingPtLt[j],StandardVectorCpt, dbInterLSmallDis, dblVerySmall);
            //        dbInterLSumRatioIntegral = dbInterLSumRatioIntegral + dbInterLSubintegral * (dblfrlength + dbltolength);

            //        tolastcpt = resultptlt[i].CorrespondingPtLt[j];
            //    }
            //    frlastcpt = resultptlt[i];
            //}
            //dbInterLSumRatioIntegral = dbInterLSumRatioIntegral / (frcpl.pPolyline.Length + tocpl.pPolyline.Length);

            //return dbInterLSumRatioIntegral;
            return 0;
        }


        ///// <summary>Property:</summary>
        //public double dbInterLSmallDis
        //{
        //    get { return _dbInterLSmallDis; }
        //    set { _dbInterLSmallDis = value; }
        //}

        ///// <summary>Property: very small distance</summary>
        //public double dblVerySmall
        //{
        //    get { return _dblVerySmall; }
        //    set { _dblVerySmall = value; }
        //}

        
    }
}
