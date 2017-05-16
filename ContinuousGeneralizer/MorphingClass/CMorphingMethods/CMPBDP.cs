using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

using MorphingClass.CEvaluationMethods;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;
using MorphingClass.CMorphingAlgorithms;
using MorphingClass.CCorrepondObjects;

namespace MorphingClass.CMorphingMethods
{
    public class CMPBDP
    {
        private List<CPolyline> _LSCPlLt = new List<CPolyline>();  //BS:LargerScale
        private List<CPolyline> _SSCPlLt = new List<CPolyline>();  //SS:SmallerScale

        
        
        private CParameterResult _ParameterResult;

        private CPolyline _FromCpl;
        private CPolyline _ToCpl;

        private CParameterInitialize _ParameterInitialize;

        public CMPBDP()
        {

        }

        public CMPBDP(CParameterInitialize ParameterInitialize)
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


        public void MPBDPMorphing()
        {
            //CParameterInitialize ParameterInitialize = _ParameterInitialize;
            //CPolyline frcpl = _LSCPlLt[0];
            //CPolyline tocpl = _SSCPlLt[0];

            //DivideCplByDP(frcpl);
            //DivideCplByDP(tocpl);

            //double dblLengthSumRatio = frcpl.pPolyline.Length / tocpl.pPolyline.Length;
            ////double dblLengthBound = _ParameterInitialize.dblLengthBound;

            //CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper();
            ////double dblSumLength = frcpl.pPolyline.Length + tocpl.pPolyline.Length;
            //CTranslation pTranslation = new CTranslation();
            //CParameterThreshold ParameterThreshold = new CParameterThreshold();
            ////ParameterThreshold.dblLengthBound = dblLengthBound;

            //int intIndex = 0;
            //double dblMin = double.MaxValue;
            //List<double> dblTranslationLt = new List<double>();
            //for (int i = 0; i < 50; i++)
            //{
            //    ParameterThreshold.dblDLengthBound = dblLengthSumRatio * (1 - 0.02 * i);
            //    ParameterThreshold.dblULengthBound = dblLengthSumRatio / (1 - 0.02 * i);

            //    //��������ƥ�䣬��ȡ��Ӧ�߶�
            //    C5.LinkedList<CCorrespondSegment> CorrespondSegmentLk = new C5.LinkedList<CCorrespondSegment>();
            //    SubPolylineMatch(frcpl, tocpl, ParameterThreshold, ref CorrespondSegmentLk);//������ƥ��ͺ�������ƥ�䶼�ɴ˺�����ִ��

            //    //��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��                
            //    List<CPoint> ResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(CorrespondSegmentLk, "Linear");
            //    double dblTranslation = pTranslation.CalTranslation(ResultPtLt);
            //    dblTranslationLt.Add(dblTranslation);
            //    if (dblTranslation < dblMin)
            //    {
            //        intIndex = i;
            //        dblMin = dblTranslation;
            //    }
            //}


            ////double dblRatio = Convert.ToDouble(ParameterInitialize.txtLengthBound.Text);
            ////ParameterThreshold.dblDLengthBound = dblLengthSumRatio * dblRatio;
            ////ParameterThreshold.dblULengthBound = dblLengthSumRatio / dblRatio;


            //ParameterThreshold.dblDLengthBound = dblLengthSumRatio * (1 - 0.02 * intIndex);
            //ParameterThreshold.dblULengthBound = dblLengthSumRatio / (1 - 0.02 * intIndex);

            ////��������ƥ�䣬��ȡ��Ӧ�߶�
            //C5.LinkedList<CCorrespondSegment> pCorrespondSegmentLk = new C5.LinkedList<CCorrespondSegment>();
            //SubPolylineMatch(frcpl, tocpl, ParameterThreshold, ref pCorrespondSegmentLk);//������ƥ��ͺ�������ƥ�䶼�ɴ˺�����ִ��

            ////��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��
            //List<CPoint> pResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(pCorrespondSegmentLk, "Linear");


            ////�����Ӧ��
            ////CHelpFuncExcel.ExportDataltToExcel(dblTranslationLt, "translationlt0", _ParameterInitialize.strSavePath);
            //CHelpFunc.SaveCtrlLine(pCorrespondSegmentLk, "MPBDPControlLine", ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);
            //CHelpFunc.SaveCorrespondLine(pResultPtLt, "MPBDPCorrLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);

            ////��ȡ�����ȫ����¼��_ParameterResult��
            //CParameterResult ParameterResult = new CParameterResult();
            //ParameterResult.FromCpl = frcpl;
            //ParameterResult.ToCpl = tocpl;

            //ParameterResult.CResultPtLt = pResultPtLt;
            //_ParameterResult = ParameterResult;
        }


        ///// <summary>�û���BLG���ķ�������Morphing����</summary>
        ///// <param name="frcpl">������߱���߶�</param>
        ///// <param name="tocpl">������߱���߶�</param>
        ///// <param name="pParameterThreshold">��ֵ����</param>
        ///// <returns>��������Ϊͬһ����������true�����򷵻�false</returns>
        ///// <remarks>DWByDP:DealWithByDP
        ///// ��������Ҫ�����ⲿ��DP�㷨�ĵ���</remarks>
        //public List<CPoint> DWByDP(CPolyline frcpl, CPolyline tocpl, double dblLengthSumRatio, string strMethod)
        //{
        //    if (frcpl.pPolyline.Length ==0||tocpl.pPolyline.Length ==0)
        //    {
        //        List<CPoint> cptlt00 = new List<CPoint>();
        //        return cptlt00;
        //    }


        //    DivideCplByDP(frcpl);
        //    DivideCplByDP(tocpl);


        //    CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper();
        //    //double dblSumLength = frcpl.pPolyline.Length + tocpl.pPolyline.Length;
        //    CTranslation pTranslation = new CTranslation();
        //    CParameterThreshold ParameterThreshold = new CParameterThreshold();

        //    int intIndex = 0;
        //    double dblMin = double.MaxValue;
        //    List<double> dblTranslationLt = new List<double>();
        //    for (int i = 0; i < 25; i++)
        //    {
        //        ParameterThreshold.dblDLengthBound = dblLengthSumRatio * (1 - 0.02 * i);
        //        ParameterThreshold.dblULengthBound = dblLengthSumRatio / (1 - 0.02 * i);

        //        //��������ƥ�䣬��ȡ��Ӧ�߶�
        //        C5.LinkedList<CCorrespondSegment> CorrespondSegmentLk = new C5.LinkedList<CCorrespondSegment>();
        //        SubPolylineMatch(frcpl, tocpl, ParameterThreshold, ref CorrespondSegmentLk);

        //        //��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��                
        //        List<CPoint> ResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(CorrespondSegmentLk, strMethod);
        //        double dblTranslation = pTranslation.CalTranslation(ResultPtLt);
        //        dblTranslationLt.Add(dblTranslation);
        //        if (dblTranslation < dblMin)
        //        {
        //            intIndex = i;
        //            dblMin = dblTranslation;
        //        }
        //    }

        //    ParameterThreshold.dblDLengthBound = dblLengthSumRatio * (1 - 0.02 * intIndex);
        //    ParameterThreshold.dblULengthBound = dblLengthSumRatio / (1 - 0.02 * intIndex);


        //    //��������ƥ�䣬��ȡ��Ӧ�߶�
        //    C5.LinkedList<CCorrespondSegment> pCorrespondSegmentLk = new C5.LinkedList<CCorrespondSegment>();
        //    SubPolylineMatch(frcpl, tocpl, ParameterThreshold, ref pCorrespondSegmentLk);//������ƥ��ͺ�������ƥ�䶼�ɴ˺�����ִ��

        //    //��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��
        //    List<CPoint> pResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(pCorrespondSegmentLk, "Linear");

        //    return pResultPtLt;
        //}

        ///// <summary>�û���BLG���ķ�������Morphing���⣬ָ���߳��Ȳ���</summary>
        ///// <param name="frcpl">������߱���߶�</param>
        ///// <param name="tocpl">������߱���߶�</param>
        ///// <param name="pParameterThreshold">��ֵ����</param>
        ///// <returns>��������Ϊͬһ����������true�����򷵻�false</returns>
        ///// <remarks>DWByDP:DealWithByDP
        ///// ��������Ҫ�����ⲿ��DP�㷨�ĵ���</remarks>
        //public C5.LinkedList<CCorrespondSegment> DWByDPDefine(CPolyline frcpl, CPolyline tocpl, CParameterThreshold ParameterThreshold)
        //{
        //    if (frcpl.pPolyline.Length == 0 || tocpl.pPolyline.Length == 0)
        //    {
        //        C5.LinkedList<CCorrespondSegment> CorrespondSegmentLk00 = new C5.LinkedList<CCorrespondSegment>();
        //        return CorrespondSegmentLk00;
        //    }

        //    DivideCplByDP(frcpl);
        //    DivideCplByDP(tocpl);           

        //    //��������ƥ�䣬��ȡ��Ӧ�߶�
        //    C5.LinkedList<CCorrespondSegment> pCorrespondSegmentLk = new C5.LinkedList<CCorrespondSegment>();
        //    SubPolylineMatch(frcpl, tocpl, ParameterThreshold, ref pCorrespondSegmentLk);//������ƥ��ͺ�������ƥ�䶼�ɴ˺�����ִ��

        //    return pCorrespondSegmentLk;
        //}

        ///// <summary>�û���BLG���ķ�������Morphing���⣬ָ���߳��Ȳ���</summary>
        ///// <param name="frcpl">������߱���߶�</param>
        ///// <param name="tocpl">������߱���߶�</param>
        ///// <param name="pParameterThreshold">��ֵ����</param>
        ///// <returns>��������Ϊͬһ����������true�����򷵻�false</returns>
        ///// <remarks>DWByDP:DealWithByDP
        ///// ��������Ҫ�����ⲿ��DP�㷨�ĵ���</remarks>
        //public C5.LinkedList<CCorrespondSegment> DWByDPDefine(CPolyline frcpl, CPolyline tocpl, double dblBound)
        //{
        //    if (frcpl.pPolyline.Length == 0 || tocpl.pPolyline.Length == 0)
        //    {
        //        C5.LinkedList<CCorrespondSegment> CorrespondSegmentLk00 = new C5.LinkedList<CCorrespondSegment>();
        //        return CorrespondSegmentLk00;
        //    }

        //    DivideCplByDP(frcpl);
        //    DivideCplByDP(tocpl);

        //    CParameterThreshold ParameterThreshold = new CParameterThreshold();
        //    ParameterThreshold.dblDLengthBound = dblBound;
        //    ParameterThreshold.dblDLengthBound = 1/dblBound;

        //    //��������ƥ�䣬��ȡ��Ӧ�߶�
        //    C5.LinkedList<CCorrespondSegment> pCorrespondSegmentLk = new C5.LinkedList<CCorrespondSegment>();
        //    SubPolylineMatch(frcpl, tocpl, ParameterThreshold, ref pCorrespondSegmentLk);//������ƥ��ͺ�������ƥ�䶼�ɴ˺�����ִ��

        //    return pCorrespondSegmentLk;
        //}

        //public void DivideCplByDP(CPolyline dcpl)
        //{
        //    List<CPoint> dcptlt = dcpl.CptLt;
        //    int intptnum = dcptlt.Count;

        //    if (intptnum <= 2)
        //    {
        //        return;
        //    }

        //    //�ҵ������������Զ�ĵ�
        //    CEdge pEdge = new CEdge(dcptlt[0], dcptlt[intptnum - 1]);
        //    double dblMaxDis = -1;
        //    int intMaxDisIndex = 0;
        //    double dblAlongDis = 0;
        //    double dblFromDis = 0;
        //    IPoint outipt = new PointClass();
        //    bool blnright = new bool();
        //    for (int i = 1; i < intptnum - 1; i++)
        //    {
        //        pEdge.QueryPointAndDistance(esriSegmentExtension.esriExtendEmbedded, (IPoint)dcptlt[i], false, outipt, ref dblAlongDis, ref dblFromDis, ref blnright);
        //        if (dblFromDis > dblMaxDis)
        //        {
        //            dblMaxDis = dblFromDis;
        //            intMaxDisIndex = i;
        //        }
        //    }

        //    //�ֱ�������ӱ�ִ�зָ����
        //    dcpl.DivideByCpt(dcptlt[intMaxDisIndex]);
        //    DivideCplByDP(dcpl.CLeftPolyline);
        //    DivideCplByDP(dcpl.CRightPolyline);
        //}

        ///// <summary>�������߳��ȵ�ƥ�䷽��</summary>
        ///// <param name="CFrPolyline">������߱���߶�</param>
        ///// <param name="CToPolyline">������߱���߶�</param>
        ///// <param name="ParameterThreshold">��ֵ����</param>
        ///// <param name="CorrespondSegmentLk">��Ӧ�߶�</param>
        //public void SubPolylineMatch(CPolyline CFrPolyline, CPolyline CToPolyline, CParameterThreshold ParameterThreshold, ref C5.LinkedList<CCorrespondSegment> CorrespondSegmentLk)
        //{
        //    //�������һ���߶�û�к��ӣ���ֱ��ƥ�䲢����
        //    if (CFrPolyline.CLeftPolyline == null || CToPolyline.CLeftPolyline == null)
        //    {
        //        CCorrespondSegment CorrespondSegment = new CCorrespondSegment(CFrPolyline, CToPolyline);
        //        CorrespondSegmentLk.Add(CorrespondSegment);
        //        return;
        //    }

        //    double dblRatioLL = CFrPolyline.CLeftPolyline.pPolyline.Length / CToPolyline.CLeftPolyline.pPolyline.Length;
        //    double dblRatioRR = CFrPolyline.CRightPolyline.pPolyline.Length / CToPolyline.CRightPolyline.pPolyline.Length;

        //    int intInsertIndex = CorrespondSegmentLk.Count;

        //    if (dblRatioLL >= ParameterThreshold.dblDLengthBound && dblRatioLL <= ParameterThreshold.dblULengthBound &&
        //        dblRatioRR >= ParameterThreshold.dblDLengthBound && dblRatioRR <= ParameterThreshold.dblULengthBound)
        //    {
        //        //��Ӧ�߶γ������
        //        SubPolylineMatch(CFrPolyline.CLeftPolyline, CToPolyline.CLeftPolyline, ParameterThreshold, ref CorrespondSegmentLk);
        //        SubPolylineMatch(CFrPolyline.CRightPolyline, CToPolyline.CRightPolyline, ParameterThreshold, ref CorrespondSegmentLk);
        //    }
        //    else
        //    {
        //        CCorrespondSegment CorrespondSegment = new CCorrespondSegment(CFrPolyline, CToPolyline);
        //        CorrespondSegmentLk.Add(CorrespondSegment);
        //        return;
        //    }

        //}




        /// <summary>���ԣ�������</summary>
        public CParameterResult ParameterResult
        {
            get { return _ParameterResult; }
            set { _ParameterResult = value; }
        }
    }
}
