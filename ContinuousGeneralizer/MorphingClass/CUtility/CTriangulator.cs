using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MorphingClass.CUtility;
using MorphingClass.CGeometry;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesFile;

namespace MorphingClass.CUtility
{
    /// <summary>
    /// Performs the Delauney triangulation on a set of vertices.
    /// </summary>
    /// <remarks>
    /// Based on Paul Bourke's "An Algorithm for Interpolating Irregularly-Spaced Data
    /// with Applications in Terrain Modelling"
    /// http://astronomy.swin.edu.au/~pbourke/modelling/triangulate/
    /// </remarks>
    public class CTriangulator
    {
        private int _intMaxID;
        private object _Missing = Type.Missing;
        private double _dblVerySmall;
        

        #region ��������������AE���룩


        //private int _intBendDepthCount;
        //private double _dblBendDepthSum;
        //private double _dblBendForestDepthAverage;


        ///// <summary>���ԣ�����ɭ��ƽ�����</summary>
        //public double dblBendForestDepthAverage
        //{
        //    get { return _dblBendForestDepthAverage; }
        //}

        /// <summary>
        /// Performs Delauney triangulation on a set of points.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The triangulation doesn't support multiple points with the same planar location.
        /// Vertex-lists with duplicate points may result in strange triangulation with intersecting EdgeLt.
        /// To avoid adding multiple points to your vertex-list you can use the following anonymous predicate
        /// method:
        /// <code>
        /// if(!Vertices.Exists(delegate(Triangulator.Geometry.Point p) { return pNew.Equals2D(p); }))
        ///		Vertices.Add(pNew);
        /// </code>
        /// </para>
        /// <para>The triangulation algorithm may be described in pseudo-code as follows:
        /// <code>
        /// subroutine Triangulate
        /// input : vertex list
        /// output : triangle list
        ///    initialize the triangle list
        ///    determine the supertriangle
        ///    add supertriangle vertices to the end of the vertex list
        ///    add the supertriangle to the triangle list
        ///    for each sample point in the vertex list
        ///       initialize the cedge buffer
        ///       for each triangle currently in the triangle list
        ///          calculate the triangle circumcircle center and radius
        ///          if the point lies in the triangle circumcircle then
        ///             add the three triangle EdgeLt to the cedge buffer
        ///             remove the triangle from the triangle list
        ///          endif
        ///       endfor
        ///       delete all doubly specified EdgeLt from the cedge buffer
        ///          this leaves the EdgeLt of the enclosing polygon only
        ///       add to the triangle list all triangles formed between the point 
        ///          and the EdgeLt of the enclosing polygon
        ///    endfor
        ///    remove any triangles from the triangle list that use the supertriangle vertices
        ///    remove the supertriangle vertices from the vertex list
        /// end
        /// </code>
        ///// </para>
        /// </remarks>
        /// <param name="Vertex">List of vertices to triangulate.</param>        
        /// <returns>Triangles referencing vertex indices arranged in clockwise order
        /// ���գ������в��䣬�����ص����������а������������ζ��������������</returns>        
        public List<CTriangle> Triangulate(ref List<CPoint> VertexLt)
        {

            int intVertexCount = VertexLt.Count;
            if (intVertexCount < 3)
                throw new ArgumentException("Need at least three vertices for triangulation");

            int intTriMax = 4 * intVertexCount;

            // Find the maximum and minimum vertex bounds.
            // This is to allow calculation of the bounding supertriangle
            double xmin = VertexLt[0].X;
            double ymin = VertexLt[0].Y;
            double xmax = xmin;
            double ymax = ymin;
            for (int i = 1; i < intVertexCount; i++)
            {
                if (VertexLt[i].X < xmin) xmin = VertexLt[i].X;
                if (VertexLt[i].X > xmax) xmax = VertexLt[i].X;
                if (VertexLt[i].Y < ymin) ymin = VertexLt[i].Y;
                if (VertexLt[i].Y > ymax) ymax = VertexLt[i].Y;
            }

            double dx = xmax - xmin;
            double dy = ymax - ymin;
            double dmax = (dx > dy) ? dx : dy;

            double xmid = (xmax + xmin) * 0.5;
            double ymid = (ymax + ymin) * 0.5;

            // Set up the supertriangle
            // This is a triangle which encompasses all the sample points.
            // The supertriangle coordinates are added to the end of the
            // vertex list. The supertriangle is the first triangle in
            // the triangle list.
            int intMaxID = GetMaxID(VertexLt);
            _intMaxID = intMaxID;
            VertexLt.Add(new CPoint(intMaxID + 1, (xmid - 2 * dmax), (ymid - dmax)));
            VertexLt.Add(new CPoint(intMaxID + 2, xmid, (ymid + 2 * dmax)));
            VertexLt.Add(new CPoint(intMaxID + 3, (xmid + 2 * dmax), (ymid - dmax)));

            List<CTriangle> TriangleLt = new List<CTriangle>();
            TriangleLt.Add(new CTriangle(VertexLt[intVertexCount], VertexLt[intVertexCount + 1], VertexLt[intVertexCount + 2])); //SuperTriangle placed at index 0

            // Include each point one at a time into the existing mesh
            for (int i = 0; i < intVertexCount; i++)
            {
                List<CEdge> EdgeLt = new List<CEdge>(); //[trimax * 3];
                // Set up the cedge buffer.
                // If the point (Vertex(i).x,Vertex(i).y) lies inside the circumcircle then the
                // three EdgeLt of that triangle are added to the cedge buffer and the triangle is removed from list.
                for (int j = 0; j < TriangleLt.Count; j++)
                {
                    if (InCircle(VertexLt[i], TriangleLt[j].CptLt[0], TriangleLt[j].CptLt[1], TriangleLt[j].CptLt[2]))
                    {
                        EdgeLt.Add(new CEdge(TriangleLt[j].CptLt[0], TriangleLt[j].CptLt[1]));
                        EdgeLt.Add(new CEdge(TriangleLt[j].CptLt[1], TriangleLt[j].CptLt[2]));
                        EdgeLt.Add(new CEdge(TriangleLt[j].CptLt[2], TriangleLt[j].CptLt[0]));
                        TriangleLt.RemoveAt(j);
                        j--;
                    }
                }
                if (i >= intVertexCount) continue; //In case we the last duplicate point we removed was the last in the array(Ӧ�ò��ᷢ��������)

                // Remove duplicate EdgeLt
                // Note: if all triangles are specified anticlockwise then all
                // interior EdgeLt are opposite pointing in direction.
                for (int j = EdgeLt.Count - 2; j >= 0; j--)
                {
                    for (int k = EdgeLt.Count - 1; k >= j + 1; k--)
                    {
                        if (EdgeLt[j].Equals(EdgeLt[k]))
                        {
                            EdgeLt.RemoveAt(k);
                            EdgeLt.RemoveAt(j);
                            k--;
                            continue;
                        }
                    }
                }
                // Form new triangles for the current point
                // Skipping over any tagged EdgeLt.
                // All EdgeLt are arranged in clockwise order.
                for (int j = 0; j < EdgeLt.Count; j++)
                {
                    if (TriangleLt.Count >= intTriMax)
                        throw new ApplicationException("Exceeded maximum EdgeLt");
                    TriangleLt.Add(new CTriangle(EdgeLt[j].FrCpt, EdgeLt[j].ToCpt, VertexLt[i]));
                }
                EdgeLt.Clear();
                EdgeLt = null;
            }

            //// Remove triangles with supertriangle vertices
            //// These are triangles which have a vertex number greater than nv
            //for (int i = TriangleLt.Count - 1; i >= 0; i--)
            //{
            //    if (TriangleLt[i].CptLt[0].ID > intMaxID || TriangleLt[i].CptLt[1].ID > intMaxID || TriangleLt[i].CptLt[2].ID > intMaxID)
            //        TriangleLt[i].isCrustTriangle = true;
            //        //TriangleLt.RemoveAt(i);

            //}

            //Remove SuperTriangle vertices
            VertexLt.RemoveAt(VertexLt.Count - 1);
            VertexLt.RemoveAt(VertexLt.Count - 1);
            VertexLt.RemoveAt(VertexLt.Count - 1);
            TriangleLt.TrimExcess();
            return TriangleLt;
        }

        /// <summary>
        /// Returns true if the point (p) lies inside the circumcircle made up by points (p1,p2,p3)
        /// </summary>
        /// <remarks>
        /// NOTE: A point on the cedge is inside the circumcircle
        /// </remarks>
        /// <param name="p">Point to check</param>
        /// <param name="p1">First point on circle</param>
        /// <param name="p2">Second point on circle</param>
        /// <param name="p3">Third point on circle</param>
        /// <returns>true if p is inside circle</returns>
        private static bool InCircle(CPoint cpt, CPoint cpt1, CPoint cpt2, CPoint cpt3)
        {
            //Return TRUE if the point (xp,yp) lies inside the circumcircle
            //made up by points (x1,y1) (x2,y2) (x3,y3)
            //NOTE: A point on the cedge is inside the circumcircle

            if (System.Math.Abs(cpt1.Y - cpt2.Y) < double.Epsilon && System.Math.Abs(cpt2.Y - cpt3.Y) < double.Epsilon)
            {
                //INCIRCUM - F - Points are coincident !!
                return false;
            }

            double m1, m2;
            double mx1, mx2;
            double my1, my2;
            double xc, yc;

            if (System.Math.Abs(cpt2.Y - cpt1.Y) < double.Epsilon)
            {
                m2 = -(cpt3.X - cpt2.X) / (cpt3.Y - cpt2.Y);
                mx2 = (cpt2.X + cpt3.X) * 0.5;
                my2 = (cpt2.Y + cpt3.Y) * 0.5;
                //Calculate CircumCircle center (xc,yc)
                xc = (cpt2.X + cpt1.X) * 0.5;
                yc = m2 * (xc - mx2) + my2;
            }
            else if (System.Math.Abs(cpt3.Y - cpt2.Y) < double.Epsilon)
            {
                m1 = -(cpt2.X - cpt1.X) / (cpt2.Y - cpt1.Y);
                mx1 = (cpt1.X + cpt2.X) * 0.5;
                my1 = (cpt1.Y + cpt2.Y) * 0.5;
                //Calculate CircumCircle center (xc,yc)
                xc = (cpt3.X + cpt2.X) * 0.5;
                yc = m1 * (xc - mx1) + my1;
            }
            else
            {
                m1 = -(cpt2.X - cpt1.X) / (cpt2.Y - cpt1.Y);
                m2 = -(cpt3.X - cpt2.X) / (cpt3.Y - cpt2.Y);
                mx1 = (cpt1.X + cpt2.X) * 0.5;
                mx2 = (cpt2.X + cpt3.X) * 0.5;
                my1 = (cpt1.Y + cpt2.Y) * 0.5;
                my2 = (cpt2.Y + cpt3.Y) * 0.5;
                //Calculate CircumCircle center (xc,yc)
                xc = (m1 * mx1 - m2 * mx2 + my2 - my1) / (m1 - m2);
                yc = m1 * (xc - mx1) + my1;
            }

            double dx = cpt2.X - xc;
            double dy = cpt2.Y - yc;
            double rsqr = dx * dx + dy * dy;
            //double r = Math.Sqrt(rsqr); //Circumcircle radius
            dx = cpt.X - xc;
            dy = cpt.Y - yc;
            double drsqr = dx * dx + dy * dy;

            return (drsqr <= rsqr);
        }



        /// <summary>
        /// ��ȡ����ID��
        /// </summary>
        /// <param name="cptlt">������</param> 
        /// <returns>�����и���ġ�ID�������ֵ</returns>
        private int GetMaxID(List<CPoint> cptlt)
        {
            int intMaxID = 0;
            for (int i = 0; i < cptlt.Count; i++)
            {
                if (intMaxID < cptlt[i].ID)
                {
                    intMaxID = cptlt[i].ID;
                }
            }
            return intMaxID;
        }
        #endregion

        /// <summary>
        /// ������С��������
        /// </summary>
        /// <param name="cpl">����</param>
        /// <param name="dblVerySmall">��Сֵ</param> 
        /// <remarks>���ھ��Ȳ�һ�£����ж϶�����ȵ�ʱ����ܻ���һЩ�鷳(�������в���Ѱ��ԭʼ��Ӧ�������澫�Ȳ�һ�µ�ķ������)</remarks>
        public List<CPoint> CreateConvexHullEdgeLt2(CPolyline cpl, double dblVerySmall)
        {
            List<CPoint> cptlt = new List<CPoint>(cpl.CptLt.Count);
            cptlt.AddRange(cpl.CptLt);

            ITopologicalOperator pTop = cpl.pPolyline as ITopologicalOperator;            
            pTop.Simplify();

            IGeometry pCHGeo = pTop.ConvexHull();
            IPointCollection4 pCol = pCHGeo as IPointCollection4;

            List<CPoint> CHPtLt = new List<CPoint>(pCol.PointCount);  //�洢������ζ���(������β��)
            for (int i = 0; i < pCol.PointCount - 1; i++)//pCol���Ѿ��������غϵ���β�㣬��˲���������һ����
            {
                CPoint cpt=new CPoint (i,pCol.get_Point(i));
                for (int j = cptlt.Count-1; j >= 0; j--)
                {
                    if (cpt.Equals2D (cptlt[j],dblVerySmall))
                    {
                        CHPtLt.Add(cptlt[j]);
                        cptlt.RemoveAt(j);
                        break;
                    }
                }
            }
            CHPtLt.Add(CHPtLt[0]);

            return CHPtLt;
        }

        //#region ������С�������Σ���AE���룩
        ///// <summary>
        ///// ������С��������
        ///// </summary>
        ///// <param name="CVetexLt">������</param> 
        ///// <param name="dblVerySmall">��Сֵ</param> 
        ///// <remarks></remarks>
        //public List<CEdge> CreateConvexHullEdgeLt(List<CPoint> CVetexLt, double dblVerySmall)
        //{
        //    //���ҵ�x������С�ĵ���Ϊ��ʼ��
        //    double dblMinX = CVetexLt[0].X;
        //    int intBeginIndex = 0;
        //    for (int i = 0; i < CVetexLt.Count; i++)
        //    {
        //        if (CVetexLt[i].X < dblMinX)
        //        {
        //            dblMinX = CVetexLt[i].X;
        //            intBeginIndex = i;
        //        }

        //    }

        //    List<CEdge> CEdgeLt = new List<CEdge>();
        //    CreateConvexHull(CVetexLt, ref CEdgeLt, intBeginIndex, intBeginIndex, dblVerySmall);
        //    return CEdgeLt;
        //}


        //private void CreateConvexHull(List<CPoint> CVetexLt, ref  List<CEdge> CEdgeLt, int intCurrentIndex, int intBeginIndex, double dblVerySmall)
        //{

        //    //�ҵ�͹�������
        //    //ÿ������һ���ߣ��ж��������е��Ƿ��ڸñߵ��ұߣ�����ǵģ���ñ�Ϊһ͹����
        //    int i = 0;
        //    IPoint ipt1 = new PointClass(); IPoint ipt2 = new PointClass();
        //    double dblAlongDis1 = new double(); double dblAlongDis2 = new double();
        //    double dblFromDis1 = new double(); double dblFromDis2 = new double();
        //    bool blnRight1 = new bool(); bool blnRight2 = new bool();
        //    for (i = 0; i < CVetexLt.Count; i++)
        //    {

        //        if (intCurrentIndex == i) continue;
        //        CEdge cln = new CEdge(CVetexLt[intCurrentIndex], CVetexLt[i]);
        //        bool isExistLeft = false;
        //        for (int j = 0; j < CVetexLt.Count; j++)
        //        {
        //            if (i == j || intCurrentIndex == j) continue;
        //            cln.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, CVetexLt[j], true, ipt1, ref dblAlongDis1, ref dblFromDis1, ref blnRight1);
        //            cln.QueryPointAndDistance(esriSegmentExtension.esriExtendEmbedded, CVetexLt[j], true, ipt2, ref dblAlongDis2, ref dblFromDis2, ref blnRight2);


        //            if (blnRight1 == true) continue;  //�������cln���ұ���û�����                    
        //            else if (blnRight1 == false)
        //            {
        //                //�㲻��cln���ұߣ����ܳ��ֵ���cln�ϣ������cln���ӳ����ϵ�����
        //                //    �������cln���ӳ����ϣ���cln����Ϊ͹������εı�
        //                //    �������cln�ϣ���cln������Ϊ͹������εı�
        //                if (dblFromDis1 > 0 && dblFromDis2 < dblVerySmall) continue;  //����cln���ӳ�����
        //                else //����cln�ϻ����cln���
        //                {
        //                    isExistLeft = true;
        //                    break;
        //                }
        //            }
        //        }
        //        if (isExistLeft == false)
        //        {
        //            CEdgeLt.Add(cln);
        //            break;
        //        }
        //    }

        //    if (intBeginIndex == i) return;//�����ص���������˳�
        //    else CreateConvexHull(CVetexLt, ref CEdgeLt, i, intBeginIndex, dblVerySmall);
        //}
        //#endregion
        

        //#region ����Լ������������AE���룩
        ///// <summary>
        ///// ����Լ��������
        ///// </summary>
        ///// <param name="CTriangleLt">�������(��Լ��Delaunay������)</param>
        ///// <param name="CEdgeLt">Լ����</param> 
        ///// <remarks></remarks>
        //public void CreateCDT(ref List<CTriangle> CTriangleLt, List<CEdge> CEdgeLt)
        //{
        //    for (int i = 0; i < CEdgeLt.Count; i++)
        //    {

        //        //����Լ���߶�
        //        CEdge cln = CEdgeLt[i];
        //        IRelationalOperator pRelationalOperator = (IRelationalOperator)cln;

        //        //��¼��Լ���߶��ཻ�������Σ���û�У����������������������Լ���߶�
        //        List<CTriangle> CrossTriangleLt = new List<CTriangle>();
        //        for (int j = CTriangleLt.Count - 1; j >= 0; j--)
        //        {
        //            if (pRelationalOperator.Crosses(CTriangleLt[j]))
        //            {
        //                CrossTriangleLt.Add(CTriangleLt[j]);
        //                CTriangleLt.RemoveAt(j);
        //            }
        //        }
        //        if (CrossTriangleLt.Count == 0) continue;
        //        //if (CrossTriangleLt.Count == 0) return;

        //        //�ֱ��ҵ�pln���Ҳ���߶�
        //        List<CEdge> LeftBLLt = new List<CEdge>();
        //        List<CEdge> RightBLLt = new List<CEdge>();
        //        for (int j = 0; j < CrossTriangleLt.Count; j++)
        //        {
        //            IPoint centerpt = new PointClass();    //��һ���߶ε��е�
        //            IPoint ipt = new PointClass();
        //            double dblAlongDis = new double();
        //            double dblFromDis = new double();
        //            bool blnRight = new bool();
        //            for (int l = 0; l < CrossTriangleLt[j].CEdgeLt.Count; l++)
        //            {
        //                if (pRelationalOperator.Crosses(CrossTriangleLt[j].CEdgeLt[l]) == false)
        //                {
        //                    //�ҵ��߶�l���е�centerpt��������centerptλ���߶�cln�����ң����ж�lλ���߶�cln������
        //                    centerpt = new PointClass();
        //                    CrossTriangleLt[j].CEdgeLt[l].QueryPoint(esriSegmentExtension.esriNoExtension, 0.5, true, centerpt);
        //                    cln.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, centerpt, true, ipt, ref dblAlongDis, ref dblFromDis, ref blnRight);
        //                    if (blnRight) RightBLLt.Add(CrossTriangleLt[j].CEdgeLt[l]);
        //                    else LeftBLLt.Add(CrossTriangleLt[j].CEdgeLt[l]);
        //                }
        //            }
        //        }

        //        //�����ĵ�����(˳ʱ��)
        //        List<CPoint> LeftPtLt = new List<CPoint>();
        //        LeftPtLt.Add(cln.FrCpt);
        //        Sortcpt(ref LeftPtLt, LeftBLLt);

        //        //���Ҳ�ĵ�����(˳ʱ��)
        //        List<CPoint> RightPtLt = new List<CPoint>();
        //        RightPtLt.Add(cln.ToCpt);
        //        Sortcpt(ref RightPtLt, RightBLLt);

        //        List<CTriangle> LeftDTLt = new List<CTriangle>();
        //        CreateDT(ref LeftDTLt, ref LeftPtLt, LeftBLLt);

        //        List<CTriangle> RightDTLt = new List<CTriangle>();
        //        CreateDT(ref RightDTLt, ref RightPtLt, RightBLLt);

        //        for (int j = 0; j < LeftDTLt.Count; j++)
        //        {
        //            CTriangleLt.Add(LeftDTLt[j]);
        //        }
        //        for (int j = 0; j < RightDTLt.Count; j++)
        //        {
        //            CTriangleLt.Add(RightDTLt[j]);
        //        }
        //    }
        //}

        ///// <summary>
        ///// ������cptlt�еĵ���ָ����Ϊ���˳ʱ������
        ///// </summary>
        ///// <param name="cptlt">������</param> 
        ///// <param name="cBLlt">�߽�����</param> 
        ///// <returns >�����и���ġ�ID�������ֵ</returns>
        //private void Sortcpt(ref List<CPoint> cptlt, List<CEdge> cBLlt)
        //{
        //    List<CEdge> newBLlt = new List<CEdge>();//Ϊ�˲��ƻ�ԭ���ı����飬�¶���һ�����飬������������в���
        //    for (int i = 0; i < cBLlt.Count; i++)
        //    {
        //        newBLlt.Add(cBLlt[i]);
        //    }

        //    while (newBLlt.Count > 0)
        //    {
        //        CPoint lastcpt = cptlt[cptlt.Count - 1];
        //        for (int j = 0; j < newBLlt.Count; j++)
        //        {
        //            if (lastcpt.Equals2D(newBLlt[j].FrCpt))
        //            {
        //                cptlt.Add(newBLlt[j].ToCpt);
        //                newBLlt.RemoveAt(j);
        //                break;
        //            }
        //            else if (lastcpt.Equals2D(newBLlt[j].ToCpt))
        //            {
        //                cptlt.Add(newBLlt[j].FrCpt);
        //                newBLlt.RemoveAt(j);
        //                break;
        //            }
        //        }
        //    }
        //}


        ///// <summary>
        ///// ������cptlt�еĵ���ָ����Ϊ���˳ʱ������
        ///// </summary>
        ///// <param name="DTLt">�յĶ������</param>  
        ///// <param name="cptlt">������</param> 
        ///// <param name="cBLlt">�߽�����</param> 
        //private void CreateDT(ref List<CTriangle> DTLt, ref List<CPoint> cptlt, List<CEdge> cBLlt)
        //{
        //    //����������
        //    CreateTriangulate(ref DTLt, ref cptlt, cBLlt, 0, cptlt.Count - 1);

        //    //�ֲ�LOP�Ż�
        //    LOPOptimize(ref DTLt);
        //}

        ///// <summary>
        ///// �ݹ�ķ�������������
        ///// </summary>
        ///// <param name="DTLt">�������</param>  
        ///// <param name="cptlt">������</param> 
        ///// <param name="cBLlt">�߽�����</param> 
        ///// <param name="intI">��ţ�ָ��cptlt�еĵ�intI����</param> 
        ///// <param name="intJ">��ţ�ָ��cptlt�еĵ�intJ����</param> 
        //private void CreateTriangulate(ref List<CTriangle> DTLt, ref List<CPoint> cptlt, List<CEdge> cBLlt, int intI, int intJ)
        //{

        //    CEdge cpl = new CEdge(cptlt[intI], cptlt[intJ]);
        //    for (int i = 0; i < cBLlt.Count; i++)
        //    {
        //        if (cpl.Equals(cBLlt[i]))
        //            return;
        //    }

        //    double dblMinDis = double.MaxValue;
        //    int intMinDisIndex = new int();
        //    for (int i = intI + 1; i < intJ; i++)
        //    {
        //        IPoint ipt = new PointClass();
        //        double dblDisAlong = new double();
        //        double dblDisFrom = new double();
        //        bool blnRight = new bool();
        //        cpl.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, cptlt[i], false, ipt, ref dblDisAlong, ref dblDisFrom, ref blnRight);
        //        if (blnRight)
        //        {
        //            MessageBox.Show("�����ߵ��ұ��ˣ���鿴CCDT��780�У�");
        //        }
        //        if (dblDisFrom < dblMinDis)
        //        {
        //            dblMinDis = dblDisFrom;
        //            intMinDisIndex = i;
        //        }
        //    }

        //    CTriangle Triangle = new CTriangle(cptlt[intI], cptlt[intMinDisIndex], cptlt[intJ]);
        //    DTLt.Add(Triangle);

        //    //���Ƶ���
        //    CreateTriangulate(ref DTLt, ref  cptlt, cBLlt, intI, intMinDisIndex);
        //    CreateTriangulate(ref DTLt, ref  cptlt, cBLlt, intMinDisIndex, intJ);
        //}


        ///// <summary>
        ///// �ֲ�LOP�Ż�
        ///// </summary>
        ///// <param name="DTLt">�����ɵĶ������</param>  
        //private void LOPOptimize(ref List<CTriangle> DTLt)
        //{
        //    int i, j, l;
        //    int m = new int();

        //    bool blnIsChange = true;
        //    while (blnIsChange == true)
        //    {
        //        blnIsChange = false;
        //        for (i = 0; i < DTLt.Count - 1; i++)
        //        {
        //            for (j = i + 1; j < DTLt.Count; j++)
        //            {
        //                //�ҵ����������εĹ�����
        //                bool IsFindSameEdge = false;
        //                for (l = 0; l < DTLt[i].CEdgeLt.Count; l++)
        //                {
        //                    for (m = 0; m < DTLt[j].CEdgeLt.Count; m++)
        //                    {
        //                        if (DTLt[i].CEdgeLt[l].Equals(DTLt[j].CEdgeLt[m]))
        //                        {
        //                            IsFindSameEdge = true;
        //                            break;
        //                        }
        //                    }
        //                    if (IsFindSameEdge)
        //                        break;
        //                }
        //                if (IsFindSameEdge == false)  //������������β����ڹ����ߣ���ֱ�ӱ�����һ��������
        //                    continue;

        //                //�������һ�������ε����Բ��������һ�������εĶ��㣬�򽻻��Խ���
        //                if (InCircle(DTLt[i].CptLt[l], DTLt[j].CptLt[0], DTLt[j].CptLt[1], DTLt[j].CptLt[2]) ||
        //                    InCircle(DTLt[j].CptLt[m], DTLt[i].CptLt[0], DTLt[i].CptLt[1], DTLt[i].CptLt[2]))
        //                {

        //                    //Ϊʲô��ֱ��������������Σ�
        //                    //�ԡ�������i��l�㡱Ϊ��㣬��������j��m�㡱Ϊ�յ���������lm����������������i��˳ʱ��ģ�
        //                    //���ԡ�������i��l�ߵ�FrCpt����Ȼ��lm����ߣ���������i��l�ߵ�ToCpt����Ȼ��lm���ұߣ���˿���lmΪ�ֽ��ߣ�
        //                    //������������˳ʱ��������
        //                    CTriangle LeftTriangle = new CTriangle(DTLt[i].CptLt[l], DTLt[i].CEdgeLt[l].FrCpt, DTLt[j].CptLt[m]);
        //                    CTriangle RightTriangle = new CTriangle(DTLt[j].CptLt[m], DTLt[i].CEdgeLt[l].ToCpt, DTLt[i].CptLt[l]);

        //                    DTLt.RemoveAt(j);
        //                    DTLt.RemoveAt(i);
        //                    DTLt.Add(LeftTriangle);
        //                    DTLt.Add(RightTriangle);
        //                    blnIsChange = true;
        //                }
        //            }
        //            if (blnIsChange == true)
        //                break;
        //        }
        //    }

        //}
        //#endregion
        

        /// <summary>
        /// ����Լ��������(����AE��TIN�ṹ)
        /// </summary>
        /// <param name="cpl">������Լ�����������߶�</param>
        /// <remarks>ע�⣺���������ı�cpl��ֵ</remarks>
        public List<CTriangle> CreateCDT(IFeatureLayer pFeatureLayer,ref CPolyline cpl, double dblVerySmall)
        {
            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            IGeoDataset pGDS = (IGeoDataset)pFeatureClass;

            IEnvelope pEnv = (IEnvelope)pGDS.Extent;
            pEnv.SpatialReference = pGDS.SpatialReference;
            double dblSuperlength = 2 * (pEnv.Width + pEnv.Height);

            IFields pFields = pFeatureClass.Fields;

            IField pHeightFiled = new FieldClass();
            //ע�⣺�˴��Ĵ���ȷʵ��̫�ã��������β�����д�ģ��Ժ���ʱ������
            try
            {
                pHeightFiled = pFields.get_Field(pFields.FindField("Id"));
            }
            catch (Exception)
            {
                pHeightFiled = pFields.get_Field(pFields.FindField("ID"));
                throw;
            }


            ITinEdit pTinEdit = new TinClass();
            pTinEdit.InitNew(pEnv);
            object Missing = Type.Missing;
            pTinEdit.AddFromFeatureClass(pFeatureClass, null, pHeightFiled, null, esriTinSurfaceType.esriTinHardLine, ref Missing);

            ITinAdvanced2 pTinAdvanced2 = (ITinAdvanced2)pTinEdit;
            List<IPolygon4> PolygonLt = new List<IPolygon4>(pTinAdvanced2.TriangleCount);
            for (int i = 1; i <= pTinAdvanced2.TriangleCount; i++)  //��ȡ����������
            {
                ITinTriangle pTinTriangle = pTinAdvanced2.GetTriangle(i);
                IPointCollection4 pCol = new PolygonClass();
                for (int j = 0; j < 3; j++)
                {
                    ITinEdge pTinEdge = new TinEdgeClass();
                    pTinEdge = pTinTriangle.get_Edge(j);
                    IPoint ipt = new PointClass();
                    ipt.PutCoords(pTinEdge.FromNode.X, pTinEdge.FromNode.Y);
                    pCol.AddPoint(ipt, ref Missing, ref Missing);
                }
                IPolygon4 pPolygon = pCol as IPolygon4;
                pPolygon.Close();
                PolygonLt.Add(pPolygon);
            }

            //��ȡ�����Σ����������α��Ϊһ��������
            List<CTriangle> CTriangleLt = new List<CTriangle>(PolygonLt.Count);
            List<CPoint> ctempptlt = new List<CPoint>(cpl.CptLt.Count);
            ctempptlt.AddRange(cpl.CptLt);
            for (int i = 0; i < PolygonLt.Count; i++)
            {
                //���������ɵ�TIN�е�������������ԭʼ������ܲ�һ�£���ԭʼ���������ҵ���Ӧ���㲢����������
                IPointCollection4 pCol = PolygonLt[i] as IPointCollection4;
                CPoint cpt0 = new CPoint();
                CPoint cpt1 = new CPoint();
                CPoint cpt2 = new CPoint();
                FindSamePoint(pCol.get_Point(0), cpl,ref ctempptlt, dblVerySmall, ref cpt0);
                FindSamePoint(pCol.get_Point(1), cpl, ref ctempptlt, dblVerySmall, ref cpt1);
                FindSamePoint(pCol.get_Point(2), cpl, ref ctempptlt, dblVerySmall, ref cpt2);

                CTriangle pCTriangle = new CTriangle(i, cpt0, cpt1, cpt2);
                if (PolygonLt[i].Length > dblSuperlength) pCTriangle.strTriType = "I";  //���һ�೬�����Σ��������Σ�
                CTriangleLt.Add(pCTriangle);
            }

            //�����µ�����
            SortedDictionary<double, CPoint> newcptslt = new SortedDictionary<double, CPoint>(new CCmpDbl());
            for (int i = 0; i < ctempptlt.Count; i++)
            {
                double dblFromStartDis = CGeoFunc.CalDistanceFromStartPoint(cpl.pPolyline, (IPoint)ctempptlt[i], true);
                newcptslt.Add(dblFromStartDis, ctempptlt[i]);
            }

            List<CPoint> newcptlt = newcptslt.Values.ToList();
                //new List<CPoint>();
            //newcptlt.AddRange(newcptslt.Values);
            //newcptlt.AddRange ()
            //for (int i = 0; i < newcptslt.Count; i++)
            //{
            //    newcptlt.Add(newcptslt.Values[i]);
            //}

            //for (int i = 0; i < newcptlt.Count ; i++)
            //{
            //    newcptlt[i].ID = i;
            //}
            cpl = new CPolyline(cpl.ID, newcptlt);

            return CTriangleLt;

        }



        /// <summary>
        /// ��ȡ����������
        /// </summary>
        /// <param name="CTriangleLt">�������</param>
        /// <param name="CVetexLt">������</param> 
        /// <remarks>SE:Share Edge ���ߣ�ÿ�������ε������߶���һ������������(��Ϊ�չ���������)</remarks>
        public void GetSETriangle(ref List<CTriangle> CTriangleLt, double dblVerySmall)
        {
            int l, m;
            m = 0;

            //��ʼ��ÿ�������Σ�����������������������Ϊ��������
            for (int i = 0; i < CTriangleLt.Count; i++)
            {
                List<CTriangle> pTriangleLt = new List<CTriangle>(3);
                for (int j = 0; j < 3; j++)
                {
                    var pTriangle = new CTriangle(-2); //TID = -2: empty triangle
                    pTriangleLt.Add(pTriangle);
                }
                CTriangleLt[i].SETriangleLt = pTriangleLt;
            }

            for (int i = 0; i < CTriangleLt.Count; i++)
            {
                int intCount = 0;
                for (l = 0; l < CTriangleLt[i].CEdgeLt.Count; l++)
                {
                    if (CTriangleLt[i].SETriangleLt[l].TID != -2)
                    {
                        intCount = intCount + 1;
                        continue;  //����ñ߶�Ӧ�Ĺ����������Ѿ����ڣ����ٱ�����ÿ��������ֻ��һ�����������Σ�
                    }

                    for (int j = i + 1; j < CTriangleLt.Count; j++)
                    {
                        //�ҵ����������εĹ�����
                        bool IsFindSameEdge = false;
                        for (m = 0; m < CTriangleLt[j].CEdgeLt.Count; m++)
                        {
                            if (CTriangleLt[j].SETriangleLt[m].TID != -2) continue;
                            double disdiff = Math.Abs(CTriangleLt[i].CEdgeLt[l].dblLength - CTriangleLt[j].CEdgeLt[m].dblLength);
                            if (CTriangleLt[i].CEdgeLt[l].Equals(CTriangleLt[j].CEdgeLt[m]))
                            {
                                CTriangleLt[i].SETriangleLt[l] = CTriangleLt[j];
                                CTriangleLt[j].SETriangleLt[m] = CTriangleLt[i];
                                IsFindSameEdge = true;
                                intCount = intCount + 1;
                                break;
                            }
                        }
                        if (IsFindSameEdge)
                            break;
                    }

                    if (intCount == 3)//�ҵ���������������������Ҫ������
                        break;
                }
                CTriangleLt[i].SETriangleNum = intCount;
            }
        }

        ///// <summary>��ȡ����ĳ�ߵ�������</summary>
        ///// <param name="CTriangleLt">�������</param>
        ///// <param name="pPolyline">����</param> 
        ///// <param name="blnRight">�Ƿ��ұߵĶ����</param>  
        ///// <remarks>blnRightΪTrueʱ���ú������������ұߵ������Σ�blnRightΪfalseʱ���ú�������������ߵ�������
        /////          ����ֱ���������߶����жϻ����һЩС���⣬����������÷ֶ��жϵķ���</remarks>
        //public void GetSideTriangle2(ref List<CTriangle> CTriangleLt, List<CEdge> CEdgeLt, bool blnRight)
        //{

        //    //�ж�ÿ�������ε�ÿ�����Ƿ����ڱ߼�������ǣ������ݸñߣ��жϸ��������ڱߵ���һ��
        //    double dblAlongDis = new double();
        //    double dblFromDis = new double();
        //    for (int i = 0; i < CTriangleLt.Count; i++)
        //    {
        //        for (int j = 0; j < CTriangleLt[i].CEdgeLt.Count; j++)
        //        {
        //            for (int l = 0; l < CEdgeLt.Count; l++)
        //            {
        //                if (CTriangleLt[i].CEdgeLt[j].Equals(CEdgeLt[l]))
        //                {
        //                    CTriangleLt[i].CEdgeLt[j].isBelongToPolyline = true;//��¼�ñ�Ϊ�����ϵıߣ����㵽ʱ���жϸ�������Ϊ����������
        //                    if (CTriangleLt[i].isSideJudge == false)  //������жϹ������������ж�
        //                    {
        //                        IPoint ipt = new PointClass();
        //                        bool blnRightSide = new bool();
        //                        CEdgeLt[l].QueryPointAndDistance(esriSegmentExtension.esriExtendEmbedded, CTriangleLt[i].CentroidCpt, false, ipt, ref dblAlongDis, ref dblFromDis, ref blnRightSide);
        //                        //�Ƿ���Ҫ�ҵ�һ���������
        //                        if (blnRightSide == blnRight) CTriangleLt[i].isNeedSide = true;
        //                        else CTriangleLt[i].isNeedSide = false;
        //                        CTriangleLt[i].isSideJudge = true;
        //                    }

        //                }
        //            }
        //        }
        //    }

        //    for (int i = 0; i < CTriangleLt.Count; i++)
        //    {
        //        if (CTriangleLt[i].isSideJudge == true) continue;
        //        for (int j = 0; j < CTriangleLt[i].SETriangleLt.Count; j++)
        //        {
        //            if (CTriangleLt[i].SETriangleLt[j].TID == -2) continue;  //��������������β����ڣ���ֱ�ӱ�����һ������������
        //            if (CTriangleLt[i].SETriangleLt[j].isNeedSide == true)
        //            {
        //                CTriangleLt[i].isNeedSide2 = true;
        //                break;
        //            }
                        
        //        }
        //    }

        //    for (int i = 0; i < CTriangleLt.Count; i++)
        //    {
        //        if (CTriangleLt[i].isSideJudge == true || CTriangleLt[i].isNeedSide2 == true) continue;
        //        for (int j = 0; j < CTriangleLt[i].SETriangleLt.Count; j++)
        //        {
        //            if (CTriangleLt[i].SETriangleLt[j].TID == -2) continue;  //��������������β����ڣ���ֱ�ӱ�����һ������������
        //            if (CTriangleLt[i].SETriangleLt[j].isNeedSide2 == true)
        //            {
        //                CTriangleLt[i].isNeedSide3 = true;
        //                break;
        //            } 
        //        }
        //    }

        //    //��Ӳ��������������
        //    for (int i = 0; i < CTriangleLt.Count; i++)
        //    {
        //        if (CTriangleLt[i].isNeedSide == true || CTriangleLt[i].isNeedSide2 == true || CTriangleLt[i].isNeedSide3 == true)
        //            CTriangleLt[i].isNeedSide = true;
        //    }
        //}

        /// <summary>ȷ�������������ߵ��ı�("Left"��"Right")</summary>
        /// <param name="CTriangleLt">�������</param>
        /// <param name="CEdgeLt">���߱�����</param> 
        /// <remarks>����ֱ���������߶����жϻ����һЩС���⣬����������÷ֶ��жϵķ���</remarks>
        public void ConfirmTriangleSide(ref List<CTriangle> CTriangleLt, CPolyline cpl, double dblVerySmall)
        {
            //�ж�ÿ�������ε�ÿ�����Ƿ����ڱ߼�������ǣ������ݸñߣ��жϸ��������ڱߵ���һ�� 
            //�ж������ε��������Ǳ߽��
            double dblAlongDis = new double();
            double dblFromDis = new double();
            for (int i = 0; i < CTriangleLt.Count; i++)
            {
                int intCount = 0;
                for (int j = 0; j < CTriangleLt[i].CEdgeLt.Count; j++)
                {
                    CEdge cedge=CTriangleLt[i].CEdgeLt[j];


                    IPoint outPointFr = new PointClass();
                    double distanceAlongCurveFr = 0;//�õ�������������ĵ���������ľ���
                    double distanceFromCurveFr = 0;//�õ㵽���ߵ�ֱ�߾���
                    bool bRightSideFr = false;
                    MessageBox.Show("CTriangulator.cs: Row914 is needed to be improved");
                    //cpl.pPolyline.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, cedge.FromPoint, false, outPointFr, ref distanceAlongCurveFr, ref distanceFromCurveFr, ref bRightSideFr);

                    IPoint outPointTo = new PointClass();
                    double distanceAlongCurveTo = 0;//�õ�������������ĵ���������ľ���
                    double distanceFromCurveTo = 0;//�õ㵽���ߵ�ֱ�߾���
                    bool bRightSideTo = false;
                    MessageBox.Show("CTriangulator.cs: Row921 is needed to be improved");
                    //cpl.pPolyline.QueryPointAndDistance(esriSegmentExtension.esriNoExtension, cedge.ToPoint, false, outPointTo, ref distanceAlongCurveTo, ref distanceFromCurveTo, ref bRightSideTo);

                    double dblDisDiff = Math.Abs(Math.Abs(distanceAlongCurveTo - distanceAlongCurveFr) - cedge.dblLength);
                    if ((dblDisDiff < dblVerySmall) && (distanceFromCurveFr < dblVerySmall) && (distanceFromCurveTo < dblVerySmall))
                    {
                        cedge.isBelongToPolyline = true;
                        if (CTriangleLt[i].isSideJudge == false)  //������жϹ������������ж�
                        {
                            CEdge CRightDirectionEdge = new CEdge();
                            if (distanceAlongCurveFr < distanceAlongCurveTo)
                            {
                                CRightDirectionEdge = cedge;
                            }
                            else if (distanceAlongCurveFr > distanceAlongCurveTo)
                            {
                                CRightDirectionEdge = new CEdge(cedge.ToCpt, cedge.FrCpt);
                            }
                            else
                            {
                                MessageBox.Show("�ж�������λ�ڶ��������ʱ���غϵ㣡");
                            }

                            IPoint ipt = new PointClass();
                            bool blnRightSide = new bool();
                            MessageBox.Show("CTriangulator.cs: Row944 is needed to be improved");
                            //CRightDirectionEdge.QueryPointAndDistance(esriSegmentExtension.esriExtendEmbedded, CTriangleLt[i].CentroidCpt.pPoint, false, ipt, ref dblAlongDis, ref dblFromDis, ref blnRightSide);
                            //�Ƿ���Ҫ�ҵ�һ���������
                            if (blnRightSide == true) CTriangleLt[i].strSide = "Right";
                            else CTriangleLt[i].strSide = "Left";
                            CTriangleLt[i].isSideJudge = true;
                        }

                        intCount++;
                    }

                    //if (intCount==3)
                    //{
                    //    int t = 5;
                    //}                 

                }
            }

            //ȷ������û�����߱߶��������������������߱ߵ������εġ����ҡ����
            for (int i = 0; i < CTriangleLt.Count; i++)
            {
                if (CTriangleLt[i].isSideJudge == true)
                {
                    CTriangle pCTriangle = CTriangleLt[i];
                    RecursiveConfirmTriangleSide(ref pCTriangle);
                }
            }          
        }

        private void RecursiveConfirmTriangleSide(ref CTriangle CCurentTriangle)
        {
            for (int i = 0; i < CCurentTriangle.SETriangleLt.Count; i++)
            {
                if (CCurentTriangle.SETriangleLt[i].TID == -2) continue;  //��������������β����ڣ���ֱ�ӱ�����һ������������
                if (CCurentTriangle.SETriangleLt[i].isSideJudge == false && CCurentTriangle.SETriangleLt[i].strTriType ==null)
                {
                    CCurentTriangle.SETriangleLt[i].strSide = CCurentTriangle.strSide;
                    CCurentTriangle.SETriangleLt[i].isSideJudge = true;
                    CTriangle CSETriangle = CCurentTriangle.SETriangleLt[i];
                    RecursiveConfirmTriangleSide(ref CSETriangle);
                }
            }
        }

        /// <summary>������������͡�II������III������IV��</summary>
        /// <param name="CTriangleLt">�������</param>
        /// <remarks>��I�������������ڴ���Լ��������ʱ���</remarks>
        public void SignTriTypeAll(ref List<CTriangle> CTriangleLt)
        {
            for (int i = 0; i < CTriangleLt.Count; i++)
            {
                if (CTriangleLt[i].strTriType != "I")
                {
                    int intCount = 0;
                    for (int j = 0; j < CTriangleLt[i].CEdgeLt.Count; j++)
                    {
                        if (CTriangleLt[i].CEdgeLt[j].isBelongToPolyline == true)  //���жϸ��������Ƿ����߶�ĳ���ʱ���Ѿ�����˸ñ��Ƿ�Ϊ���߱�
                            intCount = intCount + 1;
                    }
                    switch (intCount)
                    {
                        case 0: CTriangleLt[i].strTriType = "IV"; break;    //û�����߱ߣ�����ض����������������Σ������Ϊ"IV"��������
                        case 1: CTriangleLt[i].strTriType = "III"; break;   //��һ�����߱ߣ�����ض����������������Σ������Ϊ"III"��������
                        case 2: CTriangleLt[i].strTriType = "II"; break;    //���������߱ߣ�����ض�ֻ��һ�����������Σ������Ϊ"II"��������
                        case 3: CTriangleLt[i].strTriType = "V"; break;    //���������߱ߣ�����ض�ֻ��һ�����������Σ������Ϊ"II"��������
                        default: MessageBox.Show("δ֪����"); break;
                    }
                }
            }

        }

        ///// <summary>������������͡�I������II������III������IV��</summary>
        ///// <param name="CTriangleLt">�������</param>
        ///// <remarks>Ϊ�˽�ʡʱ�䣬ֻ����Ҫһ��������βű�ʶ�ˡ�II������III������IV����������</remarks>
        //public void SignTriTypeNeed(ref List<CTriangle> CTriangleLt)
        //{
        //    //��ʼ������������
        //    for (int i = 0; i < CTriangleLt.Count; i++)
        //        CTriangleLt[i].strTriType = "";
            

        //    for (int i = 0; i < CTriangleLt.Count ; i++)
        //    {
        //        bool blnIsI = false;
        //        for (int j = 0; j < CTriangleLt[i].CptLt.Count ; j++)
        //        {
        //            if (CTriangleLt[i].CptLt[j].ID > _intMaxID)
        //            {
        //                blnIsI = true;
        //                CTriangleLt[i].isNeedSide = false;
        //                break;
        //            }
        //        }

        //        if (blnIsI == true) CTriangleLt[i].strTriType = "I";
        //        else if (CTriangleLt[i].isNeedSide == true)  //ע�⣺Ϊ�˽�ʡʱ�䣬ֻ����Ҫһ��������βű�ʶ�ˡ�II������III������IV����������
        //        {
        //            int intCount = 0;
        //            for (int j = 0; j < CTriangleLt[i].CEdgeLt.Count; j++)
        //            {
        //                if (CTriangleLt[i].CEdgeLt[j].isBelongToPolyline == true)  //���жϸ��������Ƿ����߶�ĳ���ʱ���Ѿ�����˸ñ��Ƿ�Ϊ���߱�
        //                    intCount = intCount + 1;
        //            }
        //            switch (intCount)
        //            {
        //                case 0: CTriangleLt[i].strTriType = "IV"; break;    //û�����߱ߣ�����ض����������������Σ������Ϊ"IV"��������
        //                case 1: CTriangleLt[i].strTriType = "III"; break;   //��һ�����߱ߣ�����ض����������������Σ������Ϊ"III"��������
        //                case 2: CTriangleLt[i].strTriType = "II"; break;    //���������߱ߣ�����ض�ֻ��һ�����������Σ������Ϊ"II"��������
        //                default: MessageBox.Show("ĳ�����ε������߶������߱ߣ�");break;
        //            }
        //        }
        //    }
        //}


        /// <summary>���������Ĳ�νṹɭ�֣���Ϊһ�����߿��ܻ��ж����������߼����������</summary>
        /// <param name="CTriangleLt">�������</param>
        /// <param name="CPtlt">���ߵĵ�����</param> 
        /// <param name="strSide">���ߵ�ĳһ�ߡ�Left������Right��</param>  
        /// <remarks>��ָ����strSide������ȡĳ������ɭ��
        /// </remarks>
        public CBendForest BuildBendForestNeed2(ref List<CTriangle> CTriangleLt, List<CPoint> CPtlt, string strSide, double dblVerySmall)
        {
            CBend COriginalBend = new CBend(CPtlt);  //�½�ԭʼ��������Ȼ�á���������Ӧ�ɶ������������϶��ɣ�����������������ʹ��
            //SortedList<int, CBend> CBendForest = new SortedList<int, CBend>(new CIntCompare());  //��νṹ����ɭ��

            CBendForest pBendForest = new CBendForest();

            for (int i = 0; i < CTriangleLt.Count; i++)
            {
                if (CTriangleLt[i].strSide != strSide) continue;    //����������������Σ��������
                if (CTriangleLt[i].strTriType != "I")  //����������α����ǡ�I����������
                {
                    for (int j = 0; j < CTriangleLt[i].SETriangleLt.Count; j++)
                    {
                        if (CTriangleLt[i].CEdgeLt[j].isBelongToPolyline == false && CTriangleLt[i].SETriangleLt[j].strTriType == "I")
                        {
                            //����������α����ǡ�I���������Σ������������������С�I���������Σ��Ҹù����߲������߱ߣ����������Ϊһ��������ڣ���������                        
                            CBend CHiberarchyBend = COriginalBend.GetSubBend(CTriangleLt[i].CEdgeLt[j].FrCpt, CTriangleLt[i].CEdgeLt[j].ToCpt, strSide, dblVerySmall);
                            BuildHiberarchyOfBend(CTriangleLt, CHiberarchyBend, CTriangleLt[i], CTriangleLt[i].SETriangleLt[j], strSide, dblVerySmall);
                            pBendForest.Add(CHiberarchyBend.CptLt[0].ID, CHiberarchyBend);
                            break;
                        }
                    }
                }
            }
            return pBendForest;
        }

        /// <summary>�ݹ齨�������ṹ</summary>
        /// <param name="CTriangleLt">�������</param>
        /// <param name="CHiberarchyBend">����νṹ������</param> 
        /// <param name="CCurrentTri">��ǰ������</param>  
        /// <param name="FrontTID">��һ�������ڡ�CTriangleLt���е����</param> 
        /// <remarks></remarks>
        private void BuildHiberarchyOfBend(List<CTriangle> CTriangleLt, CBend CHiberarchyBend, CTriangle CCurrentTri, CTriangle CFrontTri, string strSide, double dblVerySmall)
        {
            CHiberarchyBend.CTriangleLt.Add(CFrontTri);

            switch (CCurrentTri.strTriType)
            {
                case "II":     //�����������Ϊ��II���������Σ������
                    CHiberarchyBend.CLeftBend = null;                
                    CHiberarchyBend.CRightBend = null;
                    return;
                case "III":    //�����������Ϊ��III���������Σ������������ȥ
                    for (int i = 0; i < CCurrentTri.SETriangleLt.Count ; i++)
                    {
                        if (CCurrentTri.SETriangleLt[i].TID == -2) 
                            throw new ArgumentException("�ڲ������ε��ڽ������ξ�Ȼ�в����ڵ������");  //�������Ӧ�ò������
                        else if (CCurrentTri.SETriangleLt[i].TID == CFrontTri.TID || CCurrentTri.SETriangleLt[i].strTriType == "I") continue;  //������ڽ�����������һ�������Σ�������
                        else if (CCurrentTri.CEdgeLt[i].isBelongToPolyline == true) continue;  //����ò������߱ߣ�����
                        else
                        {
                            BuildHiberarchyOfBend(CTriangleLt, CHiberarchyBend, CCurrentTri.SETriangleLt[i], CCurrentTri, strSide, dblVerySmall);
                            break;
                        } 
                    }
                    break;

                case "IV":     //"IV"�������ε�����鷳һ��
                    //���ȣ��ҵ���֮ǰ�����ζ�Ӧ�ıߣ����ҵ�FromPoint��ToPoint
                    int FrontEdgeNum=0;
                    for (int i = 0; i < CCurrentTri.SETriangleLt .Count ; i++)
                    {
                        if (CCurrentTri.SETriangleLt[i].TID == -2) 
                            MessageBox.Show("�ڲ������ε��ڽ������ξ�Ȼ�в����ڵ������"); //�������Ӧ�ò������
                        else if (CCurrentTri.SETriangleLt[i].TID == CFrontTri.TID)
                        {
                            FrontEdgeNum = i;
                            break;
                        }
                    }
                    CPoint CFrCpt = CCurrentTri.CEdgeLt[FrontEdgeNum].FrCpt;
                    CPoint CToCpt = CCurrentTri.CEdgeLt[FrontEdgeNum].ToCpt;                    

                    //��������һ���涨�����������εĵ㡢�߼��ڽ������ζ��ǰ�˳ʱ�뷽��洢�ģ�
                    //��ˣ��롰FrontEdgeNum�����й�����CFrCpt�ı߶�Ӧ�ķ�֧Ϊ���Һ��ӣ�RightChild����
                    //      �롰FrontEdgeNum�����й�����CToCpt�ı߶�Ӧ�ķ�֧Ϊ�����ӣ�LeftChild����
                    for (int i = 0; i < CCurrentTri.CEdgeLt.Count; i++)
                    {
                        if (i == FrontEdgeNum) continue;//����Ǳ��ߣ�������

                        if (CCurrentTri.CEdgeLt[i].FrCpt.Equals2D(CToCpt)) //����
                        {
                            CHiberarchyBend.CLeftBend = CHiberarchyBend.GetSubBend(CCurrentTri.CEdgeLt[i].FrCpt, CCurrentTri.CEdgeLt[i].ToCpt, strSide, dblVerySmall); //��������
                            CHiberarchyBend.CLeftBend.CParentBend = CHiberarchyBend; //�븸����������
                            BuildHiberarchyOfBend(CTriangleLt, CHiberarchyBend.CLeftBend, CCurrentTri.SETriangleLt[i], CCurrentTri, strSide, dblVerySmall);
                        }
                        else if (CCurrentTri.CEdgeLt[i].ToCpt.Equals2D(CFrCpt)) //�Һ���
                        {
                            CHiberarchyBend.CRightBend = CHiberarchyBend.GetSubBend(CCurrentTri.CEdgeLt[i].FrCpt, CCurrentTri.CEdgeLt[i].ToCpt, strSide, dblVerySmall); //��������
                            CHiberarchyBend.CRightBend.CParentBend = CHiberarchyBend; //�븸����������
                            BuildHiberarchyOfBend(CTriangleLt, CHiberarchyBend.CRightBend, CCurrentTri.SETriangleLt[i], CCurrentTri, strSide, dblVerySmall);
                        }
                    }

                    break;
                default:
                    MessageBox.Show("�ݹ齨����������"); 
                    break;
            }
        }

        /// <summary>����ͬ��</summary>
        /// <param name="ipt">Ŀ���</param>
        /// <param name="cptlt">������</param> 
        /// <param name="dblVerySmall">��Сֵ</param>  
        /// <remarks>���Ŀ�������ͬ�㣬�򷵻���ͬ�㣬���򣬷��ص�ǰĿ���</remarks>
        private void FindSamePoint(IPoint ipt, CPolyline cpl, ref List<CPoint> ctempptlt, double dblVerySmall, ref CPoint cpt)
        {
            cpt = new CPoint(ipt);
            for (int i = 0; i < ctempptlt.Count; i++)
            {
                if (cpt.Equals2D(ctempptlt[i]))
                {
                    cpt = ctempptlt[i];
                    return;
                }
            }

            //If the point cpt doesn't exist in ctempptlt
            double dblAlongDis = 0;
            double dblFromDis = 0;
            bool blnIsRight=false ;
            IPoint outpt = new PointClass();
            cpl.pPolyline.QueryPointAndDistance(esriSegmentExtension.esriExtendEmbedded, ipt, false, outpt, ref dblAlongDis, ref dblFromDis, ref blnIsRight);
            if (dblFromDis<dblVerySmall)
            {
                var newcpt = new CPoint(outpt);
                cpt = newcpt;
                ctempptlt.Add(newcpt);
            }
        }



    }
}
