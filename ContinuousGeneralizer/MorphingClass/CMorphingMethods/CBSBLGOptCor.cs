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
    /// �ݹ���������ṹ��BLG���ṹ�ķ���
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class CBSBLGOptCor
    {
        
        
        private CParameterResult _ParameterResult;

        private List<CPolyline> _LSCPlLt = new List<CPolyline>();  //BS:LargerScale
        private List<CPolyline> _SSCPlLt = new List<CPolyline>();  //SS:SmallerScale

        private CPolyline _FromCpl;
        private CPolyline _ToCpl;

        private CParameterInitialize _ParameterInitialize;

        public CBSBLGOptCor()
        {

        }

        public CBSBLGOptCor(CPolyline frcpl, CPolyline tocpl)
        {
            _FromCpl = frcpl;
            _ToCpl = tocpl;
        }

        public CBSBLGOptCor(CParameterInitialize ParameterInitialize)
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
            _LSCPlLt = CHelpFunc.GetCPlLtByFeatureLayer(pBSFLayer);
            _SSCPlLt = CHelpFunc.GetCPlLtByFeatureLayer(pSSFLayer);
        }

        //����������Morphing����
        public void BSBLGOptCorMorphing()
        {
            //var ParameterInitialize = _ParameterInitialize;
            //CPolyline frcpl = _LSCPlLt[0];
            //CPolyline tocpl = _SSCPlLt[0];


            ////���㼫Сֵ
            //
            //long lngStartTime = System.Environment.TickCount;  //��ʼʱ��
            //CHelpFunc.PreviousWorkCpl(ref frcpl, CEnumScale.Larger);
            //CHelpFunc.PreviousWorkCpl(ref frcpl, CEnumScale.Smaller);

            //////����CDT����ȡ����ɭ��
            ////CBendForest FromLeftBendForest = new CBendForest();
            ////CBendForest FromRightBendForest = new CBendForest();
            ////CParameterVariable pParameterVariableFrom = new CParameterVariable(frcpl, "FromCDT", ParameterInitialize.pBSFLayer, dblVerySmall);
            ////OptMPBBSL.GetBendForest(pParameterVariableFrom, ref FromLeftBendForest, ref FromRightBendForest, ParameterInitialize);//��ȡ���ߵ�����ɭ��

            ////CBendForest ToLeftBendForest = new CBendForest();
            ////CBendForest ToRightBendForest = new CBendForest();
            ////CParameterVariable pParameterVariableTo = new CParameterVariable(tocpl, "ToCDT", ParameterInitialize.pSSFLayer, dblVerySmall);
            ////OptMPBBSL.GetBendForest(pParameterVariableTo, ref ToLeftBendForest, ref ToRightBendForest, ParameterInitialize);

            //////��������ɭ�֣������������
            ////SortedList<double, CBend> FromLeftBendSlt = OptMPBBSL.NeatenBendForest(frcpl, FromLeftBendForest);//�������������е�����
            ////SortedList<double, CBend> FromRightBendSlt = OptMPBBSL.NeatenBendForest(frcpl, FromRightBendForest);
            ////SortedList<double, CBend> ToLeftBendSlt = OptMPBBSL.NeatenBendForest(tocpl, ToLeftBendForest);
            ////SortedList<double, CBend> ToRightBendSlt = OptMPBBSL.NeatenBendForest(tocpl, ToRightBendForest);


            //////������ֵ����
            ////CParameterThreshold ParameterThreshold = new CParameterThreshold();
            ////ParameterThreshold.dblFrLength = frcpl.pPolyline.Length;
            ////ParameterThreshold.dblToLength = tocpl.pPolyline.Length;
            ////ParameterThreshold.dblDLengthBound = 1 * 0.98;
            ////ParameterThreshold.dblULengthBound = 1 / 0.98;

            //////**************�����������**************//
            //////������ƥ�䣬Ѱ�Ҷ�Ӧ��������
            ////List<CCorrespondBend> pIndependCorrespondBendLt = new List<CCorrespondBend>();
            ////pIndependCorrespondBendLt.AddRange(OptMPBBSL.BendTreeMatch(FromLeftBendForest, ToLeftBendForest, ParameterThreshold));
            ////pIndependCorrespondBendLt.AddRange(OptMPBBSL.BendTreeMatch(FromRightBendForest, ToRightBendForest, ParameterThreshold));

            //////����ƥ�䣬Ѱ�Ҷ�Ӧ����
            ////List<CCorrespondBend> pCorrespondBendLt = OptMPBBSL.BendMatch(pIndependCorrespondBendLt, ParameterThreshold);

            ////��ȡ��Ӧ�߶�
            ////C5.LinkedList<CCorrSegment> pBSCorrespondSegmentLk = OptMPBBSL.DetectCorrespondSegment(frcpl, tocpl, pCorrespondBendLt);


            ////������ֵ����
            //double dblBound = 0.98;
            //CParameterThreshold ParameterThreshold = new CParameterThreshold();
            //ParameterThreshold.dblFrLength = frcpl.pPolyline.Length;
            //ParameterThreshold.dblToLength = tocpl.pPolyline.Length;
            //ParameterThreshold.dblDLengthBound = dblBound;
            //ParameterThreshold.dblULengthBound = 1 / dblBound;

            ////**************�����������**************//
            //CMPBBSL OptMPBBSL = new CMPBBSL();
            //CParameterVariable pParameterVariableFrom = new CParameterVariable(frcpl, "FromCDT", null, dblVerySmall);
            //CParameterVariable pParameterVariableTo = new CParameterVariable(tocpl, "ToCDT", null, dblVerySmall);
            //C5.LinkedList<CCorrSegment> pBSCorrespondSegmentLk = new C5.LinkedList<CCorrSegment>();
            //pBSCorrespondSegmentLk.AddRange(OptMPBBSL.DWByMPBBSL(ParameterInitialize, pParameterVariableFrom, pParameterVariableTo, ParameterThreshold));

            ////**************BLG���������**************//
            ////����BLG�����������ն�Ӧ�߶�
            //CMPBDPBL OptMPBDPBL = new CMPBDPBL();//����
            //C5.LinkedList<CCorrSegment> pBLGCorrespondSegmentLk = new C5.LinkedList<CCorrSegment>();
            //for (int j = 0; j < pBSCorrespondSegmentLk.Count; j++)
            //{
            //    pBLGCorrespondSegmentLk.AddRange(OptMPBDPBL.DWByDPLADefine(pBSCorrespondSegmentLk[j].CFrPolyline, pBSCorrespondSegmentLk[j].CToPolyline, ParameterThreshold));
            //}

            ////**************OptCor�������**************//
            //COptCor OptOptCor = new COptCor();
            //C5.LinkedList<CCorrSegment> pOptCorCorrespondSegmentLk = new C5.LinkedList<CCorrSegment>();
            //for (int j = 0; j < pBLGCorrespondSegmentLk.Count; j++)
            //{
            //    List<CPolyline> CFrEdgeLt = CGeoFunc.CreateCplLt(pOptCorCorrespondSegmentLk[j].CFrPolyline.CptLt);
            //    List<CPolyline> CToEdgeLt = CGeoFunc.CreateCplLt(pOptCorCorrespondSegmentLk[j].CToPolyline.CptLt);

            //    pOptCorCorrespondSegmentLk.AddRange(OptOptCor.DWByOptCor(frcpl, tocpl, CFrEdgeLt, CToEdgeLt, ParameterInitialize.intMaxBackK));
            //}

            //////**************OptCor�������**************//
            ////COptCorSimplified OptCorSimplified = new COptCorSimplified();
            ////C5.LinkedList<CCorrSegment> pOptCorCorrespondSegmentLk = new C5.LinkedList<CCorrSegment>();
            ////double dblX = tocpl.CptLt[0].X - frcpl.CptLt[0].X;
            ////double dblY = tocpl.CptLt[0].Y - frcpl.CptLt[0].Y;
            ////CPoint StandardVectorCpt = new CPoint(0, dblX, dblY);
            ////for (int j = 0; j < pBLGCorrespondSegmentLk.Count; j++)
            ////{
            ////    pOptCorCorrespondSegmentLk.AddRange(OptCorSimplified.DWByOptCorSimplified(pBLGCorrespondSegmentLk[j].CFrPolyline, pBLGCorrespondSegmentLk[j].CToPolyline, ParameterInitialize.intMaxBackK, StandardVectorCpt));
            ////}

            ////��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��
            //CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper();//���õ��ö���ĺ���
            //List<CPoint> pResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(pOptCorCorrespondSegmentLk, "Linear");

            ////�����Ӧ��
            //CHelpFunc.SaveCtrlLine(pOptCorCorrespondSegmentLk, "BSBLGOptCorControlLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);
            //CHelpFunc.SaveCorrespondLine(pResultPtLt, "BSBLGOptCorCorrLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);

            ////���㲢��ʾ����ʱ��
            //long lngEndTime = System.Environment.TickCount;
            //long lngTime = lngEndTime - lngStartTime;
            //_ParameterInitialize.tsslTime.Text = "Running Time: " + Convert.ToString(lngTime) + "ms";  //��ʾ����ʱ��

            ////��ȡ�����ȫ����¼��ParameterResult��
            //CTranslation pTranslation = new CTranslation();
            //CParameterResult ParameterResult = new CParameterResult();
            //ParameterResult.FromCpl = frcpl;
            //ParameterResult.ToCpl = tocpl;
            //double pdblTranslation = pTranslation.CalTranslation(pResultPtLt);
            //ParameterResult.dblTranslation = pdblTranslation;
            //ParameterResult.pCorrespondSegmentLk = pOptCorCorrespondSegmentLk;
            //ParameterResult.CResultPtLt = pResultPtLt;
            //ParameterResult.lngTime = lngTime;
            //_ParameterResult = ParameterResult;
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
