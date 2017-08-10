using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

using MorphingClass.CUtility;
using MorphingClass.CGeometry;
using MorphingClass.CCorrepondObjects;

namespace MorphingClass.CMorphingAlgorithms
{
    public class CAlgorithmsHelper
    {




        /// <summary>
        /// ��ָ����ʽ�Զ�Ӧ�߶ν��е�ƥ�䣬��ȡ��Ӧ��
        /// </summary>
        /// <param name="CorrespondSegmentLk">��Ӧ�߶���</param>
        /// <param name="strCorrType">ƥ�䷽ʽ</param>
        public List<CPoint> BuildPointCorrespondence(LinkedList<CCorrespondSegment> CorrespondSegmentLk, string strCorrType)
        {            
            List<CPoint> ResultPtLt = new List<CPoint>();
            switch (strCorrType)
            {
                case "Linear":ResultPtLt= ByLinear(CorrespondSegmentLk); break;

                default:ResultPtLt= ByLinear(CorrespondSegmentLk); break;
            }

            return ResultPtLt;
        }






        /// <summary>
        /// ͨ����Ҫ�ػ�ȡ������
        /// </summary>
        /// <param name="cpl">��Ҫ��</param>
        /// <returns>������</returns>
        private List<CPoint> ByLinear(LinkedList<CCorrespondSegment> CorrespondSegmentLk)
        {
            int intCount = 1;
            foreach (CCorrespondSegment pCorrespondSegment in CorrespondSegmentLk)
            {
                intCount += (pCorrespondSegment.CFrPolyline.CptLt.Count + pCorrespondSegment.CToPolyline.CptLt.Count - 3);
            }
            List<CPoint> CResultPtLt = new List<CPoint>(intCount);
            ////��ӵ�һ����
            CPoint cpt0 =CorrespondSegmentLk.First.Value.CFrPolyline.CptLt[0];
            cpt0.CorrespondingPtLt = new List<CPoint>();
            //CorrespondSegmentLk[0].CToPolyline.CptLt[0].isCtrl = true;  //���Ϊ���Ƶ�
            cpt0.CorrespondingPtLt.Add(CorrespondSegmentLk.First.Value.CToPolyline.CptLt[0]);
            CResultPtLt.Add(cpt0);

            CLinearInterpolationA pLinearInterpolationA = new CLinearInterpolationA();
            foreach (CCorrespondSegment pCorrespondSegment in CorrespondSegmentLk)
            {
                pLinearInterpolationA.LI(pCorrespondSegment.CFrPolyline, pCorrespondSegment.CToPolyline, ref CResultPtLt);
            }

            //���һ����Ķ�Ӧ����������������������ͬ��Ԫ�أ�ɾ��������ظ�Ԫ�أ���������һ��Ԫ��
            for (int i = 0; i < CResultPtLt.Count; i++)
            {
                for (int j = CResultPtLt[i].CorrespondingPtLt.Count - 1; j >= 0; j--)
                {
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if (CResultPtLt[i].CorrespondingPtLt[j].Equals2D (CResultPtLt[i].CorrespondingPtLt[k]))
                        {
                            CResultPtLt[i].CorrespondingPtLt.RemoveAt(j);
                            break;
                        }
                    }
                }
            }


            return CResultPtLt;
        }






    }
}
