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
    /// 以直线为移位路径
    /// </summary>
    public class CStraightLine
    {
        private CDataRecords _DataRecords;                    //records of data
        
        

        public CStraightLine()
        {

        }

        public CStraightLine(CDataRecords pDataRecords)
        {
            _DataRecords = pDataRecords;
        }


        /// <summary>
        /// 显示并返回单个插值面状要素
        /// </summary>
        /// <param name="dblProp">差值参数</param>
        /// <returns></returns>
        public CPolyline DisplayInterpolation(double dblProp)
        {
            //CPolyline cpl = CGeoFunc.GetTargetcpl(dblProp);
            //_DataRecords.ParameterInitialize.txtT.Text = "   t = " + dblProp.ToString();

            //// 清除绘画痕迹
            //IMapControl4 m_mapControl = _DataRecords.ParameterInitialize.m_mapControl;
            //IGraphicsContainer pGra = m_mapControl.Map as IGraphicsContainer;
            //pGra.DeleteAllElements();
            //CDrawInActiveView.ViewPolyline(m_mapControl, cpl);  //显示生成的线段
            //return cpl;
            return null;
        }

        /// <summary>
        /// 获取线状要素
        /// </summary>
        /// <param name="dblProp">插值参数</param>
        /// <returns>线状要素</returns>
        public CPolyline GetTargetcpl(double dblProp)
        {
            List<CCorrCpts> pCorrCptsLt = _DataRecords.ParameterResult.CCorrCptsLt;   //Read Datasets后，此处ResultPtLt中的对应点为一一对应
            List<CPoint> CTargetPtLt = new List<CPoint>();
            for (int i = 0; i < pCorrCptsLt.Count; i++)
            {
                double dblX = (1 - dblProp) * pCorrCptsLt[i].FrCpt.X + dblProp * pCorrCptsLt[i].ToCpt.X;
                double dblY = (1 - dblProp) * pCorrCptsLt[i].FrCpt.Y + dblProp * pCorrCptsLt[i].ToCpt.Y;
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
