using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;


using MorphingClass.CEntity;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;


namespace GeneralizationClass.CGeneralizationMethods
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>�˼��ܶȲ���ĺ�ϵ�򻯣�Generalization of Drainage Network with Density Differences(������,2006,���ѧ��)</remarks>
    public class CGDNWDD
    {
        
        
        private CParameterResult _ParameterResult;

        private List<CPolyline> _CPlLt;

        private CParameterInitialize _ParameterInitialize;

        public CGDNWDD()
        {

        }

        public CGDNWDD(CParameterInitialize ParameterInitialize)
        {

            //��ȡ��ǰѡ��ĵ�Ҫ��ͼ��
            IFeatureLayer pFeatureLayer = (IFeatureLayer)ParameterInitialize.m_mapFeature.get_Layer(ParameterInitialize.cboLayer.SelectedIndex);
                                                                      
            ParameterInitialize.pFeatureLayer = pFeatureLayer;
            ParameterInitialize.dblLevelExponent = Convert.ToDouble(ParameterInitialize.txtLevelExponent.Text);
            ParameterInitialize.dblOrderExponent = Convert.ToDouble(ParameterInitialize.txtOrderExponent.Text);
            _ParameterInitialize = ParameterInitialize;

            //��ȡ������
            _CPlLt = CHelperFunction.GetCPlLtByFeatureLayer(pFeatureLayer);

        }

        public void GDNWDDGeneralization()
        {

            //List<CPolyline> CPlLt = _CPlLt;
            //CParameterInitialize pParameterInitialize = _ParameterInitialize;

            //CParameterThreshold pParameterThreshold = new CParameterThreshold();
            //pParameterThreshold.dblVerySmall = CGeometricMethods.CalVerySmall(CPlLt);
            //List<CRiverNet> CRiverNetLt = BuildRiverNetLt(CPlLt, pParameterThreshold);     //���������ݽ�������
            //CalWeightiness(CRiverNetLt, pParameterInitialize);                              //�����������Ҫ��


            ////�������еĺ������ݽ����ȡ�������Ժ����е���ʽ�洢
            //List<CRiver> CResultRiverLt = new List<CRiver>();
            //for (int i = 0; i < CRiverNetLt.Count; i++)
            //{
            //    CResultRiverLt.AddRange(CRiverNetLt[i].CRiverLt);
            //}
            //CGeometricMethods.CalWeightinessUnitary(CResultRiverLt);



            ////��ȡ�����ȫ����¼��_ParameterResult��
            //CParameterResult ParameterResult = new CParameterResult();
            //ParameterResult.CResultRiverLt = CResultRiverLt;
            //_ParameterResult = ParameterResult;
        }

        #region ���������ݽ���������(����������֧��ϵ����Ӹ�֧����Ա)
        /// <summary>���������ݽ���������(����������֧��ϵ����Ӹ�֧����Ա)</summary>
        /// <param name="CPlLt">������</param>
        /// <param name="pParameterThreshold">��ֵ����</param>
        /// <returns>������</returns>
        public List<CRiverNet> BuildRiverNetLt(List<CPolyline> CPlLt, CParameterThreshold pParameterThreshold)
        {

            double dblVerySmall = CConstants.dblVerySmall;

            //�������������ɺ�������
            List<CRiver> CAllRiverLt = new List<CRiver>();
            for (int i = 0; i < CPlLt.Count; i++)
            {
                CRiver pRiver = new CRiver(i, CPlLt[i], dblVerySmall);
                CAllRiverLt.Add(pRiver);
            }

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

        /// <summary>����������ͬʱ����¼��Tocpt2�������˺����Ĳ�κ͵ȼ�</summary>
        /// <param name="CAllRiverLt">��������</param>
        /// <returns>������</returns>
        private List<CRiverNet> CreateRiverNetLt(List<CRiver> CAllRiverLt, double dblVerySmall)
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
                RecursiveSettleRiver(ref pRiverNet, CMasterRiverLt[i]);    //�ݹ�������ڸú����ĺ���
                //RecursiveFindTocpt2(CMasterRiverLt[i], dblVerySmall);      //�ҵ���ǰ���������֧�����ཻ��(���ڵ�ǰ����)������¼��֧������������
                CalLeverAndOrder(CMasterRiverLt[i]);
                pRiverNetLt.Add(pRiverNet);
            }

            return pRiverNetLt;
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

        /// <summary>��������Ĳ�κ͵ȼ�</summary>
        /// <param name="pMasterRiver">������</param>
        /// <remarks>�������Ĳ��Ϊ1��֧���Ĳ��Ϊ�����Ĳ��+1�������ĵȼ�Ϊ����������֧������֮���ټ�1</remarks>
        public void CalLeverAndOrder(CRiver pMasterRiver)
        {
            //�������Ĳ��Ϊ1
            pMasterRiver.dblLevel = 1;

            //����ȼ�
            for (int i = 0; i < pMasterRiver.CTributaryLt.Count; i++)
            {
                RecursiveCalLeverAndOrder(pMasterRiver.CTributaryLt[i]);
                pMasterRiver.dblOrder = pMasterRiver.dblOrder + pMasterRiver.CTributaryLt[i].dblOrder;
            }
            pMasterRiver.dblOrder = pMasterRiver.dblOrder + 1;
        }


        /// <summary>�ݹ��������Ĳ�κ͵ȼ�</summary>
        /// <param name="pRiver">��ǰ����</param>
        public void RecursiveCalLeverAndOrder(CRiver pRiver)
        {
            //������
            pRiver.dblLevel = pRiver.CMainStream.dblLevel + 1;

            //����ȼ�
            for (int i = 0; i < pRiver.CTributaryLt.Count; i++)
            {
                RecursiveCalLeverAndOrder(pRiver.CTributaryLt[i]);
                pRiver.dblOrder = pRiver.dblOrder + pRiver.CTributaryLt[i].dblOrder;
            }
            pRiver.dblOrder = pRiver.dblOrder + 1;


        }



        #endregion



        /// <summary>�����������Ҫ��</summary>
        /// <param name="CAllRiverLt">��������</param>
        private void CalWeightiness(List<CRiverNet> CRiverNetLt, CParameterInitialize pParameterInitialize)
        {
            //���ҵ������
            double dblMaxLevel = 0;
            for (int i = 0; i < CRiverNetLt.Count; i++)
            {
                for (int j = 0; j < CRiverNetLt[i].CRiverLt.Count; j++)
                {
                    if (CRiverNetLt[i].CRiverLt[j].dblLevel >dblMaxLevel)
                    {
                        dblMaxLevel = CRiverNetLt[i].CRiverLt[j].dblLevel;
                    }
                }
            }

            //���ݹ�ʽ���������Ҫ��
            for (int i = 0; i < CRiverNetLt.Count; i++)
            {
                for (int j = 0; j < CRiverNetLt[i].CRiverLt .Count; j++)
                {
                    double dblOrderWeightiness = Math.Pow(CRiverNetLt[i].CRiverLt[j].dblOrder, pParameterInitialize.dblOrderExponent);
                    double dblLevelWeightiness = Math.Pow((dblMaxLevel-CRiverNetLt[i].CRiverLt[j].dblLevel+1), pParameterInitialize.dblOrderExponent);
                    CRiverNetLt[i].CRiverLt[j].dblWeightiness = CRiverNetLt[i].CRiverLt[j].pPolyline.Length * dblOrderWeightiness * dblLevelWeightiness;
                }                
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
