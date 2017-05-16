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
    public class CBSBLGOptCorMM
    {
        
        
        private CParameterResult _ParameterResult;

        private List<CPolyline> _LSCPlLt = new List<CPolyline>();  //BS:LargerScale
        private List<CPolyline> _SSCPlLt = new List<CPolyline>();  //SS:SmallerScale

        private CPolyline _FromCpl;
        private CPolyline _ToCpl;

        private CParameterInitialize _ParameterInitialize;

        public CBSBLGOptCorMM()
        {

        }

        public CBSBLGOptCorMM(CPolyline frcpl, CPolyline tocpl)
        {
            _FromCpl = frcpl;
            _ToCpl = tocpl;
        }

        public CBSBLGOptCorMM(CParameterInitialize ParameterInitialize)
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

        //����������Morphing������ɾ��С����������
        public void BSBLGOptCorMMMorphing()
        {
            //CParameterInitialize ParameterInitialize = _ParameterInitialize;
            //CPolyline frcpl = _LSCPlLt[0];
            //CPolyline tocpl = _SSCPlLt[0];
            //CHelpFunc.PreviousWork(ref frcpl, ref tocpl);

            ////���㼫Сֵ
            //
            //long lngStartTime = System.Environment.TickCount;  //��ʼʱ��
                       
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
            //C5.LinkedList<CCorrespondSegment> BSCorrespondSegmentLk = new C5.LinkedList<CCorrespondSegment>();
            //BSCorrespondSegmentLk.AddRange(OptMPBBSL.DWByMPBBSL(ParameterInitialize, pParameterVariableFrom, pParameterVariableTo, ParameterThreshold));

            ////**************BLG���������**************//
            //CMPBDPBL OptMPBDPBL = new CMPBDPBL();
            //C5.LinkedList<CCorrespondSegment> BLGCorrespondSegmentLk = new C5.LinkedList<CCorrespondSegment>();
            //for (int j = 0; j < BSCorrespondSegmentLk.Count; j++)
            //{
            //    BLGCorrespondSegmentLk.AddRange(OptMPBDPBL.DWByDPLADefine(BSCorrespondSegmentLk[j].CFrPolyline, BSCorrespondSegmentLk[j].CToPolyline, ParameterThreshold));
            //}

            ////**************OptCorMM�������**************//
            //COptCorMM OptOptCorMM = new COptCorMM();
            //C5.LinkedList<CCorrespondSegment> OptCorMMCorrespondSegmentLk = new C5.LinkedList<CCorrespondSegment>();
            //for (int j = 0; j < BLGCorrespondSegmentLk.Count; j++)
            //{
            //    OptCorMMCorrespondSegmentLk.AddRange(OptOptCorMM.DWByOptCor(BLGCorrespondSegmentLk[j].CFrPolyline, BLGCorrespondSegmentLk[j].CToPolyline, ParameterInitialize.intMaxBackK));
            //}


            ////��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��
            //CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper();
            //List<CPoint> pResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(OptCorMMCorrespondSegmentLk, "Linear");

            ////�����Ӧ��
            //CHelpFunc.SaveCtrlLine(OptCorMMCorrespondSegmentLk, "BSBLGOptCorMMControlLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);
            //CHelpFunc.SaveCorrespondLine(pResultPtLt, "BSBLGOptCorMMCorrLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);

            ////���㲢��ʾ����ʱ��
            //long lngEndTime = System.Environment.TickCount;
            //long lngTime = lngEndTime - lngStartTime;
            //_ParameterInitialize.tsslTime.Text = "Running Time: " + Convert.ToString(lngTime) + "ms";  //��ʾ����ʱ��

            ////��ȡ�����ȫ����¼��_ParameterResult��
            //CParameterResult ParameterResult = new CParameterResult();
            //ParameterResult.FromCpl = frcpl;
            //ParameterResult.ToCpl = tocpl;
            //ParameterResult.pCorrespondSegmentLk = OptCorMMCorrespondSegmentLk;
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