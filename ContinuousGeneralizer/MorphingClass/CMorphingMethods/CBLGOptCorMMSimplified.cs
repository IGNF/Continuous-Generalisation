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
    public class CBLGOptCorMMSimplified
    {
        
        
        private CParameterResult _ParameterResult;

        private List<CPolyline> _LSCPlLt = new List<CPolyline>();  //BS:LargerScale
        private List<CPolyline> _SSCPlLt = new List<CPolyline>();  //SS:SmallerScale

        private CPolyline _FromCpl;
        private CPolyline _ToCpl;

        private CParameterInitialize _ParameterInitialize;

        public CBLGOptCorMMSimplified()
        {

        }

        public CBLGOptCorMMSimplified(CPolyline frcpl, CPolyline tocpl)
        {
            _FromCpl = frcpl;
            _ToCpl = tocpl;
        }

        public CBLGOptCorMMSimplified(CParameterInitialize ParameterInitialize)
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
            _LSCPlLt = CHelperFunction.GetCPlLtByFeatureLayer(pBSFLayer);
            _SSCPlLt = CHelperFunction.GetCPlLtByFeatureLayer(pSSFLayer);
        }

        //����������Morphing����
        public void BLGOptCorMMSimplifiedMorphing()
        {
            //CParameterInitialize ParameterInitialize = _ParameterInitialize;
            //CPolyline frcpl = _LSCPlLt[0];
            //CPolyline tocpl = _SSCPlLt[0];

            ////���㼫Сֵ
            //CGeometricMethods.CalDistanceParameters(frcpl);
            //

            //long lngStartTime = System.Environment.TickCount;  //��ʼʱ��            

            ////������ֵ����
            //double dblBound = 0.98;
            //CParameterThreshold ParameterThreshold = new CParameterThreshold();
            //ParameterThreshold.dblFrLength = frcpl.pPolyline.Length;
            //ParameterThreshold.dblToLength = tocpl.pPolyline.Length;
            //ParameterThreshold.dblDLengthBound = dblBound;
            //ParameterThreshold.dblULengthBound = 1 / dblBound;

            ////**************BLG���������**************//
            ////����BLG�����������ն�Ӧ�߶�
            //CMPBDPBL OptMPBDPBL = new CMPBDPBL();//����
            //C5.LinkedList<CCorrespondSegment> pBLGCorrespondSegmentLk = OptMPBDPBL.DWByDPLADefine(frcpl, tocpl, ParameterThreshold);

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

            ////��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��
            //CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper();//���õ��ö���ĺ���
            //List<CPoint> pResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(pOptCorCorrespondSegmentLk, "Linear");

            ////�����Ӧ��
            //CHelperFunction.SaveCtrlLine(pOptCorCorrespondSegmentLk, "BLGOptCorMMSimplifiedControlLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);
            //CHelperFunction.SaveCorrespondLine(pResultPtLt, "BLGOptCorMMSimplifiedCorrLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);

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
