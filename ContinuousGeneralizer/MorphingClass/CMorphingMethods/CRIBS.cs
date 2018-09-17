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
    /// ���ʹ�ö��������ṹ(Recursive Independent Bend Structures)
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class CRIBS
    {
        
        
        private CTriangulator _Triangulator = new CTriangulator();
        private CParameterResult _ParameterResult;

        private List<CPolyline> _LSCPlLt = new List<CPolyline>();  //BS:LargerScale
        private List<CPolyline> _SSCPlLt = new List<CPolyline>();  //SS:SmallerScale

        private CPolyline _FromCpl;
        private CPolyline _ToCpl;

        private double _dblCDTNum = 1;   //���ڱ�����Ҫ��δ���CDT��Ϊ��ʹ�����ɵ�CDT���������������𣬹��ô˱���

        private CParameterInitialize _ParameterInitialize;

        public CRIBS()
        {

        }

        public CRIBS(CPolyline frcpl, CPolyline tocpl)
        {
            _FromCpl = frcpl;
            _ToCpl = tocpl;
        }

        public CRIBS(CParameterInitialize ParameterInitialize)
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
        public void RIBSMorphing()
        {
            //var ParameterInitialize = _ParameterInitialize;
            //CMPBBSL OptMPBBSL = new CMPBBSL();

            //CPolyline frcpl = _LSCPlLt[0];
            //CPolyline tocpl = _SSCPlLt[0];

            //_FromCpl=frcpl;
            //_ToCpl = tocpl;

            ////*******Ϊʲô������"OptMPBBSL.DWByMPBBSL" ?
            ////*******��Ϊ���������ƥ�䷽��(����������������)��һ�����������µ��ؽ�������������

            ////���㼫Сֵ
            //
            //long lngStartTime = System.Environment.TickCount;  //��ʼʱ��

            //List<CPoint> frchcptlt = _Triangulator.CreateConvexHullEdgeLt2(frcpl, dblVerySmall);
            //CPolyline frchcpl = new CPolyline(0, frchcptlt);    //��������������������߶�

            //List<CPoint> tochcptlt = _Triangulator.CreateConvexHullEdgeLt2(tocpl, dblVerySmall);
            //CPolyline tochcpl = new CPolyline(0, tochcptlt);    //С�������������������߶�

            ////���Լ����������ͼ�㣬�Ա�������AE�еĹ���(ct:constraint)
            //List<CPolyline> frctcpllt = new List<CPolyline>(); frctcpllt.Add(frcpl); frctcpllt.Add(frchcpl);
            //List<CPolyline> toctcpllt = new List<CPolyline>(); toctcpllt.Add(tocpl); toctcpllt.Add(tochcpl);
            //IFeatureLayer pBSFLayer = CHelpFunc.SaveCPlLt(frctcpllt, "frctcpllt" + _dblCDTNum, ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);
            //IFeatureLayer pSSFLayer = CHelpFunc.SaveCPlLt(toctcpllt, "toctcpllt" + _dblCDTNum, ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);

            ////����CDT����ȡ����ɭ��
            //CBendForest FromLeftBendForest = new CBendForest();
            //CBendForest FromRightBendForest = new CBendForest();
            //CParameterVariable pParameterVariableFrom = new CParameterVariable(frcpl, "FromCDT" + _dblCDTNum, pBSFLayer, dblVerySmall);
            //OptMPBBSL.GetBendForest(pParameterVariableFrom, ref FromLeftBendForest, ref FromRightBendForest, ParameterInitialize);

            //CBendForest ToLeftBendForest = new CBendForest();
            //CBendForest ToRightBendForest = new CBendForest();
            //CParameterVariable pParameterVariableTo = new CParameterVariable(tocpl, "ToCDT" + _dblCDTNum, pSSFLayer, dblVerySmall);
            //OptMPBBSL.GetBendForest(pParameterVariableTo, ref ToLeftBendForest, ref ToRightBendForest, ParameterInitialize);

            ////��������ɭ�֣������������
            //OptMPBBSL.NeatenBendForest(frcpl, FromLeftBendForest);
            //OptMPBBSL.NeatenBendForest(frcpl, FromRightBendForest);
            //OptMPBBSL.NeatenBendForest(tocpl, ToLeftBendForest);
            //OptMPBBSL.NeatenBendForest(tocpl, ToRightBendForest);

            //_dblCDTNum = _dblCDTNum + 1;


            ////������ֵ����
            //double dblBound = 0.99;
            //CParameterThreshold ParameterThreshold = new CParameterThreshold();
            //ParameterThreshold.dblAngleBound = Math.PI * (1 - dblBound);
            //ParameterThreshold.dblDLengthBound = dblBound;
            //ParameterThreshold.dblULengthBound = 1 / dblBound;
            //ParameterThreshold.dblVerySmall = dblVerySmall;

            ////������ƥ�䣬Ѱ�Ҷ�Ӧ��������
            //List<CCorrespondBend> IndependCorrespondBendLt = new List<CCorrespondBend>();
            //IndependCorrespondBendLt.AddRange(BendTreeMatch(FromLeftBendForest, ToLeftBendForest, ParameterThreshold, ParameterInitialize));
            //IndependCorrespondBendLt.AddRange(BendTreeMatch(FromRightBendForest, ToRightBendForest, ParameterThreshold, ParameterInitialize));

            ////����ƥ�䣬Ѱ�Ҷ�Ӧ����
            //List<CCorrespondBend> CorrespondBendLt = BendMatch(IndependCorrespondBendLt, ParameterThreshold, ParameterInitialize);

            ////��ȡ��Ӧ�߶�
            //C5.LinkedList<CCorrSegment> pCorrespondSegmentLk = CHelpFunc.DetectCorrespondSegment(frcpl, tocpl, CorrespondBendLt);

            ////��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��   
            //CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper(); 
            //List<CPoint> pResultPtLt = new List<CPoint>();
            //pResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(pCorrespondSegmentLk, "Linear");

            ////���㲢��ʾ����ʱ��
            //long lngEndTime = System.Environment.TickCount;
            //long lngTime = lngEndTime - lngStartTime;
            //_ParameterInitialize.tsslTime.Text = "Running Time: " + Convert.ToString(lngTime) + "ms";  //��ʾ����ʱ��

            ////�����Ӧ��
            //CHelpFunc.SaveCtrlLine(pCorrespondSegmentLk,"CRIBSControlLine" + dblBound, _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);
            //CHelpFunc.SaveCorrespondLine(pResultPtLt, "CRIBSCorrLine" + dblBound, _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);

            ////��ȡ�����ȫ����¼��_ParameterResult��
            //CParameterResult ParameterResult = new CParameterResult();
            //ParameterResult.FromCpl = frcpl;
            //ParameterResult.ToCpl = tocpl;
            //ParameterResult.pCorrespondSegmentLk = pCorrespondSegmentLk;
            //ParameterResult.CResultPtLt = pResultPtLt;
            //ParameterResult.lngTime = lngTime;
            //_ParameterResult = ParameterResult;
        }

        //����������Morphing����
        public C5.LinkedList<CCorrSegment> DWByRIBSMorphing(CParameterInitialize pParameterInitialize, CParameterThreshold ParameterThreshold)
        {
            //CParameterInitialize ParameterInitialize = pParameterInitialize;
            //CMPBBSL OptMPBBSL = new CMPBBSL();

            //CPolyline frcpl = _FromCpl;
            //CPolyline tocpl = _ToCpl;
            //double dblVerySmall = ParameterThreshold.dblVerySmall;


            ////*******Ϊʲô������"OptMPBBSL.DWByMPBBSL" ?
            ////*******��Ϊ���������ƥ�䷽��(����������������)��һ�����������µ��ؽ�������������

            //List<CPoint> frchcptlt = _Triangulator.CreateConvexHullEdgeLt2(frcpl, dblVerySmall);
            //CPolyline frchcpl = new CPolyline(0, frchcptlt);    //��������������������߶�

            //List<CPoint> tochcptlt = _Triangulator.CreateConvexHullEdgeLt2(tocpl, dblVerySmall);
            //CPolyline tochcpl = new CPolyline(0, tochcptlt);    //С�������������������߶�

            ////���Լ����������ͼ�㣬�Ա�������AE�еĹ���(ct:constraint)
            //List<CPolyline> frctcpllt = new List<CPolyline>(); frctcpllt.Add(frcpl); frctcpllt.Add(frchcpl);
            //List<CPolyline> toctcpllt = new List<CPolyline>(); toctcpllt.Add(tocpl); toctcpllt.Add(tochcpl);
            //IFeatureLayer pBSFLayer = CHelpFunc.SaveCPlLt(frctcpllt, "frctcpllt" + _dblCDTNum, ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);
            //IFeatureLayer pSSFLayer = CHelpFunc.SaveCPlLt(toctcpllt, "toctcpllt" + _dblCDTNum, ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);

            ////����CDT����ȡ����ɭ��
            //CBendForest FromLeftBendForest = new CBendForest();
            //CBendForest FromRightBendForest = new CBendForest();
            //CParameterVariable pParameterVariableFrom = new CParameterVariable(frcpl, "FromCDT" + _dblCDTNum, ParameterInitialize.pBSFLayer, dblVerySmall);
            //OptMPBBSL.GetBendForest(pParameterVariableFrom, ref FromLeftBendForest, ref FromRightBendForest, ParameterInitialize);

            //CBendForest ToLeftBendForest = new CBendForest();
            //CBendForest ToRightBendForest = new CBendForest();
            //CParameterVariable pParameterVariableTo = new CParameterVariable(tocpl, "ToCDT" + _dblCDTNum, ParameterInitialize.pSSFLayer, dblVerySmall);
            //OptMPBBSL.GetBendForest(pParameterVariableTo, ref ToLeftBendForest, ref ToRightBendForest, ParameterInitialize);

            //_dblCDTNum = _dblCDTNum + 1;



            ////������ƥ�䣬Ѱ�Ҷ�Ӧ��������
            //List<CCorrespondBend> IndependCorrespondBendLt = new List<CCorrespondBend>();
            //IndependCorrespondBendLt.AddRange(BendTreeMatch(FromLeftBendForest, ToLeftBendForest, ParameterThreshold, ParameterInitialize));
            //IndependCorrespondBendLt.AddRange(BendTreeMatch(FromRightBendForest, ToRightBendForest, ParameterThreshold, ParameterInitialize));

            ////����ƥ�䣬Ѱ�Ҷ�Ӧ����
            //List<CCorrespondBend> CorrespondBendLt = BendMatch(IndependCorrespondBendLt, ParameterThreshold, ParameterInitialize);

            ////��ȡ��Ӧ�߶�
            //C5.LinkedList<CCorrSegment> pComplexCorrespondSegmentLk = CHelpFunc.DetectCorrespondSegment(frcpl, tocpl, CorrespondBendLt);

            //return pComplexCorrespondSegmentLk;
            return null;
        }



        /// <summary>
        /// ��������ƥ�䣬��ȡ��Ӧ�߶�
        /// </summary>
        /// <param name="CFromBendForest">�����������ɭ��</param>
        /// <param name="CToBendForest">С����������ɭ��</param>
        /// <param name="ParameterThreshold">��������</param>
        /// <param name="CorrespondSegmentLk">��Ӧ�߶���</param>
        /// <remarks>������ƥ��ͺ�������ƥ�䶼�ɱ�������ִ��
        /// ע�⣺�˴���������ƥ�估����ƥ����"MPBBSL"��������ͬ���˴������˵ݹ�ʶ���������</remarks>
        public List<CCorrespondBend> BendTreeMatch(CBendForest CFromBendForest, CBendForest CToBendForest, CParameterThreshold ParameterThreshold,CParameterInitialize pParameterInitialize)
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
            List<CCorrespondBend> pIndependCorrespondBendLt = IndependBendMatch(pFromIndependBendSlt, pToIndependBendSlt, ParameterThreshold,pParameterInitialize);

            return pIndependCorrespondBendLt;
        }




        /// <summary>
        /// ����ƥ��
        /// </summary>
        /// <param name="pFromBendSlt">������������б�</param>
        /// <param name="pToBendSlt">С�����������б�</param>
        /// <param name="dblRatioBound">������ֵ</param>
        /// <returns>��Ӧ�����б�</returns>
        /// <remarks></remarks>
        private List<CCorrespondBend> IndependBendMatch(SortedDictionary<double, CBend> pFromBendSlt, SortedDictionary<double, CBend> pToBendSlt,
                                                        CParameterThreshold ParameterThreshold, CParameterInitialize pParameterInitialize)
        {
            //���������Ķ�Ӧ�����б�
            for (int i = 0; i < pFromBendSlt.Count; i++)
            {
                pFromBendSlt.ElementAt (i).Value.pCorrespondBendLt.Clear();
            }
            for (int i = 0; i < pToBendSlt.Count; i++)
            {
                pToBendSlt.ElementAt (i).Value.pCorrespondBendLt.Clear();
            }


            //��������Ҫ��Ķ�Ӧ����
            List<CCorrespondBend> pCorrespondBendLt = new List<CCorrespondBend>();
            int intLastMatchj = 0;   //��ֵ��������ȷҪ�󣬽�Ϊ�м�������Ĺ���ֵ
            for (int i = 0; i < pFromBendSlt.Values.Count; i++)
            {
                CBend pfrbend = pFromBendSlt.ElementAt (i).Value;
                double dblRatioLengthi = pfrbend.Length / ParameterThreshold.dblFrLength;

                //int dblTempMatchj = 0;
                //Ϊ�˽�ʡʱ�䣬���м�����������
                //��intLastMatchjΪ��׼ǰ������
                for (int j = intLastMatchj; j < pToBendSlt.Values.Count; j++)
                {
                    CBend ptobend = pToBendSlt.ElementAt (i).Value;
                    double dblRatioLengthj = ptobend.Length / ParameterThreshold.dblToLength;
                    double dblStartDiff = pfrbend.dblStartRL - ptobend.dblStartRL;
                    double dblEndDiff = pfrbend.dblEndRL - ptobend.dblEndRL;
                    double dblAngleDiff = pfrbend.pBaseLine.Angle - ptobend.pBaseLine.Angle;

                    //�������λ�ò���ֵ
                    double dblRatioBoundj;
                    if (dblRatioLengthi >= dblRatioLengthj)
                    {
                        dblRatioBoundj = 0.25 * dblRatioLengthi;
                    }
                    else
                    {
                        dblRatioBoundj = 0.25 * dblRatioLengthj;
                    }

                    if (dblStartDiff < (-dblRatioBoundj))
                    {
                        break; //����Ѿ�����һ����Χ����û��Ҫ�����������
                    }

                    double dblLengthRatio = pfrbend.pBaseLine.Length / ptobend.pBaseLine.Length;

                    if ((Math.Abs(dblStartDiff) <= dblRatioBoundj) && (Math.Abs(dblEndDiff) <= dblRatioBoundj) && (Math.Abs(dblAngleDiff) <= ParameterThreshold.dblAngleBound)
                        && (dblLengthRatio >= ParameterThreshold.dblDLengthBound) && (dblLengthRatio <= ParameterThreshold.dblULengthBound))
                    {
                        CCorrespondBend pCorrespondBend = new CCorrespondBend(pFromBendSlt.ElementAt(i).Value, pToBendSlt.ElementAt(i).Value);
                        pCorrespondBendLt.Add(pCorrespondBend);

                        //*******************************//
                        List<CCorrespondBend> pRIBSCorrespondBendLt = RBS(pCorrespondBend, ParameterThreshold, pParameterInitialize);  
                        pCorrespondBendLt.AddRange(pRIBSCorrespondBendLt);

                        intLastMatchj = j;
                    }
                }
            }

            return pCorrespondBendLt;
        }

        public List<CCorrespondBend> BendMatch(List<CCorrespondBend> pIndependCorrespondBendLt, CParameterThreshold ParameterThreshold, CParameterInitialize pParameterInitialize)
        {
            List<CCorrespondBend> pCorrespondBendLt = new List<CCorrespondBend>();
            pCorrespondBendLt.AddRange(pIndependCorrespondBendLt);
            for (int i = 0; i < pIndependCorrespondBendLt.Count; i++)
            {
                RecursiveBendMatch(pIndependCorrespondBendLt[i].CFromBend, pIndependCorrespondBendLt[i].CToBend, ref pCorrespondBendLt, ParameterThreshold, pParameterInitialize);
            }
            return pCorrespondBendLt;

        }

        private void RecursiveBendMatch(CBend pFromBend, CBend pToBend, ref List<CCorrespondBend> pCorrespondBendLt, 
                                        CParameterThreshold ParameterThreshold, CParameterInitialize pParameterInitialize)
        {
            if (pFromBend.CLeftBend == null || pToBend.CLeftBend == null)
            {
                return;
            }

            double dblRatioLL = pFromBend.CLeftBend.pBaseLine.Length / pToBend.CLeftBend.pBaseLine.Length;
            double dblRatioRR = pFromBend.CRightBend.pBaseLine.Length / pToBend.CRightBend.pBaseLine.Length;


            MessageBox.Show("CRIBS: need to be improved!");
            double dblAngleDiffLL = pFromBend.CLeftBend.pBaseLine.Angle - pToBend.CLeftBend.pBaseLine.Angle;
            double dblAngleDiffRR = pFromBend.CRightBend.pBaseLine.Angle - pToBend.CRightBend.pBaseLine.Angle;

            if ((dblRatioLL >= ParameterThreshold.dblDLengthBound) && (dblRatioLL <= ParameterThreshold.dblULengthBound) &&
                (dblRatioRR >= ParameterThreshold.dblDLengthBound) && (dblRatioRR <= ParameterThreshold.dblULengthBound) &&
                (Math.Abs(dblAngleDiffLL) <= ParameterThreshold.dblAngleBound) && (dblAngleDiffRR <= ParameterThreshold.dblULengthBound))
            {
                //���������ֱ��Ӧ
                CCorrespondBend pLeftCorrespondBend = new CCorrespondBend(pFromBend.CLeftBend, pToBend.CLeftBend);
                CCorrespondBend pRightCorrespondBend = new CCorrespondBend(pFromBend.CRightBend, pToBend.CRightBend);
                pCorrespondBendLt.Add(pLeftCorrespondBend);
                pCorrespondBendLt.Add(pRightCorrespondBend);

                //*******************************//
                List<CCorrespondBend> pRIBSLeftCorrespondBendLt = RBS(pLeftCorrespondBend, ParameterThreshold, pParameterInitialize);
                pCorrespondBendLt.AddRange(pRIBSLeftCorrespondBendLt);
                List<CCorrespondBend> pRIBSRightCorrespondBendLt = RBS(pRightCorrespondBend, ParameterThreshold, pParameterInitialize);
                pCorrespondBendLt.AddRange(pRIBSRightCorrespondBendLt);

                //�������±���
                RecursiveBendMatch(pFromBend.CLeftBend, pToBend.CLeftBend, ref pCorrespondBendLt, ParameterThreshold, pParameterInitialize);
                RecursiveBendMatch(pFromBend.CRightBend, pToBend.CRightBend, ref pCorrespondBendLt, ParameterThreshold, pParameterInitialize);
            }
            else  //�˲��ز����٣����û��Ǻ����Ե�
            {
                double dblRatioLR = pFromBend.CLeftBend.pBaseLine.Length / pToBend.CRightBend.pBaseLine.Length;
                double dblRatioRL = pFromBend.CRightBend.pBaseLine.Length / pToBend.CLeftBend.pBaseLine.Length;


                if ((dblRatioLL >= ParameterThreshold.dblDLengthBound) && (dblRatioLR >= ParameterThreshold.dblDLengthBound))
                {
                    RecursiveBendMatch(pFromBend.CLeftBend, pToBend, ref pCorrespondBendLt, ParameterThreshold, pParameterInitialize);
                }
                if ((dblRatioRL >= ParameterThreshold.dblDLengthBound) && (dblRatioRR >= ParameterThreshold.dblDLengthBound))
                {
                    RecursiveBendMatch(pFromBend.CRightBend, pToBend, ref pCorrespondBendLt, ParameterThreshold, pParameterInitialize);
                }
            }
        }

        /// <summary>
        /// �ݹ���������ṹ
        /// </summary>
        /// <param name="CCorrespondBend">��Ӧ����</param>
        /// <param name="ParameterThreshold">��ֵ����</param>
        /// <param name="pRightBendForest">�����ұߵ�����ɭ��</param>
        /// <param name="strName">����������������</param>
        /// <remarks></remarks>
        public List<CCorrespondBend> RBS(CCorrespondBend pCorrespondBend, CParameterThreshold ParameterThreshold, CParameterInitialize pParameterInitialize)
        {
            string strSide = pCorrespondBend.CFromBend.strSide;
            CPolyline subfrcpl = new CPolyline(0,pCorrespondBend.CFromBend.CptLt);
            CPolyline subtocpl = new CPolyline(0,pCorrespondBend.CToBend.CptLt);

            List<CPoint> subfrchcptlt = _Triangulator.CreateConvexHullEdgeLt2(subfrcpl, CConstants.dblVerySmallCoord);
            CPolyline subfrchcpl = new CPolyline(0, subfrchcptlt);    //��������������������߶�

            List<CPoint> subtochcptlt = _Triangulator.CreateConvexHullEdgeLt2(subtocpl, CConstants.dblVerySmallCoord);
            CPolyline subtochcpl = new CPolyline(0, subtochcptlt);    //С�������������������߶�

            //�����������ͼ�㣬�Ա�������AE�еĹ���
            List<CPolyline> subfrcpllt = new List<CPolyline>(); subfrcpllt.Add(subfrcpl); subfrcpllt.Add(subfrchcpl);
            List<CPolyline> subtocpllt = new List<CPolyline>(); subtocpllt.Add(subtocpl); subtocpllt.Add(subtochcpl);
            IFeatureLayer pBSFLayer = CHelpFunc.SaveCPlLt(subfrcpllt, "subfrcpl" + _dblCDTNum, pParameterInitialize.pWorkspace, pParameterInitialize.m_mapControl);
            IFeatureLayer pSSFLayer = CHelpFunc.SaveCPlLt(subtocpllt, "subtocpl" + _dblCDTNum, pParameterInitialize.pWorkspace, pParameterInitialize.m_mapControl);

            CParameterVariable pParameterVariableFrom = new CParameterVariable(subfrcpl, "subFromCDT" + _dblCDTNum, pBSFLayer, CConstants.dblVerySmallCoord);
            CParameterVariable pParameterVariableTo = new CParameterVariable(subtocpl, "subToCDT" + _dblCDTNum, pSSFLayer, CConstants.dblVerySmallCoord);

            _dblCDTNum = _dblCDTNum + 1;

            CMPBBSL OptMPBBSL = new CMPBBSL();

            //����CDT����ȡ����ɭ��
            CBendForest FromBendForest = new CBendForest();
            GetSideBendForest(pParameterVariableFrom, ref FromBendForest, pParameterInitialize, strSide);

            CBendForest ToBendForest = new CBendForest();
            GetSideBendForest(pParameterVariableTo, ref ToBendForest, pParameterInitialize, strSide);            

            //������ƥ�䣬Ѱ�Ҷ�Ӧ��������
            List<CCorrespondBend> IndependCorrespondBendLt = BendTreeMatch(FromBendForest, ToBendForest, ParameterThreshold, pParameterInitialize);

            //����ƥ�䣬Ѱ�Ҷ�Ӧ����
            List<CCorrespondBend> CorrespondBendLt = BendMatch(IndependCorrespondBendLt, ParameterThreshold, pParameterInitialize);

            return CorrespondBendLt;
        }

        /// <summary>
        /// ��ȡ����ĳһ�ߵ�����ɭ��
        /// </summary>
        /// <param name="cpl">����</param>
        /// <param name="pParameterVariable">����������������״Ҫ�أ��轨��CDT��ͼ�㣬CDT��������ļ�����</param>
        /// <param name="pBendForest">��������һ�ߵ�����ɭ��</param>
        /// <param name="pParameterInitialize">����</param>
        /// <param name="strForeSide">��������״Ҫ�صĲ��(�Ҳ�����)���˴�����������һ�������ɭ��</param>
        /// <remarks>ע�⣺�������е�CreateCDT����ı�pParameterVariable�е�CPolyline</remarks>
        public void GetSideBendForest(CParameterVariable pParameterVariable, ref CBendForest pBendForest, CParameterInitialize pParameterInitialize, string strForeSide)
        {
            CTriangulator OptCDT = new CTriangulator();
            CPolyline newcpl = new CPolyline(pParameterVariable.CPolyline.ID, pParameterVariable.CPolyline.CptLt);
            List<CTriangle> CDTLt = OptCDT.CreateCDT(pParameterVariable.pFeatureLayer, ref newcpl, pParameterVariable.dblVerySmall);
            pParameterVariable.CPolyline = newcpl;

            //for (int i = 0; i < CDTLt.Count; i++) CDTLt[i].TID = i;  //����Ϊֹ��Լ�������ν�����ɣ��������β��ٷ����仯�����������α��           
            OptCDT.GetSETriangle(ref CDTLt, pParameterVariable.dblVerySmall);  //ȷ������������
            OptCDT.ConfirmTriangleSide(ref CDTLt, pParameterVariable.CPolyline, pParameterVariable.dblVerySmall); //ȷ����������λ�����ߵ����ұ�
            OptCDT.SignTriTypeAll(ref CDTLt);   //���I��II��III��VI��������

            if (strForeSide == "Left")
            {
                pBendForest = OptCDT.BuildBendForestNeed2(ref CDTLt, pParameterVariable.CPolyline.CptLt, "Right", pParameterVariable.dblVerySmall);
            }
            else if (strForeSide == "Right")
            {
                pBendForest = OptCDT.BuildBendForestNeed2(ref CDTLt, pParameterVariable.CPolyline.CptLt, "Left",pParameterVariable.dblVerySmall);
            }
            else
            {
                MessageBox.Show("GetSideBendForest�������⣡");
            }

            //����������
            //List<CTriangle> CTriangleLt = new List<CTriangle>();
            //for (int i = 0; i < CDTLt.Count; i++)
            //{
            //    if (CDTLt[i].strTriType != "I")
            //    {
            //        CTriangleLt.Add(CDTLt[i]);
            //    }
            //}
            //CHelpFunc.SaveTriangles(CTriangleLt, pParameterVariable.strName, pParameterInitialize.pWorkspace, pParameterInitialize.m_mapControl);
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
