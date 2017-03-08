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
    /// <summary>CMRLCut</summary>
    /// <remarks>���뱣֤�����߶εķ����ǴӺ���������ָ����������Σ�Ҳ��д�������ܵĴ�����д���</remarks>
    public class CMRLCut
    {
        
        
        private CMRL _OptMRL = new CMRL();
        private CParameterResult _ParameterResult;

        private List<CPolyline> _LSCPlLt = new List<CPolyline>();  //BS:LargerScale
        private List<CPolyline> _SSCPlLt = new List<CPolyline>();  //SS:SmallerScale

        private CParameterInitialize _ParameterInitialize;

        public CMRLCut()
        {

        }

        public CMRLCut(CParameterInitialize ParameterInitialize)
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


        public void MRLCutMorphing()
        {
            //long lngStartTime1 = 0;
            //long lngEndTime1 = 0;

            //List<CPolyline> LSCPlLt = _LSCPlLt;
            //List<CPolyline> SSCPlLt = _SSCPlLt;

            //CParameterInitialize pParameterInitialize = _ParameterInitialize;
            //CParameterThreshold pParameterThreshold = new CParameterThreshold();

            //lngStartTime1 = System.Environment.TickCount;
            //pParameterThreshold.dblBuffer = CHelperFunction.CalBuffer(LSCPlLt, SSCPlLt);      //���㻺�����뾶��С
            //lngEndTime1 = System.Environment.TickCount;
            //long lngTime1 = lngEndTime1 - lngStartTime1;

            //pParameterThreshold.dblVerySmall = CGeometricMethods.CalVerySmall(LSCPlLt[0]);         //���㼫Сֵ
            //pParameterThreshold.dblOverlapRatio = pParameterInitialize.dblOverlapRatio;

            //long lngTime2=0;
            //long lngTime3 = 0;
            //List<CRiverNet> CBSRiverNetLt = _OptMRL.BuildRiverNetLt(LSCPlLt, pParameterThreshold, ref lngTime2);     //���������ݽ�������
            //List<CRiverNet> CSSRiverNetLt = _OptMRL.BuildRiverNetLt(SSCPlLt, pParameterThreshold, ref lngTime3);     //���������ݽ�������


            //long lngStartTime4 = System.Environment.TickCount;
            //CalLengthSum(CBSRiverNetLt);   //���������ÿһ�������ĳ����ܺ�
            //CalLengthSum(CSSRiverNetLt);   //���������ÿһ�������ĳ����ܺ�

            ////CHelperFunction.SaveBuffers(CBSRiverNetLt, CSSRiverNetLt, pParameterInitialize.pWorkspace, pParameterInitialize.m_mapControl);  //����Buffer


            //List<CCorrespondRiverNet> pCorrespondRiverNetLt =_OptMRL.FindCorrespondRiverNetLt(CBSRiverNetLt, CSSRiverNetLt, pParameterThreshold);  //�Ҷ�Ӧ����

            //List<List<CPoint>> CResultPtLtLt = new List<List<CPoint>>();
            //for (int i = 0; i < pCorrespondRiverNetLt.Count; i++)
            //{
            //    _OptMRL.FindCorrespondRiverLt(pCorrespondRiverNetLt[i], pParameterThreshold);  //�ҵ���Ӧ��������Ӧ���������ݼ�¼�ڶ�Ӧ��������
            //    BuildCorrespondence(pCorrespondRiverNetLt[i], pParameterThreshold);            //�����ж�Ӧ������Morphing��ϵ
            //    CResultPtLtLt.AddRange(pCorrespondRiverNetLt[i].CResultPtLtLt);
            //}

            ////����û�ж�Ӧ������Morphing��ϵ(���и�ʱ��Ϊָ��)
            //CalCutTime(CBSRiverNetLt);
            //long lngEndTime4 = System.Environment.TickCount;
            //long lngTime4 = lngEndTime4 - lngStartTime4;

            ////���㲢��ʾ����ʱ��
            //long lngTimeSum = lngTime1 + lngTime2 + lngTime3 + lngTime4;
            //_ParameterInitialize.tsslTime.Text = "Running Time: " + Convert.ToString(lngTimeSum) + "ms";  //��ʾ����ʱ��

            ////�����Ӧ��
            //CHelperFunction.SaveCorrespondLine(CResultPtLtLt, "MRLCorrLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);

            ////��ȡ�����ȫ����¼��_ParameterResult��
            //CParameterResult ParameterResult = new CParameterResult();
            //ParameterResult.CResultCorrespondRiverNetLt = pCorrespondRiverNetLt;
            //ParameterResult.CResultPtLtLt = CResultPtLtLt;
            //ParameterResult.lngTime = lngTimeSum;
            //_ParameterResult = ParameterResult;
        }

        /// <summary>���������ÿһ�������ĳ����ܺ�</summary>
        /// <param name="pRiverNetLt">������</param>
        /// <remarks>�����ĳ����ܺͣ��ú����������֧���ĳ���֮�� </remarks>
        private void CalLengthSum(List<CRiverNet> pRiverNetLt)
        {
            for (int i = 0; i < pRiverNetLt.Count ; i++)
            {
                RecursiveCalLengthSum(pRiverNetLt[i].CMasterStream);
            }
        }

        /// <summary>�ݹ����ú����ĳ����ܺ�(�ú�����������֧���ĳ���֮��)</summary>
        /// <param name="pRiver">��ǰ����</param>
        private void RecursiveCalLengthSum(CRiver pRiver)
        {
            pRiver.LengthSum = pRiver.pPolyline.Length;
            if (pRiver.CTributaryLt.Count > 0)
            {                
                for (int i = 0; i < pRiver.CTributaryLt.Count; i++)
                {
                    RecursiveCalLengthSum(pRiver.CTributaryLt[i]);          //�ȼ����֧���ĳ����ܺͣ��ټ��㵱ǰ�����ĳ����ܺ�
                    pRiver.LengthSum =pRiver.LengthSum+ pRiver.CTributaryLt[i].LengthSum;
                }
            }
        }




        /// <summary>����������Ķ�Ӧ�����Ķ�Ӧ�������ϵ��������û�ж�Ӧ�����������</summary>
        /// <param name="pCorrespondRiverNet">��Ӧ��������</param>
        /// <param name="pParameterThreshold">��ֵ����</param>
        /// <remarks>�жϷ������ཻ�����������������������������������֮��</remarks>
        private void BuildCorrespondence(CCorrespondRiverNet pCorrespondRiverNet, CParameterThreshold pParameterThreshold)
        {
            CRiverNet pBSRiverNet = pCorrespondRiverNet.CBSRiverNet;
            CRiverNet pSSRiverNet = pCorrespondRiverNet.CSSRiverNet;
            pCorrespondRiverNet.CResultPtLtLt = new List<List<CPoint>>();

            double dblLengthSumRatio = pBSRiverNet.CMasterStream.pPolyline.Length / pSSRiverNet.CMasterStream.pPolyline.Length;
            pParameterThreshold.dblLengthSumRatio = dblLengthSumRatio;
            //ע�⣺�����������治���ڶ�Ӧ���������Ӵ˴���ʼ����Ϊ�����ڶ�Ӧ�������ڴ˺������Զ���������
            RecursiveDWExistCorrCut(pCorrespondRiverNet, pParameterThreshold, pBSRiverNet.CMasterStream);
        }

        /// <summary>������һ�������ж�Ӧ���������</summary>
        /// <param name="pCorrespondRiverNet">��Ӧ��������</param>
        /// <param name="pParameterThreshold">��ֵ����</param>
        /// <remarks>RecursiveDWExistCorr��RecursiveDealWithExistCorrepondenceRiver</remarks>
        private void RecursiveDWExistCorrCut(CCorrespondRiverNet pCorrespondRiverNet, CParameterThreshold pParameterThreshold, CRiver pBSRiver)
        {
            if (pBSRiver.CCorrRiver != null)
            {
                _OptMRL.DWExistCorr(pCorrespondRiverNet, pParameterThreshold, pBSRiver);

                //����ǰ������֧��
                for (int i = 0; i < pBSRiver.CTributaryLt.Count; i++)
                {
                    RecursiveDWExistCorrCut(pCorrespondRiverNet, pParameterThreshold, pBSRiver.CTributaryLt[i]);
                }
            }
        }



        /// <summary>�����и�ʱ��</summary>
        /// <param name="CBSRiverNetLt">������߱�������</param>
        /// <remarks></remarks>
        private void CalCutTime(List<CRiverNet> CBSRiverNetLt)
        {
            SortedList<double, CRiver> dblDataSLt = new SortedList<double, CRiver>(new CCompareDbl());
            //��Ӵ�����߱��ͼ����(����������)������û�ж�Ӧ�����ĺ���
            for (int i = 0; i < CBSRiverNetLt.Count; i++)
            {
                for (int j = 0; j < CBSRiverNetLt[i].CRiverLt.Count; j++)
                {
                    if (CBSRiverNetLt[i].CRiverLt[j].CCorrRiver == null)  //�ж�Ӧ����
                    {
                        dblDataSLt.Add(CBSRiverNetLt[i].CRiverLt[j].LengthSum, CBSRiverNetLt[i].CRiverLt[j]);
                    }
                }
            }

            int intCount = dblDataSLt.Count;
            double dblInterval = (dblDataSLt.Keys[intCount - 1] - dblDataSLt.Keys[0]) / (intCount - 1);
            double dblDCutBound = dblDataSLt.Keys[0] - dblInterval;
            double dblUCutBound = dblDataSLt.Keys[intCount - 1] + dblInterval;
            double dblDis = dblUCutBound - dblDCutBound;
            for (int i = 0; i < dblDataSLt.Count; i++)
            {
                CRiver pRiver = dblDataSLt.Values[i];
                pRiver.dblCutTime = (pRiver.LengthSum - dblDCutBound) / dblDis;
            }
        }



        /// <summary>���ԣ�������</summary>
        public CParameterResult ParameterResult
        {
            get { return _ParameterResult; }
            set { _ParameterResult = value; }
        }
    }
}
