using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;


using MorphingClass.CEntity;
using MorphingClass.CEvaluationMethods ;
using MorphingClass.CGeneralizationMethods;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;
using MorphingClass.CMorphingAlgorithms;
using MorphingClass.CCorrepondObjects;

namespace MorphingClass.CMorphingMethods
{
    /// <summary>COptCorBezier</summary>
    /// <remarks>
    /// </remarks>
    public class COptCorBezier
    {
        private CPolyline _FromCpl;
        private CPolyline _ToCpl;

        
        
        
        private CLinearInterpolationA _LinearInterpolationA = new CLinearInterpolationA();
        private CParameterResult _ParameterResult;

        CTranslation _pTranslation = new CTranslation();
        CIntegral _pIntegral = new CIntegral();
        CLengthDiff _pLengthDiff = new CLengthDiff();

        private double _dblSmallDis;


        public CPoint _StandardVectorCpt;


        private CParameterInitialize _ParameterInitialize;

        public COptCorBezier()
        {

        }


        public COptCorBezier(CParameterInitialize ParameterInitialize)
        {

            //��ȡ��ǰѡ��ĵ�Ҫ��ͼ��
            //�������Ҫ��ͼ��
            IFeatureLayer pBSFLayer = (IFeatureLayer)ParameterInitialize.m_mapFeature.get_Layer(ParameterInitialize.cboLargerScaleLayer.SelectedIndex);
                                                                       
            //С������Ҫ��ͼ��
            IFeatureLayer pSSFLayer =(IFeatureLayer)ParameterInitialize.m_mapFeature.get_Layer(ParameterInitialize.cboSmallerScaleLayer.SelectedIndex);
                                                           
            ParameterInitialize.pBSFLayer = pBSFLayer;
            ParameterInitialize.pSSFLayer = pSSFLayer;            
            ParameterInitialize.intMaxBackK = Convert.ToInt32(ParameterInitialize.txtMaxBackK.Text);
            _ParameterInitialize = ParameterInitialize;

            //��ȡ������
            List <CPolyline> _LSCPlLt = CHelpFunc.GetCPlLtByFeatureLayer(pBSFLayer);
            List<CPolyline> _SSCPlLt = CHelpFunc.GetCPlLtByFeatureLayer(pSSFLayer);

            CGeoFunc.SetCPlScaleEdgeLengthPtBelong(ref _LSCPlLt, CEnumScale.Larger);
            CGeoFunc.SetCPlScaleEdgeLengthPtBelong(ref _SSCPlLt, CEnumScale.Smaller);

            _FromCpl = _LSCPlLt[0];
            _ToCpl = _SSCPlLt[0];
        }



        public void OptCorBezierMorphing()
        {

            //CPolyline frcpl = _FromCpl;
            //CPolyline tocpl = _ToCpl;

            //int intMaxBackK = _ParameterInitialize.intMaxBackK;


            //double dblSmallestDis=CGeoFunc.CalSmallestDis(frcpl);
            //double dblSmallDis = dblSmallestDis / 4;
            ////double dblError = (CGeoFunc.CalMidLength(frcpl) +CGeoFunc.CalMidLength(tocpl)) / 2;
            //double dblFrError = dblSmallestDis;
            //double dblToError = CGeoFunc.CalSmallestDis(tocpl);

            //CBezierDetectPoint pBezierDetectPoint = new CBezierDetectPoint();
            //List<CPoint> FrBezierCptLt = pBezierDetectPoint.BezierDetectPoint(frcpl.CptLt, dblSmallDis, dblFrError);
            //List<CPoint> ToBezierCptLt = pBezierDetectPoint.BezierDetectPoint(tocpl.CptLt, dblSmallDis, dblToError);

            //List<CPolyline> CFrBezierEdgeLt = CGeoFunc.CreateCplLt(FrBezierCptLt);
            //List<CPolyline> CToBezierEdgeLt = CGeoFunc.CreateCplLt(ToBezierCptLt);

            //long lngStartTime = System.Environment.TickCount;  //��ʼʱ��
            //List<CPolyline> frlastcpllt = new List<CPolyline>(CFrBezierEdgeLt.Count);
            //List<CPolyline> tolastcpllt = new List<CPolyline>(CToBezierEdgeLt.Count);
            //CTable[,] T = CreatT(frcpl, tocpl, CFrBezierEdgeLt, CToBezierEdgeLt, ref frlastcpllt, ref tolastcpllt, intMaxBackK);  //����T����
            //C5.LinkedList<CCorrespondSegment> pCorrespondSegmentLk = FindCorrespondSegmentLk(T, frcpl, tocpl, CFrBezierEdgeLt, CToBezierEdgeLt, frlastcpllt, tolastcpllt);            

            ////��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��
            //CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper();
            //List<CPoint> pResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(pCorrespondSegmentLk, "Linear");

            ////���㲢��ʾ����ʱ��
            //long lngEndTime = System.Environment.TickCount;
            //long lngTime = lngEndTime - lngStartTime;
            //_ParameterInitialize.tsslTime.Text = "Running Time: " + Convert.ToString(lngTime) + "ms";  //��ʾ����ʱ��

            ////�����Ӧ��
            //CHelpFunc.SaveCtrlLine(pCorrespondSegmentLk, "OptCorBezierControlLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);
            //CHelpFunc.SaveCorrespondLine(pResultPtLt, "OptCorBezierCorrLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);

            ////��ȡ�����ȫ����¼��_ParameterResult��
            //CParameterResult ParameterResult = new CParameterResult();
            //ParameterResult.FromCpl = frcpl;
            //ParameterResult.ToCpl = tocpl;

            //ParameterResult.CResultPtLt = pResultPtLt;
            //ParameterResult.lngTime = lngTime;
            //_ParameterResult = ParameterResult;

        }

        /// <summary>��������Morphing����</summary>
        /// <remarks>����ָ���Ļ��ݲ���K�����ϼ�������Ľ����ֱ��Translationֵ�ȶ�</remarks>
        public void OptCorBezierMultiResultsMorphing()
        {

            //CPolyline frcpl = _FromCpl;
            //CPolyline tocpl = _ToCpl;
            // 

            //int intMaxBackK = _ParameterInitialize.intMaxBackK;

            //double dblSmallestDis = CGeoFunc.CalSmallestDis(frcpl);
            //double dblSmallDis = dblSmallestDis / 4;
            ////double dblError = (CGeoFunc.CalMidLength(frcpl) +CGeoFunc.CalMidLength(tocpl)) / 2;
            //double dblFrError = dblSmallestDis;
            //double dblToError = CGeoFunc.CalSmallestDis(tocpl);

            //CBezierDetectPoint pBezierDetectPoint = new CBezierDetectPoint();
            //List<CPoint> FrBezierCptLt = pBezierDetectPoint.BezierDetectPoint(frcpl.CptLt, dblSmallDis, dblFrError);
            //List<CPoint> ToBezierCptLt = pBezierDetectPoint.BezierDetectPoint(tocpl.CptLt, dblSmallDis, dblToError);

            //List<CPolyline> CFrBezierEdgeLt = CGeoFunc.CreateCplLt(FrBezierCptLt);
            //List<CPolyline> CToBezierEdgeLt = CGeoFunc.CreateCplLt(ToBezierCptLt);


            //List<CPolyline> frlastcpllt = new List<CPolyline>(CFrBezierEdgeLt.Count);
            //List<CPolyline> tolastcpllt = new List<CPolyline>(CToBezierEdgeLt.Count);

            //int intUpperbound = CFrBezierEdgeLt.Count + 1;
            //int intLowerbound = CToBezierEdgeLt.Count + 1;

            //List<double> dblDistanceLt = new List<double>(10);
            //List<double> dblTimeLt = new List<double>(10);
            //CTable[,] T = new CTable[intUpperbound, intLowerbound];
            //List<CPoint> pResultPtLt = new List<CPoint>();
            //for (int i = 0; i < 10; i++)
            //{
            //    long lngStartTime = System.Environment.TickCount;  //��ʼʱ��

            //    T = CreatT(frcpl, tocpl, CFrBezierEdgeLt, CToBezierEdgeLt, ref frlastcpllt, ref tolastcpllt, intMaxBackK);  //����T����                
            //    dblDistanceLt.Add(T[intUpperbound - 1, intLowerbound - 1].dblEvaluation);
            //    LinkedList<CCorrespondSegment> pCorrespondSegmentLk = FindCorrespondSegmentLk(T, frcpl, tocpl, CFrBezierEdgeLt, CToBezierEdgeLt, frlastcpllt, tolastcpllt);

            //    //��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��
            //    CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper();
            //    pResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(pCorrespondSegmentLk, "Linear");

            //    //���㲢��ʾ����ʱ��
            //    long lngEndTime = System.Environment.TickCount;
            //    long lngTime = lngEndTime - lngStartTime;
            //    dblTimeLt.Add(lngTime);

            //    //�����Ӧ��
            //    CHelpFuncExcel.ExportDataltToExcel(dblTimeLt, intMaxBackK + "Timelt0", _ParameterInitialize.strSavePath);
            //    CHelpFuncExcel.ExportDataltToExcel(dblDistanceLt, intMaxBackK + "Distancelt0", _ParameterInitialize.strSavePath);
            //    CHelpFunc.SaveCtrlLine(pCorrespondSegmentLk, intMaxBackK + "OptCorBezierControlLine", dblVerySmall, _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);
            //    CHelpFunc.SaveCorrespondLine(pResultPtLt, intMaxBackK + "OptCorBezierCorrLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);

            //    intMaxBackK = intMaxBackK + 1;
            //}


            ////��ȡ�����ȫ����¼��_ParameterResult��
            //CParameterResult ParameterResult = new CParameterResult();
            //ParameterResult.FromCpl = frcpl;
            //ParameterResult.ToCpl = tocpl;

            //ParameterResult.CResultPtLt = pResultPtLt;
            //_ParameterResult = ParameterResult;

        }

        /// <summary>����T����</summary>
        /// <param name="frcpl">���������״Ҫ��</param>
        /// <param name="tocpl">С��������״Ҫ��</param> 
        /// <param name="CFrEdgeLt">��������߶Σ�����ֻ����״Ҫ�ص�һ���֣�</param>  
        ///  <param name="CToEdgeLt">С�������߶Σ�����ֻ����״Ҫ�ص�һ���֣�</param> 
        /// <param name="intMaxBackK">����ϵ��</param> 
        /// <returns>��Ӧ�߶�</returns>
        public LinkedList<CCorrespondSegment> DWByOptCorBezier(CPolyline frcpl, CPolyline tocpl, List<CPolyline> CFrEdgeLt, List<CPolyline> CToEdgeLt, int intMaxBackK)
        {
            List<CPolyline> frlastcpllt = new List<CPolyline>();
            List<CPolyline> tolastcpllt = new List<CPolyline>();
            CTable[,] T = CreatT(frcpl, tocpl, CFrEdgeLt, CToEdgeLt, ref frlastcpllt, ref tolastcpllt, intMaxBackK);  //����T����
            LinkedList<CCorrespondSegment> pCorrespondSegmentLk = FindCorrespondSegmentLk(T, frcpl, tocpl, CFrEdgeLt, CToEdgeLt, frlastcpllt, tolastcpllt);
            return pCorrespondSegmentLk;

        }

        /// <summary>����T����</summary>
        /// <param name="frcpl">���������״Ҫ��</param>
        /// <param name="tocpl">С��������״Ҫ��</param> 
        /// <param name="CFrEdgeLt">��������߶Σ�����ֻ����״Ҫ�ص�һ���֣�</param>  
        ///  <param name="CToEdgeLt">С�������߶Σ�����ֻ����״Ҫ�ص�һ���֣�</param> 
        /// <param name="frlastcpllt">��������յ��߶Σ�ֻ��һ���㣩</param> 
        /// <param name="tolastcpllt">С�������յ��߶Σ�ֻ��һ���㣩</param>
        /// <param name="intMaxBackK">����ϵ��</param> 
        /// <returns>T����</returns>
        public CTable[,] CreatT(CPolyline frcpl, CPolyline tocpl, List<CPolyline> CFrBezierEdgeLt, List<CPolyline> CToBezierEdgeLt,
                                ref List<CPolyline> frlastcpllt, ref List<CPolyline> tolastcpllt, int intMaxBackK)
        {

            ////ע�⣺T�����е���Ÿ�ԭ���㷨�е������ͳһ�ģ����������е������Ӧ��1
            //CTable[,] T = new CTable[CFrBezierEdgeLt.Count + 1, CToBezierEdgeLt.Count + 1];  //�߶�����Ϊ����������1

            ////����ĵ�һ�м���һ�г�ʼ��
            //T[0, 0] = new CTable();
            //T[0, 0].dblEvaluation = 0;

            //CPolyline frfirstcpl = new CPolyline(0, CFrBezierEdgeLt[0].CptLt [0]);  //����״Ҫ�صĵ�һ������Ϊ�߶�
            //for (int j = 1; j <= CToBezierEdgeLt.Count; j++)
            //{
            //    T[0, j] = new CTable();
            //    T[0, j].frpart = "first";
            //    T[0, j].frfrId = 1;
            //    T[0, j].topart = "segment";
            //    T[0, j].tofrId = j;
            //    CPolyline tocplj = tocpl.GetSubPolyline(CToBezierEdgeLt[j - 1].CptLt[0], CToBezierEdgeLt[j - 1].CptLt[CToBezierEdgeLt[j - 1].CptLt.Count - 1]);
            //    T[0, j].dblEvaluation = T[0, j - 1].dblEvaluation + CalDistance(frfirstcpl, tocplj, frcpl, tocpl);
            //}

            //CPolyline tofirstcpl = new CPolyline(0, CToBezierEdgeLt[0].CptLt[0]);  //����״Ҫ�صĵ�һ������Ϊ�߶�
            //for (int i = 1; i <= CFrBezierEdgeLt.Count; i++)
            //{
            //    T[i, 0] = new CTable();
            //    T[i, 0].frpart = "segment";
            //    T[i, 0].frfrId = i;
            //    T[i, 0].topart = "first";
            //    T[i, 0].tofrId = 1;
            //    CPolyline tocpli = frcpl.GetSubPolyline(CFrBezierEdgeLt[i - 1].CptLt[0], CFrBezierEdgeLt[i - 1].CptLt[CFrBezierEdgeLt[i - 1].CptLt.Count - 1]);
            //    T[i, 0].dblEvaluation = T[i - 1, 0].dblEvaluation + CalDistance(CFrBezierEdgeLt[i - 1], tofirstcpl, frcpl, tocpl);
            //}

            ////ѭ��������ά����T�еĸ���ֵ
            //for (int i = 0; i < CFrBezierEdgeLt.Count; i++)//��������߶��յ�����׼��
            //{
            //    List<CPoint> frlastptlt = new List<CPoint>();
            //    frlastptlt.Add(CFrBezierEdgeLt[i].CptLt[1]);
            //    frlastcpllt.Add(new CPolyline(0, frlastptlt));
            //}

            //for (int i = 0; i < CToBezierEdgeLt.Count; i++)//С�������߶��յ�����׼��
            //{
            //    List<CPoint> frlastptlt = new List<CPoint>();
            //    frlastptlt.Add(CToBezierEdgeLt[i].CptLt[1]);
            //    tolastcpllt.Add(new CPolyline(0, frlastptlt));
            //}

            ////ע�⣺T�е����1ָ����һ��Ԫ�أ�����LT�У���CFrBezierEdgeLt,tolastcpllt�������1��ָ���ڶ���Ԫ��
            //for (int i = 1; i <= CFrBezierEdgeLt.Count; i++)               //������ո�ֵ
            //{
            //    for (int j = 1; j <= CToBezierEdgeLt.Count; j++)
            //    {
            //        SortedDictionary<double, CTable> dblCTableSlt = new SortedDictionary<double, CTable>(new CCmpDbl());
            //        CPolyline subfrcpl1 = frcpl.GetSubPolyline(CFrBezierEdgeLt[i - 1].CptLt[0], CFrBezierEdgeLt[i - 1].CptLt[CFrBezierEdgeLt[i - 1].CptLt.Count - 1]);
            //        CPolyline subtocpl1 = tocpl.GetSubPolyline(CToBezierEdgeLt[j - 1].CptLt[0], CToBezierEdgeLt[j - 1].CptLt[CToBezierEdgeLt[j - 1].CptLt.Count - 1]);

            //        //����table1
            //        CTable table1 = new CTable();
            //        table1.frpart = "segment";
            //        table1.frfrId = i;
            //        table1.topart = "last";
            //        table1.tofrId = j;                    
            //        table1.dblEvaluation = T[i - 1, j].dblEvaluation + CalDistance(subfrcpl1, tolastcpllt[j - 1], frcpl, tocpl);
            //        dblCTableSlt.Add(table1.dblEvaluation, table1);

            //        //����table2
            //        CTable table2 = new CTable();
            //        table2.frpart = "last";
            //        table2.frfrId = i;
            //        table2.topart = "segment";
            //        table2.tofrId = j;                    
            //        table2.dblEvaluation = T[i, j - 1].dblEvaluation + CalDistance(frlastcpllt[i - 1], subtocpl1, frcpl, tocpl);
            //        dblCTableSlt.Add(table2.dblEvaluation, table2);

            //        //����table3
            //        CTable table3 = new CTable();
            //        table3.frpart = "segment";
            //        table3.frfrId = i;
            //        table3.topart = "segment";
            //        table3.tofrId = j;
            //        table3.dblEvaluation = T[i - 1, j - 1].dblEvaluation + CalDistance(subfrcpl1, subtocpl1, frcpl, tocpl);
            //        dblCTableSlt.Add(table3.dblEvaluation, table3);

            //        //����table4j
            //        for (int k = 2; k <= intMaxBackK; k++)
            //        {
            //            if (j - k + 1 > 0)  //����տ�ʼִ��ʱ��֮ǰ�ѱ������߶������٣�����С��intMaxBackK
            //            {
            //                CTable table4j = new CTable();
            //                table4j.frpart = "segment";
            //                table4j.frfrId = i;
            //                table4j.topart = "multisegments";
            //                table4j.tofrId = j - k + 1;
            //                table4j.totoId = j;
            //                table4j.intBackK = k;
            //                CPolyline tocplj = tocpl.GetSubPolyline(CToBezierEdgeLt[j - k].CptLt[0], CToBezierEdgeLt[j - 1].CptLt[1]);
            //                table4j.dblEvaluation = T[i - 1, j - k].dblEvaluation + CalDistance(subfrcpl1, tocplj, frcpl, tocpl);
            //                dblCTableSlt.Add(table4j.dblEvaluation, table4j);
            //            }
            //        }

            //        //����table5i
            //        for (int k = 2; k <= intMaxBackK; k++)
            //        {
            //            if (i - k + 1 > 0)  //����տ�ʼִ��ʱ��֮ǰ�ѱ������߶������٣�����С��intMaxBackK
            //            {
            //                CTable table5i = new CTable();
            //                table5i.frpart = "multisegments";
            //                table5i.frfrId = i - k + 1;
            //                table5i.frtoId = i;
            //                table5i.topart = "segment";
            //                table5i.tofrId = j;
            //                table5i.intBackK = k;
            //                CPolyline frcpli = frcpl.GetSubPolyline(CFrBezierEdgeLt[i - k].CptLt[0], CFrBezierEdgeLt[i - 1].CptLt[1]);
            //                table5i.dblEvaluation = T[i - k, j - 1].dblEvaluation + CalDistance(frcpli, subtocpl1, frcpl, tocpl);
            //                dblCTableSlt.Add(table5i.dblEvaluation, table5i);
            //            }
            //        }


            //        T[i, j] = dblCTableSlt.ElementAt(0).Value;
            //    }
            //}

            //int intRowNum = T.GetUpperBound(0);
            //int intColNum = T.GetUpperBound(1);
            //double dblTranslation = T[intRowNum, intColNum].dblEvaluation;
            //return T;
            return null;
        }


        /// <summary>�Ի��ݵķ�ʽѰ�Ҷ�Ӧ�߶�</summary>
        /// <param name="frcpl">���������״Ҫ��</param>
        /// <param name="tocpl">С��������״Ҫ��</param> 
        /// <param name="CFrEdgeLt">��������߶�</param>  
        ///  <param name="CToEdgeLt">С�������߶�</param> 
        /// <param name="frlastcpllt">��������յ��߶Σ�ֻ��һ���㣩</param> 
        /// <param name="tolastcpllt">С�������յ��߶Σ�ֻ��һ���㣩</param>  
        /// <returns>��Ӧ�߶�����</returns>
        public LinkedList<CCorrespondSegment> FindCorrespondSegmentLk(CTable[,] T,  CPolyline frcpl, CPolyline tocpl, List<CPolyline> CFrBezierEdgeLt, List<CPolyline> CToBezierEdgeLt,List<CPolyline> frlastcpllt,List<CPolyline> tolastcpllt)
        {
            //LinkedList<CCorrespondSegment> CorrespondSegmentLk = new LinkedList<CCorrespondSegment>();
            //int i = CFrBezierEdgeLt.Count;
            //int j = CToBezierEdgeLt.Count;

            //while (i >= 0 && j >= 0)
            //{
            //    CPolyline frcplw = new CPolyline();
            //    CPolyline tocplw = new CPolyline();
            //    if (i == 0 && j == 0)
            //    {
            //        break;
            //    }
            //    else if (i == 0 && j > 0)  //��0�е����
            //    {
            //        frcplw = new CPolyline(0, CFrBezierEdgeLt[0].CptLt[0]);
            //        tocplw = tocpl.GetSubPolyline(CToBezierEdgeLt[0].CptLt[0], CToBezierEdgeLt[j - 1].CptLt[1]);
            //        j = 0;
            //    }
            //    else if (i > 0 && j == 0)  //��0�е����
            //    {
            //        frcplw = frcpl.GetSubPolyline(CFrBezierEdgeLt[0].CptLt[0], CFrBezierEdgeLt[i - 1].CptLt[1]);
            //        tocplw = new CPolyline(0, CToBezierEdgeLt[0].CptLt[0]);
            //        i = 0;
            //    }
            //    else  //�������е����
            //    {
            //        if (T[i, j].frpart == "segment" && T[i, j].topart == "last")
            //        {
            //            frcplw = frcpl.GetSubPolyline(CFrBezierEdgeLt[i - 1].CptLt[0], CFrBezierEdgeLt[i - 1].CptLt[1]);
            //            tocplw = tolastcpllt[j - 1];
            //            i = i - 1;
            //        }
            //        else if (T[i, j].frpart == "last" && T[i, j].topart == "segment")
            //        {
            //            frcplw = frlastcpllt[i - 1];
            //            tocplw = tocpl.GetSubPolyline(CToBezierEdgeLt[j - 1].CptLt[0], CToBezierEdgeLt[j - 1].CptLt[1]);
            //            j = j - 1;
            //        }
            //        else if (T[i, j].frpart == "segment" && T[i, j].topart == "segment")
            //        {
            //            frcplw = frcpl.GetSubPolyline(CFrBezierEdgeLt[i - 1].CptLt[0], CFrBezierEdgeLt[i - 1].CptLt[1]);
            //            tocplw = tocpl.GetSubPolyline(CToBezierEdgeLt[j - 1].CptLt[0], CToBezierEdgeLt[j - 1].CptLt[1]);
            //            i = i - 1;
            //            j = j - 1;
            //        }
            //        else if (T[i, j].frpart == "segment" && T[i, j].topart == "multisegments")
            //        {
            //            frcplw = frcpl.GetSubPolyline(CFrBezierEdgeLt[i - 1].CptLt[0], CFrBezierEdgeLt[i - 1].CptLt[1]);
            //            tocplw = tocpl.GetSubPolyline(CToBezierEdgeLt[j - T[i, j].intBackK].CptLt[0], CToBezierEdgeLt[j - 1].CptLt[1]);
            //            j = j - T[i, j].intBackK;
            //            i = i - 1;
            //        }
            //        else if (T[i, j].frpart == "multisegments" && T[i, j].topart == "segment")
            //        {
            //            frcplw = frcpl.GetSubPolyline(CFrBezierEdgeLt[i - T[i, j].intBackK].CptLt[0], CFrBezierEdgeLt[i - 1].CptLt[1]);
            //            tocplw = tocpl.GetSubPolyline(CToBezierEdgeLt[j - 1].CptLt[0], CToBezierEdgeLt[j - 1].CptLt[1]);
            //            i = i - T[i, j].intBackK;
            //            j = j - 1;
            //        }
            //    }

            //    CCorrespondSegment pCorrespondSegment = new CCorrespondSegment();
            //    pCorrespondSegment = new CCorrespondSegment(frcplw, tocplw);
            //    CorrespondSegmentLk.AddFirst(pCorrespondSegment);

            //}

            //return CorrespondSegmentLk;
            return null;
        }






        /// <summary>�������ֵ(Translationָ��ֵ)</summary>
        /// <param name="frcpl">��������߶Σ�����ֻ��һ������</param>
        /// <param name="tocpl">С�������߶Σ�����ֻ��һ������</param> 
        /// <returns>����ֵ</returns>
        public double CalDistance(CPolyline subfrcpl, CPolyline subtocpl,CPolyline frcpl, CPolyline tocpl)
        {
            List<CPoint> cresultptlt = _LinearInterpolationA.CLI(subfrcpl, subtocpl);  //ÿ�ζ��൱�ڴ����µ��߶Σ����ʹ��CLI
            double dblTranslation = _pTranslation.CalTranslation(cresultptlt);
            return dblTranslation;


            //List<CPoint> cresultptlt = _LinearInterpolationA.CLI(subfrcpl, subtocpl);  //ÿ�ζ��൱�ڴ����µ��߶Σ����ʹ��CLI
            //double dblCost1 = _pTranslation.CalRatioTranslation(cresultptlt, frcpl, tocpl);
            //double dblCost2 = _pIntegral.CalRatioIntegral(cresultptlt, frcpl, tocpl);
            //double dblCost3 = _pLengthDiff.CalRatioLengthDiff(cresultptlt, frcpl, tocpl);
            //double dblSumCost = dblCost1 + dblCost2 + dblCost3;
            //return dblSumCost;

        }

        /// <summary>���ԣ�������</summary>
        public CParameterResult ParameterResult
        {
            get { return _ParameterResult; }
            set { _ParameterResult = value; }
        }
    }
}
