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
using MorphingClass.CMorphingMethods;
using MorphingClass.CMorphingAlgorithms;

namespace MorphingClass.CMorphingMethods
{
    /// <summary>
    /// �ݹ���������ṹ��BLG���ṹ�ķ���
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class CBSBLGOptCorMMSimplified
    {
        
        
        private CParameterResult _ParameterResult;

        private List<CPolyline> _LSCPlLt = new List<CPolyline>();  //BS:LargerScale
        private List<CPolyline> _SSCPlLt = new List<CPolyline>();  //SS:SmallerScale

        private CPolyline _FromCpl;
        private CPolyline _ToCpl;

        private CParameterInitialize _ParameterInitialize;

        public CBSBLGOptCorMMSimplified()
        {

        }

        public CBSBLGOptCorMMSimplified(CPolyline frcpl, CPolyline tocpl)
        {
            _FromCpl = frcpl;
            _ToCpl = tocpl;
        }

        public CBSBLGOptCorMMSimplified(CParameterInitialize ParameterInitialize)
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
        public void BSBLGOptCorMMSimplifiedMorphing()
        {
            //CParameterInitialize ParameterInitialize = _ParameterInitialize;
            //CPolyline frcpl = _LSCPlLt[0];
            //CPolyline tocpl = _SSCPlLt[0];
            //CHelpFunc.PreviousWork(ref frcpl, ref tocpl);

            ////���㼫Сֵ
            //
            //CGeoFunc.CalDistanceParameters(frcpl);
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
            //C5.LinkedList<CCorrespondSegment> pBLGCorrespondSegmentLk = new C5.LinkedList<CCorrespondSegment>();
            //for (int j = 0; j < BSCorrespondSegmentLk.Count; j++)
            //{
            //    pBLGCorrespondSegmentLk.AddRange(OptMPBDPBL.DWByDPLADefine(BSCorrespondSegmentLk[j].CFrPolyline, BSCorrespondSegmentLk[j].CToPolyline, ParameterThreshold));
            //}

            ////**************OptCorSimplified�������**************//           
            //COptCorMMSimplified OptCorMMSimplified = new COptCorMMSimplified();
            //C5.LinkedList<CCorrespondSegment> pOptCorCorrespondSegmentLk = new C5.LinkedList<CCorrespondSegment>();
            //double dblX = tocpl.CptLt[0].X - frcpl.CptLt[0].X;
            //double dblY = tocpl.CptLt[0].Y - frcpl.CptLt[0].Y;
            //CPoint StandardVectorCpt = new CPoint(0, dblX, dblY);
            //for (int j = 0; j < pBLGCorrespondSegmentLk.Count; j++)
            //{
            //    pOptCorCorrespondSegmentLk.AddRange(OptCorMMSimplified.DWByOptCorMMSimplified(pBLGCorrespondSegmentLk[j].CFrPolyline, pBLGCorrespondSegmentLk[j].CToPolyline, ParameterInitialize.intMaxBackK, StandardVectorCpt, dblSmallDis);
            //}


            //////delete wrong control characteristic vertices
            ////double dblStandardLength = pOptCorCorrespondSegmentLk[0].CFrPolyline.CptLt[0].DistanceTo(pOptCorCorrespondSegmentLk[0].CToPolyline.CptLt[0]);
            ////for (int i = pOptCorCorrespondSegmentLk.Count -1; i >=0; i--)
            ////{
            ////    double dblCorrLength = pOptCorCorrespondSegmentLk[i].CFrPolyline.CptLt[0].DistanceTo(pOptCorCorrespondSegmentLk[i].CToPolyline.CptLt[0]);
            ////    if (Math .Abs (dblCorrLength-dblStandardLength)>dblVerySmall)
            ////    {
            ////        List<CPoint> cfrptlt = new List<CPoint>();
            ////        cfrptlt.AddRange(pOptCorCorrespondSegmentLk[i - 1].CFrPolyline.CptLt);
            ////        cfrptlt.RemoveAt(cfrptlt.Count - 1);   //delete repetitive vertice
            ////        cfrptlt.AddRange(pOptCorCorrespondSegmentLk[i - 0].CFrPolyline.CptLt);
            ////        List<CPoint> ctoptlt = new List<CPoint>();
            ////        ctoptlt.AddRange(pOptCorCorrespondSegmentLk[i - 1].CToPolyline .CptLt);
            ////        ctoptlt.RemoveAt(ctoptlt.Count - 1);   //delete repetitive vertice
            ////        ctoptlt.AddRange(pOptCorCorrespondSegmentLk[i - 0].CToPolyline.CptLt);

            ////        CCorrespondSegment pCorrespondSegment = new CCorrespondSegment(cfrptlt, ctoptlt);
            ////        pOptCorCorrespondSegmentLk.RemoveAt(i);
            ////        pOptCorCorrespondSegmentLk.RemoveAt(i-1);
            ////        pOptCorCorrespondSegmentLk.Insert(i - 1, pCorrespondSegment);
            ////    }
            ////}


            ////��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��
            //CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper();//���õ��ö���ĺ���
            //List<CPoint> pResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(pOptCorCorrespondSegmentLk, "Linear");

            ////�����Ӧ��
            //CHelpFunc.SaveCtrlLine(pOptCorCorrespondSegmentLk, "BSBLGOptCorMMSimplifiedControlLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);
            //CHelpFunc.SaveCorrespondLine(pResultPtLt, "BSBLGOptCorMMSimplifiedCorrLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);

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
