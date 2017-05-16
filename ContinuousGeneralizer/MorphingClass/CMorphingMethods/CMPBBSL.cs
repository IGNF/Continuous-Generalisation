using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

using MorphingClass.CCorrepondObjects;
using MorphingClass.CEvaluationMethods;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;
using MorphingClass.CMorphingAlgorithms;

namespace MorphingClass.CMorphingMethods
{
    /// <summary>
    /// ���������ṹ����״Ҫ��Morphing�任�������Ի��߳���Ϊƥ�����ݣ�Length��
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class CMPBBSL
    {
        
        
        private CParameterResult _ParameterResult;
        private CTriangulator _Triangulator = new CTriangulator();

        private List<CPolyline> _LSCPlLt = new List<CPolyline>();  //BS:LargerScale
        private List<CPolyline> _SSCPlLt = new List<CPolyline>();  //SS:SmallerScale

        private CPolyline _FromCpl;
        private CPolyline _ToCpl;

        private CParameterInitialize _ParameterInitialize;

        public CMPBBSL()
        {

        }

        public CMPBBSL(CPolyline frcpl, CPolyline tocpl)
        {
            _FromCpl = frcpl;
            _ToCpl = tocpl;
        }

        public CMPBBSL(CParameterInitialize ParameterInitialize)
        {
            //��ȡ��ǰѡ��ĵ�Ҫ��ͼ��
            //�������Ҫ��ͼ��
            IFeatureLayer pBSFLayer = (IFeatureLayer)ParameterInitialize.m_mapFeature.get_Layer(ParameterInitialize.cboLargerScaleLayer.SelectedIndex);
                                                                       
            //С������Ҫ��ͼ��
            IFeatureLayer pSSFLayer =(IFeatureLayer)ParameterInitialize.m_mapFeature.get_Layer(ParameterInitialize.cboSmallerScaleLayer.SelectedIndex);
                                                           


            ParameterInitialize.pBSFLayer = pBSFLayer;
            ParameterInitialize.pSSFLayer = pSSFLayer;
            _ParameterInitialize = ParameterInitialize;

            //��ȡ������
            _LSCPlLt = CHelpFunc.GetCPlLtByFeatureLayer(pBSFLayer);
            _SSCPlLt = CHelpFunc.GetCPlLtByFeatureLayer(pSSFLayer);
        }

        //����������Morphing����
        public void MPBBSLMorphing()
        {          
            //CParameterInitialize ParameterInitialize = _ParameterInitialize;
            //CGeoFunc.SetCPlScaleEdgeLengthPtBelong(ref _LSCPlLt, CEnumScale.Larger);
            //CGeoFunc.SetCPlScaleEdgeLengthPtBelong(ref _SSCPlLt, CEnumScale.Smaller);
            //CPolyline frcpl = _LSCPlLt[0];
            //CPolyline tocpl = _SSCPlLt[0];

            ////���㼫Сֵ
            //
            //long lngStartTime = System.Environment.TickCount;  //��ʼʱ��

            //List<CPoint> frchcptlt = _Triangulator.CreateConvexHullEdgeLt2(frcpl, dblVerySmall);
            //CPolyline frchcpl = new CPolyline(0, frchcptlt);    //��������������������߶�
            //frchcpl.SetPolyline();

            //List<CPoint> tochcptlt = _Triangulator.CreateConvexHullEdgeLt2(tocpl, dblVerySmall);
            //CPolyline tochcpl = new CPolyline(0, tochcptlt);    //С�������������������߶�
            //tochcpl.SetPolyline();

            ////���Լ����������ͼ�㣬�Ա�������AE�еĹ���(ct:constraint)
            //List<CPolyline> frctcpllt = new List<CPolyline>(); frctcpllt.Add(frcpl); frctcpllt.Add(frchcpl);
            //List<CPolyline> toctcpllt = new List<CPolyline>(); toctcpllt.Add(tocpl); toctcpllt.Add(tochcpl);
            //IFeatureLayer pBSFLayer = CHelpFunc.SaveCPlLt(frctcpllt, "frctcpllt", ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);
            //IFeatureLayer pSSFLayer = CHelpFunc.SaveCPlLt(toctcpllt, "toctcpllt", ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);

            ////����CDT����ȡ����ɭ��
            //CBendForest FromLeftBendForest = new CBendForest();
            //CBendForest FromRightBendForest = new CBendForest();
            //CParameterVariable pParameterVariableFrom = new CParameterVariable(frcpl, "FromCDT", pBSFLayer, dblVerySmall);
            //GetBendForest(pParameterVariableFrom,ref FromLeftBendForest, ref FromRightBendForest,ParameterInitialize);

            //CBendForest ToLeftBendForest = new CBendForest();
            //CBendForest ToRightBendForest = new CBendForest();
            //CParameterVariable pParameterVariableTo = new CParameterVariable(tocpl, "ToCDT", pSSFLayer, dblVerySmall);
            //GetBendForest(pParameterVariableTo, ref ToLeftBendForest, ref ToRightBendForest, ParameterInitialize);

            ////��������ɭ�֣������������
            //NeatenBendForest(frcpl, FromLeftBendForest);
            //NeatenBendForest(frcpl, FromRightBendForest);
            //NeatenBendForest(tocpl, ToLeftBendForest);
            //NeatenBendForest(tocpl, ToRightBendForest);

            //CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper();
            ////double dblSumLength = frcpl.pPolyline.Length + tocpl.pPolyline.Length;
            //CTranslation pTranslation = new CTranslation();

            ////������ֵ����
            //CParameterThreshold ParameterThreshold = new CParameterThreshold();
            //ParameterThreshold.dblFrLength = frcpl.pPolyline.Length;
            //ParameterThreshold.dblToLength = tocpl.pPolyline.Length;
            //ParameterThreshold.dblAngleBound = 0.262;

            //List<double> dblTranslationLt = new List<double>();
            //SortedDictionary<double, int> ResultsSlt = new SortedDictionary<double, int>(new CCmpDbl());
            //for (int i = 0; i <= 25; i++)
            //{
            //    //ParameterThreshold.dblDLengthBound = 1 * (1 - 0.02 * i);
            //    //ParameterThreshold.dblULengthBound = 1 / (1 - 0.02 * i);


            //    ////������ƥ�䣬Ѱ�Ҷ�Ӧ��������
            //    //List<CCorrespondBend> IndependCorrespondBendLt = new List<CCorrespondBend>();
            //    //IndependCorrespondBendLt.AddRange(BendTreeMatch(FromLeftBendForest, ToLeftBendForest, ParameterThreshold));
            //    //IndependCorrespondBendLt.AddRange(BendTreeMatch(FromRightBendForest, ToRightBendForest, ParameterThreshold));

            //    ////����ƥ�䣬Ѱ�Ҷ�Ӧ����
            //    //List<CCorrespondBend> CorrespondBendLt = BendMatch(IndependCorrespondBendLt, ParameterThreshold);

            //    ////��ȡ��Ӧ�߶�
            //    //C5.LinkedList<CCorrespondSegment> CorrespondSegmentLk = CHelpFunc.DetectCorrespondSegment(frcpl, tocpl, CorrespondBendLt);
            //    ////CHelpFunc.PreviousWorkCSeLt(ref CorrespondSegmentLk);

            //    ////��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��                
            //    //List<CPoint> ResultPtLt = new List<CPoint>();
            //    //ResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(CorrespondSegmentLk, "Linear");

            //    //double dblTranslation = pTranslation.CalTranslation(ResultPtLt);
            //    //dblTranslationLt.Add(dblTranslation);

            //    //ResultsSlt.Add(dblTranslation, i);
            //}

            
            ////����������һ�飡����������
            ////���ɣ��������SortedList<double, CParameterResult> ResultsSlt = new SortedList<double, CParameterResult>(new CCmpDbl())��¼�����
            ////      �����ڻ�����λ��CPoint�����Ƶ���ָ�룩������ȻӰ��CParameterResult�е�ResultPtLtֵ
            ////int intIndex = ResultsSlt.Values[0];
            ////ParameterThreshold.dblDLengthBound = 1 * (1 - 0.02 * intIndex);
            ////ParameterThreshold.dblULengthBound = 1 / (1 - 0.02 * intIndex);

            //ParameterThreshold.dblDLengthBound = 1 * 0.95;
            //ParameterThreshold.dblULengthBound = 1 / 0.95;

            ////������ƥ�䣬Ѱ�Ҷ�Ӧ��������
            //List<CCorrespondBend> pIndependCorrespondBendLt = new List<CCorrespondBend>();
            //pIndependCorrespondBendLt.AddRange(BendTreeMatch(FromLeftBendForest, ToLeftBendForest, ParameterThreshold));
            //pIndependCorrespondBendLt.AddRange(BendTreeMatch(FromRightBendForest, ToRightBendForest, ParameterThreshold));

            ////����ƥ�䣬Ѱ�Ҷ�Ӧ����
            //List<CCorrespondBend> pCorrespondBendLt = BendMatch(pIndependCorrespondBendLt, ParameterThreshold);

            ////��ȡ��Ӧ�߶�
            //LinkedList<CCorrespondSegment> pCorrespondSegmentLk = CHelpFunc.DetectCorrespondSegment(frcpl, tocpl, pCorrespondBendLt);
            ////CHelpFunc.PreviousWorkCSeLt(ref pCorrespondSegmentLk);

            ////��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��
            //List<CPoint> pResultPtLt= pAlgorithmsHelper.BuildPointCorrespondence(pCorrespondSegmentLk, "Linear");

            ////���㲢��ʾ����ʱ��
            //long lngEndTime = System.Environment.TickCount;
            //long lngTime = lngEndTime - lngStartTime;
            //ParameterInitialize.tsslTime.Text = "Running Time: " + Convert.ToString(lngTime) + "ms";  //��ʾ����ʱ��

            ////����ָ��ֵ����Ӧ��            
            //CHelpFuncExcel.ExportDataltToExcel(dblTranslationLt, "translationlt0", _ParameterInitialize.strSavePath);
            //CHelpFunc.SaveCtrlLine(pCorrespondSegmentLk, "MPBBSLControlLine",dblVerySmall , ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);
            //CHelpFunc.SaveCorrespondLine(pResultPtLt, "MPBBSLCorrLine", ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);

            ////��ȡ�����ȫ����¼��ParameterResult��
            //CParameterResult ParameterResult = new CParameterResult();
            //ParameterResult.FromCpl = frcpl;
            //ParameterResult.ToCpl = tocpl;
            //double pdblTranslation = pTranslation.CalTranslation(pResultPtLt);
            //ParameterResult.dblTranslation = pdblTranslation;
            //ParameterResult.pCorrespondSegmentLk = pCorrespondSegmentLk;
            //ParameterResult.CResultPtLt = pResultPtLt;
            //ParameterResult.lngTime = lngTime;
            //_ParameterResult = ParameterResult;
        }


        /// <summary>
        /// �û���������Morphing�������д���
        /// </summary>
        /// <param name="pParameterInitialize">����</param>
        /// <param name="pParameterVariableFrom">�йش���������ߵĲ�����������Ҫ�����˴��������Ҫ�ء��ļ�����ʱ�����֡��������ͼ��ͼ�Сֵ</param>
        /// <param name="pParameterVariableTo">�й�С���������ߵĲ�����������Ҫ�����ˣ�С��������Ҫ�ء��ļ�����ʱ�����֡�С������ͼ��ͼ�Сֵ</param>
        /// <param name="ParameterThreshold">��ֵ��������Ҫ�����ˣ���С��������Ҫ�س��ȣ��������߱���ֵ��Χ</param>
        /// <remarks>����Լ��������ʱ����ʹ������������ΪԼ����</remarks>
        public LinkedList<CCorrespondSegment> DWByMPBBSL(CParameterInitialize pParameterInitialize, CParameterVariable pParameterVariableFrom, CParameterVariable pParameterVariableTo, CParameterThreshold ParameterThreshold)
        {
            CPolyline frcpl = pParameterVariableFrom.CPolyline;
            frcpl.SetPolyline();
            CPolyline tocpl = pParameterVariableTo.CPolyline;
            tocpl.SetPolyline();

            List<CPoint> frchcptlt = _Triangulator.CreateConvexHullEdgeLt2(frcpl, pParameterVariableFrom.dblVerySmall);
            CPolyline frchcpl = new CPolyline(0, frchcptlt);    //��������������������߶�
            frchcpl.SetPolyline();

            List<CPoint> tochcptlt = _Triangulator.CreateConvexHullEdgeLt2(tocpl, pParameterVariableFrom.dblVerySmall);
            CPolyline tochcpl = new CPolyline(0, tochcptlt);    //С�������������������߶�
            tochcpl.SetPolyline();

            //���Լ����������ͼ�㣬�Ա�������AE�еĹ���(ct:constraint)
            List<CPolyline> frctcpllt = new List<CPolyline>(); frctcpllt.Add(frcpl); frctcpllt.Add(frchcpl);
            List<CPolyline> toctcpllt = new List<CPolyline>(); toctcpllt.Add(tocpl); toctcpllt.Add(tochcpl);
            IFeatureLayer pBSFLayer = CHelpFunc.SaveCPlLt(frctcpllt, "frctcpllt", pParameterInitialize.pWorkspace, pParameterInitialize.m_mapControl);
            IFeatureLayer pSSFLayer = CHelpFunc.SaveCPlLt(toctcpllt, "toctcpllt", pParameterInitialize.pWorkspace, pParameterInitialize.m_mapControl);

            pParameterVariableFrom.pFeatureLayer = pBSFLayer;
            pParameterVariableTo.pFeatureLayer = pSSFLayer;

            //����CDT����ȡ����ɭ��
            CBendForest FromLeftBendForest = new CBendForest();
            CBendForest FromRightBendForest = new CBendForest();
            GetBendForest(pParameterVariableFrom, ref FromLeftBendForest, ref FromRightBendForest, pParameterInitialize);

            CBendForest ToLeftBendForest = new CBendForest();
            CBendForest ToRightBendForest = new CBendForest();
            GetBendForest(pParameterVariableTo, ref ToLeftBendForest, ref ToRightBendForest, pParameterInitialize);




            //������ƥ�䣬Ѱ�Ҷ�Ӧ��������
            List<CCorrespondBend> IndependCorrespondBendLt = new List<CCorrespondBend>();
            IndependCorrespondBendLt.AddRange(BendTreeMatch(FromLeftBendForest, ToLeftBendForest, ParameterThreshold));
            IndependCorrespondBendLt.AddRange(BendTreeMatch(FromRightBendForest, ToRightBendForest, ParameterThreshold));

            //����ƥ�䣬Ѱ�Ҷ�Ӧ����
            List<CCorrespondBend> CorrespondBendLt = BendMatch(IndependCorrespondBendLt, ParameterThreshold);

            //��ȡ��Ӧ�߶�
            LinkedList<CCorrespondSegment> CorrespondSegmentLk = CHelpFunc.DetectCorrespondSegment(frcpl, tocpl, CorrespondBendLt);
            //CHelpFunc.PreviousWorkCSeLt(ref CorrespondSegmentLk);

            return CorrespondSegmentLk;
        }

        /// <summary>
        /// ��ȡ���ߵ�����ɭ��
        /// </summary>
        /// <param name="cpl">����</param>
        /// <param name="pLeftBendForest">������ߵ�����ɭ��</param>
        /// <param name="pRightBendForest">�����ұߵ�����ɭ��</param>
        /// <param name="strName">����������������</param>
        /// <remarks>����Լ��������ʱ����δʹ������������ΪԼ����
        ///          ע�⣺�������е�CreateCDT����ı�pParameterVariable�е�CPolyline</remarks>
        public void GetBendForest(CParameterVariable pParameterVariable, ref CBendForest pLeftBendForest, ref CBendForest pRightBendForest, CParameterInitialize pParameterInitialize)
        {
            //List<CPoint> cptlt = pParameterVariable.CPolyline.CptLt;
            //List<CEdge> CEdgeLt = new List<CEdge>();
            //for (int i = 0; i < cptlt.Count - 1; i++)
            //{
            //    CEdge pEdge = new CEdge(cptlt[i], cptlt[i + 1]);
            //    CEdgeLt.Add(pEdge);
            //}

            CTriangulator OptCDT = new CTriangulator();
            CPolyline newcpl = new CPolyline(pParameterVariable.CPolyline.ID, pParameterVariable.CPolyline.CptLt);
            List<CTriangle> CDTLt = OptCDT.CreateCDT(pParameterVariable.pFeatureLayer, ref newcpl, pParameterVariable.dblVerySmall);
            pParameterVariable.CPolyline = newcpl;

            //for (int i = 0; i < CDTLt.Count; i++) CDTLt[i].TID = i;  //����Ϊֹ��Լ�������ν�����ɣ��������β��ٷ����仯�����������α��           
            OptCDT.GetSETriangle(ref CDTLt, pParameterVariable.dblVerySmall);  //ȷ������������
            OptCDT.ConfirmTriangleSide(ref CDTLt, pParameterVariable.CPolyline, pParameterVariable.dblVerySmall); //ȷ����������λ�����ߵ����ұ�
            OptCDT.SignTriTypeAll(ref CDTLt);   //���I��II��III��VI��������

            pLeftBendForest = OptCDT.BuildBendForestNeed2(ref CDTLt, pParameterVariable.CPolyline.CptLt, "Left", pParameterVariable.dblVerySmall);
            pRightBendForest = OptCDT.BuildBendForestNeed2(ref CDTLt, pParameterVariable.CPolyline.CptLt, "Right", pParameterVariable.dblVerySmall);


            //����������
            List<CTriangle> CTriangleLt = new List<CTriangle>();
            for (int i = 0; i < CDTLt.Count; i++)
            {
                if (CDTLt[i].strTriType != "I")
                {
                    CTriangleLt.Add(CDTLt[i]);
                }
            }
            //CHelpFunc.SaveTriangles(CTriangleLt, pParameterVariable.strName, pParameterInitialize.pWorkspace, pParameterInitialize.m_mapControl);
        }

        /// <summary>
        /// ��������ɭ���е�����
        /// </summary>
        /// <param name="cpl">����</param>
        /// <param name="pBendForest">����ɭ��</param>
        /// <returns>�����λ������������б�</returns>
        /// <remarks>����������������ʼλ�ã�������˳����</remarks>
        public SortedDictionary<double, CBend> NeatenBendForest(CPolyline cpl, CBendForest pBendForest)
        {
            SortedDictionary<double, CBend> pBendSlt = new SortedDictionary<double, CBend>(new CCmpDbl());
            //��������
            for (int i = 0; i < pBendForest.Count; i++)
            {
                CBend pBend = pBendForest.ElementAt(i).Value;
                RecursiveNeatenBendForest(cpl, pBend, pBendSlt);
            }

            //���������
            for (int i = 0; i < pBendSlt.Count; i++)
            {
                pBendSlt.ElementAt(i).Value.ID = i;
            }

            return pBendSlt;

        }

        /// <summary>
        /// �ݹ���������
        /// </summary>
        /// <param name="cpl">����</param>
        /// <param name="pBendForest">����</param>
        /// <param name="pBendSlt">�����б�</param>
        private void RecursiveNeatenBendForest(CPolyline cpl, CBend pBend, SortedDictionary<double, CBend> pBendSlt)
        {
            if (pBend == null)
            {
                return;
            }

            pBend.dblStartRL = CGeoFunc.CalDistanceFromStartPoint(cpl.pPolyline, pBend.FromPoint, true);
            pBend.dblEndRL = CGeoFunc.CalDistanceFromStartPoint(cpl.pPolyline, pBend.ToPoint, true);
            pBendSlt.Add(pBend.dblStartRL, pBend);

            RecursiveNeatenBendForest(cpl, pBend.CLeftBend, pBendSlt);
            RecursiveNeatenBendForest(cpl, pBend.CRightBend, pBendSlt);
        }

        /// <summary>
        /// ��������ƥ�䣬��ȡ��Ӧ�߶�
        /// </summary>
        /// <param name="CFromBendForest">�����������ɭ��</param>
        /// <param name="CToBendForest">С����������ɭ��</param>
        /// <param name="ParameterThreshold">��������</param>
        /// <param name="CorrespondSegmentLk">��Ӧ�߶���</param>
        /// <remarks>������ƥ��ͺ�������ƥ�䶼�ɱ�������ִ��</remarks>
        public List<CCorrespondBend> BendTreeMatch(CBendForest CFromBendForest, CBendForest CToBendForest, CParameterThreshold ParameterThreshold)
        {
            //������߶�������
            SortedDictionary<double, CBend> pFromIndependBendSlt = new SortedDictionary<double, CBend>(new CCmpDbl());
            for (int i = 0; i < CFromBendForest.Count; i++)
            {
                pFromIndependBendSlt.Add(CFromBendForest.ElementAt(i).Value.dblStartRL, CFromBendForest.ElementAt(i).Value);
            }
            //С�����߶�������
            SortedDictionary<double, CBend> pToIndependBendSlt = new SortedDictionary<double, CBend>(new CCmpDbl());
            for (int i = 0; i < CToBendForest.Count; i++)
            {
                pToIndependBendSlt.Add(CToBendForest.ElementAt(i).Value.dblStartRL, CToBendForest.ElementAt(i).Value);
            }

            //��ȡ��Ӧ����
            List<CCorrespondBend> pIndependCorrespondBendLt = IndependBendMatch(pFromIndependBendSlt, pToIndependBendSlt, ParameterThreshold);

            return pIndependCorrespondBendLt;
        }

        /// <summary>
        /// ��������ƥ�䣬��ȡ��Ӧ�߶�
        /// </summary>
        /// <param name="CFromBendForest">�����������ɭ��</param>
        /// <param name="CToBendForest">С����������ɭ��</param>
        /// <param name="ParameterThreshold">��������</param>
        /// <param name="CorrespondSegmentLk">��Ӧ�߶���</param>
        /// <remarks>������ƥ��ͺ�������ƥ�䶼�ɱ�������ִ��</remarks>
        public List<CCorrespondBend> BendTreeMatch2(CBendForest CFromBendForest, CBendForest CToBendForest, CParameterThreshold ParameterThreshold)
        {
            //������߶�������
            SortedDictionary<double, CBend> pFromIndependBendSlt = new SortedDictionary<double, CBend>(new CCmpDbl());
            for (int i = 0; i < CFromBendForest.Count; i++)
            {
                pFromIndependBendSlt.Add(CFromBendForest.ElementAt(i).Value.dblStartRL, CFromBendForest.ElementAt(i).Value);
            }
            //С�����߶�������
            SortedDictionary<double, CBend> pToIndependBendSlt = new SortedDictionary<double, CBend>(new CCmpDbl());
            for (int i = 0; i < CToBendForest.Count; i++)
            {
                pToIndependBendSlt.Add(CToBendForest.ElementAt(i).Value.dblStartRL, CToBendForest.ElementAt(i).Value);
            }

            //��ȡ��Ӧ����
            List<CCorrespondBend> pIndependCorrespondBendLt = IndependBendMatch2(pFromIndependBendSlt, pToIndependBendSlt, ParameterThreshold);

            return pIndependCorrespondBendLt;
        }


        /// <summary>
        /// ����ƥ��(ȫ�ֵ�ͬ����)
        /// </summary>
        /// <param name="pFromBendSlt">������������б�</param>
        /// <param name="pToBendSlt">С�����������б�</param>
        /// <param name="dblRatioBound">������ֵ</param>
        /// <returns>��Ӧ�����б�</returns>
        /// <remarks></remarks>
        private List<CCorrespondBend> IndependBendMatch(SortedDictionary<double, CBend> pFromBendSlt, SortedDictionary<double, CBend> pToBendSlt,
                                                        CParameterThreshold ParameterThreshold)
        {
            //���������Ķ�Ӧ�����б�
            foreach (CBend pBend in pFromBendSlt.Values)
            {
                pBend.pCorrespondBendLt.Clear();
            }
            foreach (CBend pBend in pToBendSlt.Values)
            {
                pBend.pCorrespondBendLt.Clear();
            }

            //��������Ҫ��Ķ�Ӧ����
            List<CCorrespondBend> pCorrespondBendLt = new List<CCorrespondBend>();
            int intLastMatchj = 0;   //��ֵ��������ȷҪ�󣬽�Ϊ�м�������Ĺ���ֵ
            for (int i = 0; i < pFromBendSlt.Values.Count; i++)
            {
                CBend pfrbend = pFromBendSlt.ElementAt(i).Value;
                for (int j = 0; j < pToBendSlt.Values.Count; j++)
                {
                    CBend ptobend = pToBendSlt.ElementAt(i).Value;
                    double dblAngleDiff = pfrbend.pBaseLine.Angle - ptobend.pBaseLine.Angle;
                    double dblLengthRatio = pfrbend.pBaseLine.Length / ptobend.pBaseLine.Length;

                    if ((Math.Abs(dblAngleDiff) <= ParameterThreshold.dblAngleBound)
                        && (dblLengthRatio >= ParameterThreshold.dblDLengthBound) && (dblLengthRatio <= ParameterThreshold.dblULengthBound))
                    {
                        CCorrespondBend pCorrespondBend = new CCorrespondBend(pfrbend, ptobend);
                        pCorrespondBendLt.Add(pCorrespondBend);
                    }
                }
            }

            return pCorrespondBendLt;
        }

        /// <summary>
        /// ����ƥ��(ȫ�ֵ�ͬ����)
        /// </summary>
        /// <param name="pFromBendSlt">������������б�</param>
        /// <param name="pToBendSlt">С�����������б�</param>
        /// <param name="dblRatioBound">������ֵ</param>
        /// <returns>��Ӧ�����б�</returns>
        /// <remarks></remarks>
        private List<CCorrespondBend> IndependBendMatch2(SortedDictionary<double, CBend> pFromBendSlt, SortedDictionary<double, CBend> pToBendSlt,
                                                        CParameterThreshold ParameterThreshold)
        {
            //���������Ķ�Ӧ�����б�
            foreach (CBend  pBend in pFromBendSlt.Values )
            {
                pBend.pCorrespondBendLt.Clear();
            }
            foreach (CBend pBend in pToBendSlt.Values)
            {
                pBend.pCorrespondBendLt.Clear();
            }

            //��������Ҫ��Ķ�Ӧ����
            List<CCorrespondBend> pCorrespondBendLt = new List<CCorrespondBend>();
            int intI = 0;
            int intJ = 0;
            while (intI < pFromBendSlt.Count && intJ < pToBendSlt.Count)
            {
                CBend pfrbend = pFromBendSlt.ElementAt(intI).Value;
                CBend ptobend = pToBendSlt.ElementAt(intJ).Value;
                double dblRatioLengthi = pfrbend.Length / ParameterThreshold.dblFrLength;
                double dblRatioLengthj = ptobend.Length / ParameterThreshold.dblToLength;
                double dblStartDiff = pfrbend.dblStartRL - ptobend.dblStartRL;
                double dblEndDiff = pfrbend.dblEndRL - ptobend.dblEndRL;

                //�������λ�ò���ֵ
                double dblRatioBound;
                if (dblRatioLengthi <= dblRatioLengthj)
                {
                    dblRatioBound = 0.5 * dblRatioLengthi;
                }
                else
                {
                    dblRatioBound = 0.5 * dblRatioLengthj;
                }

                double dblLengthRatio = pfrbend.pBaseLine.Length / ptobend.pBaseLine.Length;
                if ((Math.Abs(dblStartDiff) < dblRatioBound) && (Math.Abs(dblEndDiff) < dblRatioBound)
                    && (dblLengthRatio >= ParameterThreshold.dblDLengthBound) && (dblLengthRatio <= ParameterThreshold.dblULengthBound))
                {
                    CCorrespondBend pCorrespondBend = new CCorrespondBend(pFromBendSlt.ElementAt(intI).Value, pToBendSlt.ElementAt(intJ).Value);
                    pCorrespondBendLt.Add(pCorrespondBend);
                    intI++;
                    intJ++;
                }
                else
                {
                    if ((ptobend.dblEndRL -pfrbend .dblStartRL) > (0.5*dblRatioLengthi) )
                    {
                        intI++;
                    }
                    if ((pfrbend.dblEndRL - ptobend.dblStartRL) > (0.5 * dblRatioLengthj))
                    {
                        intJ++;
                    }
                }
            }



            //int intLastMatchj = 0;   //��ֵ��������ȷҪ�󣬽�Ϊ�м�������Ĺ���ֵ
            //for (int i = 0; i < pFromBendSlt.Values.Count; i++)
            //{
            //    CBend pfrbend = pFromBendSlt.Values[i];
            //    double dblRatioLengthi = pfrbend.Length / ParameterThreshold.dblFrLength;


            //    //int dblTempMatchj = 0;
            //    //Ϊ�˽�ʡʱ�䣬���м�����������
            //    //��intLastMatchjΪ��׼ǰ������
            //    for (int j = intLastMatchj; j < pToBendSlt.Values.Count; j++)
            //    {
            //        CBend ptobend = pToBendSlt.Values[j];
            //        double dblRatioLengthj = ptobend.Length / ParameterThreshold.dblToLength;
            //        double dblStartDiff = pfrbend.dblStartRL - ptobend.dblStartRL;
            //        double dblEndDiff = pfrbend.dblEndRL - ptobend.dblEndRL;
            //        double dblAngleDiff = pfrbend.pBaseLine.Angle - ptobend.pBaseLine.Angle;

            //        //�������λ�ò���ֵ
            //        double dblRatioBoundj;
            //        if (dblRatioLengthi >= dblRatioLengthj)
            //        {
            //            dblRatioBoundj = 0.25 * dblRatioLengthi;
            //        }
            //        else
            //        {
            //            dblRatioBoundj = 0.25 * dblRatioLengthj;
            //        }

            //        if (dblStartDiff < (-dblRatioBoundj))
            //        {
            //            break; //����Ѿ�����һ����Χ����û��Ҫ�����������
            //        }

            //        double dblLengthRatio = pfrbend.pBaseLine.Length / ptobend.pBaseLine.Length;
            //        if ((Math.Abs(dblStartDiff) <= dblRatioBoundj) && (Math.Abs(dblEndDiff) <= dblRatioBoundj) && (Math.Abs(dblAngleDiff) <= ParameterThreshold.dblAngleBound)
            //            && (dblLengthRatio >= ParameterThreshold.dblDLengthBound) && (dblLengthRatio <= ParameterThreshold.dblULengthBound))
            //        {
            //            CCorrespondBend pCorrespondBend = new CCorrespondBend(pFromBendSlt.Values[i], pToBendSlt.Values[j]);
            //            pCorrespondBendLt.Add(pCorrespondBend);
            //            intLastMatchj = j;
            //        }
            //    }
            //}
           
            return pCorrespondBendLt;
        }

        #region ����ƥ��(ȫ�ֵ�ͬ����)
        ///// <summary>
        ///// ����ƥ��(ȫ�ֵ�ͬ����)
        ///// </summary>
        ///// <param name="pFromBendSlt">������������б�</param>
        ///// <param name="pToBendSlt">С�����������б�</param>
        ///// <param name="dblRatioBound">������ֵ</param>
        ///// <returns>��Ӧ�����б�</returns>
        ///// <remarks></remarks>
        //private List<CCorrespondBend> BendMatch(SortedList<double, CBend> pFromBendSlt, SortedList<double, CBend> pToBendSlt, double dblRatioBound)
        //{
        //    //���������Ķ�Ӧ�����б�
        //    for (int i = 0; i < pFromBendSlt.Count ; i++)
        //    {
        //        pFromBendSlt.Values[i].pCorrespondBendLt.Clear();
        //    }
        //    for (int i = 0; i < pToBendSlt.Count ; i++)
        //    {
        //        pToBendSlt.Values[i].pCorrespondBendLt.Clear();
        //    }


        //    //��������Ҫ��Ķ�Ӧ����
        //    int intLastMatchj = 0;   //��ֵ��������ȷҪ�󣬽�Ϊ�м�������Ĺ���ֵ
        //    for (int i = 0; i < pFromBendSlt.Values.Count; i++)
        //    {
        //        CBend pfrbend = pFromBendSlt.Values[i];
        //        double dblRatioBoundi = dblRatioBound * pfrbend.Length;
        //        int dblTempMatchj = 0;
        //        //Ϊ�˽�ʡʱ�䣬���м�����������
        //        //��intLastMatchjΪ��׼ǰ������
        //        for (int j = intLastMatchj; j < pToBendSlt.Values.Count; j++)
        //        {
        //            CBend ptobend = pToBendSlt.Values[j];
        //            double dblStartDiff = pfrbend.dblStartRL - ptobend.dblStartRL;
        //            double dblEndDiff = Math.Abs(pfrbend.dblEndRL - ptobend.dblEndRL);
        //            if (dblStartDiff < (-dblRatioBoundi))
        //            {
        //                break;
        //            }
        //            if ((Math.Abs(dblStartDiff) < dblRatioBoundi) && (Math.Abs(dblEndDiff) < dblRatioBoundi))
        //            {
        //                double dblSumDiff = dblStartDiff + dblEndDiff;
        //                pfrbend.pCorrespondBendLt.Add(dblSumDiff, ptobend);
        //                ptobend.pCorrespondBendLt.Add(dblSumDiff, pfrbend);
        //                dblTempMatchj = j;
        //            }
        //        }

        //        //��intLastMatchjΪ��׼��������
        //        for (int j = intLastMatchj - 1; j >= 0; j--)
        //        {
        //            CBend ptobend = pToBendSlt.Values[j];
        //            double dblStartDiff = pfrbend.dblStartRL - ptobend.dblStartRL;
        //            double dblEndDiff = Math.Abs(pfrbend.dblEndRL - ptobend.dblEndRL);
        //            if (dblStartDiff > dblRatioBoundi)
        //            {
        //                break;
        //            }
        //            if ((Math.Abs(dblStartDiff) < dblRatioBoundi) && (Math.Abs(dblEndDiff) < dblRatioBoundi))
        //            {
        //                double dblSumDiff = dblStartDiff + dblEndDiff;
        //                pfrbend.pCorrespondBendLt.Add(dblSumDiff, ptobend);
        //                ptobend.pCorrespondBendLt.Add(dblSumDiff, pfrbend);
        //                dblTempMatchj = j;
        //            }
        //        }

        //        intLastMatchj = dblTempMatchj;
        //    }

        //    //�������������Ϊ��������������ҵ�һ�Զ�Ӧ����
        //    List<CCorrespondBend> pCorrespondBendLt = new List<CCorrespondBend>();
        //    for (int i = 0; i < pFromBendSlt.Values.Count; i++)
        //    {
        //        if (pFromBendSlt.Values[i].pCorrespondBendLt.Count > 0)
        //        {
        //            CBend optimumtobend = pFromBendSlt.Values[i].pCorrespondBendLt.Values[0];
        //            if (optimumtobend.pCorrespondBendLt.Count > 0)
        //            {
        //                if (pFromBendSlt.Values[i].ID == optimumtobend.pCorrespondBendLt.Values[0].ID)
        //                {
        //                    CCorrespondBend pCorrespondBend = new CCorrespondBend(pFromBendSlt.Values[i], optimumtobend);
        //                    pCorrespondBendLt.Add(pCorrespondBend);
        //                }
        //            }
        //        }
        //    }

        //    return pCorrespondBendLt;
        //}
        #endregion

        public List<CCorrespondBend> BendMatch(List<CCorrespondBend> pIndependCorrespondBendLt, CParameterThreshold ParameterThreshold)
        {
            List<CCorrespondBend> pCorrespondBendLt = new List<CCorrespondBend>();
            pCorrespondBendLt.AddRange(pIndependCorrespondBendLt);
            for (int i = 0; i < pIndependCorrespondBendLt.Count; i++)
            {
                RecursiveBendMatch(pIndependCorrespondBendLt[i].CFromBend, pIndependCorrespondBendLt[i].CToBend, ref pCorrespondBendLt, ParameterThreshold);
            }
            return pCorrespondBendLt;

        }

        private void RecursiveBendMatch(CBend pFromBend, CBend pToBend, ref List<CCorrespondBend> pCorrespondBendLt, CParameterThreshold ParameterThreshold)
        {
            if (pFromBend.CLeftBend == null || pToBend.CLeftBend == null)
            {
                return;
            }

            double dblRatioLL = pFromBend.CLeftBend.pBaseLine.Length / pToBend.CLeftBend.pBaseLine.Length;
            double dblRatioRR = pFromBend.CRightBend.pBaseLine.Length / pToBend.CRightBend.pBaseLine.Length;
            //double dblRatioAPAP = (pFromBend.CLeftBend.Length + pFromBend.CRightBend.Length) / (pToBend.CLeftBend.Length + pToBend.CRightBend.Length);
            //double dblRatioLA = pFromBend.CLeftBend.Length / pToBend.Length;
            //double dblRatioRA = pFromBend.CRightBend.Length / pToBend.Length;



            if ((dblRatioLL >= ParameterThreshold.dblDLengthBound) && (dblRatioLL <= ParameterThreshold.dblULengthBound) &&
                (dblRatioRR >= ParameterThreshold.dblDLengthBound) && (dblRatioRR <= ParameterThreshold.dblULengthBound))
            {
                //���������ֱ��Ӧ
                CCorrespondBend pLeftCorrespondBend = new CCorrespondBend(pFromBend.CLeftBend, pToBend.CLeftBend);
                CCorrespondBend pRightCorrespondBend = new CCorrespondBend(pFromBend.CRightBend, pToBend.CRightBend);
                pCorrespondBendLt.Add(pLeftCorrespondBend);
                pCorrespondBendLt.Add(pRightCorrespondBend);

                //�������±���
                RecursiveBendMatch(pFromBend.CLeftBend, pToBend.CLeftBend, ref pCorrespondBendLt, ParameterThreshold);
                RecursiveBendMatch(pFromBend.CRightBend, pToBend.CRightBend, ref pCorrespondBendLt, ParameterThreshold);
            }
            else  //�˲��ز����٣����û��Ǻ����Ե�
            {
                double dblRatioLR = pFromBend.CLeftBend.pBaseLine.Length / pToBend.CRightBend.pBaseLine.Length;
                double dblRatioRL = pFromBend.CRightBend.pBaseLine.Length / pToBend.CLeftBend.pBaseLine.Length;


                if ((dblRatioLL >= ParameterThreshold.dblDLengthBound) && (dblRatioLR >= ParameterThreshold.dblDLengthBound))
                {
                    RecursiveBendMatch(pFromBend.CLeftBend, pToBend, ref pCorrespondBendLt, ParameterThreshold);
                }
                if ((dblRatioRL >= ParameterThreshold.dblDLengthBound) && (dblRatioRR >= ParameterThreshold.dblDLengthBound))
                {
                    RecursiveBendMatch(pFromBend.CRightBend, pToBend, ref pCorrespondBendLt, ParameterThreshold);
                }
            }
        }



       

        /// <summary>���ԣ�������</summary>
        public CParameterInitialize ParameterInitialize
        {
            get { return _ParameterInitialize; }
            set { _ParameterInitialize = value; }
        }

        /// <summary>���ԣ�������</summary>
        public CParameterResult ParameterResult
        {
            get { return _ParameterResult; }
            set { _ParameterResult = value; }
        }
    }
}
