using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;


using MorphingClass.CEntity;
using MorphingClass.CEvaluationMethods;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;
using MorphingClass.CMorphingAlgorithms;
using MorphingClass.CCorrepondObjects;
using MorphingClass.CMorphingMethods.CMorphingMethodsBase;

namespace MorphingClass.CMorphingMethods
{
    /// <summary>COptCorMM</summary>
    /// <remarks>
    /// </remarks>
    public class COptCorMM : COptCor
    {

        public COptCorMM()
        {

        }

        public COptCorMM(CParameterInitialize ParameterInitialize)
        {
            Construct(ParameterInitialize);
            _intMaxBackK = Convert.ToInt32(ParameterInitialize.txtMaxBackK.Text);
            _intMulti = Convert.ToInt32(ParameterInitialize.txtMulti.Text);
            _intIncrease = Convert.ToInt32(ParameterInitialize.txtIncrease.Text);
            _BSCPlLt = _CPlLtLt[0];
            _SSCPlLt = _CPlLtLt[1];
        }

        public CParameterResult OptCorMMMorphing()
        {
            List<CPolyline> pBSCPlLt = _BSCPlLt;
            List<CPolyline> pSSCPlLt = _SSCPlLt;
            int intMaxBackK = _intMaxBackK;
            int intMulti = _intMulti;
            int intIncrease = _intIncrease;
            double dblSmallDis = _dblSmallDis;

            long lngStartTime1 = System.Environment.TickCount;  //lngTime1
            CGeometricMethods.SetEdgeLength(ref pBSCPlLt);
            CGeometricMethods.SetEdgeLength(ref pSSCPlLt);
            long lngTime1 = System.Environment.TickCount - lngStartTime1;  //lngTime1

            double dblX = pSSCPlLt[0].FrCpt.X - pBSCPlLt[0].FrCpt.X;
            double dblY = pSSCPlLt[0].FrCpt.Y - pBSCPlLt[0].FrCpt.Y;
            CPoint StandardVetorCpt = new CPoint(0, dblX, dblY);
            //StandardVetorCpt = new CPoint(0, 0, 0);
            _StandardVetorCpt = StandardVetorCpt;
            double dblStandardLength = CGeometricMethods.CalDis(0, 0, dblX, dblY);
            List<double> dblDistanceLt = new List<double>(intMulti);
            List<double> dblTimeLt = new List<double>(intMulti);

            List<LinkedList<CCorrespondCPoint>> pCorrCptLkLt = new List<LinkedList<CCorrespondCPoint>>(pBSCPlLt.Count);
            List<LinkedList<CCorrespondCPoint>> pCtrlCptLkLt = new List<LinkedList<CCorrespondCPoint>>(pBSCPlLt.Count);
            for (int i = 0; i < intMulti; i++)
            {
                long lngStartTime2 = System.Environment.TickCount;  //lngTime2
                double dblDistance = 0;
                pCorrCptLkLt = new List<LinkedList<CCorrespondCPoint>>(pBSCPlLt.Count);   //initialize
                pCtrlCptLkLt = new List<LinkedList<CCorrespondCPoint>>(pBSCPlLt.Count);    //initialize
                for (int j = 0; j < pBSCPlLt.Count; j++)
                {
                    CTable[,] Table = CreatTable(pBSCPlLt[j], pSSCPlLt[j], intMaxBackK, StandardVetorCpt, dblSmallDis);  //����T���� 

                    dblDistance += Table[pBSCPlLt[j].cptlt.Count - 1, pSSCPlLt[j].cptlt.Count - 1].dblEvaluation;
                    LinkedList<CCorrespondCPoint> CtrlCptLk;
                    pCorrCptLkLt.Add(FindCorrespondSegmentLk(Table, out CtrlCptLk));
                    pCtrlCptLkLt.Add(CtrlCptLk);
                }
                long lngTime2 = System.Environment.TickCount - lngStartTime2;   //lngTime2
                dblTimeLt.Add(lngTime1 + lngTime2);
                dblDistanceLt.Add(dblDistance);


                //�����Ӧ��
                CHelperFunctionExcel.ExportDataltToExcel(dblTimeLt, intMaxBackK + "Timelt0", _ParameterInitialize.strSavePath);
                CHelperFunctionExcel.ExportDataltToExcel(dblDistanceLt, intMaxBackK + "Distancelt0", _ParameterInitialize.strSavePath);

                if (i == (intMulti - 1))
                {
                    CHelperFunction.SaveCtrlLine(pCtrlCptLkLt, intMaxBackK + "OptCorMMControlLine", dblStandardLength, _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);
                    CHelperFunction.SaveCorrLine(pCorrCptLkLt, intMaxBackK + "OptCorMMCorrLine", _ParameterInitialize.pWorkspace, _ParameterInitialize.m_mapControl);
                }
                intMaxBackK = intMaxBackK + intIncrease;
                _CorrCptLkLt = pCorrCptLkLt;
            }

            return null;
        }

        /// <summary>����T����</summary>
        /// <param name="frcpl">���������״Ҫ��</param>
        /// <param name="tocpl">С��������״Ҫ��</param> 
        /// <param name="CFrEdgeLt">��������߶�</param>  
        ///  <param name="CToEdgeLt">С�������߶�</param> 
        /// <param name="frlastcpllt">��������յ��߶Σ�ֻ��һ���㣩</param> 
        /// <param name="tolastcpllt">С�������յ��߶Σ�ֻ��һ���㣩</param>
        /// <param name="intMaxBackK">����ϵ��</param> 
        /// <remarks>���ͷ��ڴ�</remarks> 
        /// <returns>T����</returns>
        public override CTable[,] CreatTable(CPolyline frcpl, CPolyline tocpl, int intMaxBackK, CPoint StandardVetorCpt, double dblSmallDis)
        {
            List<CPolyline> CFrEdgeLt = CGeometricMethods.CreateCplLt(frcpl.cptlt);
            List<CPolyline> CToEdgeLt = CGeometricMethods.CreateCplLt(tocpl.cptlt);

            int intFrPtNum = frcpl.cptlt.Count;
            int intToPtNum = tocpl.cptlt.Count;

            List<CPolyline> frlastcpllt = new List<CPolyline>(intFrPtNum - 1);
            List<CPolyline> tolastcpllt = new List<CPolyline>(intToPtNum - 1);

            //ע�⣺T�����е���Ÿ�ԭ���㷨�е������ͳһ�ģ����������е������Ӧ��1
            CTable[,] T = new CTable[intFrPtNum, intToPtNum];

            //T[0,0]
            T[0, 0] = new CTable();
            T[0, 0].dblEvaluation = 0;
            LinkedList<CCorrespondCPoint> CorrCptLk0 = new LinkedList<CCorrespondCPoint>();
            CorrCptLk0.AddLast(new CCorrespondCPoint(CFrEdgeLt[0].FrCpt, CToEdgeLt[0].FrCpt));
            T[0, 0].CorrCptLk = CorrCptLk0;

            CPolyline frfirstcpl = new CPolyline(0, CFrEdgeLt[0].FrCpt);  //����״Ҫ�صĵ�һ������Ϊ�߶�
            for (int j = 1; j < intToPtNum; j++)
            {
                T[0, j] = new CTable();
                T[0, j].intBackK1 = 0;
                T[0, j].intBackK2 = 1;
                LinkedList<CCorrespondCPoint> CorrCptLkj;
                T[0, j].dblEvaluation = T[0, j - 1].dblEvaluation + CalDistance(frfirstcpl, CToEdgeLt[j - 1], StandardVetorCpt, dblSmallDis, frcpl, tocpl, out CorrCptLkj);
                T[0, j].CorrCptLk = CorrCptLkj;
            }

            CPolyline tofirstcpl = new CPolyline(0, CToEdgeLt[0].FrCpt);  //����״Ҫ�صĵ�һ������Ϊ�߶�
            for (int i = 1; i < intFrPtNum; i++)
            {
                T[i, 0] = new CTable();
                T[i, 0].intBackK1 = 1;
                T[i, 0].intBackK2 = 0;
                LinkedList<CCorrespondCPoint> CorrCptLki;
                T[i, 0].dblEvaluation = T[i - 1, 0].dblEvaluation + CalDistance(CFrEdgeLt[i - 1], tofirstcpl, StandardVetorCpt, dblSmallDis, frcpl, tocpl, out CorrCptLki);
                T[i, 0].CorrCptLk = CorrCptLki;
            }

            //ѭ��������ά����T�еĸ���ֵ
            for (int i = 0; i < CFrEdgeLt.Count; i++)//��������߶��յ�����׼��
            {
                List<CPoint> frlastptlt = new List<CPoint>(1);
                frlastptlt.Add(CFrEdgeLt[i].ToCpt);
                frlastcpllt.Add(new CPolyline(0, frlastptlt));
            }

            for (int i = 0; i < CToEdgeLt.Count; i++)//С�������߶��յ�����׼��
            {
                List<CPoint> frlastptlt = new List<CPoint>(1);
                frlastptlt.Add(CToEdgeLt[i].ToCpt);
                tolastcpllt.Add(new CPolyline(0, frlastptlt));
            }

            //ע�⣺T�е����1ָ����һ��Ԫ�أ�����LT�У���CFrEdgeLt,tolastcpllt�������1��ָ���ڶ���Ԫ��
            for (int i = 1; i < intFrPtNum; i++)               //������ո�ֵ
            {
                int intBackKforI = Math.Min(i, intMaxBackK);   //����տ�ʼִ��ʱ��֮ǰ�ѱ������߶������٣�����С��intMaxBackK
                for (int j = 1; j < intToPtNum; j++)
                {
                    int intBackKforJ = Math.Min(j, intMaxBackK);   //����տ�ʼִ��ʱ��֮ǰ�ѱ������߶������٣�����С��intMaxBackK
                    List<CTable> CTableLt = new List<CTable>(intBackKforI * intBackKforJ);
                    for (int k1 = 1; k1 <= intBackKforI; k1++)
                    {
                        for (int k2 = 1; k2 <= intBackKforJ; k2++)
                        {
                            CTable tableij = new CTable();
                            tableij.intBackK1 = k1;
                            tableij.intBackK2 = k2;
                            LinkedList<CCorrespondCPoint> CorrCptLkij;
                            CPolyline frcpli = frcpl.GetSubPolyline(CFrEdgeLt[i - k1].FrCpt, CFrEdgeLt[i - 1].ToCpt);
                            CPolyline tocplj = tocpl.GetSubPolyline(CToEdgeLt[j - k2].FrCpt, CToEdgeLt[j - 1].ToCpt);
                            tableij.dblEvaluation = T[i - k1, j - k2].dblEvaluation + CalDistance(frcpli, tocplj, StandardVetorCpt, dblSmallDis, frcpl, tocpl, out CorrCptLkij);
                            tableij.CorrCptLk = CorrCptLkij;
                            CTableLt.Add(tableij);
                        }
                    }

                    //find the minimum one
                    T[i, j] = FindMinTable(CTableLt);
                }
            }

            return T;
        }

    }
}
