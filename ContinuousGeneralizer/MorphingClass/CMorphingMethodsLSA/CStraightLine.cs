using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;

using MorphingClass.CCorrepondObjects;
using MorphingClass.CEvaluationMethods;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;
using MorphingClass.CMorphingAlgorithms;

using VBClass;



namespace MorphingClass.CMorphingMethodsLSA
{
    /// <summary>
    /// ��ֱ��Ϊ��λ·��
    /// </summary>
    public class CStraightLine
    {
        private CDataRecords _DataRecords;                    //���ݼ�¼
        
        

        public CStraightLine()
        {

        }

        public CStraightLine(CDataRecords pDataRecords)
        {
            _DataRecords = pDataRecords;
        }


        /// <summary>
        /// ��ʾ�����ص�����ֵ��״Ҫ��
        /// </summary>
        /// <param name="dblProportion">��ֵ����</param>
        /// <returns></returns>
        public CPolyline DisplayInterpolation(double dblProportion)
        {
            //CPolyline cpl = CGeometricMethods.GetTargetcpl(dblProportion);
            //_DataRecords.ParameterInitialize.txtT.Text = "   t = " + dblProportion.ToString();

            //// ����滭�ۼ�
            //IMapControl4 m_mapControl = _DataRecords.ParameterInitialize.m_mapControl;
            //IGraphicsContainer pGra = m_mapControl.Map as IGraphicsContainer;
            //pGra.DeleteAllElements();
            //CHelperFunction.ViewPolyline(m_mapControl, cpl);  //��ʾ���ɵ��߶�
            //return cpl;
            return null;
        }

        /// <summary>
        /// ��ȡ��״Ҫ��
        /// </summary>
        /// <param name="dblProportion">��ֵ����</param>
        /// <returns>��״Ҫ��</returns>
        public CPolyline GetTargetcpl(double dblProportion)
        {
            List<CCorrCpts> pCorrCptsLt = _DataRecords.ParameterResult.CCorrCptsLt;   //��ȡ���ݺ󣬴˴�ResultPtLt�еĶ�Ӧ��Ϊһһ��Ӧ
            List<CPoint> CTargetPtLt = new List<CPoint>();
            for (int i = 0; i < pCorrCptsLt.Count; i++)
            {
                double dblX = (1 - dblProportion) * pCorrCptsLt[i].FrCpt.X + dblProportion * pCorrCptsLt[i].ToCpt.X;
                double dblY = (1 - dblProportion) * pCorrCptsLt[i].FrCpt.Y + dblProportion * pCorrCptsLt[i].ToCpt.Y;
                CPoint cpt = new CPoint(i, dblX, dblY);

                if (pCorrCptsLt[i].FrCpt.isCtrl == true)
                {
                    cpt.isCtrl = true;
                }
                else
                {
                    cpt.isCtrl = false;
                }

                CTargetPtLt.Add(cpt);
            }
            CPolyline cpl = new CPolyline(0, CTargetPtLt);
            return cpl;        
        }
    }
}
