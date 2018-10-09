using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;

namespace MorphingClass.CEntity
{
    public class CRiver :CLinearEntity 
    {

        private double _dblLengthSum;         //�ܺ�������(�ú��������֧�����ȵ��ܺ�)
        private double _dblLevel;            //�����Ĳ��(������Ϊ1��֧����Ϊ�����Ĳ��+1)����ӦΪ���ͣ�Ϊ������㶨��Ϊ˫����
        private double _dblOrder;            //��ָ����ʽ����ĺ����ȼ�����ӦΪ���ͣ�Ϊ������㶨��Ϊ˫����
        private double _dblWeightiness;      //��������Ҫ��
        private double _dblWeightinessUnitary;  //��һ����ĺ�����Ҫ��
               
        private CRiver _CMainStream;                         //����
        private CRiver _CCorrRiver;                          //��Ӧ����
        
        private List<CPoint> _CCorrTriJunctionPtLt;    //��Ӧ֧����Ӧ��ϵ�(CorrepondenceTributaryJunctionPtLt)
        private List<CRiver> _CTributaryLt;            //֧��
        
        

        


         public CRiver()
        {
             
        }



        //public CRiver(int intID, IPolyline5 pPolyline)
        //{
        //    _intID = intID;

        //    IPointCollection4 pCol = new PolylineClass();
        //    pCol = (IPointCollection4)pPolyline;
        //    this.SetPointCollection(pCol);
        //    this._pPolyline = pPolyline;

        //    _CptLt = pHelperFunction.GetCPtLtByIPl(pPolyline);
        //}

        public CRiver(int intID, CPolyline cpl,double dblBuffer, double dblVerySmall)
        {
            _intID = intID;

            this .pPolyline =cpl.pPolyline;
            this.CptLt = cpl.CptLt;
            _CTributaryLt = new List<CRiver>();
            CreateBufferAndSmallBuffer( dblBuffer, dblVerySmall);
        }

        public CRiver(int intID, CPolyline cpl, double dblVerySmall)
        {
            _intID = intID;
            this.pPolyline = cpl.pPolyline;
            this.CptLt = cpl.CptLt;
            _CTributaryLt = new List<CRiver>();
            CreateSmallBufferForEnds(dblVerySmall);
            CreateSmallBufferForPolyline(dblVerySmall);
        }

        public CRiver(int intID, List<CPoint> cptlt0)
        {
            _intID = intID;
            this.CptLt = cptlt0;

            IPointCollection4 pCol = new PolylineClass();
            for (int i = 0; i < cptlt0.Count; i++)
            {
                pCol.AddPoint((IPoint)cptlt0[i]);
            }

            IPolyline5 pPolyline = pCol as IPolyline5;
            this.pPolyline = pPolyline;
            //FormPolyline(cptlt0);
            _CTributaryLt = new List<CRiver>();
        }

        /// <summary>���ԣ��ܺ�������(�ú��������֧�����ȵ��ܺ�)</summary>
        public double LengthSum
        {
            get { return _dblLengthSum; }
            set { _dblLengthSum = value; }
        }

       

        /// <summary>���ԣ���Ӧ֧����Ӧ��ϵ�(CorrepondenceTributaryJunctionPtLt)</summary>
        public List<CPoint> CCorrTriJunctionPtLt
        {
            get { return _CCorrTriJunctionPtLt; }
            set { _CCorrTriJunctionPtLt = value; }
        }

        /// <summary>���ԣ�����</summary>
        public CRiver CMainStream
        {
            get { return _CMainStream; }
            set { _CMainStream = value; }
        }

        /// <summary>���ԣ���Ӧ����</summary>
        public CRiver CCorrRiver
        {
            get { return _CCorrRiver; }
            set { _CCorrRiver = value; }
        }

      


        /// <summary>���ԣ�֧��</summary>
        public List<CRiver> CTributaryLt
        {
            get { return _CTributaryLt; }
            set { _CTributaryLt = value; }
        }

         /// <summary>���ԣ������Ĳ��(������Ϊ1��֧����Ϊ�����Ĳ��+1)����ӦΪ���ͣ�Ϊ������㶨��Ϊ˫����</summary>
        public double dblLevel
        {
            get { return _dblLevel; }
            set { _dblLevel = value; }
        }

        /// <summary>���ԣ���ָ����ʽ����ĺ����ȼ�����ӦΪ���ͣ�Ϊ������㶨��Ϊ˫����</summary>
        public double dblOrder
        {
            get { return _dblOrder; }
            set { _dblOrder = value; }
        }

        /// <summary>���ԣ���������Ҫ��</summary>
        public double dblWeightiness
        {
            get { return _dblWeightiness; }
            set { _dblWeightiness = value; }
        }

        /// <summary>���ԣ���һ����ĺ�����Ҫ��</summary>
        public double dblWeightinessUnitary
        {
            get { return _dblWeightinessUnitary; }
            set { _dblWeightinessUnitary = value; }
        }



        #region IEquatable<dEdge> Members

        /// <summary>
        /// Checks whether two cedges are equal disregarding the direction of the cedges
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CRiver other)
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


    }


   
}
