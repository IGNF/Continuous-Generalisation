using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;

namespace MorphingClass.CEntity
{
    public class CLinearEntity : CPolyline
    {
        //protected double _dblSmallBufferArea;
        protected double _dblReductionRatio;    //��������˥������
        protected double _dblCutTime;           //�������и�ʱ��

        protected IGeometry _pSmallBufferGeo;              //΢С�����������
        protected IGeometry _pToptSmallBufferGeo;
        protected IGeometry _pFrptSmallBufferGeo;
        protected IGeometry _pBufferGeoWithoutEnds;


        protected List<CPoint> _CResultPtLt;

        protected bool _isDisplay;          //�Ƿ���ʾ��
        protected CPolyline _DisplayCpl;
        protected CPolyline _CPl;

        /// <summary>���ɻ����������</summary>
        /// <param name="dblVerySmall">һ����Сֵ</param>
        /// <param name="dblBuffer">�������뾶</param>
        public void CreateBufferAndSmallBuffer(double dblBuffer, double dblVerySmall)
        {
            //����������
            this.CreateBuffer(dblBuffer);

            //CreateBufferWithoutEnds(_pBufferGeo, dblBuffer);
            //this .cut

            //���ǵ����ݴ洢�����⣬����С�������������ж��߶��Ƿ��ཻ��
            ITopologicalOperator pTop = this.pPolyline as ITopologicalOperator;
            IGeometry pSmallGeo = pTop.Buffer(dblVerySmall);
            _pSmallBufferGeo = pSmallGeo;

            //���ǵ����ݴ洢�����⣬����ToPoint��С�������������ж��߶��Ƿ��ཻ��
            ITopologicalOperator pTopTo = this.pPolyline.ToPoint as ITopologicalOperator;
            IGeometry pToPointSmallGeo = pTopTo.Buffer(dblVerySmall);
            _pToptSmallBufferGeo = pToPointSmallGeo;

            //���ǵ����ݴ洢�����⣬����ToPoint��С�������������ж��߶��Ƿ��ཻ��
            ITopologicalOperator pTopFr = this.pPolyline.FromPoint as ITopologicalOperator;
            IGeometry pFrPointSmallGeo = pTopFr.Buffer(dblVerySmall);
            _pFrptSmallBufferGeo = pFrPointSmallGeo;
        }


        /// <summary>���ɻ����������</summary>
        /// <param name="dblVerySmall">һ����Сֵ</param>
        /// <param name="dblBuffer">�������뾶</param>
        public void CreateSmallBufferForEnds(double dblVerySmall)
        {
            //���ǵ����ݴ洢�����⣬����ToPoint��С�������������ж��߶��Ƿ��ཻ��
            ITopologicalOperator pTopTo = this.pPolyline.ToPoint as ITopologicalOperator;
            IGeometry pToPointSmallGeo = pTopTo.Buffer(dblVerySmall);
            _pToptSmallBufferGeo = pToPointSmallGeo;

            //���ǵ����ݴ洢�����⣬����ToPoint��С�������������ж��߶��Ƿ��ཻ��
            ITopologicalOperator pTopFr = this.pPolyline.FromPoint as ITopologicalOperator;
            IGeometry pFrPointSmallGeo = pTopFr.Buffer(dblVerySmall);
            _pFrptSmallBufferGeo = pFrPointSmallGeo;
        }

        /// <summary>���ɻ����������</summary>
        /// <param name="dblVerySmall">һ����Сֵ</param>
        /// <param name="dblBuffer">�������뾶</param>
        public void CreateSmallBufferForPolyline(double dblVerySmall)
        {
            //���ǵ����ݴ洢�����⣬����С�������������ж��߶��Ƿ��ཻ��
            ITopologicalOperator pTop = this.pPolyline as ITopologicalOperator;
            IGeometry pSmallGeo = pTop.Buffer(dblVerySmall);
            _pSmallBufferGeo = pSmallGeo;
        }


        //to be improved, a vertical cutter may cause unexpected problems
        //
        public void CreateBufferWithoutEnds(IGeometry fBufferGeo, double dblBuffer)
        {
            //StartCutPolyline
            ILine pStartNormal = new LineClass();
            this.pPolyline.QueryNormal(esriSegmentExtension.esriNoExtension, 0, true, dblBuffer, pStartNormal);
            IPoint StartFrPt = new PointClass();
            pStartNormal.QueryPoint(esriSegmentExtension.esriExtendAtFrom, -1.5, true, StartFrPt);
            IPoint StartToPt = new PointClass();
            pStartNormal.QueryPoint(esriSegmentExtension.esriExtendAtTo, 1.5, true, StartToPt);

            IPointCollection4 pStartCutterCol = new PolylineClass();
            pStartCutterCol.AddPoint(StartFrPt);
            pStartCutterCol.AddPoint(StartToPt);
            IPolyline5 pStartCutter = pStartCutterCol as IPolyline5;

            //EndCutPolyline
            ILine pEndNormal = new LineClass();
            this.pPolyline.QueryNormal(esriSegmentExtension.esriNoExtension, 1, true, dblBuffer, pEndNormal);
            IPoint EndFrPt = new PointClass();
            pEndNormal.QueryPoint(esriSegmentExtension.esriExtendAtFrom, -1.5, true, EndFrPt);
            IPoint EndToPt = new PointClass();
            pEndNormal.QueryPoint(esriSegmentExtension.esriExtendAtTo, 1.5, true, EndToPt);

            IPointCollection4 pEndCutterCol = new PolylineClass();
            pEndCutterCol.AddPoint(EndFrPt);
            pEndCutterCol.AddPoint(EndToPt);
            IPolyline5 pEndCutter = pEndCutterCol as IPolyline5;

            //TopologicalOperators
            ITopologicalOperator pStartPtTop = this.pPolyline.FromPoint as ITopologicalOperator;
            IGeometry pStartPtBuffer = pStartPtTop.Buffer(dblBuffer);

            ITopologicalOperator pEndPtTop = this.pPolyline.ToPoint as ITopologicalOperator;
            IGeometry pEndPtBuffer = pEndPtTop.Buffer(dblBuffer);

            //Cut
            ITopologicalOperator pStartPtBufferTop = pStartPtBuffer as ITopologicalOperator;
            IGeometry StartLeftOutGeometry;
            IGeometry StartRightOutGeometry;
            pStartPtBufferTop.Cut(pStartCutter, out StartLeftOutGeometry, out StartRightOutGeometry);

            ITopologicalOperator pEndPtBufferTop = pEndPtBuffer as ITopologicalOperator;
            IGeometry EndLeftOutGeometry;
            IGeometry EndRightOutGeometry;
            pEndPtBufferTop.Cut(pEndCutter, out EndLeftOutGeometry, out EndRightOutGeometry);

            //Difference
            ITopologicalOperator pBufferTop = fBufferGeo as ITopologicalOperator;
            IGeometry pDiffStart = pBufferTop.Difference(StartRightOutGeometry);
            ITopologicalOperator pDiffStartTop = pDiffStart as ITopologicalOperator;
            IGeometry pDiffEnd = pDiffStartTop.Difference(EndLeftOutGeometry);

            _pBufferGeoWithoutEnds = pDiffEnd;
        }

        ///// <summary>���ԣ�΢С���������</summary>
        //public double dblSmallBufferArea
        //{
        //    get { return _dblSmallBufferArea; }
        //    set { _dblSmallBufferArea = value; }
        //}

        /// <summary>���ԣ�΢С�����������</summary>
        public IGeometry pSmallBufferGeo
        {
            get { return _pSmallBufferGeo; }
            set { _pSmallBufferGeo = value; }
        }

        /// <summary>���ԣ�ToPoint��΢С�����������</summary>
        public IGeometry pToptSmallBufferGeo
        {
            get { return _pToptSmallBufferGeo; }
            set { _pToptSmallBufferGeo = value; }
        }

        /// <summary>���ԣ�ToPoint��΢С�����������</summary>
        public IGeometry pFrptSmallBufferGeo
        {
            get { return _pFrptSmallBufferGeo; }
            set { _pFrptSmallBufferGeo = value; }
        }

        /// <summary>���ԣ�</summary>
        public IGeometry pBufferGeoWithoutEnds
        {
            get { return _pBufferGeoWithoutEnds; }
            set { _pBufferGeoWithoutEnds = value; }
        }

        public List<CPoint> CResultPtLt
        {
            get { return _CResultPtLt; }
            set { _CResultPtLt = value; }
        }

        public CPolyline CPl
        {
            get { return _CPl; }
            set { _CPl = value; }
        }

        /// <summary>���ԣ���������˥������</summary>
        public double dblReductionRatio
        {
            get { return _dblReductionRatio; }
            set { _dblReductionRatio = value; }
        }

        /// <summary>���ԣ��������и�ʱ��</summary>
        public double dblCutTime
        {
            get { return _dblCutTime; }
            set { _dblCutTime = value; }
        }

        /// <summary>���ԣ��Ƿ���ʾ��</summary>
        public bool isDisplay
        {
            get { return _isDisplay; }
            set { _isDisplay = value; }
        }

        /// <summary>���ԣ�������ʾ���߶�</summary>
        public CPolyline DisplayCpl
        {
            get { return _DisplayCpl; }
            set { _DisplayCpl = value; }
        }
    }
}
