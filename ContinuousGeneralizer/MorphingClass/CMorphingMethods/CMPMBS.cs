using System;
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
    /// ���������ṹ����״Ҫ��Morphing�任�������Գ���Ϊƥ�����ݣ�Length��
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class CMPMBS
    {
        
        
        private CParameterResult _ParameterResult;

        private List<CPolyline> _LSCPlLt = new List<CPolyline>();  //BS:LargerScale
        private List<CPolyline> _SSCPlLt = new List<CPolyline>();  //SS:SmallerScale

        private CPolyline _FromCpl;
        private CPolyline _ToCpl;

        private CParameterInitialize _ParameterInitialize;

        public CMPMBS()
        {

        }

        public CMPMBS(CPolyline frcpl, CPolyline tocpl)
        {
            _FromCpl = frcpl;
            _ToCpl = tocpl;
        }

        public CMPMBS(CParameterInitialize ParameterInitialize)
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

        //����������Morphing������ɾ��С����������
        public void MPBBSLMorphing()
        {
            //var ParameterInitialize = _ParameterInitialize;
            //CPolyline frcpl = _LSCPlLt[0];
            //CPolyline tocpl = _SSCPlLt[0];

            ////���㼫Сֵ
            //CGeoFunc.CalDistanceParameters(_LSCPlLt, _SSCPlLt);

            //long lngStartTime = System.Environment.TickCount;  //��ʼʱ��

            //CMPBBSL OptMPBBSL = new CMPBBSL();
            
            ////����CDT����ȡ����ɭ��
            //CBendForest FromLeftBendForest = new CBendForest();
            //CBendForest FromRightBendForest = new CBendForest();
            //CParameterVariable pParameterVariableFrom = new CParameterVariable(frcpl, "FromCDT", ParameterInitialize.pBSFLayer, dblVerySmall);
            //OptMPBBSL.GetBendForest(pParameterVariableFrom, ref FromLeftBendForest, ref FromRightBendForest, ParameterInitialize);

            //CBendForest ToLeftBendForest = new CBendForest();
            //CBendForest ToRightBendForest = new CBendForest();
            //CParameterVariable pParameterVariableTo = new CParameterVariable(tocpl, "ToCDT", ParameterInitialize.pSSFLayer, dblVerySmall);
            //OptMPBBSL.GetBendForest(pParameterVariableTo, ref ToLeftBendForest, ref ToRightBendForest, ParameterInitialize);

            ////��������ɭ�֣������������
            //SortedList<double, CBend> FromLeftBendSlt = OptMPBBSL.NeatenBendForest(frcpl, FromLeftBendForest);
            //SortedList<double, CBend> FromRightBendSlt = OptMPBBSL.NeatenBendForest(frcpl, FromRightBendForest);
            //SortedList<double, CBend> ToLeftBendSlt = OptMPBBSL.NeatenBendForest(tocpl, ToLeftBendForest);
            //SortedList<double, CBend> ToRightBendSlt = OptMPBBSL.NeatenBendForest(tocpl, ToRightBendForest);

            //////���������ֵ
            ////double dblRatioBound = CalRatioBound(frcpl);

            //CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper();
            //CTranslation pTranslation = new CTranslation();

            //////������ֵ����
            ////CParameterThreshold ParameterThreshold = new CParameterThreshold();
            ////ParameterThreshold.dblFrLength = frcpl.pPolyline.Length;
            ////ParameterThreshold.dblToLength = tocpl.pPolyline.Length;

            ////List<double> dblTranslationLt = new List<double>();
            ////SortedList<double, int> ResultsSlt = new SortedList<double, int>(new CCmpDbl());

            ////������ֵ����
            //double dblBound = 0.98;
            //CParameterThreshold ParameterThreshold = new CParameterThreshold();
            //ParameterThreshold.dblFrLength = frcpl.pPolyline.Length;
            //ParameterThreshold.dblToLength = tocpl.pPolyline.Length;
            //ParameterThreshold.dblDLengthBound = dblBound;
            //ParameterThreshold.dblULengthBound = 1 / dblBound;

            ////������ƥ�䣬Ѱ�Ҷ�Ӧ��������
            //List<CCorrespondBend> LeftIndependCorrespondBendLt = new List<CCorrespondBend>();
            //LeftIndependCorrespondBendLt = OptMPBBSL.BendTreeMatch(FromLeftBendForest, ToLeftBendForest, ParameterThreshold);

            //List<CCorrespondBend> RightIndependCorrespondBendLt = new List<CCorrespondBend>();
            //RightIndependCorrespondBendLt = OptMPBBSL.BendTreeMatch(FromRightBendForest, ToRightBendForest, ParameterThreshold);












            //for (int i = 0; i <= 25; i++)
            //{
            //    ParameterThreshold.dblDLengthBound = 1 * (1 - 0.02 * i);
            //    ParameterThreshold.dblULengthBound = 1 / (1 - 0.02 * i);


            //    //������ƥ�䣬Ѱ�Ҷ�Ӧ��������
            //    List<CCorrespondBend> IndependCorrespondBendLt = new List<CCorrespondBend>();
            //    IndependCorrespondBendLt.AddRange(BendTreeMatch(FromLeftBendForest, ToLeftBendForest, ParameterThreshold));
            //    IndependCorrespondBendLt.AddRange(BendTreeMatch(FromRightBendForest, ToRightBendForest, ParameterThreshold));

            //    //����ƥ�䣬Ѱ�Ҷ�Ӧ����
            //    List<CCorrespondBend> CorrespondBendLt = BendMatch(IndependCorrespondBendLt, ParameterThreshold);

            //    //��ȡ��Ӧ�߶�
            //    C5.LinkedList<CCorrSegment> CorrespondSegmentLk = DetectCorrespondSegment(frcpl, tocpl, CorrespondBendLt);

            //    //��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��                
            //    List<CPoint> ResultPtLt = new List<CPoint>();
            //    ResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(CorrespondSegmentLk, "Linear");

            //    double dblTranslation = pTranslation.CalTranslation(ResultPtLt);
            //    dblTranslationLt.Add(dblTranslation);

            //    ResultsSlt.Add(dblTranslation, i);
            //}


            ////����������һ�飡����������
            ////���ɣ��������SortedList<double, CParameterResult> ResultsSlt = new SortedList<double, CParameterResult>(new CCmpDbl())��¼�����
            ////      �����ڻ�����λ��CPoint������ȻӰ��CParameterResult�е�ResultPtLtֵ
            //int intIndex = ResultsSlt.Values[0];
            //ParameterThreshold.dblDLengthBound = 1 * (1 - 0.02 * intIndex);
            //ParameterThreshold.dblULengthBound = 1 / (1 - 0.02 * intIndex);


            ////������ƥ�䣬Ѱ�Ҷ�Ӧ��������
            //List<CCorrespondBend> pIndependCorrespondBendLt = new List<CCorrespondBend>();
            //pIndependCorrespondBendLt.AddRange(BendTreeMatch(FromLeftBendForest, ToLeftBendForest, ParameterThreshold));
            //pIndependCorrespondBendLt.AddRange(BendTreeMatch(FromRightBendForest, ToRightBendForest, ParameterThreshold));

            ////����ƥ�䣬Ѱ�Ҷ�Ӧ����
            //List<CCorrespondBend> pCorrespondBendLt = BendMatch(pIndependCorrespondBendLt, ParameterThreshold);

            ////��ȡ��Ӧ�߶�
            //C5.LinkedList<CCorrSegment> pCorrespondSegmentLk = DetectCorrespondSegment(frcpl, tocpl, pCorrespondBendLt);

            ////��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��                
            //List<CPoint> pResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(pCorrespondSegmentLk, "Linear");

            ////���㲢��ʾ����ʱ��
            //long lngEndTime = System.Environment.TickCount;
            //long lngTime = lngEndTime - lngStartTime;
            //ParameterInitialize.tsslTime.Text = "Running Time: " + Convert.ToString(lngTime) + "ms";  //��ʾ����ʱ��

            ////����ָ��ֵ����Ӧ��            
            //CHelpFuncExcel.ExportDataltToExcel(dblTranslationLt, "translationlt0", _ParameterInitialize.strSavePath);
            //CHelpFunc.SaveCtrlLine(pCorrespondSegmentLk, "MPBBSLControlLine", ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);
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

       




































        

        public List<CCorrespondBend> MBendMatch(List<CCorrespondBend> pIndependCorrespondBendLt,CParameterThreshold ParameterThreshold, string strSide)
        {
            //List<CCorrespondBend> pCorrespondBendLt = new List<CCorrespondBend>();
            //pCorrespondBendLt.AddRange(pIndependCorrespondBendLt);
            //for (int i = 0; i < pIndependCorrespondBendLt.Count; i++)
            //{
            //    RecursiveBendMatch(pIndependCorrespondBendLt[i].CFromBend, pIndependCorrespondBendLt[i].CToBend, ref pCorrespondBendLt, ParameterThreshold);
            //}
            //return pCorrespondBendLt;
            return null;

        }

        private void RecursiveMBendMatch(CBend pFromBend, CBend pToBend, ref List<CCorrespondBend> pCorrespondBendLt, CParameterThreshold ParameterThreshold)
        {





            ////����CDT����ȡ����ɭ��
            //CBendForest FromLeftBendForest = new CBendForest();
            //CBendForest FromRightBendForest = new CBendForest();
            //CParameterVariable pParameterVariableFrom = new CParameterVariable(frcpl, "FromCDT", ParameterInitialize.pBSFLayer, dblVerySmall);
            //OptMPBBSL.GetBendForest(pParameterVariableFrom, ref FromLeftBendForest, ref FromRightBendForest, ParameterInitialize);

            //CBendForest ToLeftBendForest = new CBendForest();
            //CBendForest ToRightBendForest = new CBendForest();
            //CParameterVariable pParameterVariableTo = new CParameterVariable(tocpl, "ToCDT", ParameterInitialize.pSSFLayer, dblVerySmall);
            //OptMPBBSL.GetBendForest(pParameterVariableTo, ref ToLeftBendForest, ref ToRightBendForest, ParameterInitialize);

            ////��������ɭ�֣������������
            //SortedList<double, CBend> FromLeftBendSlt = OptMPBBSL.NeatenBendForest(frcpl, FromLeftBendForest);
            //SortedList<double, CBend> FromRightBendSlt = OptMPBBSL.NeatenBendForest(frcpl, FromRightBendForest);
            //SortedList<double, CBend> ToLeftBendSlt = OptMPBBSL.NeatenBendForest(tocpl, ToLeftBendForest);
            //SortedList<double, CBend> ToRightBendSlt = OptMPBBSL.NeatenBendForest(tocpl, ToRightBendForest);















            //if (pFromBend.CLeftBend == null || pToBend.CLeftBend == null)
            //{
            //    return;
            //}

            //double dblRatioLL = pFromBend.CLeftBend.pBaseLine.Length / pToBend.CLeftBend.pBaseLine.Length;
            //double dblRatioRR = pFromBend.CRightBend.pBaseLine.Length / pToBend.CRightBend.pBaseLine.Length;



            //if ((dblRatioLL >= ParameterThreshold.dblDLengthBound) && (dblRatioLL <= ParameterThreshold.dblULengthBound) &&
            //    (dblRatioRR >= ParameterThreshold.dblDLengthBound) && (dblRatioRR <= ParameterThreshold.dblULengthBound))
            //{
            //    //���������ֱ��Ӧ
            //    CCorrespondBend pLeftCorrespondBend = new CCorrespondBend(pFromBend.CLeftBend, pToBend.CLeftBend);
            //    CCorrespondBend pRightCorrespondBend = new CCorrespondBend(pFromBend.CRightBend, pToBend.CRightBend);
            //    pCorrespondBendLt.Add(pLeftCorrespondBend);
            //    pCorrespondBendLt.Add(pRightCorrespondBend);

            //    //�������±���
            //    RecursiveBendMatch(pFromBend.CLeftBend, pToBend.CLeftBend, ref pCorrespondBendLt, ParameterThreshold);
            //    RecursiveBendMatch(pFromBend.CRightBend, pToBend.CRightBend, ref pCorrespondBendLt, ParameterThreshold);
            //}           
        }

        /// <summary>
        /// ��ȡ���ߵ�����ɭ��
        /// </summary>
        /// <param name="cpl">����</param>
        /// <param name="pLeftBendForest">������ߵ�����ɭ��</param>
        /// <param name="pRightBendForest">�����ұߵ�����ɭ��</param>
        /// <param name="strName">����������������</param>
        /// <remarks>����Լ��������ʱ����δʹ������������ΪԼ����</remarks>
        public void MGetBendForest(CParameterVariable pParameterVariable, ref CBendForest pBendForest, string strSide, CParameterInitialize pParameterInitialize)
        {
            //List<CPoint> cptlt = pParameterVariable.CPolyline.CptLt;
            //List<CEdge> CEdgeLt = new List<CEdge>();
            //for (int i = 0; i < cptlt.Count - 1; i++)
            //{
            //    CEdge pEdge = new CEdge(cptlt[i], cptlt[i + 1]);
            //    CEdgeLt.Add(pEdge);
            //}

            //CTriangulator OptCDT = new CTriangulator();
            //List<CTriangle> CDTLt = OptCDT.CreateCDT(pParameterVariable);

            ////for (int i = 0; i < CDTLt.Count; i++) CDTLt[i].TID = i;  //����Ϊֹ��Լ�������ν�����ɣ��������β��ٷ����仯�����������α��           
            //OptCDT.GetSETriangle(ref CDTLt);  //ȷ������������
            //OptCDT.ConfirmTriangleSide(ref CDTLt, CEdgeLt); //ȷ����������λ�����ߵ����ұ�
            //OptCDT.SignTriTypeAll(ref CDTLt);   //���I��II��III��VI��������

            //pLeftBendForest = OptCDT.BuildBendForestNeed2(ref CDTLt, cptlt, "Left");
            //pRightBendForest = OptCDT.BuildBendForestNeed2(ref CDTLt, cptlt, "Right");

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
