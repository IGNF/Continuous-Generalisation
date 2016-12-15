using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Output;

using MorphingClass.CGeometry;
using MorphingClass.CUtility;
using MorphingClass.CCorrepondObjects;
using MorphingClass.CEntity;

namespace MorphingClass.CEvaluationMethods
{


    public class CLengthDiff
    {
        private CDataRecords _pDataRecords;
        
        private List<CParameterResult> _pParameterResultToExcelLt = new List<CParameterResult>();
        //private double _dblSumLength = 0;
        //private double _intIntersectNum = 0;

        public CLengthDiff()
        {

        }


        public CLengthDiff(CDataRecords pDataRecords)
        {
            _pDataRecords = pDataRecords;
            //_pDataRecords.ParameterResultToExcel = _pParameterResultToExcel;
            //_pDataRecords.ParameterResultToExcelLt = _pParameterResultToExcelLt;
            //_pParameterResultToExcel.strEvaluationMethod = "LengthDiff";
        }


        /// <summary>����LengthDiffָ��ֵ�����������</summary>
        /// <returns>LengthDiffָ��ֵ</returns>
        /// <remarks></remarks>
        public double CalLengthDiff()
        {
            MessageBox.Show("wrong call in clengthdiff!");
            //CParameterResult ParameterResult = _pDataRecords.ParameterResult;
            //List<CPoint> resultptlt = ParameterResult.CResultPtLt;
            //CParameterInitialize ParameterInitialize = _pDataRecords.ParameterInitialize;
            //StatusStrip ststMain = ParameterInitialize.ststMain;
            //ToolStripStatusLabel tsslTime = ParameterInitialize.tsslTime;
            //ToolStripStatusLabel tsslMessage = ParameterInitialize.tsslMessage;
            //ToolStripProgressBar tspbMain = ParameterInitialize.tspbMain;
            //tsslMessage.Text = "���ڼ���LengthDiff...";
            //ststMain.Refresh();
            //long lngStartTime = System.Environment.TickCount;

            ////���ɼ���LengthDiffָ��ֵ���߶�
            //List<CPoint> CFrPtLtToExcel = new List<CPoint>();
            //List<CPoint> CToPtLtToExcel = new List<CPoint>();
            //List<CPoint> LengthDiffptlt = new List<CPoint>();
            //int intptnum = 0;
            //for (int i = 0; i < resultptlt.Count; i++)
            //{
            //    for (int j = 0; j < resultptlt[i].CorrespondingPtLt.Count; j++)
            //    {
            //        double dblX = resultptlt[i].CorrespondingPtLt[j].X - resultptlt[i].X;
            //        double dblY = resultptlt[i].CorrespondingPtLt[j].Y - resultptlt[i].Y;
            //        CPoint cpt = new CPoint(intptnum, dblX, dblY);
            //        LengthDiffptlt.Add(cpt);

            //        CFrPtLtToExcel.Add(resultptlt[i]);
            //        CToPtLtToExcel.Add(resultptlt[i].CorrespondingPtLt[j]);
            //        intptnum = intptnum + 1;
            //    }
            //    tspbMain.Value = (i + 1) * 50 / (resultptlt.Count);
            //}

            ////��ӵ�һ��Ԫ��
            //List<double> dblLengthDiffLt = new List<double>();
            //List<double> dblSumLengthDiffLt = new List<double>();
            //dblLengthDiffLt.Add(0);
            //dblSumLengthDiffLt.Add(0);
            //double dblSumLenth = 0;
            //for (int i = 1; i < LengthDiffptlt.Count; i++)
            //{
            //    double dblLength = CGeometricMethods.CalDis(LengthDiffptlt[i - 1], LengthDiffptlt[i]);
            //    dblLengthDiffLt.Add(dblLength);
            //    dblSumLenth = dblSumLenth + dblLength;
            //    dblSumLengthDiffLt.Add(dblSumLenth);

            //    tspbMain.Value = (i + 1) * 50 / (LengthDiffptlt.Count) + 50;
            //}

            //CParameterResult pParameterResultToExcel = new CParameterResult();
            //pParameterResultToExcel.strEvaluationMethod = "LengthDiff";
            //pParameterResultToExcel.FromPtLt = CFrPtLtToExcel;
            //pParameterResultToExcel.ToPtLt = CToPtLtToExcel;
            //pParameterResultToExcel.TranlationPtLt = LengthDiffptlt;
            //pParameterResultToExcel.dblLengthDiffLt = dblLengthDiffLt;
            //pParameterResultToExcel.dblSumLengthDiffLt = dblSumLengthDiffLt;
            //_pDataRecords.ParameterResultToExcel = pParameterResultToExcel;

            //long lngEndTime = System.Environment.TickCount;
            //long lngTime = lngEndTime - lngStartTime;
            //tsslTime.Text = "LengthDiffRunning Time: " + Convert.ToString(lngTime) + "ms";  //��ʾ����ʱ��
            //tsslMessage.Text = "LengthDiff������ɣ�";

            //return dblSumLenth;
            return 0;
        }


        /// <summary>����LengthDiffָ��ֵ</summary>
        /// <param name="resultptlt">��Ӧ�������</param>
        /// <returns>LengthDiffָ��ֵ</returns>
        /// <remarks>���ͷ��ڴ�</remarks>
        public double CalLengthDiff(List<CPoint> resultptlt)
        {
            MessageBox.Show("wrong call in clengthdiff!");
            ////���ɼ���Integralָ��ֵ���߶�
            //double dblSumLengthDiff = 0;
            //for (int i = 0; i < resultptlt.Count - 1; i++)
            //{

            //    IPointCollection4 pCol0 = new PolygonClass();
            //    CPoint cpt0 = new CPoint();
            //    CPoint cpt2 = new CPoint();
            //    if (resultptlt[i].CorrespondingPtLt.Count > 1)
            //    {
            //        //fromcpl�е�һ�����Ӧtocpl�ж��������Σ�ֱ�Ӽ�������������
            //        for (int j = 0; j < resultptlt[i].CorrespondingPtLt.Count - 1; j++)
            //        {
            //            double dblLengthDiff = CGeometricMethods.CalDis(resultptlt[i].CorrespondingPtLt[j], resultptlt[i].CorrespondingPtLt[j + 1]);
            //            dblSumLengthDiff += dblLengthDiff;
            //        }
            //        //��resultptlt[i]�����Ӧ�������е����һ����Ӧ����Ϊ��һ���ı��ε���������
            //        cpt0 = resultptlt[i];
            //        cpt2 = resultptlt[i].CorrespondingPtLt[resultptlt[i].CorrespondingPtLt.Count - 1];
            //    }
            //    else if (resultptlt[i].CorrespondingPtLt.Count == 1)
            //    {
            //        //��resultptlt[i]�����Ӧ�������еĵ�һ����Ӧ�㣨Ҳ����һ������Ϊ��һ���ı��ε���������
            //        //fromcpl�еĶ�����Ӧtocpl��һ���������Ҳ����������
            //        cpt0 = resultptlt[i];
            //        cpt2 = resultptlt[i].CorrespondingPtLt[0];
            //    }

            //    double dblLength1 = CGeometricMethods.CalDis(cpt0, resultptlt[i + 1]);
            //    double dblLength2 = CGeometricMethods.CalDis(cpt2, resultptlt[i + 1].CorrespondingPtLt[0]);
            //    double dblLengthDiff2 = Math.Abs(dblLength1 - dblLength2);

            //    dblSumLengthDiff += dblLengthDiff2;
            //}

            //return dblSumLengthDiff;
            return 0;
        }


        /// <summary>����RatioLengthDiffָ��ֵ</summary>
        /// <param name="resultptlt">��Ӧ�������</param>
        /// <returns>RatioLengthDiffָ��ֵ</returns>
        /// <remarks>further weighted by the ratio</remarks>
        public double CalRatioLengthDiff(List<CPoint> resultptlt, CPolyline frcpl, CPolyline tocpl)
        {
            double dblSumRatioLengthDiff = 0;
            CPoint frlastcpt = resultptlt[0];
            CPoint tolastcpt = resultptlt[0].CorrespondingPtLt[0];
            for (int i = 0; i < resultptlt.Count; i++)
            {
                double dblfrlength = CGeometricMethods.CalDis(resultptlt[i], frlastcpt);//����Ȩ��ֵ����
                for (int j = 0; j < resultptlt[i].CorrespondingPtLt.Count; j++)
                {
                    double dbltolength = CGeometricMethods.CalDis(resultptlt[i].CorrespondingPtLt[j], tolastcpt);//����Ȩ��ֵ����
                    dblSumRatioLengthDiff = Math.Abs(dblfrlength - dbltolength) * (dblfrlength + dbltolength);

                    tolastcpt = resultptlt[i].CorrespondingPtLt[j];  //����tolastcpt
                }
                frlastcpt = resultptlt[i];
            }

            dblSumRatioLengthDiff = dblSumRatioLengthDiff / (frcpl.pPolyline.Length + tocpl.pPolyline.Length);
            return dblSumRatioLengthDiff;
        }
 

      

    }
}
