using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;

using ESRI.ArcGIS.Geometry;

using MorphingClass.CUtility;
using MorphingClass.CEntity ;
using MorphingClass.CCorrepondObjects ;
using MorphingClass.CGeometry.CGeometryBase;

namespace MorphingClass.CGeometry
{
    public class CPolyline : CPolyBase<CPolyline>
    {
        private static int _intStaticGID;

        //protected int _intMaxDisNum;             //�������Զ��Ķ������
        //protected CPoint _MaxDisCpt;
        protected IGeometry _pBufferGeo;
        protected double _dblBufferArea;         //�����Ļ��������
        //protected double _dblMaxDis;           //�������Զ�������֮��ľ���
        //protected double _dblRatioLocationAlongBaseline;
        //protected bool _isMaxDisCptRightSide;
        protected double _dblVtPV;
        protected double _dblLengthV;
        protected IPolyline5 _pPolyline;


        //protected CHelpFunc _pHelperFunction=new CHelpFunc ();
        protected CPolyline _CorrCPl;


        protected CVirtualPolyline _pVirtualPolyline;

        protected List<CPolyline> _SubCPlLt;
        //protected List<CPolyline> _CorrCPlLt;
        //protected CPolyline _CParentPolyline;
        //protected CPolyline _CLeftPolyline;
        //protected CPolyline _CRightPolyline;

        //protected int _intFrcpt2PlID;
        //protected int _intFrcpt2PtID;
        //protected int _intTocpt2PlID;
        //protected int _intTocpt2PtID;

        protected CPoint _Frcpt2;
        protected CPoint _Tocpt2;

        //protected bool _isCircle;




        private CEdge _pBaseLine;     //��������(�����ɵ��С�ĵ�ָ���Ŵ�ĵ�)
        //private bool _isFormPolyline = false;


        public CPolyline()
            : this(-2, CHelpFunc.MakeLt<CPoint>())
        {

        }

        public CPolyline(int intID, IPointCollection4 pCol)
            : this(intID, pCol as IPolyline5)
        {

        }

        public CPolyline(int intID, ICurve pCurve)
            : this(intID, pCurve as IPolyline5)
        {
        }

        public CPolyline(int intID, IPolyline5 pPolyline)
            : this(intID, CHelpFunc.GetCPtLtByIPl(pPolyline), false)
        {
            this.pPolyline = pPolyline;
        }

        public CPolyline(CRiver pRiver, bool blnSetPolyline = false)
            : this(pRiver.ID, pRiver.CptLt, blnSetPolyline)
        {

        }

        public CPolyline(int intID, CEdge pEdge, bool blnSetPolyline = false)
            : this(intID, pEdge.FrCpt, pEdge.ToCpt, blnSetPolyline)
        {

        }

        public CPolyline(CPoint cpt, bool blnSetPolyline = false)
            : this(cpt.ID, CHelpFunc.MakeLt(cpt), blnSetPolyline)
        {

        }

        public CPolyline(int intID, CPoint cpt, bool blnSetPolyline = false)
            : this(intID, CHelpFunc.MakeLt(cpt), blnSetPolyline)
        {

        }



        public CPolyline(int intID, CBend pBend, bool blnSetPolyline = false)
            : this(intID, pBend.CptLt, blnSetPolyline)
        {
        }

        public CPolyline(CCorrCpts CorrCpt, bool blnSetPolyline = false)
            : this(CorrCpt.FrCpt.ID, CorrCpt.FrCpt, CorrCpt.ToCpt, blnSetPolyline)
        {
        }

        public CPolyline(int intID, CCorrCpts CorrCpt, bool blnSetPolyline = false)
            : this(intID, CorrCpt.FrCpt, CorrCpt.ToCpt, blnSetPolyline)
        {
        }

        public CPolyline(CPoint cpt1, CPoint cpt2, bool blnSetPolyline = false)
            : this(cpt1.ID, CHelpFunc.MakeLt<CPoint>(cpt1, cpt2), blnSetPolyline)
        {
        }

        public CPolyline(int intID, CPoint cpt1, CPoint cpt2, bool blnSetPolyline = false)
            : this(intID, CHelpFunc.MakeLt<CPoint>(cpt1, cpt2), blnSetPolyline)
        {
        }

        /// <summary>
        /// the main constructor
        /// </summary>
        /// <remarks>all the constructors, except the one wihout parameter, will invoke this constructor</remarks>
        public CPolyline(int intID, List<CPoint> cptlt0, bool blnSetPolyline = false)
        {
            this.GID = _intStaticGID++;
            _intID = intID;

            FormPolyBase(cptlt0);

            if (blnSetPolyline == true)
            {
                SetPolyline();
            }
        }


        //public void DivideByCpt(CPoint dcpt)
        //{
        //    List<CPoint> dcptlt = _CptLt;
        //    int intIndex = dcptlt.IndexOf(dcpt);
        //    List<CPoint> dleftcptlt = new List<CPoint>();
        //    List<CPoint> drightcptlt = new List<CPoint>();
        //    for (int i = 0; i <= intIndex; i++)
        //    {
        //        dleftcptlt.Add(dcptlt[i]);
        //    }
        //    _CLeftPolyline = new CPolyline(dleftcptlt[0].ID, dleftcptlt);
        //    _CLeftPolyline.CParentPolyline = this;

        //    for (int i = intIndex; i < dcptlt.Count; i++)
        //    {
        //        drightcptlt.Add(dcptlt[i]);
        //    }
        //    _CRightPolyline = new CPolyline(dcpt.ID, drightcptlt);
        //    _CRightPolyline.CParentPolyline = this;
        //}

        /// <summary>���ɻ����������</summary>
        public void CreateBuffer(double dblDis)
        {
            ITopologicalOperator pTop = _pPolyline as ITopologicalOperator;
            _pBufferGeo = pTop.Buffer(dblDis);
            IArea pArea = _pBufferGeo as IArea;
            _dblBufferArea = pArea.Area;
        }




        public void SetVirtualPolyline()
        {
            List<CPoint> fcptlt = this.CptLt;
            _pVirtualPolyline = new CVirtualPolyline(fcptlt[0].ID, 0, fcptlt.Count - 1);
        }

        public CEdge SetBaseLine(bool isSetLength = false, bool isSetAxisAngle = false)
        {
            var pbaseline = new CEdge(_FrCpt, _ToCpt);

            if (isSetLength == true)
            {
                pbaseline.SetLength();
            }

            if (isSetAxisAngle == true)
            {
                pbaseline.SetAxisAngle();
            }
            _pBaseLine = pbaseline;

            return pbaseline;
        }



        

       

        /// <summary>
        /// compute the length of the edges from the Start vertex, and store the value in the "ToCPoint"
        /// </summary>
        public void SetAbsLengthFromStart()
        {
            CGeoFunc.CalAbsLengthFromStart(this.CptLt);
        }

        /// <summary>
        /// compute the relative length of the edges from the Start vertex, and store the value in the "ToCPoint"
        /// </summary>
        public void SetRatioLengthFromStart()
        {
            CGeoFunc.CalRatioLengthFromStart(this.CptLt);
        }

        /// <summary>
        /// ͨ�����������ڵ��ȡ�����ߵ�������
        /// </summary>
        /// <param name="cpt1">���ȡ���������ϵ��յ�֮һ</param>
        /// <param name="cpt2">���ȡ���������ϵ��յ�֮һ 2</param>
        public CPolyline GetSubPolyline(CPoint cpt1, CPoint cpt2)
        {
            CPolyline subcpl = GetSubPolyline(cpt1.ID, cpt2.ID);

            if (subcpl.FrCpt.Equals2D(cpt1) == false || subcpl.ToCpt.Equals2D(cpt2) == false)
            {
                MessageBox.Show("there is a problem in GetSubPolyline!");
            }

            return subcpl;
        }

        public CPolyline GetSubPolyline(int frID, int toID)
        {
            List<CPoint> fcptlt = this.CptLt;
            int intCount = toID - frID + 1;
            int intStartIndex = frID - fcptlt[0].ID;

            List<CPoint> subcptlt = fcptlt.GetRange(intStartIndex, intCount);
            return new CPolyline(frID, subcptlt);
        }

        public void UnionCpl(CPolyline other, ref bool isUnioned)
        {
            isUnioned = true;
            List<CPoint> fcptlt = this.CptLt;
            List<CPoint> ocptlt = other.CptLt;
            List<CPoint> newcptlt = new List<CPoint>();

            if (CCmpMethods.CmpCptYX(fcptlt.GetLastT(), ocptlt.First()) == 0)
            {
                newcptlt.AddRange(fcptlt);
                newcptlt.RemoveLastT();
                newcptlt.AddRange(ocptlt);
            }
            else if (CCmpMethods.CmpCptYX(fcptlt.GetLastT(), ocptlt.GetLastT()) == 0)
            {
                newcptlt.AddRange(fcptlt);
                newcptlt.RemoveLastT();

                var copiedreversedcptlt = CHelpFunc.CopyCptLt(ocptlt);
                copiedreversedcptlt.Reverse();

                newcptlt.AddRange(copiedreversedcptlt);
            }
            else if (CCmpMethods.CmpCptYX(fcptlt.First(), ocptlt.First()) == 0)
            {
                var copiedreversedcptlt = CHelpFunc.CopyCptLt(ocptlt);
                copiedreversedcptlt.Reverse();

                newcptlt.AddRange(copiedreversedcptlt);
                newcptlt.RemoveLastT();
                newcptlt.AddRange(fcptlt);
            }
            else if (CCmpMethods.CmpCptYX(fcptlt.First(), ocptlt.GetLastT()) == 0)
            {
                //CPolyline cplCopyOther = other.CopyCpl();
                newcptlt.AddRange(ocptlt);
                newcptlt.RemoveLastT();
                newcptlt.AddRange(fcptlt);
            }
            else
            {
                isUnioned = false;
                return;
            }

            for (int i = 0; i < newcptlt.Count; i++)
            {
                newcptlt[i].ID = i;
            }

            FormPolyBase(newcptlt);
        }

        public CPolyline CopyCpl()
        {
            return new CPolyline(_intID, CHelpFunc.CopyCptLt(this.CptLt));
        }

        //public void CheckIsCircle()
        //{
        //    int intResult = CCmpMethods.Cmp(_CptLt[0], _CptLt[_CptLt.Count - 1]);
        //    if (intResult == 0)
        //    {
        //        _isCircle = true;
        //    }
        //    else
        //    {
        //        _isCircle = false;
        //    }
        //}

        public void ReverseCpl()
        {
            List<CPoint> fcptlt = this.CptLt;
            CHelpFunc.ReverseCptLt(ref fcptlt);
            FormPolyBase(fcptlt);
        }

        public void SetCptBelongedCpl()
        {
            foreach (CPoint cpt in this.CptLt)
            {
                cpt.BelongedCpl = this;
            }
        }

        public void SetCEdgeBelongedCpl()
        {
            foreach (CEdge cedge in this.CEdgeLt)
            {
                cedge.BelongedCpl = this;
            }
        }

        public void SetCEdgeTwinBelongedCpl()
        {
            foreach (CEdge cedge in this.CEdgeLt)
            {
                cedge.cedgeTwin.BelongedCpl = this;
            }
        }

        /// <summary>
        /// ��ȡ��������ߵı߳�
        /// </summary>
        public double GetMaxELength()
        {

            double dblMaxLength = 0;
            List<CPoint> fcptlt = this.CptLt;
            for (int i = 0; i < fcptlt.Count - 1; i++)
            {
                double dblLength = fcptlt[i].DistanceTo(fcptlt[i + 1]);
                if (dblLength > dblMaxLength)
                {
                    dblMaxLength = dblLength;
                }
            }
            return dblMaxLength;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateSubPllt()
        {
            List<CPoint> cptlt = this.CptLt;
            List<CPolyline> subcpllt = new List<CPolyline>(cptlt.Count - 1);
            for (int i = 0; i < cptlt.Count - 1; i++)
            {
                List<CPoint> subcptlt = new List<CPoint>(2);
                subcptlt.Add(cptlt[i]);
                subcptlt.Add(cptlt[i + 1]);
                CPolyline subcpl = new CPolyline(i, subcptlt);
                subcpllt.Add(subcpl);
            }
            _SubCPlLt = subcpllt;
        }

        //this method takes a lot of time, please do not use it if not necessary
        public IPolyline5 SetPolyline()
        {
            this.pPolyline = CGeoFunc.GetIptColFromCptLt
                (this.CptLt, esriGeometryType.esriGeometryPolyline) as IPolyline5;
            return this.pPolyline;
        }

        public IPolyline5 JudgeAndSetPolyline()
        {
            if (_pPolyline == null)
            {
                return SetPolyline();
            }
            else
            {
                return _pPolyline;
            }
        }

        public override IGeometry JudgeAndSetAEGeometry()
        {
            return JudgeAndSetPolyline();
        }

        public override void SetAEGeometryNull()
        {
            _pPolyline = null;
        }

        public override IGeometry GetAEObject()
        {
            return _pPolyline;
        }

        /// <summary>���ԣ����������</summary>
        public double dblLengthV
        {
            get { return _dblLengthV; }
            set { _dblLengthV = value; }
        }

        /// <summary>���ԣ����������</summary>
        public double dblVtPV
        {
            get { return _dblVtPV; }
            set { _dblVtPV = value; }
        }

        /// <summary>���ԣ����������</summary>
        public double dblBufferArea
        {
            get { return _dblBufferArea; }
        }

        ///// <summary>���ԣ�DP�㷨��������</summary>
        //public double dblMaxDis
        //{
        //    get { return _dblMaxDis; }
        //    set { _dblMaxDis = value; }
        //}

        ///// <summary>���ԣ�ĳ���ڻ�����ͶӰ������λ��</summary>
        //public double dblRatioLocationAlongBaseline
        //{
        //    get { return _dblRatioLocationAlongBaseline; }
        //    set { _dblRatioLocationAlongBaseline = value; }
        //}



        ///// <summary>���ԣ�</summary>
        //public int intFrcpt2PlID
        //{
        //    get { return _intFrcpt2PlID; }
        //    set { _intFrcpt2PlID = value; }
        //}

        ///// <summary>���ԣ�</summary>
        //public int intFrcpt2PtID
        //{
        //    get { return _intFrcpt2PtID; }
        //    set { _intFrcpt2PtID = value; }
        //}

        ///// <summary>���ԣ�</summary>
        //public int intTocpt2PlID
        //{
        //    get { return _intTocpt2PlID; }
        //    set { _intTocpt2PlID = value; }
        //}

        ///// <summary>���ԣ�</summary>
        //public int intTocpt2PtID
        //{
        //    get { return _intTocpt2PtID; }
        //    set { _intTocpt2PtID = value; }
        //}

        ///// <summary>���ԣ��������Զ��Ķ������</summary>
        //public int intMaxDisNum
        //{
        //    get { return _intMaxDisNum; }
        //    set { _intMaxDisNum = value; }
        //}

        //public bool isMaxDisCptRightSide
        //{
        //    get { return _isMaxDisCptRightSide; }
        //    set { _isMaxDisCptRightSide = value; }
        //}

        /// <summary>���ԣ������������</summary>
        public IGeometry pBufferGeo
        {
            get { return _pBufferGeo; }
            set { _pBufferGeo = value; }
        }

        public CVirtualPolyline pVirtualPolyline
        {
            get { return _pVirtualPolyline; }
            set { _pVirtualPolyline = value; }
        }

        public IPolyline5 pPolyline
        {
            get { return _pPolyline; }
            set
            {
                _pPolyline = value;
                _pGeo = value;
            }
        }

        /// <summary>��������</summary>
        public CEdge pBaseLine
        {
            get { return _pBaseLine; }
            set { _pBaseLine = value; }
        }

        //public List<CPoint> GetCptLt()
        //{
        //    return _CptLt; 
        //}





        //public CPoint MaxDisCpt
        //{
        //    get { return _MaxDisCpt; }
        //    set { _MaxDisCpt = value; }
        //}

        /// <summary>ָ����ɫ�����߶�</summary>
        public List<CPolyline> SubCPlLt
        {
            get { return _SubCPlLt; }
            set { _SubCPlLt = value; }
        }

        /// <summary>Corresponding CPolylines</summary>
        //public List<CPolyline> CorrCPlLt
        //{
        //    get { return _CorrCPlLt; }
        //    set { _CorrCPlLt = value; }
        //}

        /// <summary>Corresponding CPolyline</summary>
        //public CPolyline CorrCPl
        //{
        //    get { return _CorrCPl; }
        //    set { _CorrCPl = value; }
        //}

        ///// <summary>��������</summary>
        //public CPolyline CParentPolyline
        //{
        //    get { return _CParentPolyline; }
        //    set { _CParentPolyline = value; }
        //}

        ///// <summary>���֧����</summary>
        //public CPolyline CLeftPolyline
        //{
        //    get { return _CLeftPolyline; }
        //    set { _CLeftPolyline = value; }
        //}

        ///// <summary>�ҷ�֧����</summary>
        //public CPolyline CRightPolyline
        //{
        //    get { return _CRightPolyline; }
        //    set { _CRightPolyline = value; }
        //}

        public CPoint Frcpt2
        {
            get { return _Frcpt2; }
            set { _Frcpt2 = value; }
        }

        public CPoint Tocpt2
        {
            get { return _Tocpt2; }
            set { _Tocpt2 = value; }
        }

        //public bool isCircle
        //{
        //    get { return _isCircle; }
        //    set { _isCircle = value; }
        //}

        #region IEquatable<dEdge> Members

        /// <summary>
        /// Checks whether two cedges are equal disregarding the direction of the cedges
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CPolyline other)
        {
            IPointCollection4 pThisCol = new PolylineClass();
            pThisCol = (IPointCollection4)this.pPolyline;

            IPointCollection4 potherCol = new PolylineClass();
            potherCol = (IPointCollection4)other.pPolyline;

            if (pThisCol.PointCount != potherCol.PointCount)
                return false;


            bool blnEqual = true;
            for (int i = 0; i < pThisCol.PointCount; i++)
            {
                if ((pThisCol.get_Point(i).X != potherCol.get_Point(i).X) || (pThisCol.get_Point(i).Y != potherCol.get_Point(i).Y))
                {
                    blnEqual = false;
                    break;
                }
            }
            return blnEqual;


        }

        #endregion


        public void SetEmpty2()
        {
            //    this._pPolyline.SetEmpty();
            //    if (_pBufferGeo!=null )
            //    {
            //        _pBufferGeo.SetEmpty();
            //    }                             
            //    if (_SubCPlLt!=null)
            //    {
            //        for (int i = 0; i < _SubCPlLt.Count; i++)
            //        {
            //            _SubCPlLt[i].SetEmpty2();
            //        }
            //    }
            //    if (_CLeftPolyline != null)
            //    {
            //        _CLeftPolyline.SetEmpty2();
            //    }
            //    if (_CRightPolyline != null)
            //    {
            //        _CRightPolyline.SetEmpty2();
            //    }
            //    if (_pBaseLine != null)
            //    {
            //        _pBaseLine.SetEmpty2();
            //    }
            //}
        }
    }

    public class CCorrCplInfo
    {
        private CPolyline _pCorrCpl;
        private double _dblOverlapArea;
        private double _dblOverlapRatio;

        public CCorrCplInfo(CPolyline CorrCpl, double fdblOverlapRatio, double fdblOverlapArea)
        {
            _pCorrCpl = CorrCpl;
            _dblOverlapRatio = fdblOverlapRatio;
            _dblOverlapArea = fdblOverlapArea;
        }


        /// <summary>���ԣ���Ӧ�߽�</summary>
        public CPolyline pCorrCpl
        {
            get { return _pCorrCpl; }
            set { _pCorrCpl = value; }
        }

        /// <summary>���ԣ�</summary>
        public double dblOverlapArea
        {
            get { return _dblOverlapArea; }
            set { _dblOverlapArea = value; }
        }

        /// <summary>���ԣ�</summary>
        public double dblOverlapRatio
        {
            get { return _dblOverlapRatio; }
            set { _dblOverlapRatio = value; }
        }

















    }
    //public class CDivideInfo
}