using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;

using MorphingClass.CEntity;
using MorphingClass.CMorphingMethods;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;


namespace GeneralizationClass.CGeneralizationMethods
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Douglas-Peucker���ۺ��㷨</remarks>
    public class CDP
    {
        
        
        private CParameterResult _ParameterResult;

        private List<CPolyline> _CPlLt;

        private CMPBDPBL _pMPBDPBL = new CMPBDPBL();
        private CParameterInitialize _ParameterInitialize;

        public CDP()
        {

        }

        public CDP(CParameterInitialize ParameterInitialize)
        {

            //��ȡ��ǰѡ��ĵ�Ҫ��ͼ��
            IFeatureLayer pFeatureLayer = (IFeatureLayer)ParameterInitialize.m_mapFeature.get_Layer(ParameterInitialize.cboLayer.SelectedIndex);
                                                                      
            ParameterInitialize.pFeatureLayer = pFeatureLayer;
            _ParameterInitialize = ParameterInitialize;

            //��ȡ������
            _CPlLt = CHelperFunction.GetCPlLtByFeatureLayer(pFeatureLayer);

        }

        //public void DPGeneralization()
        //{
        //   CPolyline cpl = _CPlLt[0];
        //    CParameterInitialize pParameterInitialize = _ParameterInitialize;           
        //    _pMPBDPBL.DivideCplByDP(cpl);

        //    //��ȡ�����ȫ����¼��_ParameterResult��
        //    List<CPolyline> cresultpllt = new List<CPolyline>();
        //    cresultpllt.Add(cpl);
        //    CParameterResult ParameterResult = new CParameterResult();
        //    ParameterResult.CResultPlLt = cresultpllt;
        //    _ParameterResult = ParameterResult;
        //}

        ///// <summary>
        ///// ��ʾ�����ص�����ֵ�߶�
        ///// </summary>
        ///// <param name="pDataRecords">���ݼ�¼</param>
        ///// <param name="dblDPBound">��ֵ����</param>
        ///// <returns>��ֵ�߶�</returns>
        //public CPolyline DisplayInterpolation(CDataRecords pDataRecords, double dblDPBound)
        //{
        //    CPolyline cpl = _CPlLt[0];
        //    CPolyline resultcpl = _pMPBDPBL.GetCplByDP(cpl, dblDPBound);

        //    // ����滭�ۼ�
        //    IMapControl4 m_mapControl = pDataRecords.ParameterInitialize.m_mapControl;
        //    IGraphicsContainer pGra = m_mapControl.Map as IGraphicsContainer;
        //    pGra.DeleteAllElements();
        //    m_mapControl.ActiveView.Refresh();
        //    CHelperFunction.ViewPolyline(m_mapControl, resultcpl);  //��ʾ���ɵ��߶�
        //    return resultcpl;
        //}

        /////// <summary>
        /////// ��ʾ�����ص�����ֵ�߶�
        /////// </summary>
        /////// <param name="pDataRecords">���ݼ�¼</param>
        /////// <param name="dblProportion">��ֵ����</param>
        /////// <returns>��ֵ�߶�</returns>
        ////public CPolyline DisplayInterpolation(CDataRecords pDataRecords, int intResidualNum)
        ////{
        ////    CPolyline cpl = _CPlLt[0];
        ////    CPolyline resultcpl = _pMPBDPBL.GetCplByDP(cpl, dblDPBound);

        ////    // ����滭�ۼ�
        ////    IMapControl4 m_mapControl = pDataRecords.ParameterInitialize.m_mapControl;
        ////    IGraphicsContainer pGra = m_mapControl.Map as IGraphicsContainer;
        ////    pGra.DeleteAllElements();
        ////    m_mapControl.ActiveView.Refresh();
        ////    CHelperFunction.ViewPolyline(m_mapControl, resultcpl);  //��ʾ���ɵ��߶�
        ////    return resultcpl;
        ////}

        //public CPolyline GetCplByDP(CPolyline cpl, double dblDPBound)
        //{
        //    CPolyline resultcpl = _pMPBDPBL.GetCplByDP(cpl, dblDPBound);
        //    int intptnum = resultcpl.CptLt.Count;
        //    return resultcpl;
        //}



        public void DivideCplByDP(CPolyline dcpl, CVirtualPolyline pVtPl)
        {
            List<CPoint> dcptlt = dcpl.CptLt;
            if ((pVtPl.intToID - pVtPl.intFrID) < 2)
            {
                return;
            }

            //�ҵ������������Զ�ĵ�
            CEdge pEdge = new CEdge(dcptlt[pVtPl.intFrID], dcptlt[pVtPl.intToID]);
            double dblMaxDis = -1;
            int intMaxDisID = 0;
            double dblRatioLocationAlongBaseline = 0;
            double dblAlongDis = 0;
            double dblFromDis = 0;
            bool isMaxDisCptRightSide = false;
            IPoint outipt = new PointClass();
            bool blnright = new bool();
            for (int i = pVtPl.intFrID + 1; i < pVtPl.intToID; i++)
            {
                dblFromDis = CGeometricMethods.CalDisPointToLine(dcptlt[pVtPl.intFrID], dcptlt[pVtPl.intToID], dcptlt[i]);
                if (dblFromDis > dblMaxDis)
                {
                    dblMaxDis = dblFromDis;
                    intMaxDisID = i;
                    dblRatioLocationAlongBaseline = dblAlongDis;
                    isMaxDisCptRightSide = blnright;
                }
                //outipt.SetEmpty();
            }


            //�ֱ�������ӱ�ִ�зָ����
            pVtPl.intMaxDisID = intMaxDisID;
            pVtPl.dblMaxDis = dblMaxDis;
            pVtPl.dblRatioLocationAlongBaseline = dblRatioLocationAlongBaseline;
            pVtPl.isMaxDisCptRightSide = isMaxDisCptRightSide;
            pVtPl.DivideByID(intMaxDisID);

            DivideCplByDP(dcpl, pVtPl.CLeftPolyline);
            DivideCplByDP(dcpl, pVtPl.CRightPolyline);
        }


        public void RecursivelyCollectMaxDis(CVirtualPolyline pVtPl, ref SortedList<double, int> dblMaxDisSLt)
        {
            if (pVtPl.CLeftPolyline == null)
            {
                return;
            }
            dblMaxDisSLt.Add(pVtPl.dblMaxDis, pVtPl.intMaxDisID);
            RecursivelyCollectMaxDis(pVtPl.CLeftPolyline, ref dblMaxDisSLt);
            RecursivelyCollectMaxDis(pVtPl.CRightPolyline, ref dblMaxDisSLt);
        }

        /// <summary>���ԣ�������</summary>
        public CParameterResult ParameterResult
        {
            get { return _ParameterResult; }
            set { _ParameterResult = value; }
        }
    }
}
