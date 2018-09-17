using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geometry;
using MorphingClass.CUtility;
using MorphingClass.CGeometry;


namespace MorphingClass.CEntity
{
    public class CRiverNet
    {
        private int _intRNID = -1;                        //�߶κ�
       
        private CRiver _CMasterStream;                         //������
        private List<CRiver> _CRiverLt;            //����
        private CRiverNet _CCorrRiverNet;              //��Ӧ����

        public CRiverNet()
        {

        }

        public CRiverNet(int pintRNID, CRiver pCMasterStream)
        {
            _intRNID = pintRNID;
            _CMasterStream = pCMasterStream;
            _CRiverLt = new List<CRiver>();
            _CRiverLt.Add(pCMasterStream);
        }


        /// <summary>���ԣ�������</summary>
        public int RNID
        {
            get { return _intRNID; }
            set { _intRNID = value; }
        }

        /// <summary>���ԣ�������</summary>
        public CRiver CMasterStream
        {
            get { return _CMasterStream; }
            set { _CMasterStream = value; }
        }

        /// <summary>���ԣ�����</summary>
        public List<CRiver> CRiverLt
        {
            get { return _CRiverLt; }
            set { _CRiverLt = value; }
        }

        /// <summary>���ԣ�����</summary>
        public CRiverNet CCorrRiverNet
        {
            get { return _CCorrRiverNet; }
            set { _CCorrRiverNet = value; }
        }


    }
}
