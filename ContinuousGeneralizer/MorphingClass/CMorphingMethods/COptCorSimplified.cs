using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;


using MorphingClass.CEntity;
using MorphingClass.CEvaluationMethods ;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;
using MorphingClass.CMorphingAlgorithms;
using MorphingClass.CCorrepondObjects;

namespace MorphingClass.CMorphingMethods
{
    /// <summary>COptCorSimplified</summary>
    /// <remarks>
    /// </remarks>
    public class COptCorSimplified
    {
        private CPolyline _FromCpl;
        private CPolyline _ToCpl;

        //private CHelperFunction _pHelperFunction = new CHelperFunction();
        //private CHelperFunctionExcel _pHelperFunctionExcel = new CHelperFunctionExcel();
        //private CMathStatistic _MathStatistic = new CMathStatistic();
        private CLinearInterpolationA _LinearInterpolationA = new CLinearInterpolationA();
        private CParameterResult _ParameterResult;
        private CTranslation _Translation;
        private CDeflection _Deflection;
        private CPoint _StandardVetorCpt;


        private CParameterInitialize _ParameterInitialize;

        public COptCorSimplified()
        {

        }

        public COptCorSimplified(CParameterInitialize ParameterInitialize)
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
            List<CPolyline> _BSCPlLt = CHelperFunction.GetCPlLtByFeatureLayer(pBSFLayer);
            List<CPolyline> _SSCPlLt = CHelperFunction.GetCPlLtByFeatureLayer(pSSFLayer);

            _FromCpl = _BSCPlLt[0];
            _ToCpl = _SSCPlLt[0];
        }

        public void OptCorSimplifiedMorphing()
        {

            //CPolyline frcpl = _FromCpl;
            //CPolyline tocpl = _ToCpl;

            //frcpl.SetEdgeLength();
            //tocpl.SetEdgeLength();

            //int intMaxBackK = _ParameterInitialize.intMaxBackK;
            //double dblSmallDis = CGeometricMethods.CalSmallDis(frcpl);

            //List<CPolyline> CFrEdgeLt = CGeometricMethods.CreateCplLt(frcpl.cptlt);
            //List<CPolyline> CToEdgeLt = CGeometricMethods.CreateCplLt(tocpl.cptlt);
           
            //double dblX = tocpl.cptlt[0].X - frcpl.cptlt[0].X;
            //double dblY = tocpl.cptlt[0].Y - frcpl.cptlt[0].Y;
            //CPoint StandardVetorCpt = new CPoint(0, dblX, dblY);

            ////List<CPolyline> CFrEdgeLt = CGeometricMethods.CreateCplLt(frcpl.cptlt);
            ////List<CPolyline> CToEdgeLt = CGeometricMethods.CreateCplLt(tocpl.cptlt);

            ////CTable[,] T = CreatT(frcpl, tocpl, CFrEdgeLt, CToEdgeLt, intMaxBackK, StandardVetorCpt);  //����T����
            ////C5.LinkedList<CCorrespondSegment> pCorrespondSegmentLk = FindCorrespondSegmentLk(T, frcpl, tocpl, CFrEdgeLt, CToEdgeLt);

            //long lngStartTime = System.Environment.TickCount;  //��ʼʱ��
            //List<CPolyline> frlastcpllt = new List<CPolyline>();
            //List<CPolyline> tolastcpllt = new List<CPolyline>();
            //CTable[,] T = CreatT(frcpl, tocpl, CFrEdgeLt, CToEdgeLt, intMaxBackK, StandardVetorCpt);  //����T����
            //C5.LinkedList<CCorrespondSegment> pCorrespondSegmentLk = FindCorrespondSegmentLk(T, frcpl, tocpl, CFrEdgeLt, CToEdgeLt);

            ////��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��
            //CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper();
            //List<CPoint> pResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(pCorrespondSegmentLk, "Linear");

            ////���㲢��ʾ����ʱ��
            //long lngEndTime = System.Environment.TickCount;
            //long lngTime = lngEndTime - lngStartTime;
            //_ParameterInitialize.tsslTime.Text = "Running Time: " + Convert.ToString(lngTime) + "ms";  //��ʾ����ʱ��

            ////�����Ӧ��
            //CHelperFunction.SaveCtrlLine(pCorrespondSegmentLk, "OptCorSimplifiedControlLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);
            //CHelperFunction.SaveCorrespondLine(pResultPtLt, "OptCorSimplifiedCorrLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);

            ////��ȡ�����ȫ����¼��_ParameterResult��
            //CParameterResult ParameterResult = new CParameterResult();
            //ParameterResult.FromCpl = frcpl;
            //ParameterResult.ToCpl = tocpl;

            //ParameterResult.CResultPtLt = pResultPtLt;
            //ParameterResult.lngTime = lngTime;
            //_ParameterResult = ParameterResult;

        }

        /// <summary>��������Morphing����</summary>
        /// <remarks>����ָ���Ļ��ݲ���K�����ϼ�������Ľ����ֱ��Translationֵ�ȶ�</remarks>
        public void OptCorSimplifiedMultiResultsMorphing()
        {

            //CPolyline frcpl = _FromCpl;
            //CPolyline tocpl = _ToCpl;

            //int intMaxBackK = _ParameterInitialize.intMaxBackK;

            //List<CPolyline> CFrEdgeLt = CGeometricMethods.CreateCplLt(frcpl.cptlt);
            //List<CPolyline> CToEdgeLt = CGeometricMethods.CreateCplLt(tocpl.cptlt);

            //long lngStartTime = System.Environment.TickCount;  //��ʼʱ��
            //List<CPolyline> frlastcpllt = new List<CPolyline>();
            //List<CPolyline> tolastcpllt = new List<CPolyline>();

            //int intUpperbound = frcpl.cptlt.Count;
            //int intLowerbound = tocpl.cptlt.Count;

            //List<double> dblTranslationLt = new List<double>();
            //CTable[,] T = new CTable[intUpperbound, intLowerbound];
            //for (int i = 0; i < 5; i++)
            //{
            //    T = CreatT(frcpl, tocpl, CFrEdgeLt, CToEdgeLt, intMaxBackK);  //����T����                
            //    dblTranslationLt.Add(T[intUpperbound - 1, intLowerbound - 1].dblEvaluation);
            //    //if (dblTranslationLt.Count >= 5)
            //    //{
            //    //    int intCount = dblTranslationLt.Count;
            //    //    if ((dblTranslationLt[intCount - 1] == dblTranslationLt[intCount - 2]) && (dblTranslationLt[intCount - 1] == dblTranslationLt[intCount - 3]) &&
            //    //        (dblTranslationLt[intCount - 1] == dblTranslationLt[intCount - 4]) && (dblTranslationLt[intCount - 1] == dblTranslationLt[intCount - 5]))
            //    //    {
            //    //        break;
            //    //    }
            //    //}
            //    //break;
            //    intMaxBackK = intMaxBackK + 1;
            //}
            
            //C5.LinkedList<CCorrespondSegment> pCorrespondSegmentLk = FindCorrespondSegmentLk(T, frcpl, tocpl, CFrEdgeLt, CToEdgeLt);

            ////��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��
            //CAlgorithmsHelper pAlgorithmsHelper = new CAlgorithmsHelper();
            //List<CPoint> pResultPtLt = pAlgorithmsHelper.BuildPointCorrespondence(pCorrespondSegmentLk, "Linear");

            ////�����Ӧ��
            //CHelperFunctionExcel.ExportDataltToExcel(dblTranslationLt, "translationlt0", _ParameterInitialize.strSavePath);
            //CHelperFunction.SaveCtrlLine(pCorrespondSegmentLk, "OptCorSimplifiedControlLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);
            //CHelperFunction.SaveCorrespondLine(pResultPtLt, "OptCorSimplifiedCorrLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);

            ////���㲢��ʾ����ʱ��
            //long lngEndTime = System.Environment.TickCount;
            //long lngTime = lngEndTime - lngStartTime;
            //_ParameterInitialize.tsslTime.Text = "Running Time: " + Convert.ToString(lngTime) + "ms";  //��ʾ����ʱ��

            ////��ȡ�����ȫ����¼��_ParameterResult��
            //CParameterResult ParameterResult = new CParameterResult();
            //ParameterResult.FromCpl = frcpl;
            //ParameterResult.ToCpl = tocpl;

            //ParameterResult.CResultPtLt = pResultPtLt;
            //ParameterResult.lngTime = lngTime;
            //_ParameterResult = ParameterResult;

        }


        public C5.LinkedList<CCorrespondSegment> DWByOptCorSimplified(CPolyline frcpl, CPolyline tocpl, int intMaxBackK, CPoint StandardVetorCpt)
        {
            double dblSmallDis = CGeometricMethods.CalSmallDis(frcpl);

            List<CPolyline> CFrEdgeLt = CGeometricMethods.CreateCplLt(frcpl.cptlt);
            List<CPolyline> CToEdgeLt = CGeometricMethods.CreateCplLt(tocpl.cptlt);

            CTable[,] T = CreatT(frcpl, tocpl, CFrEdgeLt, CToEdgeLt, intMaxBackK, StandardVetorCpt);  //����T����
            C5.LinkedList<CCorrespondSegment> pCorrespondSegmentLk = FindCorrespondSegmentLk(T, frcpl, tocpl, CFrEdgeLt, CToEdgeLt);

            return pCorrespondSegmentLk;

        }
        /// <summary>����T����</summary>
        /// <param name="frcpl">���������״Ҫ��</param>
        /// <param name="tocpl">С��������״Ҫ��</param> 
        /// <param name="CFrEdgeLt">��������߶�</param>  
        ///  <param name="CToEdgeLt">С�������߶�</param> 
        /// <param name="frlastcpllt">��������յ��߶Σ�ֻ��һ���㣩</param> 
        /// <param name="tolastcpllt">С�������յ��߶Σ�ֻ��һ���㣩</param>
        /// <param name="intMaxBackK">����ϵ��</param> 
        /// <returns>T����</returns>
        public CTable[,] CreatT(CPolyline frcpl, CPolyline tocpl, List<CPolyline> CFrEdgeLt, List<CPolyline> CToEdgeLt, int intMaxBackK, CPoint StandardVetorCpt)
        {

            //ע�⣺T�����е���Ÿ�ԭ���㷨�е������ͳһ�ģ����������е������Ӧ��1
            CTable[,] T = new CTable[frcpl.cptlt.Count, tocpl.cptlt.Count];  //�߶�����Ϊ����������1

            //����ĵ�һ�м���һ�г�ʼ��
            T[0, 0] = new CTable();
            T[0, 0].dblEvaluation = 0;

            CPolyline frfirstcpl = new CPolyline(0, frcpl.cptlt[0]);  //����״Ҫ�صĵ�һ������Ϊ�߶�
            for (int j = 1; j <= CToEdgeLt.Count; j++)
            {
                T[0, j] = new CTable();
                T[0, j].dblEvaluation =-1;
            }

            for (int i = 1; i <= CFrEdgeLt.Count; i++)
            {
                T[i, 0] = new CTable();
                T[i, 0].dblEvaluation = -1;
            }


            int intI = CFrEdgeLt.Count;
            int intJ = CToEdgeLt.Count;
            if (intJ == 1)
            {
                T[intI, intJ] = new CTable();
                T[intI, intJ].frfrId = 1;
                T[intI, intJ].frtoId = intI;
                T[intI, intJ].tofrId = intJ;
                T[intI, intJ].intBackK = intI;
                CPolyline frcpli = frcpl.GetSubPolyline(CFrEdgeLt[intI - intI].cptlt[0], CFrEdgeLt[intI - 1].cptlt[1]);
                T[intI, intJ].dblEvaluation = T[0, 0].dblEvaluation + CalTDistance(frcpli, CToEdgeLt[intJ - 1], StandardVetorCpt);               
            }
            else if (intJ > 1)
            {
                //ѭ��������ά����T�еĸ���ֵ
                //ע�⣺T�е����1ָ����һ��Ԫ�أ�����LT�У���CFrEdgeLt,tolastcpllt�������1��ָ���ڶ���Ԫ��
                //ǰ�᣺�����ڵĶ�Ӧ��ϵΪÿ������С��������״Ҫ���ϵ��߶Ρ���Ӧһ���������ϴ��������״Ҫ���ϵ��߶Ρ����������ڡ����Ӧ�߶Ρ��򡰽ϴ��������״Ҫ���ϵ�һ���߶ζ�Ӧ�����С�������ϵ��߶Ρ�
                for (int i = 1; i <= CFrEdgeLt.Count - 1; i++)               //������ո�ֵ
                {
                    //�������������С��������״Ҫ���ϵ�һ���߶Ρ���Ӧ���ϴ��������״Ҫ��ǰi���߶Ρ��������ڡ�ǰ�ᡱ�����ϴ��������״Ҫ���ϵĵ�һ���߶Ρ����衰�����ڡ���ĳ����С��������״Ҫ���ϵ��߶Ρ�����˸ò��費�ܷ����������ѭ����
                    //�˴��Լ��������j��ѭ���У��ƺ����ǿ����������� CFrEdgeLt.Count-i>= CToEdgeLt.Count-j
                    T[i, 1] = new CTable();
                    T[i, 1].frfrId = 1;
                    T[i, 1].frtoId = i;
                    T[i, 1].tofrId = 1;
                    T[i, 1].intBackK = i;
                    CPolyline frcpli = frcpl.GetSubPolyline(CFrEdgeLt[0].cptlt[0], CFrEdgeLt[i - 1].cptlt[1]);
                    T[i, 1].dblEvaluation = T[0, 0].dblEvaluation + CalTDistance(frcpli, CToEdgeLt[0], StandardVetorCpt);

                    //�ڸ�ѭ�������������У�
                    //    1����j = CToEdgeLt.Count��Ϊ�������������С��������״Ҫ���ϵ����һ���߶Ρ���������Ӧ�Ķ�Ӧ�߶Σ���˸ò��費�ܷ����ѭ����
                    //    2����j <= i��������ǰ�ᣬ������j <= iʱ������ T[i, j]��������
                    for (int j = 2; (j <= CToEdgeLt.Count - 1 && j <= i); j++)
                    {
                        SortedDictionary<double, CTable> dblCTableSlt = new SortedDictionary<double, CTable>(new CDblDecCompare());
                        for (int k = 1; k <= intMaxBackK; k++)
                        {
                            //if�������������У�
                            //    1��(i - k >= 1)������տ�ʼִ��ʱ��֮ǰ�ѱ������߶������٣�����С��intMaxBackK
                            //    2�����ڡ�ǰ�ᡱ���ڽϴ��������״Ҫ����λ�ڻ��ݷ�Χ֮ǰ���߶�����(i - k)������ڽ�С��������״Ҫ����Ŀ���߶�j֮ǰ���߶�����(j - 1)
                            //��ˣ���������Ӧ��Ϊ(i - k >= 1) && ((i - k) >=(j-1))�����ǵ�j>=2����˵ڶ��������ȵ�һ�����������ϸ񣬿���ʡȥ��һ������
                            if ((i - k) >= (j - 1))
                            {
                                CTable table5i = new CTable();
                                table5i.frfrId = i - k + 1;
                                table5i.frtoId = i;
                                table5i.tofrId = j;
                                table5i.intBackK = k;
                                CPolyline frcplik = frcpl.GetSubPolyline(CFrEdgeLt[i - k].cptlt[0], CFrEdgeLt[i - 1].cptlt[1]);
                                table5i.dblEvaluation = T[i - k, j - 1].dblEvaluation + CalTDistance(frcplik, CToEdgeLt[j - 1], StandardVetorCpt);
                                dblCTableSlt.Add(table5i.dblEvaluation, table5i);
                            }
                            else
                            {
                                break;
                            }
                        }
                        T[i, j] = dblCTableSlt.ElementAt(0).Value;
                    }
                }

                //���һ��Ԫ�� 
                SortedDictionary<double, CTable> dblCTableSlt2 = new SortedDictionary<double, CTable>(new CDblDecCompare());
                for (int k = 1; k <= intMaxBackK; k++)
                {
                    //if�������������У�
                    //    1��(i - k >= 1)������տ�ʼִ��ʱ��֮ǰ�ѱ������߶������٣�����С��intMaxBackK
                    //    2�����ڡ�ǰ�ᡱ���ڽϴ��������״Ҫ����λ�ڻ��ݷ�Χ֮ǰ���߶�����(i - k)������ڽ�С��������״Ҫ����Ŀ���߶�j֮ǰ���߶�����(j - 1)
                    //��ˣ���������Ӧ��Ϊ(i - k >= 1) && ((i - k) >=(j-1))�����ǵ�j>=2����˵ڶ��������ȵ�һ�����������ϸ񣬿���ʡȥ��һ������
                    if ((intI - k) >= (intJ - 1))
                    {
                        CTable table5i = new CTable();
                        table5i.frfrId = intI - k + 1;
                        table5i.frtoId = intI;
                        table5i.tofrId = intJ;
                        table5i.intBackK = k;
                        CPolyline frcpli = frcpl.GetSubPolyline(CFrEdgeLt[intI - k].cptlt[0], CFrEdgeLt[intI - 1].cptlt[1]);
                        table5i.dblEvaluation = T[intI - k, intJ - 1].dblEvaluation + CalTDistance(frcpli, CToEdgeLt[intJ - 1], StandardVetorCpt);
                        dblCTableSlt2.Add(table5i.dblEvaluation, table5i);
                    }
                    else
                    {
                        break;
                    }
                }
                T[intI, intJ] = dblCTableSlt2.ElementAt(0).Value;
            }
           

            double dblTranslation = T[intI, intJ].dblEvaluation;
            return T;
        }


        /// <summary>�Ի��ݵķ�ʽѰ�Ҷ�Ӧ�߶�</summary>
        /// <param name="frcpl">���������״Ҫ��</param>
        /// <param name="tocpl">С��������״Ҫ��</param> 
        /// <param name="CFrEdgeLt">��������߶�</param>  
        ///  <param name="CToEdgeLt">С�������߶�</param> 
        /// <param name="frlastcpllt">��������յ��߶Σ�ֻ��һ���㣩</param> 
        /// <param name="tolastcpllt">С�������յ��߶Σ�ֻ��һ���㣩</param>  
        /// <returns>��Ӧ�߶�����</returns>
        public C5.LinkedList<CCorrespondSegment> FindCorrespondSegmentLk(CTable[,] T,  CPolyline frcpl, CPolyline tocpl, List<CPolyline> CFrEdgeLt, List<CPolyline> CToEdgeLt)
        {
            C5.LinkedList<CCorrespondSegment> CorrespondSegmentLk = new C5.LinkedList<CCorrespondSegment>();
            int i = CFrEdgeLt.Count;
            int j = CToEdgeLt.Count;

            while (i >= 0 && j >= 0)
            {
                CPolyline frcplw = new CPolyline();
                CPolyline tocplw = new CPolyline();
                if (i == 0 && j == 0)
                {
                    break;
                }               
                else  //�������е����
                {
                    frcplw = frcpl.GetSubPolyline(CFrEdgeLt[i - T[i, j].intBackK].cptlt[0], CFrEdgeLt[i - 1].cptlt[1]);
                    tocplw = CToEdgeLt[j - 1];
                    i = i - T[i, j].intBackK;
                    j = j - 1;
                }

                CCorrespondSegment pCorrespondSegment = new CCorrespondSegment();
                pCorrespondSegment = new CCorrespondSegment(frcplw, tocplw);
                CorrespondSegmentLk.Insert(0,pCorrespondSegment);
            }

            return CorrespondSegmentLk;

        }






        /// <summary>�������ֵ(Translationָ��ֵ)</summary>
        /// <param name="frcpl">��������߶Σ�����ֻ��һ������</param>
        /// <param name="tocpl">С�������߶Σ�����ֻ��һ������</param> 
        /// <returns>����ֵ</returns>
        public double CalTDistance(CPolyline frcpl, CPolyline tocpl,CPoint StandardVetorCpt)
        {
            //List<CPoint> cresultptlt = _LinearInterpolationA.CLI(frcpl, tocpl);  //ÿ�ζ��൱�ڴ����µ��߶Σ����ʹ��CLI
            ////_Translation = new CTranslation();
            ////double dblTranslation = _Translation.CalTranslation(cresultptlt);
            ////return dblTranslation;


            //double dblDeflection = _Deflection.CalDeflection(cresultptlt, StandardVetorCpt);
            //return dblDeflection;
            return 0;
        }

        /// <summary>���ԣ�������</summary>
        public CParameterResult ParameterResult
        {
            get { return _ParameterResult; }
            set { _ParameterResult = value; }
        }
    }
}
