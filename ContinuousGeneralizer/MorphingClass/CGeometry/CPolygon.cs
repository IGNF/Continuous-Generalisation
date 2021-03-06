﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using ESRI.ArcGIS.Geometry;

using MorphingClass.CUtility;
using MorphingClass.CEntity;
using MorphingClass.CGeometry.CGeometryBase;
using MorphingClass.CCorrepondObjects;

using ClipperLib;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

namespace MorphingClass.CGeometry
{
    public class CPolygon : CPolyBase
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 
        /// 要使用边（多边形）进行空间分析，需执行函数FormCEdge（FormPolygon），生成边（多边形）之后，方可使用
        /// </summary>
        /// <remarks></remarks>

        private static int _intStaticGID;

        private CEdge _OuterCmptCEdge;    //if a face has OuterCmptCEdge == null, then this face is super face
        private CEdge _cedgeStartAtLeftMost;

        /// <summary>
        /// Inner component CEdgeLt
        /// </summary>
        private List<CEdge> _InnerCmptCEdgeLt;     //counter clockwise???

        public double dblAreaSimple { get; set; }
        public bool WasTooSmall { get; set; }

        public int intType { get; set; }
        public int intTypeIndex { get; set; } //the index (0, 1, 2, ...) of a type; used for access type distance directly

        public CPolygon ParentCpg { get; set; }
        //public CPolygon ClipCpg { get; set; }

        /// <summary>for a polygon, there is one element in ClipCpgLt;
        /// but for a hole, there may be several elements in ClipCpgLt </summary>    
        public List<CPolygon> ClipCpgLt { get; set; }  
        public CPolygon ExteriorOffsetCpg { get; set; }
        //public cp CPolygon ClipBridge { get; set; }

        public Paths LastTimePaths { get; set; }
        public Path ExteriorPath { get; set; }
        public PolyTree pPolyTree { get; set; }

        public List<CPolygon> HoleCpgLt { get; set; }
        public CPoint CentroidCptSimple { get; set; }

        public List<CPolygon> SubCpgLt { get; set; }
        public List<CptEdgeDis> BridgeCptEdgeDisLt { get; set; }
        
        //public Dictionary<CValPairIncr<CPolygon>, CptEdgeDis> BridgeCpipeDt { get; set; }

        //public CPolygon AssigningFace { get; set; }
        //public List<CPolygon> AssignedFaceLt { get; set; }

        private IPolygon4 _pPolygon;

        //private CPoint _CentroidCpt;
        private CPoint _LeftMostCpt;

        public bool IsHole { get; set; }
        public bool IsMerged { get; set; }
        public bool IsSubCpg { get; set; }

        public List<double> AxisAngleLt { get; set; }
        public List<double> ReverseAxisAngleLt { get; set; }
        //public bool IsOriginal { get; set; } = true;


        public CPolygon(int intID, IPolygon4 pPolygon)
            : this(intID, CHelpFunc.GetIpgExteriorCptLt(pPolygon).ToList(), 
                  CHelpFunc.GetIpgInteriorCptLtEb(pPolygon))
        {
            this.pPolygon = pPolygon;
        }

        //public CPolygon(int intID, List<CPoint> cptlt)
        //    : this(intID, CHelpFunc.MakeLt(1, cptlt))
        //{
        //}

        /// <summary>
        /// Initializes a new instance of a CPolygon, the first point and the last point in cptlt must have the same coordinates
        /// </summary>
        public CPolygon(int intID, List<CPoint> cptlt, IEnumerable<List<CPoint>> holecptltlt)
            :this(intID,cptlt,CGeoFunc.GenerateHoleCpgLt(holecptltlt).ToList())
        {
        }

        public CPolygon(List<CPoint> cptlt)
    : this(intDefaultID, cptlt)
        {
        }

        /// <summary>
        /// Initializes a new instance of a CPolygon, 
        /// the first point and the last point in cptlt must have the same coordinates
        /// </summary>
        public CPolygon(int intID = intDefaultID, List<CPoint> cptlt = null, List<CPolygon> holecpglt = null)
        {
            this.GID = _intStaticGID++;
            _intID = intID;
            this.strShape = "Polygon";

            if (cptlt != null && cptlt.Count > 0)
            {
                cptlt[cptlt.Count - 1] = cptlt[0];  //make sure that the first vertex and the last vertex are identical
            }
            


            FormPolyBase(cptlt);


            if (holecpglt != null)
            {
                foreach (var holecpg in holecpglt)
                {
                    holecpg.IsHole = true;
                }
                this.HoleCpgLt = holecpglt;
            }

        }
        

        public override void JudgeAndFormCEdgeLt()
        {
            if (this.CEdgeLt == null)
            {
                FormCEdgeLt();
            }            
        }

        public override void FormCEdgeLt()
        {
            this.CEdgeLt = CGeoFunc.FormCEdgeEb(this.CptLt).ToList();

            if (this.HoleCpgLt != null)
            {
                foreach (var holecpg in this.HoleCpgLt)
                {
                    holecpg.FormCEdgeLt();
                }
            }
        }

        public List<CEdge> GetAllCEdgeLt()
        {
            this.JudgeAndFormCEdgeLt();
            var AllCEdgeLt = new List<CEdge>(this.CEdgeLt);
            if (this.HoleCpgLt != null)
            {
                foreach (var holecpg in this.HoleCpgLt)
                {
                    AllCEdgeLt.AddRange(holecpg.GetAllCEdgeLt());
                }
            }
            return AllCEdgeLt;
        }

        public override int GetEdgeCount()
        {
            var intEdgeCount = this.CptLt.Count-1;
            
            if (this.HoleCpgLt != null)
            {
                foreach (var holecpg in this.HoleCpgLt)
                {
                    intEdgeCount += holecpg.GetEdgeCount();
                }
            }
            return intEdgeCount;
        }

        public override void SetCEdgeToCpts()
        {
            this.CEdgeLt.ForEach(cedge => cedge.SetCEdgeToCpts());

            if (this.HoleCpgLt != null)
            {
                foreach (var holecpg in this.HoleCpgLt)
                {
                    holecpg.SetCEdgeToCpts();
                }
            }
        }

        public override void SetCEdgeLtLength()
        {
            this.CEdgeLt.ForEach(cedge => cedge.SetLength());
           
            if (this.HoleCpgLt != null)
            {
                foreach (var holecpg in this.HoleCpgLt)
                {
                    holecpg.SetCEdgeLtLength();
                }
            }
        }

        public override void SetCEdgeLtAxisAngle()
        {
            this.CEdgeLt.ForEach(cedge => cedge.SetAxisAngle());

            if (this.HoleCpgLt != null)
            {
                foreach (var holecpg in this.HoleCpgLt)
                {
                    holecpg.SetCEdgeLtAxisAngle();
                }
            }
        }

        public void SetAxisAngleAndReverseLt()
        {
            var cpg = this;

            cpg.JudgeAndFormCEdgeLt();
            cpg.CEdgeLt.ForEach(cedge => cedge.JudgeAndSetAxisAngle());
            var fAxisAngleLt = new List<double>(cpg.CEdgeLt.Count);
            foreach (var cedge in this.CEdgeLt)
            {
                cedge.JudgeAndSetAxisAngle();
                fAxisAngleLt.Add(cedge.dblAxisAngle);
            }

            //the index of ReverseAxisAngleLt indicates the index of the starting point
            var fReverseAxisAngleLt = new List<double>(cpg.CEdgeLt.Count);
            fReverseAxisAngleLt.Add(CGeoFunc.CalReversedCEdgeAxisAngle(fAxisAngleLt.Last()));
            for (int i = 1; i < fAxisAngleLt.Count; i++)
            {
                fReverseAxisAngleLt.Add(CGeoFunc.CalReversedCEdgeAxisAngle(fAxisAngleLt[i - 1]));
            }

            this.AxisAngleLt = fAxisAngleLt;
            this.ReverseAxisAngleLt = fReverseAxisAngleLt;
        }

        public void SetExteriorPath()
        {
            this.ExteriorPath = clipperMethods.GeneratePathByCptEb(this.CptLt, true);

            if (this.HoleCpgLt != null)
            {
                foreach (var holecpg in this.HoleCpgLt)
                {
                    holecpg.SetExteriorPath();
                }
            }
        }

        public Paths GetAllPaths()
        {
            var allPaths = new Paths();
            allPaths.Add(this.ExteriorPath);

            if (this.HoleCpgLt != null)
            {
                foreach (var holecpg in this.HoleCpgLt)
                {
                    allPaths.AddRange(holecpg.GetAllPaths());
                }
            }
            return allPaths;
        }

        public void SetAreaSimple()
        {
            var cpgSK = new Stack<CPolygon>();
            cpgSK.Push(this);

            do
            {
                var cpg = cpgSK.Pop();
                cpg.dblAreaSimple = CGeoFunc.CalAreaForCptEb(cpg.CptLt);

                //add the holes
                if (cpg.HoleCpgLt != null)
                {
                    foreach (var holecpg in cpg.HoleCpgLt)
                    {
                        cpgSK.Push(holecpg);
                    }
                }
            } while (cpgSK.Count > 0);
        }

        public override void SetAngleDiffLt()
        {
            var cpgSK = new Stack<CPolygon>();
            cpgSK.Push(this);

            do
            {
                var cpg = cpgSK.Pop();
                SetCpbAngleDiffLt(cpg as CPolyBase);

                //add the holes
                if (cpg.HoleCpgLt != null)
                {
                    foreach (var holecpg in cpg.HoleCpgLt)
                    {
                        cpgSK.Push(holecpg);
                    }
                }
            } while (cpgSK.Count>0);
        }

        

        //public override void JudgeAndFormCEdgeLt()
        //{
        //    if (this.CEdgeLt == null)
        //    {
        //        FormCEdgeLt();
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>SetPolygon will first set IPoint</remarks>
        public IPolygon4 SetPolygon()
        {
            //Build a polygon segment-by-segment.
            IPolygon4 polygon = new PolygonClass();

            IGeometryCollection geometryCollection = (IGeometryCollection)polygon;

            var exteriorCptLt = this.CptLt;
            if (CGeoFunc.IsClockwise(exteriorCptLt, true) ==false )
            {
                exteriorCptLt = exteriorCptLt.AsEnumerable().Reverse().ToList(); //this will not change this.CptLt
            }

            geometryCollection.AddGeometry(CGeoFunc.GetIrgFromCptLt(exteriorCptLt));
            //add the holes
            if (this.HoleCpgLt != null)
            {
                foreach (var holecpg in this.HoleCpgLt)
                {
                    var interiorCptLt = holecpg.CptLt;
                    if (CGeoFunc.IsClockwise(exteriorCptLt, true) == true)
                    {
                        interiorCptLt = interiorCptLt.AsEnumerable().Reverse().ToList(); //this will not change holecpg.CptLt
                    }
                    geometryCollection.AddGeometry(CGeoFunc.GetIrgFromCptLt(interiorCptLt));
                }
            }
            //polygon.Close();
            this.pPolygon = polygon;
            return polygon;
        }

        public IPolygon4 JudgeAndSetPolygon()
        {
            if (_pPolygon == null)
            {
                return SetPolygon();
            }
            else
            {
                return _pPolygon;
            }
        }

        public override IGeometry JudgeAndSetAEGeometry()
        {
            return JudgeAndSetPolygon();
        }

        public override void SetAEGeometryNull()
        {
            _pPolygon = null;
        }

        public override IGeometry GetAEObject()
        {
            return _pPolygon;
        }

        public void SetGeometricProperties()
        {
            this.FormCEdgeLt();
            this.SetCEdgeLtLength();
            this.SetCEdgeLtAxisAngle();
            this.SetAngleDiffLt();
        }

        public void RemoveClosePoints()
        {
            var cpgSK = new Stack<CGeometry.CPolygon>();
            cpgSK.Push(this);

            do
            {
                var cpg = cpgSK.Pop();
                cpg.CptLt = CGeoFunc.RemoveClosePointsForCptEb(cpg.CptLt, true).ToList();

                if (cpg.HoleCpgLt != null)
                {
                    foreach (var holecpg in HoleCpgLt)
                    {
                        cpgSK.Push(holecpg);
                    }
                }
            } while (cpgSK.Count > 0);
        }

        public IEnumerable<List<CPoint>> GetHoleCptLtEb()
        {
            if (this.HoleCpgLt != null)
            {
                foreach (var holecpg in this.HoleCpgLt)
                {
                    yield return holecpg.CptLt;
                }
            }
        }

        public void SetOuterFaceCptlt(bool clockwise = true, bool blnIdentical = true, CPoint StartPositionCpt = null)
        {
            this.CptLt = GetOuterCptEb(clockwise, blnIdentical, StartPositionCpt).ToList();
        }

        public IEnumerable<CPoint> GetOuterCptEb(bool clockwise = true, bool blnIdentical = true, CPoint StartPositionCpt=null)
        {
            var pOuterCmptCEdge = _OuterCmptCEdge;
            if (pOuterCmptCEdge == null)
            {
                throw new ArgumentException("Super face does not have an outer ring!");
            }

            if (StartPositionCpt != null)
            {
                pOuterCmptCEdge = TraverseToGetCorrectCEdgeComponent(pOuterCmptCEdge, StartPositionCpt);
            }



            if (clockwise == true)   //for an outer path, the edges are stored counter-clockwise in DCEL
            {
                foreach (var cpt in TraverseToGetCptEb(pOuterCmptCEdge, false))
                {
                    yield return cpt;
                }
            }
            else
            {
                foreach (var cpt in TraverseToGetCptEb(pOuterCmptCEdge, true))
                {
                    yield return cpt;
                }
            }

            if (blnIdentical == true)
            {
                yield return pOuterCmptCEdge.FrCpt;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cedgeComponent"></param>
        /// <param name="blnNext"></param>
        /// <returns></returns>
        private static CEdge TraverseToGetCorrectCEdgeComponent(CEdge cedgeComponent, CPoint cpt)
        {
            var currentcedge = cedgeComponent;
            do
            {                
                if (CCmpMethods.CmpCptYX(currentcedge.FrCpt, cpt)==0)
                {
                    return currentcedge;
                }

                currentcedge = currentcedge.cedgeNext;
            } while (currentcedge.GID != cedgeComponent.GID);
            throw new ArgumentException("we cannot find an appropriate edge!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cedgeComponent"></param>
        /// <param name="blnNext"></param>
        /// <returns></returns>
        private static IEnumerable<CPoint> TraverseToGetCptEb(CEdge cedgeComponent, bool blnNext)
        {
            var currentcedge = cedgeComponent;
            do
            {
                yield return currentcedge.FrCpt;

                if (blnNext == true)
                {
                    currentcedge = currentcedge.cedgeNext;
                }
                else
                {
                    currentcedge = currentcedge.cedgePrev;
                }

            } while (currentcedge.GID != cedgeComponent.GID);
        }

        private static IEnumerable<CEdge> TraverseToGetCEdgeEb(CEdge cedgeComponent, bool blnNext)
        {
            var currentcedge = cedgeComponent;
            do
            {
                yield return currentcedge;

                if (blnNext == true)
                {
                    currentcedge = currentcedge.cedgeNext;
                }
                else
                {
                    currentcedge = currentcedge.cedgePrev;
                }

            } while (currentcedge.GID != cedgeComponent.GID);
        }


        public List<List<CPoint>> GetInnerCptLtLt(bool clockwise = true, bool blnIdentical = true)
        {
            var innercptltlt = new List<List<CPoint>>();
            if (_InnerCmptCEdgeLt != null && _InnerCmptCEdgeLt.Count > 0)
            {
                foreach (var cedgeInnerComponent in _InnerCmptCEdgeLt)
                {
                    innercptltlt.Add(GetInnerCptEb(cedgeInnerComponent, clockwise, blnIdentical).ToList());
                }
            }
            else
            {
                throw new ArgumentException("This face does not have inner components!");
            }
            return innercptltlt;
        }

        public List<CPoint> GetOnlyInnerCptLt(bool clockwise = true, bool blnIdentical = true)
        {
            //var innercptltlt = new List<List<CPoint>>();
            if (_InnerCmptCEdgeLt != null && _InnerCmptCEdgeLt.Count == 1)
            {
                return GetInnerCptEb(_InnerCmptCEdgeLt.First(), clockwise, blnIdentical).ToList();
            }
            else
            {
                if (_InnerCmptCEdgeLt != null && _InnerCmptCEdgeLt.Count > 0)
                {
                    var cpglt = new List<CPolygon>();
                    foreach (var cptlt in GetInnerCptLtLt())
                    {
                        cpglt.Add(new CPolygon(-1, cptlt));
                    }

                    CSaveFeature.SaveCpgEb(clipperMethods.ScaleCpgEb( cpglt,1/CConstants.dblFclipper), "OuterFace");
                }

                throw new ArgumentException("This face has no or more than one inner components!");
            }
        }

        public List<CEdge> GetOnlyInnerCEdgeLt(bool clockwise = true)
        {
            //var innercptltlt = new List<List<CPoint>>();
            if (_InnerCmptCEdgeLt != null && _InnerCmptCEdgeLt.Count == 1)
            {
                return GetInnerCEdgeEb(_InnerCmptCEdgeLt.First(), clockwise).ToList();
            }
            else
            {
                throw new ArgumentException("This face has no or more than one inner components!");
            }
        }

        public IEnumerable<CPoint> GetInnerCptEb(CEdge cedgeComponent, bool clockwise = true, bool blnIdentical = true)
        {
            var pOuterCmptCEdge = cedgeComponent;

            if (clockwise == true)  //for an inner path, the edges are stored clockwise in DCEL
            {
                foreach (var cpt in TraverseToGetCptEb(pOuterCmptCEdge, true))
                {
                    yield return cpt;
                }
            }
            else
            {
                foreach (var cpt in TraverseToGetCptEb(pOuterCmptCEdge, false))
                {
                    yield return cpt;
                }
            }

            if (blnIdentical == true)
            {
                yield return pOuterCmptCEdge.FrCpt;
            }
        }

        public IEnumerable<CEdge> GetInnerCEdgeEb(CEdge cedgeComponent, bool clockwise = true)
        {
            var pOuterCmptCEdge = cedgeComponent;

            if (clockwise == true)  //for an inner path, the edges are stored clockwise in DCEL
            {
                foreach (var cedge in TraverseToGetCEdgeEb(pOuterCmptCEdge, true))
                {
                    yield return cedge;
                }
            }
            else
            {
                foreach (var cedge in TraverseToGetCEdgeEb(pOuterCmptCEdge, false))
                {
                    yield return cedge;
                }
            }
        }

        /// <summary>
        /// (counter clockwise???)
        /// </summary>
        /// <remarks>inner components</remarks>
        public List<CEdge> InnerCmptCEdgeLt
        {
            get { return _InnerCmptCEdgeLt; }
            set { _InnerCmptCEdgeLt = value; }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <remarks>outer component</remarks>
        public CEdge OuterCmptCEdge
        {
            get { return _OuterCmptCEdge; }
            set { _OuterCmptCEdge = value; }
        }

        public CEdge cedgeStartAtLeftMost
        {
            get { return _cedgeStartAtLeftMost; }
            set { _cedgeStartAtLeftMost = value; }
        }



        /// <summary>
        /// 中心点
        /// </summary>
        public CPoint SetCentroidCptSimple()
        {
            double dblSumX = 0;
            double dblSumY = 0;
            var cptlt = this.CptLt;
            for (int i = 0; i < cptlt.Count - 1; i++)
            {
                dblSumX += cptlt[i].X;
                dblSumY += cptlt[i].Y;
            }

            this.CentroidCptSimple = new CPoint(0, dblSumX / (cptlt.Count - 1), dblSumY / (cptlt.Count - 1));
            return this.CentroidCptSimple;
        }



        public CPoint LeftMostCpt
        {
            get { return _LeftMostCpt; }
            set { _LeftMostCpt = value; }
        }



        //public CPatch cpatch
        //{
        //    get { return _cpatch; }
        //    set { _cpatch = value; }
        //}

        public IPolygon4 pPolygon
        {
            get { return _pPolygon; }
            set
            {
                _pPolygon = value;
                _pGeo = value;
            }
        }



    }
}
