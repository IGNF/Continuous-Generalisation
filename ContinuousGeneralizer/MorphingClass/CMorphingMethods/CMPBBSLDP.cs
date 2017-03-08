using System;
using System.Linq;
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
    /// ���������ṹ����״Ҫ��Morphing�任�������Գ���Ϊƥ�����ݣ�Length��������һ������BLG���ķ���
    /// </summary>
    /// <remarks>
    /// </remarks>
    public class CMPBBSLDP
    {
        
        
        private CParameterResult _ParameterResult;
        private CTriangulator _Triangulator = new CTriangulator();

        private List<CPolyline> _LSCPlLt = new List<CPolyline>();  //BS:LargerScale
        private List<CPolyline> _SSCPlLt = new List<CPolyline>();  //SS:SmallerScale

        private CPolyline _FromCpl;
        private CPolyline _ToCpl;

        private CParameterInitialize _ParameterInitialize;

        public CMPBBSLDP(CPolyline frcpl, CPolyline tocpl)
        {
            _FromCpl = frcpl;
            _ToCpl = tocpl;
        }

        public CMPBBSLDP(CParameterInitialize ParameterInitialize)
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
            _LSCPlLt = CHelperFunction.GetCPlLtByFeatureLayer(pBSFLayer);
            _SSCPlLt = CHelperFunction.GetCPlLtByFeatureLayer(pSSFLayer);
        }

        //����������Morphing������ɾ��С����������
        public void MPBBSLDPMorphing()
        {
            //CParameterInitialize ParameterInitialize = _ParameterInitialize;
            //CMPBBSL OptMPBBSL = new CMPBBSL();
            ////OptMPBBSL.ParameterInitialize = ParameterInitialize;
            //CGeometricMethods.SetCPlScaleEdgeLengthPtBelong(ref _LSCPlLt, CEnumScale.Larger);
            //CGeometricMethods.SetCPlScaleEdgeLengthPtBelong(ref _SSCPlLt, CEnumScale.Smaller);
            //CPolyline frcpl = _LSCPlLt[0];
            //CPolyline tocpl = _SSCPlLt[0];

            ////���㼫Сֵ
            // 
            //long lngStartTime = System.Environment.TickCount;  //��ʼʱ��



            //////������ֵ����
            ////double dblBound = 0.94;
            ////CParameterThreshold ParameterThreshold = new CParameterThreshold();
            ////ParameterThreshold.dblFrLength = frcpl.pPolyline.Length;
            ////ParameterThreshold.dblToLength = tocpl.pPolyline.Length;
            ////ParameterThreshold.dblDLengthBound = dblBound;
            ////ParameterThreshold.dblULengthBound = 1 / dblBound;

            //////**************�����������**************//
            ////CMPBBSL OptMPBBSL = new CMPBBSL();
            ////CParameterVariable pParameterVariableFrom = new CParameterVariable(frcpl, "FromCDT", ParameterInitialize.pBSFLayer, dblVerySmall);
            ////CParameterVariable pParameterVariableTo = new CParameterVariable(tocpl, "ToCDT", ParameterInitialize.pSSFLayer, dblVerySmall);
            ////C5.LinkedList<CCorrespondSegment> pBSCorrespondSegmentLk = new C5.LinkedList<CCorrespondSegment>();
            ////pBSCorrespondSegmentLk.AddRange(OptMPBBSL.DWByMPBBSL(ParameterInitialize, pParameterVariableFrom, pParameterVariableTo, ParameterThreshold));

            //List<CPoint> frchcptlt = _Triangulator.CreateConvexHullEdgeLt2(frcpl, dblVerySmall);
            //CPolyline frchcpl = new CPolyline(0, frchcptlt);    //��������������������߶�
            //frchcpl.SetPolyline();

            //List<CPoint> tochcptlt = _Triangulator.CreateConvexHullEdgeLt2(tocpl, dblVerySmall);
            //CPolyline tochcpl = new CPolyline(0, tochcptlt);    //С�������������������߶�
            //tochcpl.SetPolyline();

            ////���Լ����������ͼ�㣬�Ա�������AE�еĹ���(ct:constraint)
            //List<CPolyline> frctcpllt = new List<CPolyline>(); frctcpllt.Add(frcpl); frctcpllt.Add(frchcpl);
            //List<CPolyline> toctcpllt = new List<CPolyline>(); toctcpllt.Add(tocpl); toctcpllt.Add(tochcpl);
            //IFeatureLayer pBSFLayer = CHelperFunction.SaveCPlLt(frctcpllt, "frctcpllt", ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);
            //IFeatureLayer pSSFLayer = CHelperFunction.SaveCPlLt(toctcpllt, "toctcpllt", ParameterInitialize.pWorkspace, ParameterInitialize.m_mapControl);

            //////**************�����������**************//
            ////����CDT����ȡ����ɭ��
            //CBendForest FromLeftBendForest = new CBendForest();
            //CBendForest FromRightBendForest = new CBendForest();
            //CParameterVariable pParameterVariableFrom = new CParameterVariable(frcpl, "FromCDT", pBSFLayer, dblVerySmall);
            //OptMPBBSL.GetBendForest(pParameterVariableFrom, ref FromLeftBendForest, ref FromRightBendForest, ParameterInitialize);//��ȡ���ߵ�����ɭ��

            //CBendForest ToLeftBendForest = new CBendForest();
            //CBendForest ToRightBendForest = new CBendForest();
            //CParameterVariable pParameterVariableTo = new CParameterVariable(tocpl, "ToCDT", pSSFLayer, dblVerySmall);
            //OptMPBBSL.GetBendForest(pParameterVariableTo, ref ToLeftBendForest, ref ToRightBendForest, ParameterInitialize);

            ////��������ɭ�֣������������
            //OptMPBBSL.NeatenBendForest(frcpl, FromLeftBendForest);
            //OptMPBBSL.NeatenBendForest(frcpl, FromRightBendForest);
            //OptMPBBSL.NeatenBendForest(tocpl, ToLeftBendForest);
            //OptMPBBSL.NeatenBendForest(tocpl, ToRightBendForest);

            ////double dblSumLength = frcpl.pPolyline.Length + tocpl.pPolyline.Length;
            //CTranslation pTranslation = new CTranslation();
            //CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper();//���õ��ö���ĺ���
            //CMPBDPBL OptMPBDPBL = new CMPBDPBL();//����

            ////��ֵ����
            //CParameterThreshold ParameterThreshold = new CParameterThreshold();
            //ParameterThreshold.dblFrLength = frcpl.pPolyline.Length;
            //ParameterThreshold.dblToLength = tocpl.pPolyline.Length;
            //ParameterThreshold.dblAngleBound = 0.262;

            //List<double> dblTranslationLt = new List<double>(26);
            //SortedDictionary<double, int> ResultsSlt = new SortedDictionary<double, int>(new CCompareDbl());
            ////ѭ���ķ����ʺ�������ֵ
            //for (int i = 0; i <= 25; i++)
            //{
            //    double dblBound = 1 - 0.02 * i;
            //    ParameterThreshold.dblDLengthBound = dblBound;
            //    ParameterThreshold.dblULengthBound = 1 / dblBound;

            //    //������ƥ�䣬Ѱ�Ҷ�Ӧ��������
            //    List<CCorrespondBend> IndependCorrespondBendLt = new List<CCorrespondBend>();
            //    IndependCorrespondBendLt.AddRange(OptMPBBSL.BendTreeMatch2(FromLeftBendForest, ToLeftBendForest, ParameterThreshold));
            //    IndependCorrespondBendLt.AddRange(OptMPBBSL.BendTreeMatch2(FromRightBendForest, ToRightBendForest, ParameterThreshold));

            //    //����ƥ�䣬Ѱ�Ҷ�Ӧ����
            //    List<CCorrespondBend> CorrespondBendLt = OptMPBBSL.BendMatch(IndependCorrespondBendLt, ParameterThreshold);

            //    //��ȡ��Ӧ�߶�
            //    //C5.LinkedList<CCorrespondSegment> BSCorrespondSegmentLk = CHelperFunction.DetectCorrespondSegment(frcpl, tocpl, CorrespondBendLt);

            //    //**************BLG���������**************//
            //    //����BLG�����������ն�Ӧ�߶�                
            //    //C5.LinkedList<CCorrespondSegment> BLGCorrespondSegmentLk = new C5.LinkedList<CCorrespondSegment>();
            //    for (int j = 0; j < BSCorrespondSegmentLk.Count; j++)//AddRange�������ã���������
            //    {
            //        BLGCorrespondSegmentLk.AddAll(OptMPBDPBL.DWByDPLDefine(BSCorrespondSegmentLk[j].CFrPolyline, BSCorrespondSegmentLk[j].CToPolyline, ParameterThreshold));
            //    }

            //    //��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��                
            //    //List<CPoint> ResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(BLGCorrespondSegmentLk, "Linear");
            //    double dblTranslation = pTranslation.CalTranslation(ResultPtLt);
            //    dblTranslationLt.Add(dblTranslation);

            //    ResultsSlt.Add(dblTranslation, i);
            //}


            ////����������һ�飡����������
            ////���ɣ��������SortedList<double, CParameterResult> ResultsSlt = new SortedList<double, CParameterResult>(new CCompareDbl())��¼�����
            ////      �����ڻ�����λ��CPoint�����Ƶ���ָ�룩������ȻӰ��CParameterResult�е�ResultPtLtֵ
            //int intIndex = ResultsSlt.ElementAt(0).Value;
            //ParameterThreshold.dblDLengthBound = 1 * (1 - 0.02 * intIndex);
            //ParameterThreshold.dblULengthBound = 1 / (1 - 0.02 * intIndex);

            ////ParameterThreshold.dblDLengthBound = 0.96;
            ////ParameterThreshold.dblULengthBound = 1/0.96;

            ////������ƥ�䣬Ѱ�Ҷ�Ӧ��������
            //List<CCorrespondBend> pIndependCorrespondBendLt = new List<CCorrespondBend>();
            //pIndependCorrespondBendLt.AddRange(OptMPBBSL.BendTreeMatch2(FromLeftBendForest, ToLeftBendForest, ParameterThreshold));
            //pIndependCorrespondBendLt.AddRange(OptMPBBSL.BendTreeMatch2(FromRightBendForest, ToRightBendForest, ParameterThreshold));

            ////����ƥ�䣬Ѱ�Ҷ�Ӧ����
            //List<CCorrespondBend> pCorrespondBendLt = OptMPBBSL.BendMatch(pIndependCorrespondBendLt, ParameterThreshold);

            ////��ȡ��Ӧ�߶�
            //C5.LinkedList<CCorrespondSegment> pBSCorrespondSegmentLk = CHelperFunction.DetectCorrespondSegment(frcpl, tocpl, pCorrespondBendLt);

            ////����BLG�����������ն�Ӧ�߶�
            //C5.LinkedList<CCorrespondSegment> pBLGCorrespondSegmentLk = new C5.LinkedList<CCorrespondSegment>();
            //for (int j = 0; j < pBSCorrespondSegmentLk.Count; j++)
            //{
            //    pBLGCorrespondSegmentLk.AddAll(OptMPBDPBL.DWByDPLDefine(pBSCorrespondSegmentLk[j].CFrPolyline, pBSCorrespondSegmentLk[j].CToPolyline, ParameterThreshold));
            //}

            ////��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��                
            //List<CPoint> pResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(pBLGCorrespondSegmentLk, "Linear"); 

            ////���㲢��ʾ����ʱ��
            //long lngEndTime = System.Environment.TickCount;
            //long lngTime = lngEndTime - lngStartTime;
            //_ParameterInitialize.tsslTime.Text = "Running Time: " + Convert.ToString(lngTime) + "ms";  //��ʾ����ʱ��

            ////�����Ӧ��
            //CHelperFunctionExcel.ExportDataltToExcel(dblTranslationLt, "translationlt0", _ParameterInitialize.strSavePath);
            //CHelperFunction.SaveCtrlLine(pBLGCorrespondSegmentLk, "BSBLGControlLine", dblVerySmall, _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);
            //CHelperFunction.SaveCorrespondLine(pResultPtLt, "BSBLGCorrLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);

            ////��ȡ�����ȫ����¼��ParameterResult��
            //CParameterResult ParameterResult = new CParameterResult();
            //ParameterResult.FromCpl = frcpl;
            //ParameterResult.ToCpl = tocpl;
            //double pdblTranslation = pTranslation.CalTranslation(pResultPtLt);
            //ParameterResult.dblTranslation = pdblTranslation;
            //ParameterResult.pCorrespondSegmentLk = pBLGCorrespondSegmentLk;
            //ParameterResult.CResultPtLt = pResultPtLt;
            //ParameterResult.lngTime = lngTime;
            //_ParameterResult = ParameterResult;
        }


        /// <summary>���ԣ�������</summary>
        public CParameterResult ParameterResult
        {
            get { return _ParameterResult; }
            set { _ParameterResult = value; }
        }
    }
}
