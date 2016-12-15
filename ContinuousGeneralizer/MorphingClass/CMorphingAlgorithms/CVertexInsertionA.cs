using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;

using MorphingClass.CUtility;
using MorphingClass.CGeometry;
namespace MorphingClass.CMorphingAlgorithms
{
    public class CVertexInsertionA
    {
        



        /// <summary>
        /// VI������ȡ��Ӧ�㣬�˺���ΪLI��������ں���(����һ�Զ�Ӧ�߶δ����ʱ��Ӵ˴�����)
        /// </summary>
        /// <param name="CFrPolyline">��������߶�</param>
        /// <param name="CToPolyline">С�������߶�</param>
        /// <param name="ResultPtLt">�������</param>
        /// <remarks>Ӧע������һ�������ÿһ�Զ�Ӧ�߶εĵ�һ���㶼����һ���д������</remarks>
        public List<CPoint> CVI(CPolyline CFrPolyline, CPolyline CToPolyline)
        {
            List<CPoint> ResultPtLt = new List<CPoint>();
            CPoint cfrpt0 = CFrPolyline.CptLt[0];
            cfrpt0.CorrespondingPtLt = new List<CPoint>();
            cfrpt0.CorrespondingPtLt.Add(CToPolyline.CptLt[0]);
            ResultPtLt.Add(cfrpt0);
            VI(CFrPolyline, CToPolyline, ref ResultPtLt);
            return ResultPtLt;
        }

        /// <summary>
        /// ��LI������ȡ��Ӧ��(�Զ��Ӧ�߶δ����ʱ��Ӵ˴�����)
        /// </summary>
        /// <param name="CFrPolyline">��������߶�</param>
        /// <param name="CToPolyline">С�������߶�</param>
        /// <param name="ResultPtLt">�������</param>
        /// <remarks>Ӧע������һ�������ÿһ�Զ�Ӧ�߶εĵ�һ���㶼����һ���д������</remarks>
        public void VI(CPolyline CFrPolyline, CPolyline CToPolyline, ref List<CPoint> ResultPtLt)
        {
            List<CPoint> frptlt = CFrPolyline.CptLt;
            List<CPoint> toptlt = CToPolyline.CptLt;
            for (int i = 1; i < frptlt.Count; i++)
                frptlt[i].CorrespondingPtLt = new List<CPoint>();

            //����һ���߶�ֻ��һ���������
            if (frptlt.Count == 1)
            {
                for (int i = 1; i < toptlt.Count; i++)
                    frptlt[0].CorrespondingPtLt.Add(toptlt[i]);
                ResultPtLt.Add(frptlt[0]);
                return;
            }
            else if (toptlt.Count == 1)
            {
                for (int i = 1; i < frptlt.Count; i++)
                {
                    frptlt[i].CorrespondingPtLt.Add(toptlt[0]);
                    ResultPtLt.Add(frptlt[i]);
                }
                return;
            }

            //Ϊ��Ӱ��ԭ�������ݣ������µĵ���
            List<CPoint> nfrptlt = new List<CPoint>();
            for (int i = 0; i < frptlt.Count ; i++)
                nfrptlt.Add(frptlt[i]);
            List<CPoint> ntoptlt = new List<CPoint>();
            for (int i = 0; i < toptlt.Count; i++)
                ntoptlt.Add(toptlt[i]);

            //��������ٵĵ��в��붥�㣬ʹ���������
            if (nfrptlt.Count > ntoptlt.Count)
            {
                int inttInsertNum = nfrptlt.Count - ntoptlt.Count;
                InsertVertex(ntoptlt, inttInsertNum);
            }
            else if (nfrptlt.Count < ntoptlt.Count)
            {
                int intfInsertNum = ntoptlt.Count - nfrptlt.Count;
                InsertVertex(nfrptlt, intfInsertNum);
            }

            //һһ��Ӧ
            for (int i = 1; i < nfrptlt.Count; i++)
            {
                nfrptlt[i].CorrespondingPtLt.Add(ntoptlt[i]);
                ResultPtLt.Add(nfrptlt[i]);
            }           
           
        }



        /// <summary>
        /// ����һ�������ĵ�
        /// </summary>
        /// <param name="cptlt">����</param>
        /// <param name="intInsertNum">�����������</param>
        private void InsertVertex(List<CPoint> cptlt, int intInsertNum)
        {
            //��ÿ���㼰����֮ǰ��ľ�����밴�����С�����������
            SortedList<double, CPoint> dblDisLt = new SortedList<double, CPoint>(new CDblCompare());
            for (int i = 1; i < cptlt.Count; i++)
            {
                double dblDis = CGeometricMethods.CalDis(cptlt[i - 1], cptlt[i]);
                dblDisLt.Add(dblDis, cptlt[i]);
            }

            //����intInsertNum���㣨ÿ������ߵ��е����㣩
            for (int i = 0; i < intInsertNum; i++)
            {

                CPoint cpt0 = dblDisLt.Values[dblDisLt.Values.Count -1];
                int intIndex = cptlt.IndexOf(cpt0);
                //����Ҫ����ĵ�
                double dblnewX = (cptlt[intIndex - 1].X + cptlt[intIndex].X) / 2;
                double dblnewY = (cptlt[intIndex - 1].Y + cptlt[intIndex].Y) / 2;
                CPoint newcpt = new CPoint(0, dblnewX, dblnewY);
                newcpt.CorrespondingPtLt = new List<CPoint>();
                cptlt.Insert(intIndex, newcpt);//�����

                //ɾ��ԭ���ĵ㼰�����¼�����������¼�¼
                double dblnewDis = dblDisLt.Keys[dblDisLt.Values.Count - 1] / 2;
                dblDisLt.RemoveAt(dblDisLt.Values.Count - 1);
                dblDisLt.Add(dblnewDis, newcpt);
                dblDisLt.Add(dblnewDis, cpt0);
            }
        }


 

    }
}
