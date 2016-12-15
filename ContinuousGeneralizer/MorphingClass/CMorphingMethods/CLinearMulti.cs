using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;


using MorphingClass.CEntity;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;
using MorphingClass.CMorphingAlgorithms;
using MorphingClass.CCorrepondObjects;

namespace MorphingClass.CMorphingMethods
{
    /// <summary>CLinearMulti</summary>
    /// <remarks>���뱣֤�����߶εķ����ǴӺ���������ָ����������Σ�Ҳ��д�������ܵĴ�����д���
    ///          ����洢��
    ///                   �����Ķ�Ӧ���������洢�������Լ��ġ�pBSRiver.CResultPtLt�С���
    ///               ���⣬pCorrespondRiverNet.CResultPtLtLt��洢�˶�Ӧ�����и������Ķ�Ӧ����������
    ///               ���գ�ParameterResult.CResultPtLtLt�洢�˸���Ӧ�����и������Ķ�Ӧ����������
    ///          Multi����Ϊ���������ļ�
    ///</remarks>
    public class CLinearMulti
    {
        
        
        private CParameterResult _ParameterResult;

        private List<CPolyline> _LSCPlLt = new List<CPolyline>();  //BS:LargerScale
        private List<CPolyline> _SSCPlLt = new List<CPolyline>();  //SS:SmallerScale

        private CParameterInitialize _ParameterInitialize;

        public CLinearMulti()
        {

        }

        public CLinearMulti(CParameterInitialize ParameterInitialize)
        {
            //��ȡ��ǰѡ��ĵ�Ҫ��ͼ��
            //�������Ҫ��ͼ��
            IFeatureLayer pBSFLayer = (IFeatureLayer)ParameterInitialize.m_mapFeature.get_Layer(ParameterInitialize.cboLargerScaleLayer.SelectedIndex);
                                                                       
            //С������Ҫ��ͼ��
            IFeatureLayer pSSFLayer =(IFeatureLayer)ParameterInitialize.m_mapFeature.get_Layer(ParameterInitialize.cboSmallerScaleLayer.SelectedIndex);
                                                           


            ParameterInitialize.pBSFLayer = pBSFLayer;
            ParameterInitialize.pSSFLayer = pSSFLayer;
            ParameterInitialize.dblOverlapRatio = Convert.ToDouble(ParameterInitialize.txtOverlapRatio.Text);
            _ParameterInitialize = ParameterInitialize;

            //��ȡ������
            _LSCPlLt = CHelperFunction.GetCPlLtByFeatureLayer(pBSFLayer);
            _SSCPlLt = CHelperFunction.GetCPlLtByFeatureLayer(pSSFLayer);
        }

        public void LinearMultiMorphing()
        {

            //List<CPolyline> LSCPlLt = _LSCPlLt;
            //List<CPolyline> SSCPlLt = _SSCPlLt;

            //CParameterInitialize pParameterInitialize = _ParameterInitialize;
            //CParameterThreshold pParameterThreshold = new CParameterThreshold();
            //pParameterThreshold.dblBuffer = CHelperFunction.CalBuffer(LSCPlLt, SSCPlLt);                      //���㻺�����뾶��С
            //pParameterThreshold.dblVerySmall = pParameterThreshold.dblBuffer / 200;
            //pParameterThreshold.dblOverlapRatio = pParameterInitialize.dblOverlapRatio;

            ////���ݴ�����߱�����������ɺ�������
            //List<CRiver> CBSRiverLt = new List<CRiver>();
            //for (int i = 0; i < LSCPlLt.Count; i++)
            //{
            //    CRiver pRiver = new CRiver(i, LSCPlLt[i],pParameterThreshold.dblBuffer, CConstants.dblVerySmall);
            //    CBSRiverLt.Add(pRiver);
            //}

            ////����С�����߱�����������ɺ�������
            //List<CRiver> CSSRiverLt = new List<CRiver>();
            //for (int i = 0; i < SSCPlLt.Count; i++)
            //{
            //    CRiver pRiver = new CRiver(i, SSCPlLt[i], pParameterThreshold.dblBuffer, CConstants.dblVerySmall);
            //    CSSRiverLt.Add(pRiver);
            //}

            //List<CCorrespondRiver> pCorrespondRiverLt = FindCorrespondRiverLt(CBSRiverLt, CSSRiverLt, pParameterThreshold);
            ////CLinear 

            //List<List<CPoint>> CResultPtLtLt = new List<List<CPoint>>();
            //CLinearInterpolationA pLinearInterpolation = new CLinearInterpolationA();
            //for (int i = 0; i < pCorrespondRiverLt.Count; i++)
            //{
            //    CPolyline frcpl = new CPolyline(pCorrespondRiverLt[i].CFromRiver);
            //    CPolyline tocpl = new CPolyline(pCorrespondRiverLt[i].CToRiver);
            //    List<CPoint> ResultPtLt = pLinearInterpolation.CLI(frcpl, tocpl);
            //    CResultPtLtLt.Add(ResultPtLt);
            //}

            //CHelperFunction.SaveCorrespondLine(CResultPtLtLt, "LinearCorrLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);


            ////��ȡ�����ȫ����¼��_ParameterResult��
            //CParameterResult ParameterResult = new CParameterResult();
            //ParameterResult.CResultPtLtLt = CResultPtLtLt;
            //_ParameterResult = ParameterResult;

        }


        /// <summary>�Ҷ�Ӧ����</summary>
        /// <param name="CBSRiverLt">������߱�����</param>
        /// <param name="CSSRiverLt">С�����߱�����</param>
        /// <param name="pParameterThreshold">��ֵ����</param>
        /// <returns>��Ӧ������</returns>
        /// <remarks>��Ӧ���������ݼ�¼�ڶ�Ӧ����</remarks>
        private List<CCorrespondRiver> FindCorrespondRiverLt(List<CRiver> CBSRiverLt, List<CRiver> CSSRiverLt, CParameterThreshold pParameterThreshold)
        {
            List<CCorrespondRiver> pCorrespondRiverLt = new List<CCorrespondRiver>();
            for (int i = 0; i < CBSRiverLt.Count; i++)
            {
                for (int j = 0; j < CSSRiverLt.Count; j++)
                {
                    bool blnIsOverlap = CGeometricMethods.IsOverlap(CBSRiverLt[i], CSSRiverLt[j], pParameterThreshold.dblOverlapRatio);
                    if (blnIsOverlap == true)
                    {
                        CBSRiverLt[i].CCorrRiver = CSSRiverLt[j];
                        CSSRiverLt[j].CCorrRiver = CBSRiverLt[i];
                        CCorrespondRiver pCorrespondRiver = new CCorrespondRiver(CBSRiverLt[i], CSSRiverLt[j]);
                        pCorrespondRiverLt.Add(pCorrespondRiver);
                        CSSRiverLt.RemoveAt(j);
                        break;
                    }
                }
            }
            return pCorrespondRiverLt;
        }



        /// <summary>���ԣ�������</summary>
        public CParameterResult ParameterResult
        {
            get { return _ParameterResult; }
            set { _ParameterResult = value; }
        }
    }
}
