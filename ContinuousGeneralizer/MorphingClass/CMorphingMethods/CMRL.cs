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
using MorphingClass.CEvaluationMethods;

namespace MorphingClass.CMorphingMethods
{
    /// <summary>CMRL</summary>
    /// <remarks>���뱣֤�����߶εķ����ǴӺ���������ָ����������Σ�Ҳ��д�������ܵĴ�����д���
    ///          ����洢��
    ///                   �����Ķ�Ӧ���������洢�������Լ��ġ�pBSRiver.CResultPtLt�С���
    ///               ���⣬pCorrespondRiverNet.CResultPtLtLt��洢�˶�Ӧ�����и������Ķ�Ӧ����������
    ///               ���գ�ParameterResult.CResultPtLtLt�洢�˸���Ӧ�����и������Ķ�Ӧ����������
    ///</remarks>
    public class CMRL
    {
        
        
        private CParameterResult _ParameterResult;

        private List<CPolyline> _LSCPlLt = new List<CPolyline>();  //BS:LargerScale
        private List<CPolyline> _SSCPlLt = new List<CPolyline>();  //SS:SmallerScale

        private CParameterInitialize _ParameterInitialize;

        public CMRL()
        {

        }

        public CMRL(CParameterInitialize ParameterInitialize)
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
            _LSCPlLt = CHelpFunc.GetCPlLtByFeatureLayer(pBSFLayer);
            _SSCPlLt = CHelpFunc.GetCPlLtByFeatureLayer(pSSFLayer);
        }

        public void MRLMorphing()
        {
            
            //List<CPolyline> LSCPlLt = _LSCPlLt;
            //List<CPolyline> SSCPlLt = _SSCPlLt;

            //CParameterInitialize pParameterInitialize = _ParameterInitialize;
            //CParameterThreshold pParameterThreshold = new CParameterThreshold();
            //pParameterThreshold.dblBuffer =CHelpFunc.CalBuffer(LSCPlLt, SSCPlLt);                      //���㻺�����뾶��С
            //pParameterThreshold.dblVerySmall = pParameterThreshold.dblBuffer/200;
            ////pParameterThreshold.dblVerySmall = CGeoFunc.CalVerySmall(LSCPlLt);         //���㼫Сֵ
            //pParameterThreshold.dblOverlapRatio = pParameterInitialize.dblOverlapRatio;

            //long lngTime1=0;
            //long lngTime2=0;

            //List<CRiverNet> CBSRiverNetLt = BuildRiverNetLt(LSCPlLt, pParameterThreshold, ref lngTime1);     //���������ݽ�������
            //List<CRiverNet> CSSRiverNetLt = BuildRiverNetLt(SSCPlLt, pParameterThreshold, ref lngTime2);     //���������ݽ�������

            //CHelpFunc.SaveBuffers(CBSRiverNetLt, CSSRiverNetLt, pParameterInitialize.pWorkspace, pParameterInitialize.m_mapControl);  //����Buffer


            //List<CCorrespondRiverNet> pCorrespondRiverNetLt = FindCorrespondRiverNetLt(CBSRiverNetLt, CSSRiverNetLt, pParameterThreshold);  //�Ҷ�Ӧ����

            //List<List<CPoint>> CResultPtLtLt = new List<List<CPoint>>();
            //List<CRiver> CResultRiverLt = new List<CRiver>();
            //for (int i = 0; i < pCorrespondRiverNetLt.Count; i++)
            //{
                
            //    FindCorrespondRiverLt(pCorrespondRiverNetLt[i], pParameterThreshold);  //�ҵ���Ӧ��������Ӧ���������ݼ�¼�ڶ�Ӧ��������
            //    BuildCorrespondence(pCorrespondRiverNetLt[i], pParameterThreshold);                         //����Morphing��ϵ
            //    CResultPtLtLt.AddRange(pCorrespondRiverNetLt[i].CResultPtLtLt);
            //    //CResultRiverLt.AddRange(pCorrespondRiverNetLt[i].CResultRiverLt);
            //}

            
            ////�����Ӧ��
            //CHelpFunc.SaveCorrespondLine(CResultPtLtLt, "MRLCorrLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);

            ////��ȡ�����ȫ����¼��_ParameterResult��
            //CParameterResult ParameterResult = new CParameterResult();
            //ParameterResult.CResultCorrespondRiverNetLt = pCorrespondRiverNetLt;
            //ParameterResult.CResultPtLtLt = CResultPtLtLt;
            ////ParameterResult.CResultRiverLt = CResultRiverLt;
            //_ParameterResult = ParameterResult;




            ////CMathStatistic.KillProcess();


        }

        #region ���������ݽ���������(����������֧��ϵ����Ӹ�֧����Ա)
        /// <summary>���������ݽ���������(����������֧��ϵ����Ӹ�֧����Ա)</summary>
        /// <param name="CPlLt">������</param>
        /// <param name="pParameterThreshold">��ֵ����</param>
        /// <returns>������</returns>
        public List<CRiverNet> BuildRiverNetLt(List<CPolyline> CPlLt, CParameterThreshold pParameterThreshold,ref long lngTime)
        {
            double dblVerySmall = CConstants.dblVerySmallCoord;
            double dblBuffer = pParameterThreshold.dblBuffer;

            //�������������ɺ�������
            long lngStartTime = System.Environment.TickCount;
            List<CRiver> CAllRiverLt = new List<CRiver>();
            for (int i = 0; i < CPlLt.Count; i++)
            {
                CRiver pRiver = new CRiver(i, CPlLt[i],dblBuffer, dblVerySmall);
                CAllRiverLt.Add(pRiver);
            }
            long lngEndTime = System.Environment.TickCount;
            lngTime = lngEndTime - lngStartTime;

            CreateRiverRelationship(ref CAllRiverLt);   //������������ĸ�֧��ϵ
            List<CRiverNet> pRiverNetLt = CreateRiverNetLt(CAllRiverLt, dblVerySmall); // ��������
            return pRiverNetLt;
        }

        /// <summary>������������ĸ�֧��ϵ</summary>
        /// <param name="CAllRiverLt">��������</param>
        private void CreateRiverRelationship(ref List<CRiver> CAllRiverLt)
        {
            //������������ĸ�֧��ϵ
            for (int i = 0; i < CAllRiverLt.Count - 1; i++)
            {
                IRelationalOperator pSmallBufferRel = CAllRiverLt[i].pSmallBufferGeo as IRelationalOperator;
                IRelationalOperator pToptSmallBufferGeo = CAllRiverLt[i].pToptSmallBufferGeo as IRelationalOperator;
                for (int j = i + 1; j < CAllRiverLt.Count; j++)
                {
                    bool isDisjoint = pSmallBufferRel.Disjoint(CAllRiverLt[j].pPolyline);
                    if (isDisjoint == false)
                    {
                        if (pToptSmallBufferGeo.Disjoint(CAllRiverLt[j].pToptSmallBufferGeo) == false) //��������������յ��غϣ�����֮�䲻���ڸ�֧��ϵ
                        {
                            continue;
                        }

                        if (CAllRiverLt[i].CMainStream != null) //���"����i"���˸ɺ�������"����j"��Ȼ������֧��
                        {
                            CAllRiverLt[j].CMainStream = CAllRiverLt[i];
                            CAllRiverLt[i].CTributaryLt.Add(CAllRiverLt[j]);
                        }
                        else if (CAllRiverLt[j].CMainStream != null) //���"����j"���˸ɺ�������"����i"��Ȼ������֧��
                        {
                            CAllRiverLt[i].CMainStream = CAllRiverLt[j];
                            CAllRiverLt[j].CTributaryLt.Add(CAllRiverLt[i]);
                        }
                        else
                        {
                            bool isDisjoint2 = pSmallBufferRel.Disjoint(CAllRiverLt[j].pPolyline.ToPoint);
                            if (isDisjoint2 == false)
                            {
                                CAllRiverLt[j].CMainStream = CAllRiverLt[i];
                                CAllRiverLt[i].CTributaryLt.Add(CAllRiverLt[j]);
                            }
                            else
                            {
                                CAllRiverLt[i].CMainStream = CAllRiverLt[j];
                                CAllRiverLt[j].CTributaryLt.Add(CAllRiverLt[i]);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>����������ͬʱ����¼��Tocpt2</summary>
        /// <param name="CAllRiverLt">��������</param>
        /// <returns>������</returns>
        private List<CRiverNet> CreateRiverNetLt(List<CRiver> CAllRiverLt,double dblVerySmall)
        {
            List<CRiver> CMasterRiverLt = new List<CRiver>();
            //�ҵ���������
            for (int i = 0; i < CAllRiverLt.Count; i++)
            {
                if (CAllRiverLt[i].CMainStream == null)
                {
                    //û�и����ĺ�����Ϊ������
                    CMasterRiverLt.Add(CAllRiverLt[i]);
                }
            }

            //����������
            List<CRiverNet> pRiverNetLt = new List<CRiverNet>();
            for (int i = 0; i < CMasterRiverLt.Count; i++)
            {
                CRiverNet pRiverNet = new CRiverNet(i, CMasterRiverLt[i]); //�½����������������
                RecursiveFindTocpt2(CMasterRiverLt[i], dblVerySmall);      //�ҵ���ǰ���������֧�����ཻ��(���ڵ�ǰ����)������¼��֧������������
                RecursiveSettleRiver(ref pRiverNet, CMasterRiverLt[i]);    //�ݹ�������ڸú����ĺ���
                pRiverNetLt.Add(pRiverNet);
            }

            return pRiverNetLt;
        }

        /// <summary>�ҵ���ǰ���������֧�����ཻ��(���ڵ�ǰ����)������¼��֧������������(˳�������֧����¼�����ε����ε�����)</summary>
        /// <param name="pRiver">��ǰ����</param>
        /// <param name="dblVerySmall">һ���ǳ�С��ֵ</param>
        /// <remarks>����һ����������ж�Ӧ�������ĺ�������仯��������ƽ�ƣ�ʱ������������ڲ����ƶ���������ѽڻ��ཻ��
        /// ���Ҫ��¼�˴��ཻ�㣬�Է����������</remarks>
        public void RecursiveFindTocpt2(CRiver pRiver, double dblVerySmall)
        {
            if (pRiver.CTributaryLt.Count == 0)
            {
                return;  //���������֧������ֱ�ӷ���
            }

            //����׼��
            List<CRiver> pTributaryLt = new List<CRiver>();
            pTributaryLt.AddRange(pRiver.CTributaryLt);

            //�ҵ�ÿ��֧�����յ�"Tocpt"������ӵ�����pTributaryTocptLt
            List<CPoint> pTributaryTocptLt = new List<CPoint>();
            for (int i = 0; i < pTributaryLt.Count; i++)
            {
                pTributaryTocptLt.Add(pTributaryLt[i].CptLt[pTributaryLt[i].CptLt.Count - 1]);
            }

            //Ѱ�Ҹ�֧����Tocpt2
            List<CRiver> pNewTributaryLt = new List<CRiver>();
            for (int i = 0; i < pRiver.CptLt.Count; i++)
            {
                for (int j = pTributaryTocptLt.Count - 1; j >= 0; j--)  
                {
                    //Ϊ�˼������·�ڡ���������˴��ҵ������Ӧ����Ѱ��(һ������ܶ�Ӧ���֧��)�����if��û�з���"break"
                    if (pRiver.CptLt[i].Equals2D(pTributaryTocptLt[j]))
                    {
                        pTributaryLt[j].Tocpt2 = pRiver.CptLt[i];
                        pNewTributaryLt.Add(pTributaryLt[j]);  //�����ε����μ�¼֧��
                        pTributaryLt.RemoveAt(j);       //Ϊ��ʡʱ�䣬�ҵ����Ƴ��ú���
                        pTributaryTocptLt.RemoveAt(j);  //Ϊ��ʡʱ�䣬�ҵ�"Tocpt2"���Ƴ��õ�
                    }
                }
            }
            pRiver.CTributaryLt = pNewTributaryLt;  //�õ��ź����֧��

            //���ζԸú�����֧�����в���
            for (int i = 0; i < pRiver.CTributaryLt.Count; i++)
            {
                RecursiveFindTocpt2(pRiver.CTributaryLt[i], dblVerySmall);
            }
        }

        /// <summary>�ݹ�������ڸú����ĺ���</summary>
        /// <param name="pRiverNet">����</param>
        /// <param name="CurrentRiver">��ǰ����</param>
        private void RecursiveSettleRiver(ref CRiverNet pRiverNet, CRiver CurrentRiver)
        {
            if (CurrentRiver.CTributaryLt.Count == 0)
            {
                return;
            }

            for (int i = 0; i < CurrentRiver.CTributaryLt.Count; i++)
            {
                pRiverNet.CRiverLt.Add(CurrentRiver.CTributaryLt[i]);
                RecursiveSettleRiver(ref pRiverNet, CurrentRiver.CTributaryLt[i]);
            }
        }
        
        #endregion

        #region �Ҹ��ֶ�Ӧ��ϵ(������Ӧ��������Ӧ����)
        /// <summary>�ҵ���Ӧ����</summary>
        /// <param name="CBSRiverNetLt">������߱�����</param>
        /// <param name="CSSRiverNetLt">С�����߱�����</param>
        /// <param name="pParameterThreshold">��ֵ����</param>
        /// <returns>��Ӧ������</returns>
        /// <remarks></remarks>
        public List<CCorrespondRiverNet> FindCorrespondRiverNetLt(List<CRiverNet> CBSRiverNetLt, List<CRiverNet> CSSRiverNetLt, CParameterThreshold pParameterThreshold)
        {
            //����׼��
            List<CRiverNet> pBSRiverNetLt = new List<CRiverNet>();
            pBSRiverNetLt.AddRange(CBSRiverNetLt);
            List<CRiverNet> pSSRiverNetLt = new List<CRiverNet>();
            pSSRiverNetLt.AddRange(CSSRiverNetLt);

            //������Ӧ������ϵ
            List<CCorrespondRiverNet> CCorrespondRiverNetLt = new List<CCorrespondRiverNet>();
            for (int i = 0; i < pBSRiverNetLt.Count; i++)
            {
                bool blnIsOverlap = false;
                //ͨ�������������Ƿ��ص����ж��������Ƿ�Ϊͬһ�����Ĳ�ͬ�����߱��
                for (int j = 0; j < pSSRiverNetLt.Count; j++)
                {
                    blnIsOverlap = CGeoFunc.IsOverlap(pBSRiverNetLt[i].CMasterStream, pSSRiverNetLt[j].CMasterStream, pParameterThreshold.dblOverlapRatio);
                    if (blnIsOverlap == true)
                    {
                        //����������Ӧ��ϵ
                        pBSRiverNetLt[i].CCorrRiverNet = pSSRiverNetLt[j];
                        pSSRiverNetLt[j].CCorrRiverNet = pBSRiverNetLt[i];
                        CCorrespondRiverNet pCorrespondRiverNet = new CCorrespondRiverNet(pBSRiverNetLt[i], pSSRiverNetLt[j]);
                        pCorrespondRiverNet.blnCorr = true;
                        CCorrespondRiverNetLt.Add(pCorrespondRiverNet);
                        pSSRiverNetLt.RemoveAt(j);
                        break;
                    }
                }
                if (blnIsOverlap == false)
                {
                    pBSRiverNetLt[i].CCorrRiverNet = null;
                    CCorrespondRiverNet pCorrespondRiverNet = new CCorrespondRiverNet(pBSRiverNetLt[i], null);
                    pCorrespondRiverNet.blnCorr = false;
                    CCorrespondRiverNetLt.Add(pCorrespondRiverNet);
                }
            }
            return CCorrespondRiverNetLt;
        }

        /// <summary>����һ�Զ�Ӧ��������һ���Ҷ�Ӧ������ͬʱ��¼��Ӧ֧����Ӧ�����</summary>
        /// <param name="CBSRiverNetLt">������߱�����</param>
        /// <param name="CSSRiverNetLt">С�����߱�����</param>
        /// <param name="pParameterThreshold">��ֵ����</param>
        /// <returns>��Ӧ������</returns>
        /// <remarks>��Ӧ���������ݼ�¼�ڶ�Ӧ����</remarks>
        public void FindCorrespondRiverLt(CCorrespondRiverNet pCorrespondRiverNet, CParameterThreshold pParameterThreshold)
        {
            List<CCorrespondRiver> pCorrespondRiverLt = new List<CCorrespondRiver>();  //����¼�ɹ�ƥ��ĺ�����
            if (pCorrespondRiverNet.blnCorr == true)
            {
                //�ݹ��ҵ���Ӧ������ͬʱ��¼��Ӧ֧����Ӧ�����
                RecursiveFindCorrespondRiverLt(ref pCorrespondRiverLt, pCorrespondRiverNet.CBSRiverNet.CMasterStream, pCorrespondRiverNet.CSSRiverNet.CMasterStream, pParameterThreshold);
            }
            pCorrespondRiverNet.CCorrespondRiverLt = pCorrespondRiverLt;
        }


        /// <summary>�ݹ��ҵ���Ӧ������ͬʱ��¼��Ӧ֧����Ӧ�����</summary>
        /// <param name="CCorrespondRiverLt">��Ӧ������¼��</param>
        /// <param name="pBSRiver">������߱�����</param>
        /// <param name="pSSRiver">С�����߱�����</param>
        /// <param name="pParameterThreshold">��ֵ����</param>
        /// <remarks ></remarks>
        private void RecursiveFindCorrespondRiverLt(ref List<CCorrespondRiver> CCorrespondRiverLt,CRiver pBSRiver,CRiver pSSRiver,  CParameterThreshold pParameterThreshold)
        {
            //����������Ӧ��ϵ
            pBSRiver.CCorrRiver = pSSRiver;
            pSSRiver.CCorrRiver = pBSRiver;
            CCorrespondRiver pCorrespondRiver = new CCorrespondRiver(pSSRiver, pBSRiver);
            pCorrespondRiver.blnCorr = true;
            CCorrespondRiverLt.Add(pCorrespondRiver);

            //���С�����߱�������֧������
            if (pSSRiver.CTributaryLt == null)
            {
                return;
            }

            //����׼��
            pBSRiver.CCorrTriJunctionPtLt = new List<CPoint>();
            pSSRiver.CCorrTriJunctionPtLt = new List<CPoint>();
            List<CRiver> pBSTributaryLt = pBSRiver.CTributaryLt;
            List<CRiver> pSSTributaryLt = new List<CRiver>();
            pSSTributaryLt.AddRange(pSSRiver.CTributaryLt);

            //����Ѱ�Ҷ�Ӧ����
            for (int i = 0; i < pBSTributaryLt.Count; i++)
            {
                bool blnIsOverlap = false;
                for (int j = 0; j < pSSTributaryLt.Count; j++)
                {
                    blnIsOverlap = CGeoFunc.IsOverlap(pBSTributaryLt[i], pSSTributaryLt[j], pParameterThreshold.dblOverlapRatio);  //�ж��������Ƿ��ص�
                    if (blnIsOverlap == true)
                    {
                        pBSRiver.CCorrTriJunctionPtLt.Add(pBSTributaryLt[i].Tocpt2);
                        pSSRiver.CCorrTriJunctionPtLt.Add(pSSTributaryLt[j].Tocpt2);
                        RecursiveFindCorrespondRiverLt(ref CCorrespondRiverLt, pBSTributaryLt[i], pSSTributaryLt[j], pParameterThreshold); //�ݹ��ҵ���Ӧ����
                        pSSTributaryLt.RemoveAt(j);
                        break;
                    }
                }
            }

            //��¼��Ӧ������Ŀ���������ڽ�ԭ�������ж�Ӧ�ָ�����߾���
            //���������£�����㶼�ǡ�����·�ڡ�����ʱ����"CCorrTriJunctionPtLt"�в������غϵ�
            //���������Ϊ���ġ������������·�ڡ�ʱ������"CCorrTriJunctionPtLt"�����غϵ㣬Ӧֻ��������һ����
            for (int i = pBSRiver.CCorrTriJunctionPtLt.Count -1; i >0; i--)
            {
                if (pBSRiver.CCorrTriJunctionPtLt[i].Equals2D (pBSRiver.CCorrTriJunctionPtLt[i-1]))
                {
                    pBSRiver.CCorrTriJunctionPtLt.RemoveAt(i);
                    pSSRiver.CCorrTriJunctionPtLt.RemoveAt(i);
                }
            }


        }

        #endregion

        #region ����������Ķ�Ӧ�������ϵ(����û�ж�Ӧ���������)
        /// <summary>����������Ķ�Ӧ�������ϵ(����û�ж�Ӧ���������)</summary>
        /// <param name="pCorrespondRiverNet">��Ӧ��������</param>
        /// <param name="pParameterThreshold">��ֵ����</param>
        /// <remarks>�жϷ������ཻ�����������������������������������֮��</remarks>
        private void BuildCorrespondence(CCorrespondRiverNet pCorrespondRiverNet, CParameterThreshold pParameterThreshold)
        {
            CRiverNet pBSRiverNet = pCorrespondRiverNet.CBSRiverNet;
            CRiverNet pSSRiverNet = pCorrespondRiverNet.CSSRiverNet;
            pCorrespondRiverNet.CResultPtLtLt = new List<List<CPoint>>();
            //pCorrespondRiverNet.CResultRiverLt = new List<CRiver>();

            double dblLengthSumRatio = pBSRiverNet.CMasterStream.pPolyline.Length / pSSRiverNet.CMasterStream.pPolyline.Length;
            pParameterThreshold.dblLengthSumRatio = dblLengthSumRatio;
            //ע�⣺�����������治���ڶ�Ӧ���������Ӵ˴���ʼ����Ϊ�����ڶ�Ӧ�������ڴ˺������Զ�ת�봦�����ڶ�Ӧ�����ĺ���
            RecursiveDWExistCorr(pCorrespondRiverNet, pParameterThreshold, pBSRiverNet.CMasterStream);
            
        }

        /// <summary>������һ�������ж�Ӧ���������</summary>
        /// <param name="pCorrespondRiverNet">��Ӧ��������</param>
        /// <param name="dblLengthSumRatio">С�����߱�����</param>
        /// <param name="pParameterThreshold">��ֵ����</param>
        /// <remarks>RecursiveDWExistCorr��RecursiveDealWithExistCorrepondenceRiver
        /// ע�⣺�����������治���ڶ�Ӧ���������Ӵ˴���ʼ����Ϊ�����ڶ�Ӧ�������ڴ˺������Զ�ת�봦�����ڶ�Ӧ�����ĺ���</remarks>
        public void RecursiveDWExistCorr(CCorrespondRiverNet pCorrespondRiverNet, CParameterThreshold pParameterThreshold, CRiver pBSRiver)
        {
            if (pBSRiver.CCorrRiver != null)
            {
                DWExistCorr(pCorrespondRiverNet, pParameterThreshold, pBSRiver);
                //����ǰ������֧��
                for (int i = 0; i < pBSRiver.CTributaryLt.Count; i++)
                {
                    RecursiveDWExistCorr(pCorrespondRiverNet, pParameterThreshold, pBSRiver.CTributaryLt[i]);
                }
            }
            else
            {
                pBSRiver.dblReductionRatio = 1;
                //pCorrespondRiverNet.CResultRiverLt.Add(pBSRiver);
                //����ǰ������֧��
                for (int i = 0; i < pBSRiver.CTributaryLt.Count; i++)
                {
                    RecursiveDWNotExistCorr(pCorrespondRiverNet, pBSRiver.CTributaryLt[i], 1);
                }
            }

        }

        /// <summary>������һ�������ж�Ӧ���������</summary>
        /// <param name="pCorrespondRiverNet">��Ӧ��������</param>
        /// <param name="dblLengthSumRatio">С�����߱�����</param>
        /// <param name="pParameterThreshold">��ֵ����</param>
        /// <remarks>DWExistCorr��DealWithExistCorrepondenceRiver
        /// �����Ķ�Ӧ���������洢�������Լ��ġ�pBSRiver.CResultPtLt�С���
        /// ����pCorrespondRiverNet.CResultPtLtLt��洢�˶�Ӧ�����и������Ķ�Ӧ��������</remarks>
        public void DWExistCorr(CCorrespondRiverNet pCorrespondRiverNet, CParameterThreshold pParameterThreshold, CRiver pBSRiver)
        {
            //pBSRiver.CResultPtLt = new List<CPoint>();
            //CRiver pSSRiver = pBSRiver.CCorrRiver;
            //CMPBDP OptMPBDP = new CMPBDP();
            //if ((pBSRiver.CCorrTriJunctionPtLt != null) &&
            //    (pBSRiver.CCorrTriJunctionPtLt.Count != 0) &&
            //    (pBSRiver.CCorrTriJunctionPtLt.Count == pSSRiver.CCorrTriJunctionPtLt.Count))
            //{
            //    //����׼��
            //    List<CPoint> pBSCorrTriJunctionPtLt = new List<CPoint>();
            //    pBSCorrTriJunctionPtLt.Add(pBSRiver.CptLt[0]);
            //    pBSCorrTriJunctionPtLt.AddRange(pBSRiver.CCorrTriJunctionPtLt);
            //    pBSCorrTriJunctionPtLt.Add(pBSRiver.CptLt[pBSRiver.CptLt.Count - 1]);

            //    List<CPoint> pSSCorrTriJunctionPtLt = new List<CPoint>();
            //    pSSCorrTriJunctionPtLt.Add(pSSRiver.CptLt[0]);
            //    pSSCorrTriJunctionPtLt.AddRange(pSSRiver.CCorrTriJunctionPtLt);
            //    pSSCorrTriJunctionPtLt.Add(pSSRiver.CptLt[pSSRiver.CptLt.Count - 1]);

            //    List<CPolyline> pBSsubcpllt = new List<CPolyline>();
            //    List<CPolyline> pSSsubcpllt = new List<CPolyline>();
            //    for (int i = 0; i < pBSCorrTriJunctionPtLt.Count - 1; i++)
            //    {
            //        CPolyline pBSsubcpl = pBSRiver.GetSubPolyline(pBSCorrTriJunctionPtLt[i], pBSCorrTriJunctionPtLt[i + 1]);
            //        CPolyline pSSsubcpl = pSSRiver.GetSubPolyline(pSSCorrTriJunctionPtLt[i], pSSCorrTriJunctionPtLt[i + 1]);

            //        MessageBox.Show("CMRL.cs: Row499 is needed to be improved");
            //        //OptMPBDP.DivideCplForDP(pBSsubcpl);
            //        //OptMPBDP.DivideCplForDP(pSSsubcpl);

            //        pBSsubcpllt.Add(pBSsubcpl);
            //        pSSsubcpllt.Add(pSSsubcpl);
            //    }

            //    CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper();
            //    CParameterThreshold ParameterThreshold = new CParameterThreshold();
            //    //double dblSumLength = frcpl.pPolyline.Length + tocpl.pPolyline.Length;
            //    CTranslation pTranslation = new CTranslation();
            //    double dblMin = double.MaxValue;
            //    int intIndex = 0;
            //    for (int i = 0; i < 25; i++)
            //    {
            //        ParameterThreshold.dblDLengthBound = pParameterThreshold.dblLengthSumRatio * (1 - 0.02 * i);
            //        ParameterThreshold.dblULengthBound = pParameterThreshold.dblLengthSumRatio / (1 - 0.02 * i);

            //        List<CPoint> ResultPtLt = new List<CPoint>();
            //        for (int j = 0; j < pBSsubcpllt.Count; j++)
            //        {
            //            //��������ƥ�䣬��ȡ��Ӧ�߶�
            //            C5.LinkedList<CCorrSegment> CorrespondSegmentLk = new C5.LinkedList<CCorrSegment>();
            //            MessageBox.Show("CMRL.cs: Row523 is needed to be improved");
            //            //OptMPBDP.SubPolylineMatch(pBSsubcpllt[j], pSSsubcpllt[j], ParameterThreshold, ref CorrespondSegmentLk);

            //            //��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��                
            //            ResultPtLt.AddRange(pAlgorithmsHelper.BuildPointCorrespondence(CorrespondSegmentLk, "Linear"));

            //            if ((j > 0) && (i < pBSsubcpllt.Count - 1))
            //            {   //���ǵ��߶���Ӵ��Ķ��㱻��������Ӧ�㣬���ɾ��һ�Զ�Ӧ��
            //                ResultPtLt.RemoveAt(ResultPtLt.Count - 1);
            //            }
            //        }

            //        double dblTranslation = pTranslation.CalTranslation(ResultPtLt);
            //        if (dblTranslation < dblMin)
            //        {
            //            intIndex = i;
            //            dblMin = dblTranslation;
            //        }
            //    }

            //    //�����ѽ�
            //    ParameterThreshold.dblDLengthBound = pParameterThreshold.dblLengthSumRatio * (1 - 0.02 * intIndex);
            //    ParameterThreshold.dblULengthBound = pParameterThreshold.dblLengthSumRatio / (1 - 0.02 * intIndex);
            //    List<CPoint> pResultPtLt = new List<CPoint>();
            //    for (int j = 0; j < pBSsubcpllt.Count; j++)
            //    {
            //        //��������ƥ�䣬��ȡ��Ӧ�߶�
            //        C5.LinkedList<CCorrSegment> CorrespondSegmentLk = new C5.LinkedList<CCorrSegment>();
            //        MessageBox.Show("CMRL.cs: Row551 is needed to be improved");
            //        //OptMPBDP.SubPolylineMatch(pBSsubcpllt[j], pSSsubcpllt[j], ParameterThreshold, ref CorrespondSegmentLk);

            //        //��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��                
            //        pResultPtLt.AddRange(pAlgorithmsHelper.BuildPointCorrespondence(CorrespondSegmentLk, "Linear"));

            //        if ((j > 0) && (j < pBSsubcpllt.Count - 1))
            //        {   //���ǵ��߶���Ӵ��Ķ��㱻��������Ӧ�㣬���ɾ��һ�Զ�Ӧ��
            //            pResultPtLt.RemoveAt(pResultPtLt.Count - 1);
            //        }
            //    }

            //    pBSRiver.CResultPtLt = pResultPtLt;
            //    pCorrespondRiverNet.CResultPtLtLt.Add(pResultPtLt);


            //}
            //else
            //{
            //    CPolyline pBScpl = new CPolyline(pBSRiver);
            //    CPolyline pSScpl = new CPolyline(pSSRiver);
            //    MessageBox.Show("CMRL.cs: Row570 is needed to be improved");
            //    //pBSRiver.CResultPtLt = OptMPBDP.DWByDP(pBScpl, pSScpl, pParameterThreshold.dblLengthSumRatio, "Linear");
            //    pCorrespondRiverNet.CResultPtLtLt.Add(pBSRiver.CResultPtLt);
            //}
        }

        /// <summary>������һ������û�ж�Ӧ���������</summary>
        /// <param name="pCorrespondRiverNet">��Ӧ��������</param>
        /// <param name="pBSRiver">��ǰ������߱�����</param>
        /// <param name="dblMainReductionRatio">��һ��������������</param>
        /// <remarks>RecursiveDWNotExistCorr��RecursiveDealWithNotExistCorrepondenceRiver</remarks>
        private void RecursiveDWNotExistCorr(CCorrespondRiverNet pCorrespondRiverNet, CRiver pBSRiver, double dblMainReductionRatio)
        {
            CRiver pMainStream = pBSRiver.CMainStream;
            IPoint intersectipt = pBSRiver.pPolyline.ToPoint;
            double dblFromStartLength = CGeoFunc.CalDistanceFromStartPoint((IPolyline5)pMainStream, intersectipt, false);
            double dblCurrentReductionRatio = pMainStream.pPolyline.Length / dblFromStartLength;
            pBSRiver.dblReductionRatio = dblCurrentReductionRatio * dblMainReductionRatio;
            //pCorrespondRiverNet.CResultRiverLt.Add(pBSRiver);

            //����ǰ������֧��
            for (int i = 0; i < pBSRiver.CTributaryLt.Count; i++)
            {
                RecursiveDWNotExistCorr(pCorrespondRiverNet, pBSRiver.CTributaryLt[i], pBSRiver.dblReductionRatio);
            }
        }



      
        #endregion     

        /// <summary>���ԣ�������</summary>
        public CParameterResult ParameterResult
        {
            get { return _ParameterResult; }
            set { _ParameterResult = value; }
        }
    }
}
